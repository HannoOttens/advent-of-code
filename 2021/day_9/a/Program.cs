using System.Diagnostics;
using static System.Math;

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


int riskLevel = 0;
var offsets = new (int,int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
for(int x = 1; x < sizeX-1; x++) 
	for(int y = 1; y < sizeY-1; y++) {
		bool isLowest = true;
		foreach((int dx, int dy) in offsets)
			if (grid[x,y] >= grid[x+dx,y+dy]) 
				isLowest = false;
		if (isLowest) riskLevel += 1 + grid[x,y];
	}

Console.WriteLine($"Risklevel: {riskLevel}");