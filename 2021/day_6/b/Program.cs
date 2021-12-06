using System.Diagnostics;

var lins = File.ReadAllLines("../input.txt");
var fish = lins[0].Split(',')
				  .Select(n => int.Parse(n))
				  .ToList();

var stopwatch = new Stopwatch();
stopwatch.Start();

long simiFish(int start) {
	var fiss = new long[9];
	fiss[start] = 1;
	var fisB = new long[9];
	int days = 0;
	while (days < 256) {
		// Buffer vullen met nieuwe waarden
		for (int i = 0; i < 9; i++) {
			bool newFish = (i == 0);
			int nextState = (newFish ? 6 : i - 1);

			fisB[nextState] += fiss[i];
			if(newFish) fisB[8] += fiss[i];
		}

		// Buffer terugkopieren
		for (int i = 0; i < 9; i++) { fiss[i] = fisB[i]; fisB[i] = 0; }
		days++;

		// TODO: Maar 1x doen
	}
	return fiss.Aggregate((s, n) => s + n);
}

long[] fishTable = new long[6];
for (int i = 1; i < 7; i++) {
	fishTable[i-1] = simiFish(i);
}

long total = fish.Aggregate(0L, (s,f) => s + fishTable[f-1]);

stopwatch.Stop();
Console.WriteLine($"Total fish: {total}");
Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds/1000.0}s");
