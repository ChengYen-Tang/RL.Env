using System.Text.Json;

namespace RL.Env.Tests.Spaces;

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
        MultiDiscrete _ = new(np.array(new uint[,] { { 1, 2, 3 }, { 4, 5, 6 } }, type), type);
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

    public static ICollection<object[]> TestFlatDimData
        => new[]
        {
            new object[] { new MultiDiscrete(np.array(new uint[] { 2, 2 }, np.UInt32)), 4 },
            new object[] { new MultiDiscrete(np.array(new uint[,] { { 2, 3 }, { 3, 2 } }, np.UInt32)), 10 },
        };
    [DynamicData(nameof(TestFlatDimData))]
    [TestMethod]
    public void TestFlatDim(Space space, int flatDim)
    {
        Assert.AreEqual(space.FlatDim(), flatDim);
    }

    [DynamicData(nameof(TestFlatDimData))]
    [TestMethod]
    public void TestSerialization(DigitalSpace space, int _)
    {
        string jsonString = JsonSerializer.Serialize(space, Env.Utils.Serialization.Options.SerializerOptions);
        MultiDiscrete? multiDiscrete = JsonSerializer.Deserialize<Space>(jsonString, Env.Utils.Serialization.Options.SerializerOptions) as MultiDiscrete;

        Assert.IsNotNull(multiDiscrete);
        Assert.IsTrue(space.Type == multiDiscrete.Type);
        Assert.IsTrue(space.NpRandom.randn() == multiDiscrete.NpRandom.randn());
        Assert.IsTrue(space.NpRandom.randn() == multiDiscrete.NpRandom.randn());
        Assert.IsTrue(space.Shape == multiDiscrete.Shape);
        Assert.IsTrue(np.array_equal((space as MultiDiscrete)!.Nvec, multiDiscrete!.Nvec));
        Assert.IsTrue(np.array_equal(space.High, multiDiscrete.High));
        Assert.IsTrue(np.array_equal(space.Low, multiDiscrete.Low));
        Assert.IsTrue(np.array_equal(space.BoundedBelow, multiDiscrete.BoundedBelow));
        Assert.IsTrue(np.array_equal(space.BoundedAbove, multiDiscrete.BoundedAbove));
    }
}
