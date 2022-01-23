using AutoVac.Clientship.InstructionSet;

using System.Diagnostics;

namespace AutoVac.VacDynamics.DiagnosticLogging;

internal static class DiagnosticLogger
{

  [Conditional("DEBUG")]
  public static void LogInstruction(InstructionKind ik) => Console.WriteLine(ik.ToString());

  [Conditional("DEBUG")]
  public static void LogBackOffInstruction(InstructionKind ik) => Console.WriteLine("Back off: " + ik.ToString());

  [Conditional("DEBUG")]
  public static void Log(IReadOnlyCollection<InstructionKind> backOffSequence)
    => Console.WriteLine($"Initiating backoff sequence {string.Join(", ", backOffSequence.Select(x => x.ToString()))}.");
}
