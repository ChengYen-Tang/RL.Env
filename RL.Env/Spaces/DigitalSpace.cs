using RL.Env.Utils.Serialization;
using System.Text.Json.Serialization;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
using JsonConverterAttribute = System.Text.Json.Serialization.JsonConverterAttribute;

namespace RL.Env.Spaces;

/// <summary>
/// A Space is the set of all possible numerical values for an observation or action.
/// </summary>
[JsonDerivedType(typeof(Box), typeDiscriminator: nameof(Box))]
[JsonDerivedType(typeof(Discrete), typeDiscriminator: nameof(Discrete))]
[JsonDerivedType(typeof(MultiBinary), typeDiscriminator: nameof(MultiBinary))]
[JsonDerivedType(typeof(MultiDiscrete), typeDiscriminator: nameof(MultiDiscrete))]
public abstract partial class DigitalSpace : Space
{
    [JsonConverter(typeof(NdarrayJsonConverter))]
    public ndarray Low { get; private init; } = null!;
    [JsonConverter(typeof(NdarrayJsonConverter))]
    public ndarray High { get; private init; } = null!;
    [JsonInclude]
    [JsonConverter(typeof(NdarrayJsonConverter))]
    public ndarray BoundedBelow { get; private set; } = null!;
    [JsonInclude]
    [JsonConverter(typeof(NdarrayJsonConverter))]
    public ndarray BoundedAbove { get; private set; } = null!;

    public DigitalSpace(ndarray low, ndarray high, shape shape, dtype type, Union<np.random, uint>? seed = null)
        : base(shape, type, seed)
    {
        CheckInitParameter(low, high, shape, type);
        Low = low.astype(type, false);
        High = high.astype(type, false);
        CoculateBounded();
    }

    protected DigitalSpace(ndarray low, ndarray high, ndarray boundedBelow, ndarray boundedAbove, shape shape, dtype type, np.random npRandom)
        : base(shape, type, npRandom)
        => (Low, High, BoundedBelow, BoundedAbove) = (low, high, boundedBelow, boundedAbove);

    /// <summary>
    /// Returns a sample from the space.
    /// </summary>
    /// <returns></returns>
    public override ndarray Sample()
    {
        ndarray unbounded = ~BoundedBelow & ~BoundedAbove;
        ndarray uppBounded = ~BoundedBelow & BoundedAbove;
        ndarray lowBounded = BoundedBelow & ~BoundedAbove;
        ndarray bounded = BoundedBelow & BoundedAbove;
        ndarray High = Type.Kind == 'f' || Type.Kind == 'd' ? this.High : this.High + 1;
        ndarray sample = np.empty(Shape, Type);
        sample[unbounded] = NpRandom.normal(0.0, 1.0, (unbounded[unbounded] as ndarray)!.shape);
        sample[lowBounded] = NpRandom.exponential(1.0, (lowBounded[lowBounded] as ndarray)!.shape) + Low[lowBounded];
        sample[uppBounded] = -NpRandom.exponential(1.0, (uppBounded[uppBounded] as ndarray)!.shape) + High[uppBounded];
        sample[bounded] = NpRandom.uniform(Low[bounded], High[bounded], (bounded[bounded] as ndarray)!.shape);
        return sample;
    }

    /// <summary>
    /// Check if the sample is valid.
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public override Result CheckConditions(ndarray array)
    {
        Result result = base.CheckConditions(array);
        if (result.IsFailed)
            return result;
        if (!np.allb(array <= High))
            return Result.Fail(new string[] { array.ToString(), "Value must be less than or equal to high." });
        if (!np.allb(array >= Low))
            return Result.Fail(new string[] { array.ToString(), "Value must be greater than or equal to low." });
        return Result.Ok();
    }

    public override string ToString()
    {
        string baseString = base.ToString();
        baseString += $"Low: {Low}\nHigh: {High}";
        return baseString;
    }

    protected abstract Result CheckType(dtype type);

    private void CheckInitParameter(ndarray low, ndarray high, shape shape, dtype type)
    {
        ArgumentNullException.ThrowIfNull(low);
        ArgumentNullException.ThrowIfNull(high);
        Result checkResult = CheckType(type);
        if (checkResult.IsFailed)
            throw new ArgumentException(checkResult.Errors[0].Message);
        if (low.shape != shape)
            throw new ArgumentException("The low array does not match the shape defined by the condition.");
        if (high.shape != shape)
            throw new ArgumentException("The high array does not match the shape defined by the condition.");
        if (np.anyb(low > high))
            throw new ArgumentException("Low must be less than high.");
    }

    private void CoculateBounded()
    {
        BoundedBelow = Low > double.NegativeInfinity;
        BoundedAbove = High < double.PositiveInfinity;
    }

    public static bool operator ==(DigitalSpace obj1, DigitalSpace obj2)
        => obj1.Equals(obj2);

    public static bool operator !=(DigitalSpace obj1, DigitalSpace obj2)
        => !obj1.Equals(obj2);
}
