namespace RL.Env.Spaces;

/// <summary>
/// This class represents a finite subset of integers, more specifically a set of the form :math:`\{ a, a+1, \dots, a+n-1 \}`.
/// 
/// Example::
/// 
/// >>> Discrete(2)            # {0, 1}
/// >>> Discrete(3, start=-1)  # {-1, 0, 1}
/// </summary>
public class Discrete : DigitalSpace
{
    private static readonly dtype defaultType = np.Int32;

    public Discrete(long number, long start = 0, uint? seed = null)
        : base(np.full(new shape(1), start, defaultType), np.full(new shape(1), number - 1 + start, defaultType), new(1), defaultType, seed)
    { }

    public Discrete(long number, np.random npRandom, long start = 0)
        : base(np.full(new shape(1), start, defaultType), np.full(new shape(1), number - 1 + start, defaultType), new(1), defaultType, npRandom)
    { }

    public Discrete(long number, dtype type, long start = 0, uint? seed = null)
        : base(np.full(new shape(1), start, type), np.full(new shape(1), number - 1 + start, type), new(1), type, seed)
    { }

    public Discrete(long number, dtype type, np.random npRandom, long start = 0)
        : base(np.full(new shape(1), start, type), np.full(new shape(1), number - 1 + start, type), new(1), type, npRandom)
    { }

    protected override Result CheckType(dtype type)
    {
        if (type == np.Int8 || type == np.Int16 || type == np.Int32 || type == np.Int64 ||
            type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64)
            return Result.Ok();
        return Result.Fail("Discrete only supports int and uint types.");
    }
}
