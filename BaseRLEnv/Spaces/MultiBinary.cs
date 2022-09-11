﻿namespace BaseRLEnv.Spaces;

public class MultiBinary : DigitalSpace
{
    private static readonly dtype defaultType = np.UInt8;

    public MultiBinary(shape shape, uint? seed = null)
    : base(np.full(shape, 0, defaultType), np.full(shape, 1, defaultType), shape, defaultType, seed)
    { }

    public MultiBinary(shape shape, np.random npRandom)
    : base(np.full(shape, 0, defaultType), np.full(shape, 1, defaultType), shape, defaultType, npRandom)
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