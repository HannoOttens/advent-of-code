using System.Text;
using static System.Math;
using Pixels = System.Collections.Generic.Dictionary<(int x,int y), bool>;

// Globals
var offsets = new (int,int)[] { 
	(-1,-1), ( 0,-1), ( 1,-1), 
	(-1, 0), ( 0, 0), ( 1, 0),
	(-1, 1), ( 0, 1), ( 1, 1)
};

// Readin
var lines = File.ReadAllLines("../input.txt");
string enhancer = lines[0];

var pixels = new Pixels();
for(int y = 0; y < lines.Length - 2; y++)
for(int x = 0; x < lines.Length - 2; x++)
	if(lines[y+2][x] == '#') {
		pixels[(x,y)] = true;
	}

var p2 = runFilter(pixels, false);
File.WriteAllText("../output1.txt", showPixels(p2, true));
var p3 = runFilter(p2, true);
File.WriteAllText("../output2.txt", showPixels(p3, false));
Console.WriteLine(p3.Count((kv) => kv.Value));

Pixels runFilter(Pixels pixels, bool infyOn) {
	var newPixels = new Pixels();
	foreach(var kv in pixels) 
		foreach((int dx, int dy) in offsets) {
			int x = kv.Key.x + dx;
			int y = kv.Key.y + dy;
			if (!newPixels.ContainsKey((x,y)))
				newPixels[(x,y)] = makePixel(pixels, x, y, infyOn);
		}
	return newPixels;
}

bool makePixel(Pixels pixels, int x, int y, bool infyOn) {
	int i = 8;
	int v = 0;
	foreach((int dx, int dy) in offsets) {
		int bx = x + dx;
		int by = y + dy;
		if (pixels.ContainsKey((bx,by))) {
			if(pixels[(bx,by)]) 
				v += (int)Pow(2, i);
		} else if (infyOn) 
			v += (int)Pow(2, i);
		i--;
	}
	return enhancer[v] == '#';
}

string showPixels(Pixels pixels, bool infyOn) {
	(int xMax, int yMax) = pixels.Aggregate((0,0), (bounds, kv) => {
		return (
			Max(bounds.Item1, kv.Key.x), 
			Max(bounds.Item2, kv.Key.y));
	});
	(int xMin, int yMin) = pixels.Aggregate((0,0), (bounds, kv) => {
		return (
			Min(bounds.Item1, kv.Key.x), 
			Min(bounds.Item2, kv.Key.y));
	});

	var sb = new StringBuilder();
	for (int y = yMin; y <= yMax; y++) {
		for (int x = xMin; x <= xMax; x++) {
			bool on = false;
			if(pixels.ContainsKey((x,y))) 
				on = pixels[(x,y)];
			else 
				on = infyOn;
			sb.Append(on ? '#' : '.');
		}
		sb.Append('\n');
	}
	return sb.ToString();
}