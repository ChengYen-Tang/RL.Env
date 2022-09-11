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
public class Box : DigitalSpace
{
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
        : base(low, high, shape, type, seed) { }

    public Box(ndarray low, ndarray high, shape shape, dtype type, np.random npRandom)
        : base(low, high, shape, type, npRandom) { }

    protected override bool CheckType(dtype type)
        => type == np.Int8 || type == np.Int16 || type == np.Int32 || type == np.Int64 ||
        type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64 ||
        type == np.Float32 || type == np.Float64;
}
