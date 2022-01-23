namespace AutoVac.Clientship.InstructionSet;

public enum InstructionKind : sbyte
{
  TurnRight   = 2,
  TurnLeft    = -2,
  Advance     = 1,
  Back        = -1,
  Clean       = 0,
}
