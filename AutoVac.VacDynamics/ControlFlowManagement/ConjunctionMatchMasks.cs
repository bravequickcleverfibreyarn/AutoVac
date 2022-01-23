namespace AutoVac.VacDynamics.ControlFlowManagement;

/// <summary>
/// Mask constans used in logical conjuction (AND) operations. 
/// </summary>
internal static class ConjunctionMatchMasks
{
  public static class InstructionKind
  {
    public const int TurnCommandMask = 3;
    public const int TurnCommandMaskResult = 2;
  }

  public static class CardinalDirection
  {
    public const int YAxisMask = 5;
    public const int YAxisMaskResult = 0;
  }
}
