using System.Diagnostics;
using static System.Math;

// Globals
var offsets = new (int,int)[] { 
	(-1,-1), ( 0,-1), ( 1,-1), 
	(-1, 0),          ( 1, 0),
	(-1, 1), ( 0, 1), ( 1, 1)
};

// Readin
var lines = File.ReadAllLines("../input.txt");
int sizeX = lines.Length+2;
int sizeY = lines[0].Length+2;
var grid = new int[sizeX,sizeY];
for(int x = 1; x < sizeX-1; x++) 
	for(int y = 1; y < sizeY-1; y++) {
		char c = lines[x-1][y-1];
		grid[x,y] = int.Parse(new string(new char[] {c}));
	}

// Surround with high numbers to prevent border checks
for(int x = 0; x < sizeX; x++) grid[x,0] 	   = -1;
for(int y = 0; y < sizeY; y++) grid[0,y] 	   = -1;
for(int x = 0; x < sizeX; x++) grid[x,sizeY-1] = -1;
for(int x = 0; x < sizeY; x++) grid[sizeX-1,x] = -1;

int flashes = 0;
var flashed = new bool[sizeX, sizeY];

for(int day = 0; day < 100; day++) {
	// Increase all by 1, reset flags
	for(int x = 1; x < sizeX-1; x++) 
	for(int y = 1; y < sizeY-1; y++) {
		flashed[x,y] = false;
		if (grid[x,y] >= 0) grid[x,y]++;
	}
	
	// Cascade flashing
	int lastFlashCount = -1;
	int flashCount = 0;
	while (lastFlashCount < flashCount) {
		lastFlashCount = flashCount;
		for(int x = 1; x < sizeX-1; x++) 
		for(int y = 1; y < sizeY-1; y++) 
			if (grid[x,y] > 9 && !flashed[x,y]) {
				flashCount++;
				flashed[x,y] = true;
				foreach((int dx, int dy) in offsets)
					if (grid[x+dx,y+dy] >= 0) 
						grid[x+dx,y+dy]++;
			}
	}
	flashes += flashCount;

	// Reset all that flashed
	for(int x = 1; x < sizeX-1; x++) 
	for(int y = 1; y < sizeY-1; y++) {
		if(flashed[x,y]) grid[x,y] = 0;
	}
}

Console.WriteLine($"Flashes: {flashes}");



string printGrid(int[,] grid) {
	string rslt = "";
	for(int x = 1; x < sizeX-1; x++) {
		for(int y = 1; y < sizeY-1; y++) {
			rslt += grid[x,y].ToString();
		}
		rslt += "\n";
	}
	return rslt;
}