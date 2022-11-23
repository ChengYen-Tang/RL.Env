namespace RL.Env.Spaces;

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
public class Box : DigitalSpace
{
    /// <summary>
    /// Checks whether this space can be flattened to a :class:`spaces.Box`.
    /// </summary>
    public override bool IsNpFlattenable => true;

    public Box(double low, double high, shape shape, uint? seed = null)
    : this(low, high, shape, np.Float64, seed)
    { }

    public Box(double low, double high, shape shape, np.random npRandom)
        : this(low, high, shape, np.Float64, npRandom)
    { }

    public Box(double low, double high, uint? seed = null)
        : this(low, high, new(1), np.Float64, seed)
    { }

    public Box(double low, double high, np.random npRandom)
        : this(low, high, new(1), np.Float64, npRandom)
    { }

    public Box(double low, double high, shape shape, dtype type, uint? seed = null)
        : this(np.full(shape, low, type), np.full(shape, high, type), shape, type, seed)
    { }

    public Box(double low, double high, shape shape, dtype type, np.random npRandom)
        : this(np.full(shape, low, type), np.full(shape, high, type), shape, type, npRandom)
    { }

    public Box(ndarray low, ndarray high, shape shape, uint? seed = null)
        : this(low, high, shape, low.Dtype, seed)
    => Box.CheckBounds(low, high);

    public Box(ndarray low, ndarray high, shape shape, np.random npRandom)
        : this(low, high, shape, low.Dtype, npRandom)
    => Box.CheckBounds(low, high);

    public Box(ndarray low, ndarray high, uint? seed = null)
        : this(low, high, low.shape, low.Dtype, seed)
    => Box.CheckBounds(low, high);

    public Box(ndarray low, ndarray high, np.random npRandom)
        : this(low, high, low.shape, low.Dtype, npRandom)
    => Box.CheckBounds(low, high);

    public Box(ndarray low, ndarray high, shape shape, dtype type, uint? seed = null)
        : base(low, high, shape, type, seed) { }

    public Box(ndarray low, ndarray high, shape shape, dtype type, np.random npRandom)
        : base(low, high, shape, type, npRandom) { }

    protected override Result CheckType(dtype type)
    {
        if (type == np.Int8 || type == np.Int16 || type == np.Int32 || type == np.Int64 ||
            type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64 ||
            type == np.Float32 || type == np.Float64)
            return Result.Ok();
        return Result.Fail("Box only supports numeric types, but not support Decimal type.");
    }

    private static void CheckBounds(ndarray low, ndarray high)
    {
        if (low.shape != high.shape)
            throw new ArgumentException("low and high must have the same shape.");
        if (low.Dtype != high.Dtype)
            throw new ArgumentException("low and high must have the same dtype.");
    }

    public override long FlatDim()
        => Shape.iDims.Aggregate((total, next) => total * next);
}
