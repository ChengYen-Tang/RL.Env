namespace BaseRLEnv.Spaces;

/// <summary>
/// Specifically, a Box represents the Cartesian product of n closed intervals.
/// Each interval has the form of one of :math:`[a, b]`, :math:`(-\infty, b]`,
/// :math:`[a, \infty)`, or :math:`(-\infty, \infty)`.
/// 
/// There are two common use cases:
/// 
///  * Identical bound for each dimension::
///     >>> Box(low= -1.0, high= 2.0, shape= (3, 4), dtype= np.float32)
///     Box(3, 4)
///     
/// * Independent bound for each dimension::
///     >>> Box(low= np.array([-1.0, -2.0]), high=np.array([2.0, 4.0]), dtype=np.float32)
///     Box(2,)
/// </summary>
public class Box : Space
{
    public ndarray Low { get; private init; }
    public ndarray High { get; private init; }
    public ndarray BoundedBelow { get; private set; }
    public ndarray BoundedAbove { get; private set; }

    public Box(double low, double high, shape shape, dtype type, uint? seed = null)
        : this(np.full(shape, low, type), np.full(shape, high, type), shape, type, seed)
    {
        if (low >= high)
            throw new ArgumentException("Low must be less than high.");
    }

    public Box(double low, double high, shape shape, dtype type, np.random npRandom)
        : this(np.full(shape, low, type), np.full(shape, high, type), shape, type, npRandom)
    {
        if (low >= high)
            throw new ArgumentException("Low must be less than high.");
    }

    public Box(ndarray low, ndarray high, shape shape, dtype type, uint? seed = null)
        : base(shape, type, seed)
    {
        CheckInitParameter(low, high, shape, type);
        Low = low;
        High = high;
        CoculateBounded();
    }

    public Box(ndarray low, ndarray high, shape shape, dtype type, np.random npRandom)
        : base(shape, type, npRandom)
    {
        CheckInitParameter(low, high, shape, type);
        Low = low;
        High = high;
        CoculateBounded();
    }

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
        ndarray sample = np.empty(Shape, Type);
        sample[unbounded] = NpRandom.normal(0.0, 1.0, (unbounded[unbounded] as ndarray).shape);
        sample[lowBounded] = NpRandom.exponential(1.0, (lowBounded[lowBounded] as ndarray).shape) + Low[lowBounded];
        sample[uppBounded] = -NpRandom.exponential(1.0, (uppBounded[uppBounded] as ndarray).shape) + High[uppBounded];
        sample[bounded] = NpRandom.uniform(Low[bounded], High[bounded], (bounded[bounded] as ndarray).shape);
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
        var a = array <= High;
        if (!np.allb(array <= High))
            return Result.Fail(new string[] { array.ToString(), "Value must be less than or equal to high." });
        if (!np.allb(array >= Low))
            return Result.Fail(new string[] { array.ToString(), "Value must be greater than or equal to low." });
        return Result.Ok();
    }

    private static void CheckInitParameter(ndarray low, ndarray high, shape shape, dtype type)
    {
        ArgumentNullException.ThrowIfNull(low);
        ArgumentNullException.ThrowIfNull(high);
        if (!CheckType(type))
            throw new ArgumentException("Box only supports numeric types, but not support Decimal type.");
        if (low.shape != shape)
            throw new ArgumentException("The low array does not match the shape defined by the condition.");
        if (high.shape != shape)
            throw new ArgumentException("The high array does not match the shape defined by the condition.");
        if (low.Dtype != type)
            throw new ArgumentException("The low array does not match the type defined by the condition.");
        if (high.Dtype != type)
            throw new ArgumentException("The high array does not match the type defined by the condition.");
    }

    private static bool CheckType(dtype type)
        => type == np.Int8 || type == np.Int16 || type == np.Int32 || type == np.Int64 ||
        type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64 ||
        type == np.Float32 || type == np.Float64;

    private void CoculateBounded()
    {
        BoundedBelow = Low > double.NegativeInfinity;
        BoundedAbove = High < double.PositiveInfinity;
    }
}
