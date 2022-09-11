namespace BaseRLEnv.Spaces;

/// <summary>
/// It is useful to represent game controllers or keyboards where each key can be represented as a discrete action space.
/// 
/// Example::
/// >> d = MultiDiscrete(np.array([[1, 2], [3, 4]]))
/// >> d.sample()
/// array([[0, 0],
///     [2, 3]])
/// </summary>
public class MultiDiscrete : DigitalSpace
{
    private static readonly dtype defaultType = np.Int32;

    public MultiDiscrete(ndarray number, uint? seed = null)
    : base(np.full(number.shape, 0, defaultType), number - 1, number.shape, defaultType, seed)
    { }

    public MultiDiscrete(ndarray number, np.random npRandom)
        : base(np.full(number.shape, 0, defaultType), number - 1, number.shape, defaultType, npRandom)
    { }

    public MultiDiscrete(ndarray number, dtype type, uint? seed = null)
        : base(np.full(number.shape, 0, type), number - 1, number.shape, type, seed)
    { }

    public MultiDiscrete(ndarray number, dtype type, np.random npRandom)
        : base(np.full(number.shape, 0, type), number - 1, number.shape, type, npRandom)
    { }

    protected override Result CheckType(dtype type)
    {
        if (type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64)
            return Result.Ok();
        return Result.Fail("MultiDiscrete only supports uint types.");
    }
}
