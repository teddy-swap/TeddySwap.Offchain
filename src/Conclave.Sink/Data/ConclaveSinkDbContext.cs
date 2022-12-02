
using Conclave.Sink.Models;
using Microsoft.EntityFrameworkCore;

namespace Conclave.Sink.Data;

public class ConclaveSinkDbContext : DbContext
{
    public DbSet<AddressByStake> AddressByStake => Set<AddressByStake>();
    public DbSet<BalanceByAddress> BalanceByAddress => Set<BalanceByAddress>();
    public DbSet<TxInput> TxInput => Set<TxInput>();
    public DbSet<TxOutput> TxOutput => Set<TxOutput>();
    public DbSet<Block> Block => Set<Block>();
    public ConclaveSinkDbContext(DbContextOptions<ConclaveSinkDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Primary Keys
        modelBuilder.Entity<AddressByStake>().HasKey(s => s.StakeAddress);
        modelBuilder.Entity<BalanceByAddress>().HasKey(s => s.Address);
        modelBuilder.Entity<TxInput>().HasKey(txInput => new { txInput.TxHash, txInput.TxOutputHash, txInput.TxOutputIndex });
        modelBuilder.Entity<TxOutput>().HasKey(txOut => new { txOut.TxHash, txOut.Index });
        modelBuilder.Entity<Block>().HasKey(block => block.BlockHash);

        // Relations
        modelBuilder.Entity<TxInput>()
            .HasOne<TxOutput>(txInput => txInput.TxOutput)
            .WithMany(txOutput => txOutput.Inputs)
            .HasForeignKey(txInput => new { txInput.TxOutputHash, txInput.TxOutputIndex });

        base.OnModelCreating(modelBuilder);
    }
}