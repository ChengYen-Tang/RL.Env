using System.Text.Json;

namespace RL.Env.Tests.Spaces;

[TestClass]
public class MultiBinaryTests
{
    public static ICollection<object[]> SupportType
        => new[]
        {
            new[] { np.Int8 }, new[] { np.Int16 }, new[] { np.Int32 }, new[] { np.Int64 },
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
            new[] { np.Float32 }, new[] { np.Float64 }
        };

    [DynamicData(nameof(SupportType))]
    [TestMethod]
    public void TestCheckInitParameterCheckSupportType(dtype type)
    {
        MultiBinary _ = new(new shape(2, 3), type);
    }

    [ExpectedException(typeof(ArgumentException), "MultiBinary only supports int and uint types.")]
    [DynamicData(nameof(NotSupportType))]
    [TestMethod]
    public void TestCheckInitParameterCheckNotSupportType(dtype type)
    {
        MultiBinary _ = new(new shape(2, 3), type);
    }

    private MultiBinary multiBinary = null!;

    [TestInitialize]
    public void Init()
        => multiBinary = new(new shape(2, 3));

    [TestMethod]
    public void TestSample()
    {
        for (int i = 0; i < 100; i++)
        {
            Result result = multiBinary.CheckConditions(multiBinary.Sample());
            Assert.IsTrue(result.IsSuccess);
        }
    }

    public static ICollection<object[]> TestFlatDimData
        => new[]
        {
            new object[] { new MultiBinary(8), 8 },
            new object[] { new MultiBinary(new shape(2, 3)), 6 },
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
        MultiBinary? multiBinary = JsonSerializer.Deserialize<Space>(jsonString, Env.Utils.Serialization.Options.SerializerOptions) as MultiBinary;

        Assert.IsNotNull(multiBinary);
        Assert.IsTrue(space == multiBinary);
        Assert.IsFalse(space != multiBinary);
        Assert.AreEqual(space, multiBinary);
        Assert.IsTrue(space.Type == multiBinary.Type);
        Assert.IsTrue(space.NpRandom.randn() == multiBinary.NpRandom.randn());
        Assert.IsTrue(space.NpRandom.randn() == multiBinary.NpRandom.randn());
        Assert.IsTrue(space.Shape == multiBinary.Shape);
        Assert.IsTrue(np.array_equal(space.High, multiBinary.High));
        Assert.IsTrue(np.array_equal(space.Low, multiBinary.Low));
        Assert.IsTrue(np.array_equal(space.BoundedBelow, multiBinary.BoundedBelow));
        Assert.IsTrue(np.array_equal(space.BoundedAbove, multiBinary.BoundedAbove));
    }
}
