using System.Diagnostics;
using static System.Math;

// 'global' vars
var offsets = new (int,int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

// Readin
var lines = File.ReadAllLines("../input.txt");
int sizeX = lines.Length+2;
int sizeY = lines[0].Length+2;
var grid = new byte[sizeX,sizeY];
for(int x = 1; x < sizeX-1; x++) 
	for(int y = 1; y < sizeY-1; y++) {
		char c = lines[x-1][y-1];
		grid[x,y] = byte.Parse(new string(new char[] {c}));
	}

// Surround with high numbers to prevent border checks
for(int x = 0; x < sizeX; x++) grid[x,0] = 255;
for(int y = 0; y < sizeY; y++) grid[0,y] = 255;
for(int x = 0; x < sizeX; x++) grid[x,sizeY-1] = 255;
for(int x = 0; x < sizeY; x++) grid[sizeX-1,x] = 255;

// Find basins
var basins = new List<SortedSet<(int,int)>>();
for(int x = 1; x < sizeX-1; x++) 
	for(int y = 1; y < sizeY-1; y++) {
		bool isLowest = true;
		foreach((int dx, int dy) in offsets)
			if (grid[x,y] >= grid[x+dx,y+dy]) 
				isLowest = false;
		if (isLowest) basins.Add(findBasin(grid, x, y));
	}

// Filter doubles
var selectedBasins = new List<SortedSet<(int,int)>>();
foreach (var basin in basins) {
	bool alreadySelected = false;
	for (int i = 0; i < selectedBasins.Count; i++)
		if(selectedBasins[i].First() == basin.First()) alreadySelected = true;
	
	if (!alreadySelected) selectedBasins.Add(basin);
}

// Find largest three
int[] sizes = new int [selectedBasins.Count];
for (int i = 0; i < selectedBasins.Count; i++)
	sizes[i] = selectedBasins[i].Count;
Array.Sort(sizes);
int riskFactor = sizes[selectedBasins.Count - 1]
			   * sizes[selectedBasins.Count - 2]
			   * sizes[selectedBasins.Count - 3];
Console.WriteLine($"Risklevel: {riskFactor}");


// Find all elements belonging to a basin
SortedSet<(int,int)> findBasin(
	byte[,] grid, 
	int x, int y, 
	SortedSet<(int,int)> basin = null) 
{
	if(basin == null) 
		basin = new SortedSet<(int,int)>() {(x,y)};
	
	foreach((int dx, int dy) in offsets)
		if(grid[x+dx,y+dy] < 9)
			if(basin.Add((x+dx,y+dy)))
				findBasin(grid, x+dx,y+dy, basin);

	return basin;
}

