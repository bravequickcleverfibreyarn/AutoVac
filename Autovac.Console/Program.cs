using Autovac.Console.InputModel.Conversion;
using Autovac.Console.InputModel.Serialization;
using Autovac.Console.InputModel.Validation;

using AutoVac.Clientship;
using AutoVac.VacDynamics.Execution;

if(args is null || args.Length != 2)
{
  Console.WriteLine("Try again with parameters <sourcepath|JSON> <resultpath|JSON>!");
  return;
}

string inputFileName = args[0];
if(!File.Exists(inputFileName))
{
  Console.WriteLine($"Source file {inputFileName} does not exist!");
  return;
}

string? resultFileName = args[1];
try
{
  File.Create(resultFileName).Dispose();
}
catch(Exception e)
{
  Console.WriteLine($"Cannot create result file {resultFileName}! {e.Message}");
}

string input;
try
{
  input = File.ReadAllText(inputFileName);
}
catch(Exception e)
{
  Console.WriteLine($"Cannot process input file {inputFileName}! {e.Message}");
  return;
}

RawExecutionSettings? rawSettings;
try
{
  rawSettings = InputOutputSerializer.Deserialize(input);
}
catch(Exception e)
{
  Console.WriteLine($"Cannot parse input file {inputFileName}! {e.Message}");
  return;
}

if(rawSettings is null)
{
  Console.WriteLine($"Error parsing input file {inputFileName}!");
  return;
}

try
{
  new ExecutionSettingsValidator().Validate(rawSettings);
}
catch(ValidationException e)
{
  Console.WriteLine($"Invalid data of {e.ValueName}! {e.Message}");
  return;
}

FineExecutionSettings fineSettings = new SettingsConvertor().Convert(rawSettings);

ExecutionCentral executor = new (fineSettings.GroundPlan);

//bool executing = true;
//ExecutionResult result = null!;

ExecutionResult result = executor.Execute
(
  fineSettings.Position,
  fineSettings.InstructionSequence,
  fineSettings.BatteryLevel
);

////use this to indicate processing when diagnostics do not log to console
//_ = Task.Run
//(
// () =>
// {
//   result = executor.Execute
//   (
//     fineSettings.Position,
//     fineSettings.InstructionSequence,
//     fineSettings.BatteryLevel
//   );
// }
//)
//.ContinueWith(t => Volatile.Write(ref executing, false));

//while(Volatile.Read(ref executing))
//{
//  for(int i = 0;i < 15;++i)
//  {
//    Console.Write(". ");
//    await Task.Delay(100);
//  }
//  Console.WriteLine();
//}

string output;
try
{
  output = InputOutputSerializer.Serialize(result);
}
catch(Exception e)
{
  Console.WriteLine($"Error during result serialization! {e.Message}");
  return;
}

try
{
  File.WriteAllText(path: resultFileName, contents: output);
}
catch(Exception e)
{
  Console.WriteLine($"Error during writing file {resultFileName}! {e.Message}");
}
