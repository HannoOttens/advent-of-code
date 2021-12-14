using static System.Math;

// Readin
var lines = File.ReadAllLines("../input.txt");
var dots = new (int x,int y)[lines.Length];
int nDots = 0;
while(lines[nDots].Length > 0) {
	var parts = lines[nDots].Split(',');
	dots[nDots].x = int.Parse(parts[0]);
	dots[nDots].y = int.Parse(parts[1]);
	nDots++;
}

for(int fold = nDots + 1; fold < nDots + 2; fold++) {
	(int xFold, int yFold) = parseFold(lines[fold]);
	Console.WriteLine($"X: {xFold}, Y: {yFold}");
	
	for (int i = 0; i < nDots; i ++) {
		dots[i].x = dots[i].x < xFold ? dots[i].x : (2*xFold - dots[i].x);
		dots[i].y = dots[i].y < yFold ? dots[i].y : (2*yFold - dots[i].y);
	}	
}
showBoard();

var unique = new SortedSet<(int,int)>();
for (int i = 0; i < nDots; i ++) unique.Add((dots[i].x,dots[i].y));
Console.WriteLine($"Dots: {unique.Count}");

(int xFold, int yFold) parseFold(string fold) {
	string foldInstr = fold.Split(' ')[2];
	string[] parts = foldInstr.Split('=');
	if(parts[0] == "x")
		return (int.Parse(parts[1]), int.MaxValue);
	else
		return (int.MaxValue, int.Parse(parts[1]));
}

void showBoard() {
	var unique = new SortedSet<(int,int)>();
	int xMax = 0;
	int yMax = 0;
	for (int i = 0; i < nDots; i ++) {
		xMax = Max(xMax, dots[i].x);
		yMax = Max(yMax, dots[i].y);
		unique.Add((dots[i].x,dots[i].y));
	};
	for (int y = 0; y <= yMax; y ++) {
		for (int x = 0; x <= xMax; x ++) {
			if(unique.Contains((x,y)))
				Console.Write("X");
			else
				Console.Write(".");
		}
		Console.WriteLine();
	}
	Console.WriteLine();
}