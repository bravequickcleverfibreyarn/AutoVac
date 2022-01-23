using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;
using AutoVac.VacDynamics.ControlFlowManagement;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoVac.VacDynamics.Tests;

[TestClass]
public sealed class LogicalConjuctionMasksTests
{

  [TestMethod]
  public void InstructionKind_TurnCommandMaskTests()
  {
    const int result = ConjunctionMatchMasks.InstructionKind.TurnCommandMaskResult;

    IReadOnlyCollection<int> expectedResults          = new[] { result, result, 1, 3, 0 };
    IReadOnlyCollection<InstructionKind> instructions = new[]
    {
      InstructionKind.TurnLeft,
      InstructionKind.TurnRight,
      InstructionKind.Advance,
      InstructionKind.Back,
      InstructionKind.Clean
    };

    IEnumerable<int> results = instructions.Select(x => (int) x & ConjunctionMatchMasks.InstructionKind.TurnCommandMask);

    Assert.IsTrue(expectedResults.SequenceEqual(results));

    // safe check for new instructions
    Assert.AreEqual(instructions.Count, Enum.GetValues<InstructionKind>().Length);
  }

  [TestMethod]
  public void CardinalDirection_YAxisMaskTests()
  {
    const int result = ConjunctionMatchMasks.CardinalDirection.YAxisMaskResult;

    IReadOnlyCollection<int> expectedResults          = new[] { result, result, 1, 1 };
    IReadOnlyCollection<CardinalDirection> directions = new[]
    {
      CardinalDirection.North,
      CardinalDirection.South,
      CardinalDirection.West,
      CardinalDirection.East
    };

    IEnumerable<int> results = directions.Select(x => (int) x & ConjunctionMatchMasks.CardinalDirection.YAxisMask);

    Assert.IsTrue(expectedResults.SequenceEqual(results));

    // safe check for new directions
    Assert.AreEqual(directions.Count, Enum.GetValues<CardinalDirection>().Length);
  }
}
