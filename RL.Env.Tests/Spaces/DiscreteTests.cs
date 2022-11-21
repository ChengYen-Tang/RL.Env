﻿namespace BaseRLEnvTests.Spaces;

[TestClass]
public class DiscreteTests
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
        Discrete _ = new(2, type);
    }

    [ExpectedException(typeof(ArgumentException), "Discrete only supports numeric types, but not support Decimal type.")]
    [DynamicData(nameof(NotSupportType))]
    [TestMethod]
    public void TestCheckInitParameterCheckNotSupportType(dtype type)
    {
        Discrete _ = new(2, type);
    }

    private Discrete discrete = null!;
    private static readonly dtype type = np.Int32;

    [TestInitialize]
    public void Init()
        => discrete = new(2, type);

    [TestMethod]
    public void TestSample()
    {
        for (int i = 0; i < 100; i++)
        {
            Result result = discrete.CheckConditions(discrete.Sample());
            Assert.IsTrue(result.IsSuccess);
        }
    }
}