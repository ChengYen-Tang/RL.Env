---
description: >-
  Other doc contents will be added after the reinforcement learning algorithm is
  completed.
---

# Introduction

RLEnv is an open source dotnet library for developing and comparing reinforcement learning algorithms by providing a standard API to communicate between learning algorithms and environments, as well a standard set of environments compliant with that API.

### nuget

The project must be a .Net 6 or above project.

{% embed url="https://www.nuget.org/packages/BaseRLEnv" %}

### Example

```csharp
    public class MockEnv : BaseEnv<DigitalSpace>
    {
        public MockEnv()
        {
            ActionSpace = new Discrete(3, -1);
            ObservationSpace = new Box(2, 6, new shape(2, 3), np.Float32);
            RewardRange = new(5, 2); // If not set, default value is (inf, -inf)
        }

        public override ndarray? Render(RanderMode randerMode)
            => throw new NotImplementedException();

        public override ResetResult Reset(uint? seed = null, Dictionary<string, dynamic>? options = null)
            => new(ObservationSpace.Sample(), new());

        public override StepResult Step(ndarray action)
            => new(ObservationSpace.Sample(), 10, false, false, new());
    }
```
