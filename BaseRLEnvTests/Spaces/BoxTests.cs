﻿namespace BaseRLEnvTests.Spaces;

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

    [DataRow(1, 1)]
    [DataRow(2, 1)]
    [ExpectedException(typeof(ArgumentException), "Low must be less than high.")]
    [TestMethod]
    public void TestCheckLowGreaterOrEqualHigh(double low, double high)
    {
        Box _ = new(low, high, new(2, 3), np.Int32);
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
}
