using System.Reflection;

var dayClasses = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.Namespace == "AdventOfCode2024.Days" && t.Name.StartsWith("Day"))
    .OrderByDescending(t => t.Name)
    .ToList();

if (dayClasses.Count != 0)
{
    var latestDayClass = dayClasses.First();
    var solveMethod = latestDayClass.GetMethod("Solve");

    if (solveMethod != null)
    {
        WriteHeader(latestDayClass.Name);

        solveMethod.Invoke(null, null);
    }
    else
    {
        Console.WriteLine("Solve method not found in the latest day class.");
    }
}
else
{
    Console.WriteLine("No day classes found.");
}

return;

void WriteHeader(string latestDayClassName)
{
    var dayNumber = latestDayClassName[3..];
    var header = $"=== Day {dayNumber} ===";
        
    Console.WriteLine(new string('=', header.Length));
    Console.WriteLine(header);
    Console.WriteLine(new string('=', header.Length));
    Console.WriteLine();
}