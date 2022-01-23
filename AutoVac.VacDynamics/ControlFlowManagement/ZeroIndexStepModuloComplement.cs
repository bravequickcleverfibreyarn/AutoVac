namespace AutoVac.VacDynamics.ControlFlowManagement;

/// <remarks>
/// Computes complements for zero-based indexes where item complements lies step size from each other.
/// </remarks>
internal class ZeroIndexStepModuloComplement
{
  private readonly int step;
  private readonly int modulo;

  public ZeroIndexStepModuloComplement(int step, int modulo)
  {
    this.step = step;
    this.modulo = modulo;
  }

  public int GetComplement(int index) => (index + step) % modulo;
}
