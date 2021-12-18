// 'global' vars
const byte VISITED = 0b_1000_0000;
var offsets = new (int dx,int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

// Readin
var lines = File.ReadAllLines("../input.txt");
int sizeX = lines.Length;
int sizeY = lines[0].Length;
var grid = new byte[sizeX,sizeY];
for(int x = 0; x < sizeX; x++) 
	for(int y = 0; y < sizeY; y++) {
		char c = lines[x][y];
		grid[x,y] = byte.Parse(new string(new char[] {c}));
	}

State state = new State(-1,-1,0,0,0);
var states = new PriorityQueue<State, int>();
var fromGrid = new (int,int)[sizeX,sizeY];
while((state.x != sizeX-1) || (state.y != sizeY-1)) {
	if(!visited(grid[state.x,state.y])) {
		fromGrid[state.x,state.y] = (state.fromX, state.fromY);
		foreach(var offset in offsets) {
			int x = state.x + offset.dx;
			int y = state.y + offset.dy;
			
			if(boundsCheck(x,y) && !visited(grid[x,y])) {
				var newCost = state.cost + getDanger(grid[x,y]);
				states.Enqueue(new State(state.x,state.y,x,y,newCost), newCost);
			}
		}
	}

	setVisited(ref grid[state.x,state.y]);
	state = states.Dequeue();
}
fromGrid[state.x,state.y] = (state.fromX, state.fromY);


for(int x = 0; x < sizeX; x++) 
	for(int y = 0; y < sizeY; y++) {
		clearVisited(ref grid[x,y]);
	}

(int x, int y) target = (sizeX-1,sizeY-1);
while(target.x >= 0) {
	setVisited(ref grid[target.x,target.y]);
	target = fromGrid[target.x,target.y];
}


Console.WriteLine($"Lowest cost path: {state.cost}");
Console.WriteLine(printGrid(grid));

bool boundsCheck(int x, int y) {
	return (x>=0) && (y>=0) && (x<sizeX) && (y<sizeY);
}

bool visited(byte v) {
	return (v & VISITED) > 0;
}

void setVisited(ref byte v) {
	v = (byte)(v | VISITED);
}

void clearVisited(ref byte v) {
	v = (byte)(v & ~VISITED);
}

int getDanger(byte v) {
	return (v & ~VISITED);
}

string printGrid(byte[,] grid) {
	string rslt = "";
	for(int x = 0; x < sizeX; x++) {
		for(int y = 0; y < sizeY; y++) {
			if(!visited(grid[x,y]))
				rslt += getDanger(grid[x,y]);
			else
				rslt += '.';
		}
		rslt += "\n";
	}
	return rslt;
}


public record struct State(int fromX,int fromY,int x,int y,int cost);
