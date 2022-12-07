using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Models.Addresses;
using Conclave.Sink.Data;
using Conclave.Common.Models;
using Microsoft.EntityFrameworkCore;
using Conclave.Sink.Models.OuraEvents;

namespace Conclave.Sink.Reducers;

[OuraReducer(OuraVariant.TxInput, OuraVariant.TxOutput)]
public class BalanceByAddressReducer : OuraReducerBase
{
    private readonly ILogger<BalanceByAddressReducer> _logger;
    private IDbContextFactory<ConclaveSinkDbContext> _dbContextFactory;
    public BalanceByAddressReducer(
        ILogger<BalanceByAddressReducer> logger,
        IDbContextFactory<ConclaveSinkDbContext> dbContextFactory)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task ReduceAsync(OuraEvent ouraEvent)
    {
        using ConclaveSinkDbContext _dbContext = await _dbContextFactory.CreateDbContextAsync();
        await (ouraEvent.Variant switch
        {
            OuraVariant.TxInput => Task.Run(async () =>
            {
                OuraTxInputEvent? txInputEvent = ouraEvent as OuraTxInputEvent;
                if (txInputEvent is not null && txInputEvent.TxInput is not null)
                {
                    TxOutput? input = await _dbContext.TxOutputs
                        .Where(txOut => txOut.TxHash == txInputEvent.TxInput.TxHash && txOut.Index == txInputEvent.TxInput.Index).FirstOrDefaultAsync();

                    if (input is not null)
                    {

                        BalanceByAddress? entry = await _dbContext.BalanceByAddress
                            .Where((bba) => bba.Address == input.Address)
                            .FirstOrDefaultAsync();

                        if (entry is not null)
                        {
                            entry.Balance -= input.Amount;
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }),
            OuraVariant.TxOutput => Task.Run(async () =>
            {
                OuraTxOutputEvent? txOutputEvent = ouraEvent as OuraTxOutputEvent;
                if (txOutputEvent is not null &&
                    txOutputEvent.TxOutput is not null &&
                    txOutputEvent.TxOutput.Amount is not null)
                {
                    Address outputAddress = new Address(txOutputEvent.TxOutput.Address);
                    ulong amount = (ulong)txOutputEvent.TxOutput.Amount;

                    BalanceByAddress? entry = await _dbContext.BalanceByAddress
                        .Where((bba) => bba.Address == outputAddress.ToString())
                        .FirstOrDefaultAsync();

                    if (entry is not null)
                    {
                        entry.Balance += amount;
                    }
                    else
                    {
                        await _dbContext.BalanceByAddress.AddAsync(new()
                        {
                            Address = outputAddress.ToString(),
                            Balance = amount
                        });
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }),
            _ => Task.Run(() => { })
        });
    }

    public async Task RollbackAsync(Block rollbackBlock)
    {
        using ConclaveSinkDbContext _dbContext = await _dbContextFactory.CreateDbContextAsync();
        IEnumerable<TxInput> consumed = await _dbContext.TxInputs
            .Include(txInput => txInput.TxOutput)
            .Include(txInput => txInput.Transaction)
            .ThenInclude(tx => tx.Block)
            .Where(txInput => txInput.Transaction.Block == rollbackBlock)
            .ToListAsync();
        IEnumerable<TxOutput> produced = await _dbContext.TxOutputs
            .Include(txOutput => txOutput.Transaction)
            .ThenInclude(tx => tx.Block)
            .Where(txOutput => txOutput.Transaction.Block == rollbackBlock)
            .ToListAsync();

        // process consumed
        IEnumerable<Task> consumeTasks = consumed.ToList().Select(txInput => Task.Run(async () =>
        {
            if (txInput.TxOutput is not null)
            {
                BalanceByAddress? entry = await _dbContext.BalanceByAddress
                           .Where((bba) => bba.Address == txInput.TxOutput.Address)
                           .FirstOrDefaultAsync();

                if (entry is not null)
                {
                    entry.Balance += txInput.TxOutput.Amount;
                }
            }
        }));

        foreach (Task consumeTask in consumeTasks) await consumeTask;

        // process produced
        IEnumerable<Task> produceTasks = produced.ToList().Select(txOutput => Task.Run(async () =>
        {
            BalanceByAddress? entry = await _dbContext.BalanceByAddress
                .Where((bba) => bba.Address == txOutput.Address)
                .FirstOrDefaultAsync();

            if (entry is not null)
            {
                entry.Balance -= txOutput.Amount;
            }
        }));

        foreach (Task produceTask in produceTasks) await produceTask;

        await _dbContext.SaveChangesAsync();
    }
}