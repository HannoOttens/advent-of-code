using System.Diagnostics;
using static System.Math;

int fuelCost(int[] crabs, int align) {
	return crabs.Aggregate(0, (s, c) => s + Abs(c - align));
}


// Start
var lines = File.ReadAllLines("../input.txt");
var crabs = lines[0].Split(',')
					.Select(n => int.Parse(n))
					.ToArray();

var stopwatch = new Stopwatch();
stopwatch.Start();

Array.Sort(crabs);

int lo = 0;
int hi = crabs.Length - 1;
int minFuel = int.MaxValue;
while(hi - 1 > lo) {
	int midpoint = (hi - lo) / 2;
	int mid = fuelCost(crabs, crabs[midpoint]);
	int hig = fuelCost(crabs, crabs[midpoint+1]);
	int low = fuelCost(crabs, crabs[midpoint-1]);
	if(hig > mid)
		hi = midpoint;
	else if (low > mid)
		low = midpoint;
	else {
		hi = midpoint;
		lo = midpoint;
	}
	Console.WriteLine(midpoint);
}

int alignAt = hi;
int fuel = fuelCost(crabs, crabs[alignAt]);


stopwatch.Stop();
Console.WriteLine($"Alignment: {alignAt}, Fuel: {fuel}");
// drawScreen(screen);
Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds/1000.0}s");