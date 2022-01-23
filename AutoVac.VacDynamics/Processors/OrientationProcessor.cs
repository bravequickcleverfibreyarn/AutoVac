using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;

using static AutoVac.Clientship.InstructionSet.InstructionKind;

namespace AutoVac.VacDynamics.Processors;

internal static class OrientationProcessor
{
  public static bool Orientate(ref CardinalDirection orientation, InstructionKind instruction)
  {
    int index = (int) orientation;

    switch(instruction)
    {
      case TurnLeft:
      {
        if(index == 0)
          index = 3;
        else
          index -= 1;
        break;
      }
      case TurnRight:
      {
        if(index == 3)
          index = 0;
        else index += 1;
        break;
      }
      default:
        return false;
    }

    orientation = (CardinalDirection) index;
    return true;
  }
}
