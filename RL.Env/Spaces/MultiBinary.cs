namespace RL.Env.Spaces;

/// <summary>
/// Elements of this space are binary arrays of a shape that is fixed during construction.
/// 
/// Example Usage::
/// 
/// >>> observation_space = MultiBinary(5)
/// >>> observation_space.sample()
///     array([0, 1, 0, 1, 0], dtype=int8
/// >>> observation_space = MultiBinary([3, 2])
/// >>> observation_space.sample()
///     array([[0, 0],
///         [0, 1],
///         [1, 1]], dtype=int8)
/// </summary>
public class MultiBinary : DigitalSpace
{
    private static readonly dtype defaultType = np.UInt8;

    public MultiBinary(int shape, uint? seed = null)
        : this(shape, defaultType, seed)
    { }

    public MultiBinary(int shape, np.random npRandom)
        : this(shape, defaultType, npRandom)
    { }

    public MultiBinary(shape shape, uint? seed = null)
        : this(shape, defaultType, seed)
    { }

    public MultiBinary(shape shape, np.random npRandom)
        : this(shape, defaultType, npRandom)
    { }

    public MultiBinary(int shape, dtype type, uint? seed = null)
        : this(new shape(shape), type, seed)
    { }

    public MultiBinary(int shape, dtype type, np.random npRandom)
        : this(new shape(shape), type, npRandom)
    { }

    public MultiBinary(shape shape, dtype type, uint? seed = null)
        : base(np.full(shape, 0, type), np.full(shape, 1, type), shape, type, seed)
    { }

    public MultiBinary(shape shape, dtype type, np.random npRandom)
        : base(np.full(shape, 0, type), np.full(shape, 1, type), shape, type, npRandom)
    { }

    protected override Result CheckType(dtype type)
    {
        if (type == np.Int8 || type == np.Int16 || type == np.Int32 || type == np.Int64 ||
            type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64)
            return Result.Ok();
        return Result.Fail("MultiBinary only supports int and uint types.");
    }
}
