namespace BaseRLEnvTests.Spaces;

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

    [ExpectedException(typeof(ArgumentException), "The low array does not match the shape defined by the condition.")]
    [TestMethod]
    public void TestCheckInitParameterCheckLowShape()
    {
        dtype type = np.Int32;
        shape shape = new(2, 3);
        Box _ = new(np.zeros(new shape(1, 2), type), np.ones(shape, type), shape, type);
    }

    [ExpectedException(typeof(ArgumentException), "The low array does not match the type defined by the condition.")]
    [TestMethod]
    public void TestCheckInitParameterCheckLowType()
    {
        dtype type = np.Int32;
        shape shape = new(2, 3);
        Box _ = new(np.zeros(shape, np.Float32), np.ones(shape, type), shape, type);
    }

    [ExpectedException(typeof(ArgumentException), "The high array does not match the shape defined by the condition.")]
    [TestMethod]
    public void TestCheckInitParameterCheckHighShape()
    {
        dtype type = np.Int32;
        shape shape = new(2, 3);
        Box _ = new(np.zeros(shape, type), np.ones(new shape(1, 2), type), shape, type);
    }

    [ExpectedException(typeof(ArgumentException), "The high array does not match the type defined by the condition.")]
    [TestMethod]
    public void TestCheckInitParameterCheckHighType()
    {
        dtype type = np.Int32;
        shape shape = new(2, 3);
        Box _ = new(np.zeros(shape, type), np.ones(shape, np.Float32), shape, type);
    }

    [DataRow(1, 1)]
    [DataRow(2, 1)]
    [ExpectedException(typeof(ArgumentException), "Low must be less than high.")]
    [TestMethod]
    public void TestCheckLowGreaterOrEqualHigh(double low, double high)
    {
        Box _ = new(low, high, new(2, 3), np.Int32);
    }

    private Box box = null;
    private static dtype type = np.Float32;

    [TestInitialize]
    public void Init()
        => box = new(np.array(new int[,] { { 0, 1, 2 }, { 2, 1, 0 } }, type), np.array(new int[,] { { 7, 8, 9 }, { 9, 8, 7 } }, type), new(2, 3), type);

    [TestMethod]
    public void TestCheckConditions()
        => box.CheckConditions(np.array(new int[,] { { 4, 5, 6 }, { 6, 5, 4 } }, type));

    public static ICollection<object[]> ConditionsLessOrEqualDatas
        => new[]
        {
            new object[] { np.array(new int[,] { { 10, 8, 9 }, { 9, 8, 7 } }, type), "Value must be less than or equal to high." },
            new object[] { np.array(new int[,] { { 7, 8, 9 }, { 10, 8, 7 } }, type), "Value must be less than or equal to high." },
            new object[] { np.array(new int[,] { { 7, 10, 9 }, { 9, 10, 7 } }, type), "Value must be less than or equal to high." },
            new object[] { np.array(new int[,] { { 0, 0, 2 }, { 2, 1, 0 } }, type), "Value must be greater than or equal to low." },
            new object[] { np.array(new int[,] { { 0, 1, 2 }, { 0, 1, 0 } }, type), "Value must be greater than or equal to low." },
            new object[] { np.array(new int[,] { { 0, 1, 0 }, { 2, 0, 0 } }, type), "Value must be greater than or equal to low." }
        };
    [DynamicData(nameof(ConditionsLessOrEqualDatas))]
    [TestMethod]
    public void TestCheckConditionsFailed(ndarray array, string errorMessage)
    {
        Result result = box.CheckConditions(array);
        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(errorMessage, result.Errors[1].Message);
    }

    [TestMethod]
    public void TestSample()
    {
        for (int i = 0; i < 100; i++)
        {
            Result result = box.CheckConditions(box.Sample());
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
