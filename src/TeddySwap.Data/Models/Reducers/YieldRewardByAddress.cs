namespace TeddySwap.Data.Models.Reducers;
public record YieldRewardByAddress (
    string Address,
    string PoolId,
    ulong Amount,
    ulong LPAmount,
    ulong Bonus,
    decimal PoolShare,
    string[] TBCs,
    bool IsClaimed,
    string? ClaimTxId,
    ulong BlockNumber,
    ulong Slot,
    DateTimeOffset Timestamp
);