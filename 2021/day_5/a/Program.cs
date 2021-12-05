using System.Diagnostics;
var lines = File.ReadAllLines("../input.txt");
var stopwatch = new Stopwatch();
stopwatch.Start();

(Line[],int xMax,int yMax) parseLines() {
	var lins = new Line[lines.Length];
	Line line;
	int xMax = 0;
	int yMax = 0;
	for (int i = 0; i < lines.Length; i++) {
		var points = lines[i].Split(" -> ");
		var p1 = points[0].Split(',');
		var p2 = points[1].Split(',');
		// Zorg dat punten altijd van laag -> hoog gaan
		line.x1 = Math.Min(int.Parse(p1[0]), int.Parse(p2[0]));
		line.y1 = Math.Min(int.Parse(p1[1]), int.Parse(p2[1]));
		line.x2 = Math.Max(int.Parse(p1[0]), int.Parse(p2[0]));
		line.y2 = Math.Max(int.Parse(p1[1]), int.Parse(p2[1]));
		// Lengte van lijn
		line.dx = line.x2 - line.x1;
		line.dy = line.y2 - line.y1;
		line.length = (int)Math.Round(Math.Sqrt(line.dx*line.dx + line.dy*line.dy));
		lins[i] = line;
		// Groote van veld
		xMax = Math.Max(xMax, line.x2);
		yMax = Math.Max(yMax, line.y2);
	}
	return (lins, xMax, yMax);
}

// Scherm tekeken
void drawScreen(byte[,] screen) {
	for (int x = 0; x < screen.GetLength(0); x++) {
		for (int y = 0; y < screen.GetLength(1); y++)
			Console.Write(screen[x,y] > 0 ? $"{screen[x,y]} " : ". ");
		Console.WriteLine();
	}
}

// Lijn plotten
void plotLine(byte[,] screen, in Line line) {
	var dx = (line.x2 - line.x1) / line.length;
	var dy = (line.y2 - line.y1) / line.length;
	var x = line.x1 - dx;
	var y = line.y1 - dy;
	while((x != line.x2) || (y != line.y2)) {
		x += dx; y += dy;
		screen[x,y] += 1;
	}
}

(var lins, int xMax, int yMax) = parseLines();

// Plot the lines!
var screen = new byte[xMax+1,yMax+1];
for(int l = 0; l < lins.Length; l++)
	if (lins[l].dx == 0 || lins[l].dy == 0) 
		plotLine(screen, lins[l]);

// Count!
int doubleV = 0;
for (int x = 0; x < screen.GetLength(0); x++)
	for (int y = 0; y < screen.GetLength(1); y++)
		if(screen[x,y] > 1) doubleV++;


stopwatch.Stop();
Console.WriteLine($"Double hits: {doubleV}");
// drawScreen(screen);
Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds/1000.0}s");


// Datatypes
struct Line {
	public int x1;
	public int y1;
	public int x2;
	public int y2;
	public int dx;
	public int dy;
	public int length;
}