using Newtonsoft.Json.Bson;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
    public void TestSystemTextJsonSerialization(DigitalSpace space, int _)
    {
        string jsonString = JsonSerializer.Serialize(space, Env.Utils.Serialization.Options.SystemTextJsonSerializerOptions);
        MultiBinary? multiBinary = JsonSerializer.Deserialize<Space>(jsonString, Env.Utils.Serialization.Options.SystemTextJsonSerializerOptions) as MultiBinary;

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
        MultiBinary? multiBinary = jsonSerializer.Deserialize<Space>(reader) as MultiBinary;

        Assert.IsNotNull(multiBinary);
        Assert.IsTrue(space == multiBinary);
        Assert.IsFalse(space != multiBinary);
        Assert.AreEqual(space, multiBinary);
        Assert.IsTrue(space.Type == multiBinary.Type);
        Assert.IsTrue(space.NpRandom.randn() == multiBinary.NpRandom.randn());
        Assert.IsTrue(space.NpRandom.randn() == multiBinary.NpRandom.randn());
        Assert.IsTrue(space.Shape == multiBinary.Shape);
        Assert.AreEqual((space as MultiBinary)!.N, multiBinary.N);
        Assert.IsTrue(np.array_equal(space.High, multiBinary.High));
        Assert.IsTrue(np.array_equal(space.Low, multiBinary.Low));
        Assert.IsTrue(np.array_equal(space.BoundedBelow, multiBinary.BoundedBelow));
        Assert.IsTrue(np.array_equal(space.BoundedAbove, multiBinary.BoundedAbove));
    }

    [TestMethod]
    public void TestReshape()
    {
        ndarray array = np.array(new int[,] { { 1, 0, 1 }, { 0, 1, 0 } });
        MultiBinary multiBinary = new(array.shape);
        ndarray flattenArray = MultiBinary.Flatten(array);
        Assert.AreEqual(1, flattenArray.shape.iDims.Length);
        Assert.AreEqual(6, flattenArray.shape[0]);
        ndarray array1 = multiBinary.ToMultiBinaryShape(flattenArray);
        Assert.IsTrue(np.array_equal(array, array1));
    }
}
