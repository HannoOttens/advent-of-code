using System.Diagnostics;
using static System.Math;

int OneToN(int n) {
	return n * (n + 1) / 2;
}

int fuelCost(int[] crabs, int align) {
	return crabs.Aggregate(0, (s, c) => s + OneToN(Abs(c - align)));
}


// Start
var lines = File.ReadAllLines("../input.txt");
var crabs = lines[0].Split(',')
					.Select(n => int.Parse(n))
					.ToArray();

var stopwatch = new Stopwatch();
stopwatch.Start();

int min = crabs.Min();
int max = crabs.Max();
int minFuel = int.MaxValue;
for(int i = min; i < max; i++) {
	int currFuel = fuelCost(crabs, i);
	minFuel = Min(currFuel, minFuel);
}

stopwatch.Stop();
Console.WriteLine($"Fuel: {minFuel}");
// drawScreen(screen);
Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds/1000.0}s");