using Newtonsoft.Json.Bson;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RL.Env.Tests.Spaces;

[TestClass]
public class BoxTests
{
    public static ICollection<object[]> SupportType
        => new[]
        {
            new[] { np.Int8 }, new[] { np.Int16 }, new[] { np.Int32 }, new[] { np.Int64 },
            new[] { np.UInt8 }, new[] { np.UInt16 }, new[] { np.UInt32 }, new[] { np.UInt64 },
            new[] { np.Float32 }, new[] { np.Float64 }
        };

    public static ICollection<object[]> NotSupportType
        => new[]
        {
            new[] { np.Strings },
            new[] { np.Object },
            new[] { np.Bool },
            new[] { np.Complex },
            new[] { np.Decimal }
        };

    [DynamicData(nameof(SupportType))]
    [TestMethod]
    public void TestCheckInitParameterCheckSupportType(dtype type)
    {
        Box _ = new(0, 1, new(2, 3), type);
    }

    [ExpectedException(typeof(ArgumentException), "Box only supports numeric types, but not support Decimal type.")]
    [DynamicData(nameof(NotSupportType))]
    [TestMethod]
    public void TestCheckInitParameterCheckNotSupportType(dtype type)
    {
        Box _ = new(0, 1, new(2, 3), type);
    }

    [ExpectedException(typeof(ArgumentException), "Low must be less than high.")]
    [TestMethod]
    public void TestCheckLowGreaterOrEqualHigh()
    {
        Box _ = new(2, 1, new(2, 3), np.Int32);
    }

    private Box box = null!;
    private static readonly dtype type = np.Float32;

    [TestInitialize]
    public void Init()
        => box = new(np.array(new int[,] { { 0, 1, 2 }, { 2, 1, 0 } }, type), np.array(new int[,] { { 7, 8, 9 }, { 9, 8, 7 } }, type), new(2, 3), type);

    [TestMethod]
    public void TestSample()
    {
        for (int i = 0; i < 100; i++)
        {
            Result result = box.CheckConditions(box.Sample());
            Assert.IsTrue(result.IsSuccess);
        }
    }

    public static ICollection<object[]> TestFlatDimData
        => new[]
        {
            new object[] { new Box(0.0, 1.0), 1 },
            new object[] { new Box(0, np.Inf, new shape(2, 2)), 4 },
            new object[] { new Box(np.array(new float[] { -10.0f, 0.0f }), np.array(new float[] { 10.0f, 10.0f })), 2 },
            new object[] { new Box(-np.Inf, 0.0, new shape(2, 1)), 2 },
            new object[] { new Box(0, np.Inf, new shape(2, 1)), 2 },
        };
    [DynamicData(nameof(TestFlatDimData))]
    [TestMethod]
    public void TestFlatDim(Space space, int flatDim)
    {
        Assert.AreEqual(space.FlatDim(), flatDim);
    }

    [DynamicData(nameof(TestFlatDimData))]
    [TestMethod]
    public void TestSystemTextJsonSerialization(DigitalSpace space, int _)
    {
        string jsonString = JsonSerializer.Serialize(space, Env.Utils.Serialization.Options.SystemTextJsonSerializerOptions);
        Box? box = JsonSerializer.Deserialize<Space>(jsonString, Env.Utils.Serialization.Options.SystemTextJsonSerializerOptions) as Box;

        Assert.IsNotNull(box);
        Assert.IsTrue(space == box);
        Assert.IsFalse(space != box);
        Assert.AreEqual(space, box);
        Assert.IsTrue(space.Type == box.Type);
        Assert.IsTrue(space.NpRandom.randn() == box.NpRandom.randn());
        Assert.IsTrue(space.NpRandom.randn() == box.NpRandom.randn());
        Assert.IsTrue(space.Shape == box.Shape);
        Assert.IsTrue(np.array_equal(space.High, box.High));
        Assert.IsTrue(np.array_equal(space.Low, box.Low));
        Assert.IsTrue(np.array_equal(space.BoundedBelow, box.BoundedBelow));
        Assert.IsTrue(np.array_equal(space.BoundedAbove, box.BoundedAbove));
    }

    [DynamicData(nameof(TestFlatDimData))]
    [TestMethod]
    public void TestNewtonsoftSerialization(DigitalSpace space, int _)
    {
        Newtonsoft.Json.JsonSerializer jsonSerializer = Newtonsoft.Json.JsonSerializer.Create(Env.Utils.Serialization.Options.NewtonsoftSerializerOptions);
        using MemoryStream ms = new();
        using BsonDataWriter writer = new(ms);
        jsonSerializer.Serialize(writer, space);
        ms.Position = 0;
        using BsonDataReader reader = new(ms);
        Box? box = jsonSerializer.Deserialize<Space>(reader) as Box;

        Assert.IsNotNull(box);
        Assert.IsTrue(space == box);
        Assert.IsFalse(space != box);
        Assert.AreEqual(space, box);
        Assert.IsTrue(space.Type == box.Type);
        Assert.IsTrue(space.NpRandom.randn() == box.NpRandom.randn());
        Assert.IsTrue(space.NpRandom.randn() == box.NpRandom.randn());
        Assert.IsTrue(space.Shape == box.Shape);
        Assert.IsTrue(np.array_equal(space.High, box.High));
        Assert.IsTrue(np.array_equal(space.Low, box.Low));
        Assert.IsTrue(np.array_equal(space.BoundedBelow, box.BoundedBelow));
        Assert.IsTrue(np.array_equal(space.BoundedAbove, box.BoundedAbove));
    }
}
