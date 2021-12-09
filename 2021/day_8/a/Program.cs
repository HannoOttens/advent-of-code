using System.Diagnostics;
using static System.Math;

// Start
var lines = File.ReadAllLines("../input.txt");

var codes = new List<List<string>>();
var numbs = new List<List<string>>();
for(int l = 0; l < lines.Length; l++) {
	codes.Add(lines[l].Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
	numbs.Add(lines[l].Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
}

var stopwatch = new Stopwatch();
stopwatch.Start();

int n = 0;
for(int i = 0; i < numbs.Count; i++) {
	foreach(string digit in numbs[i]) {
		switch (digit.Length) {
			case 2: n++; break;
			case 3: n++; break;
			case 4: n++; break;
			case 7: n++; break;
		}
	}
}

stopwatch.Stop();
Console.WriteLine($"Total unique: {n}");
// drawScreen(screen);
Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds/1000.0}s");