var lines = File.ReadAllLines("../in.txt");

int maxC = 0;
int curC = 0;
foreach (var l in lines) {
	if (string.IsNullOrEmpty(l)) {
		maxC = Math.Max(maxC, curC);
		curC = 0;
	} else {
		int curr = int.Parse(l);
		curC += curr;
	}
}
maxC = Math.Max(maxC, curC);

Console.WriteLine(maxC);