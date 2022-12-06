using System.Net;
using System.Text.Json;
using Conclave.Sink.Data;
using Conclave.Sink.Models;
using Conclave.Sink.Services;
using Microsoft.EntityFrameworkCore;

namespace Conclave.Sink.Reducers;

[OuraReducer(OuraVariant.PoolRetirement)]
public class PoolRetirementReducer : OuraReducerBase
{
    private readonly ILogger<PoolRetirementReducer> _logger;
    private readonly IDbContextFactory<ConclaveSinkDbContext> _dbContextFactory;
    private readonly CardanoService _cardanoService;
    public PoolRetirementReducer(
        ILogger<PoolRetirementReducer> logger,
        IDbContextFactory<ConclaveSinkDbContext> dbContextFactory,
        CardanoService cardanoService)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _cardanoService = cardanoService;
    }

    public async Task ReduceAsync(OuraEvent ouraEvent)
    {
        using ConclaveSinkDbContext _dbContext = await _dbContextFactory.CreateDbContextAsync();

        OuraPoolRetirementEvent? poolRetirementEvent = ouraEvent as OuraPoolRetirementEvent;
        if (poolRetirementEvent is not null &&
            poolRetirementEvent.Context is not null &&
            poolRetirementEvent.PoolRetirement is not null &&
            poolRetirementEvent.PoolRetirement.Pool is not null &&
            poolRetirementEvent.PoolRetirement.Epoch is not null &&
            poolRetirementEvent.Context.TxHash is not null)
        {
            Block? block = await _dbContext.Block
                .Where(b => b.BlockHash == poolRetirementEvent.Context.BlockHash)
                .FirstOrDefaultAsync();

            Transaction? transaction = await _dbContext.Transaction
                .Where(t => t.Hash == poolRetirementEvent.Context.TxHash)
                .FirstOrDefaultAsync();

            if (block is not null &&
                transaction is not null)
            {
                await _dbContext.PoolRetirement.AddAsync(new()
                {
                    Pool = poolRetirementEvent.PoolRetirement.Pool,
                    EffectiveEpoch = (ulong)poolRetirementEvent.PoolRetirement.Epoch,
                    TxHash = poolRetirementEvent.Context.TxHash,
                    Block = block,
                    Transaction = transaction
                });
            }

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RollbackAsync(Block rollbackBlock)
    {
        await Task.CompletedTask;
    }
}