namespace BaseRLEnvTests.Spaces;

[TestClass]
public class MultiDiscreteTests
{
    public static ICollection<object[]> SupportType
    => new[]
    {
            new[] { np.UInt8 }, new[] { np.UInt16 }, new[] { np.UInt32 }, new[] { np.UInt64 }
    };

    public static ICollection<object[]> NotSupportType
        => new[]
        {
            new[] { np.Strings },
            new[] { np.Object },
            new[] { np.Bool },
            new[] { np.Complex },
            new[] { np.Decimal },
            new[] { np.Int8 }, new[] { np.Int16 }, new[] { np.Int32 }, new[] { np.Int64 },
            new[] { np.Float32 }, new[] { np.Float64 }
        };

    [DynamicData(nameof(SupportType))]
    [TestMethod]
    public void TestCheckInitParameterCheckSupportType(dtype type)
    {
        MultiDiscrete _ = new(np.array(new uint[,] { { 1, 2, 3 }, { 4, 5, 6 } }, type), type);
    }

    [ExpectedException(typeof(ArgumentException), "MultiDiscrete only supports uint types.")]
    [DynamicData(nameof(NotSupportType))]
    [TestMethod]
    public void TestCheckInitParameterCheckNotSupportType(dtype type)
    {
        MultiDiscrete _ = new(np.array(new uint[,] { { 1, 2, 3 }, { 4, 5, 6 } }, type),  type);
    }

    private MultiDiscrete multiDiscrete = null!;
    private static readonly dtype type = np.UInt32;

    [TestInitialize]
    public void Init()
        => multiDiscrete = new(np.array(new uint[,] { { 1, 2, 3 }, { 4, 5, 6 } }, type), type);

    [TestMethod]
    public void TestSample()
    {
        for (int i = 0; i < 100; i++)
        {
            Result result = multiDiscrete.CheckConditions(multiDiscrete.Sample());
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
