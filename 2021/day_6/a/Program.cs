using System.Diagnostics;

var lins = File.ReadAllLines("../input.txt");
var fish = lins[0].Split(',')
				  .Select(n => int.Parse(n))
				  .ToList();

var stopwatch = new Stopwatch();
stopwatch.Start();

int simiFish(int start) {
	var fiss = new List<int> { start };
	int days = 0;
	while (days < 80) {
		fiss = fiss.Select(f => f - 1)
			.Aggregate(new List<int>()
						, (l, f) => {
							if(f < 0)
								l.AddRange(new List<int>{6,8});
							else
								l.Add(f);
							return l;
						});
		days++;
	}
	return fiss.Count;
}

int[] fishTable = new int[7];
for (int i = 1; i < 7; i++) {
	fishTable[i] = simiFish(i);
}

int total = fish.Aggregate(0, (s,f) => s + fishTable[f]);

stopwatch.Stop();
Console.WriteLine($"Total fish: {total}");
Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds/1000.0}s");
