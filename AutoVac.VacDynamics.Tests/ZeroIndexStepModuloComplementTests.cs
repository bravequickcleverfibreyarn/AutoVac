using AutoVac.Clientship.Spatial;
using AutoVac.VacDynamics.ControlFlowManagement;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace AutoVac.VacDynamics.Tests;

[TestClass]
public sealed class ZeroIndexStepModuloComplementTests
{
  [TestMethod]
  public void CardinalDirectionAxisSidesAreComplements()
  {
    const int expectedDirectionsCount = 4;
    Assert.AreEqual(expectedDirectionsCount, Enum.GetValues<CardinalDirection>().Length);

    ZeroIndexStepModuloComplement complement = new (2, expectedDirectionsCount);

    Assert.AreEqual(CardinalDirection.North, Complement(CardinalDirection.South));
    Assert.AreEqual(CardinalDirection.East, Complement(CardinalDirection.West));
    Assert.AreEqual(CardinalDirection.South, Complement(CardinalDirection.North));
    Assert.AreEqual(CardinalDirection.West, Complement(CardinalDirection.East));

    CardinalDirection Complement(CardinalDirection some) => (CardinalDirection) complement.GetComplement((int) some);
  }

}
