using AutoVac.Clientship;
using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;
using AutoVac.VacDynamics.Execution;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static AutoVac.Clientship.Spatial.GroundSection;

namespace AutoVac.VacDynamics.Tests;

[TestClass]
public sealed class BackOffStrategyTests
{
  [TestMethod]
  public void AutoVacIsStuck__EndsExecutionAfterStrategyExhaustion()
  {
    var groundPlan = new GroundSection[][]
    {
      new [] { Wall, Wall, Wall },
      new [] { Wall, Space, Wall },
      new [] { Wall, Wall, Wall },
    };

    ExecutionCentral executor = new (groundPlan);

    Position position = new (1,1);
    OrientedPosition orientedPosition = new
    (
      position,
      CardinalDirection.West
    );

    ExecutionResult result = executor.Execute(orientedPosition, new InstructionKind[] { InstructionKind.Advance }, 20);

    (Position finalPostion, CardinalDirection finalFacing) = result.Final;

    Assert.AreEqual(1, result.Battery);
    Assert.AreEqual(0, result.Cleaned.Count);
    Assert.AreEqual(1, result.Visited.Count);
    Assert.AreEqual(position, result.Visited[0]);
    Assert.AreEqual(position, finalPostion);
    Assert.AreEqual(CardinalDirection.East, finalFacing);
  }

  [TestMethod]
  public void AutoVacIsStuck__EndsExecutionAfterBatteryExhaustion()
  {
    var groundPlan = new GroundSection[][]
    {
      new [] { Wall, Wall, Wall },
      new [] { Wall, Space, Wall },
      new [] { Wall, Wall, Wall },
    };

    ExecutionCentral executor = new (groundPlan);

    Position position = new (1,1);
    OrientedPosition orientedPosition = new
    (
      position,
      CardinalDirection.West
    );

    ExecutionResult result = executor.Execute(orientedPosition, new InstructionKind[] { InstructionKind.Advance }, 4);

    (Position finalPostion, CardinalDirection finalFacing) = result.Final;

    Assert.AreEqual(1, result.Battery);
    Assert.AreEqual(0, result.Cleaned.Count);
    Assert.AreEqual(1, result.Visited.Count);
    Assert.AreEqual(position, result.Visited[0]);
    Assert.AreEqual(position, finalPostion);
    Assert.AreEqual(CardinalDirection.North, finalFacing);
  }
}