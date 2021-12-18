﻿// 'global' vars
const byte VISITED = 0b_1000_0000;
var offsets = new (int dx,int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

// Readin
var lines = File.ReadAllLines("../input.txt");
int sizeX = lines.Length;
int sizeY = lines[0].Length;
var grid = new byte[sizeX*5,sizeY*5];
for(int x = 0; x < sizeX; x++) 
	for(int y = 0; y < sizeY; y++) {
		char c = lines[x][y];
		grid[x,y] = byte.Parse(new string(new char[] {c}));
	}

for(int gridX = 0; gridX < 5; gridX++) 
for(int gridY = 0; gridY < 5; gridY++) 
	if(gridX > 0 || gridY > 0)
		for(int x = 0; x < sizeX; x++) 
		for(int y = 0; y < sizeY; y++) {
			int newVal = grid[x,y]+gridX+gridY;
			if(newVal > 9) newVal = newVal - 9;
			grid[x+gridX*sizeX,y+gridY*sizeY] = (byte)newVal;
		}


(int x,int y,int cost) state = (0,0,0);
var states = new PriorityQueue<(int,int,int), int>();

while((state.x != 5*sizeX-1) || (state.y != 5*sizeY-1)) {
	if(!visited(grid[state.x,state.y]))
		foreach(var offset in offsets) {
			int x = state.x + offset.dx;
			int y = state.y + offset.dy;
			if(boundsCheck(x,y) && !visited(grid[x,y])) {
				var newCost = state.cost + getDanger(grid[x,y]);
				states.Enqueue((x,y,newCost), newCost);
			}
		}

	grid[state.x,state.y] = setVisited(grid[state.x,state.y]);
	state = states.Dequeue();
}
Console.WriteLine($"Lowest cost path: {state.cost}");
Console.WriteLine(printGrid(grid));

bool boundsCheck(int x, int y) {
	return (x>=0) && (y>=0) && (x<sizeX*5) && (y<sizeY*5);
}

bool visited(byte v) {
	return (v & VISITED) > 0;
}

byte setVisited(byte v) {
	return (byte)(v | VISITED);
}

int getDanger(byte v) {
	return (v & ~VISITED);
}

string printGrid(byte[,] grid) {
	string rslt = "";
	for(int x = 0; x < 5*sizeX; x++) {
		for(int y = 0; y < 5*sizeY; y++) {
			// if(!visited(grid[x,y]))
				rslt += getDanger(grid[x,y]);
			// else
				// rslt += 'X';
		}
		rslt += "\n";
	}
	return rslt;
}