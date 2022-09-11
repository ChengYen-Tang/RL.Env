namespace BaseRLEnv.Spaces;

public class MultiBinary : DigitalSpace
{
    public MultiBinary(shape shape, uint? seed = null)
    : base(np.full(shape, 0, np.UInt8), np.full(shape, 1, np.UInt8), shape, np.UInt8, seed)
    { }

    public MultiBinary(shape shape, np.random npRandom)
    : base(np.full(shape, 0, np.UInt8), np.full(shape, 1, np.UInt8), shape, np.UInt8, npRandom)
    { }

    public MultiBinary(shape shape, dtype type, uint? seed = null)
        : base(np.full(shape, 0, type), np.full(shape, 1, type), shape, type, seed)
    { }

    public MultiBinary(shape shape, dtype type, np.random npRandom)
    : base(np.full(shape, 0, type), np.full(shape, 1, type), shape, type, npRandom)
    { }

    protected override bool CheckType(dtype type)
        => type == np.Int8 || type == np.Int16 || type == np.Int32 || type == np.Int64 ||
        type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64;
}
