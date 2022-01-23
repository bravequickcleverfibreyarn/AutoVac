using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;
using AutoVac.VacDynamics.Execution;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace AutoVac.VacDynamics.Tests;

[TestClass]
public sealed class InputsValidationTests
{

  // instructions

  [TestMethod]
  [DataRow(true)]
  [DataRow(false)]
  public void InstructionSequenceTetst(bool nullSequence)
  {
    InstructionKind[]? sequence = nullSequence ? null : new InstructionKind[0];

    var groundPlan = new GroundSection[][] { new [] { GroundSection.Space } };
    ExecutionCentral executor = new (groundPlan);

    Action test = () => executor.Execute(default(OrientedPosition), sequence!, default(int));
    if(nullSequence)
      _ = Assert.ThrowsException<ArgumentNullException>(test);
    else
      test();
  }

  [TestMethod]
  [DataRow(default(InstructionKind), false)]
  [DataRow((InstructionKind) sbyte.MaxValue, true)]
  public void InstructionTests(InstructionKind instruction, bool throws)
  {
    var groundPlan = new GroundSection[][] { new [] { GroundSection.Space } };
    ExecutionCentral executor = new (groundPlan);

    Action test = () => executor.Execute(default(OrientedPosition), new[] { instruction }, default(int));
    if(throws)
      _ = Assert.ThrowsException<ArgumentException>(test);
    else
      test();
  }

  // ground sections

  [TestMethod]
  [DataRow(true)]
  [DataRow(false)]
  public void GroundPlanTests(bool nullPlan)
  {
    GroundSection[][]? plan = nullPlan ? null : new GroundSection[0][];

#pragma warning disable CA1806 // Do not ignore method results
    Action test = () => new ExecutionCentral(plan!);
#pragma warning restore CA1806 // Do not ignore method results
    if(nullPlan)
      _ = Assert.ThrowsException<ArgumentNullException>(test);
    else
      test();
  }

  [TestMethod]
  [DataRow(true)]
  [DataRow(false)]
  public void GroundPlanRowTests(bool nullRow)
  {
    GroundSection[]? row = nullRow ? null : new GroundSection[0];

    var groundPlan = new GroundSection[][] { row! };

#pragma warning disable CA1806 // Do not ignore method results
    Action test = () => new ExecutionCentral(groundPlan);
#pragma warning restore CA1806 // Do not ignore method results

    if(nullRow)
      _ = Assert.ThrowsException<ArgumentException>(test);
    else
      test();
  }

  [TestMethod]
  [DataRow(default(GroundSection), false)]
  [DataRow((GroundSection) byte.MaxValue, true)]
  public void GroundSectionTests(GroundSection section, bool throws)
  {
    var groundPlan = new GroundSection[][] { new[] { section } };

#pragma warning disable CA1806 // Do not ignore method results
    Action test = () => new ExecutionCentral(groundPlan);
#pragma warning restore CA1806 // Do not ignore method results

    if(throws)
      _ = Assert.ThrowsException<ArgumentException>(test);
    else
      test();
  }

  // oriented position

  [TestMethod]
  [DataRow(default(CardinalDirection), false)]
  [DataRow((CardinalDirection) byte.MaxValue, true)]
  public void CardinalDirectionTests(CardinalDirection direction, bool throws)
  {
    var groundPlan = new GroundSection[][] { new [] { GroundSection.Space } };
    ExecutionCentral executor = new (groundPlan);

    OrientedPosition orientedPosition = new
    (
      default(Position),
      direction
    );

    Action test = () => executor.Execute(orientedPosition, new InstructionKind[0], default(int));
    if(throws)
      _ = Assert.ThrowsException<ArgumentException>(test);
    else
      test();
  }


  [TestMethod]
  [DataRow(0, 1, true)]
  [DataRow(1, 0, true)]
  [DataRow(0, 0, false)]
  public void PositionTests(int x, int y, bool throws)
  {
    var groundPlan = new GroundSection[][] { new [] { GroundSection.Space } };
    ExecutionCentral executor = new (groundPlan);

    OrientedPosition orientedPosition = new
    (
      new Position((ushort)x,(ushort)y),
      default(CardinalDirection)
    );

    Action test = () => executor.Execute(orientedPosition, new InstructionKind[0], default(int));
    if(throws)
      _ = Assert.ThrowsException<ArgumentException>(test);
    else
      test();
  }
}
