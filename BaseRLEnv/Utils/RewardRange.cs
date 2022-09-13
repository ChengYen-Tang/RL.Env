namespace BaseRLEnv.Utils;

public sealed class RewardRange
{
    public double Max { get; init; }
    public double Min { get; init; }

    public RewardRange(double max, double min)
    {
        ArgumentNullException.ThrowIfNull(max);
        ArgumentNullException.ThrowIfNull(min);
        if (max <= min)
            throw new Error("Min must be less than Max.");
        Max = max;
        Min = min;
    }

    public void CheckConditions(double reward)
    {
        ArgumentNullException.ThrowIfNull(reward);
        if (reward > Max || reward < Min)
            throw new Error($"Reward must be >= {Min}, and reward must be <= {Max}.");
    }
}
