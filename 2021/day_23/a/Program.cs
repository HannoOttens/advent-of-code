var lines = File.ReadAllLines("input.txt");

const int MAX_ROOM = 15;
const int MAX_NODE = 23;
string[] map = new string[] {
	"#############",
	"#QR.S.T.U.VW#",
	"###A#E#I#M###",
	"###B#F#J#N###",
	"###C#G#K#O###",
	"###D#H#L#P###",
	"#############"
};

(int,int)[] offsets = new (int,int)[] {
	(-1, 0), (1, 0), (0, -1), (0, 1)
};

int charToIndex(char c) {
	return ((int)c % 32) - 1;
}

List<int> computePath(sbyte[,] graph, int i, int j) {
	// Route vinden (dijkstra)
	var visited = new List<int>();
	var states = new PriorityQueue<(int, int, int), int>();
	var fromArry = new int[MAX_NODE];
	(int pos, int prev, int cost) state = (i, -1, 0);
	while(state.pos != j) {
		if(!visited.Contains(state.pos)) {
			visited.Add(state.pos);
			fromArry[state.pos] = (state.prev);
			for(int k = 0; k < MAX_NODE; k++) {
				if(graph[state.pos, k] == 127 || visited.Contains(k)) continue;
				var newCost = state.cost + graph[state.pos,k];
				states.Enqueue((k,state.pos,newCost), newCost);
			}
		}
		state = states.Dequeue();
	}
	fromArry[j] = state.prev;
	
	// Route terughalen
	List<int> route = new List<int>() { j };
	int p = j;
	while(p != i) {
		int prev = fromArry[p];
		route.Add(prev);
		p = prev;
	}
	route.Reverse();
	return route;
}

List<int>[,] allPaths(sbyte[,] graph) {
	var paths = new List<int>[MAX_NODE,MAX_NODE];
	for (int i = 0; i < MAX_NODE; ++i)
		for (int j = 0; j < MAX_NODE; ++j)
			if(validMove(i,j)) {
				paths[i,j] = computePath(graph, i,j);
			}

	return paths;
}

void floydWarshall(sbyte[,] graph)
{
	for (int k = 0; k < MAX_NODE; ++k)
		for (int i = 0; i < MAX_NODE; ++i)
			for (int j = 0; j < MAX_NODE; ++j)
				if (graph[i, k] + graph[k, j] < graph[i, j])
					graph[i, j] = (sbyte)(graph[i, k] + graph[k, j]);
}

bool roomToHall(int from, int dest) {
	return (from <= MAX_ROOM) && (dest > MAX_ROOM);
}

bool hallToRoom(int from, int dest) {
	return (from > MAX_ROOM) && (dest <= MAX_ROOM);
}

bool validMove(int from, int dest) {
	return (roomToHall(from, dest) || hallToRoom(from, dest));
}

void applyRules(sbyte[,] graph) {
	for (int i = 0; i < MAX_NODE; ++i)
		for (int j = 0; j < MAX_NODE; ++j)
			if (!validMove(i, j))
				graph[i, j] = -1;
}

List<int>[,] paths;
List<(int,int)> validPairs = new List<(int, int)>();
sbyte[,] makeTable() {
	var tabl = new sbyte[MAX_NODE,MAX_NODE];
	for(int x = 0; x < MAX_NODE; x++)
		for(int y = 0; y < MAX_NODE; y++)
			tabl[x,y] = sbyte.MaxValue;

	for(int x = 0; x < map.Length; x++) {
		for(int y = 0; y < map[0].Length; y++) {
			if (map[x][y] == '#') continue;
			int from = charToIndex(map[x][y]);

			foreach((int dx, int dy) in offsets) {
				if (map[x+dx][y+dy] == '#') continue;

				// Om t hoekje kijken
				if (map[x+dx][y+dy] == '.')
					foreach((int dx2, int dy2) in offsets) {
						if (map[x+dx+dx2][y+dy+dy2] == '#' 
							|| map[x+dx+dx2][y+dy+dy2] == '.') continue;
						if ((x+dx+dx2) == x && (y+dy+dy2) == y) continue;
						int to = charToIndex(map[x+dx+dx2][y+dy+dy2]);
						tabl[from,to] = 2;
					}
				// A->B
				else {
					int to = charToIndex(map[x+dx][y+dy]);
					tabl[from,to] = 1;
				}
			}
		}	
	}

	// Post process to get routes
	paths = allPaths(tabl); 
	floydWarshall(tabl);
	applyRules(tabl);
	for(int x = 0; x < MAX_NODE; x++)
		for(int y = 0; y < MAX_NODE; y++)
			if(tabl[x,y] > 0) validPairs.Add((x,y));

	// Show table
	for(int x = 0; x < MAX_NODE; x++) {
		Console.Write("\t");
		Console.Write((char)((int)('A') + x));
	}
	Console.WriteLine();

	for(int x = 0; x < MAX_NODE; x++) {
		Console.Write((char)((int)('A') + x));
		Console.Write(":\t");
		for(int y = 0; y < MAX_NODE; y++) {
			if(tabl[x,y] > 0) Console.Write(' ');
			Console.Write(tabl[x,y]);
			Console.Write('\t');
		}
		Console.WriteLine();
	}
	return tabl;
}

var stepCost = makeTable();

var costMultiplier = new Dictionary<char, int>() {
	{ 'A', 1 },
	{ 'B', 10 },
	{ 'C', 100 },
	{ 'D', 1000 }
};
var roomFor = new Dictionary<int, char>() {
	{ 0, 'A' },
	{ 1, 'B' },
	{ 2, 'C' },
	{ 3, 'D' }
};
var visited = new Dictionary<string, bool>();

var cols = new char[MAX_NODE];
cols[0]  = lines[2][3];
cols[1]  = lines[3][3];
cols[2]  = lines[4][3];
cols[3]  = lines[5][3];
cols[4]  = lines[2][5];
cols[5]  = lines[3][5];
cols[6]  = lines[4][5];
cols[7]  = lines[5][5];
cols[8]  = lines[2][7];
cols[9]  = lines[3][7];
cols[10] = lines[4][7];
cols[11] = lines[5][7];
cols[12] = lines[2][9];
cols[13] = lines[3][9];
cols[14] = lines[4][9];
cols[15] = lines[5][9];
cols[16] = '.';
cols[17] = '.';
cols[18] = '.';
cols[19] = '.';
cols[20] = '.';
cols[21] = '.';
cols[22] = '.';

(char[] arr, int cost, int lastToMove) state = (cols,0,-1);
var states = new PriorityQueue<(char[],int,int), int>();
int iter = 0;
while(!isOrganized(state.arr)) {
	iter++;
	if (iter % 1000_000 == 0)  {
		Console.WriteLine(state.cost);
		Console.WriteLine(iter);
	}
	string stateStr = new string(state.arr);
	if(!visited.ContainsKey(stateStr)) {
		visited[stateStr] = true;
		foreach((int from, int dest) in validPairs)
			if(state.arr[from] != '.' 
				&& state.arr[dest] == '.'
				&& canGoInRoom(state.arr, from, dest)
				&& validRoute(state.arr, from, dest))
			{
				char[] newS = new char[MAX_NODE];
				for(int c = 0; c < MAX_NODE; c++) newS[c] = state.arr[c];
				newS[dest] = state.arr[from];
				newS[from] = '.';
				if(!visited.ContainsKey(new string(newS))) {
					var newCost = state.cost + stepCost[from,dest] * costMultiplier[state.arr[from]];
					states.Enqueue((newS,newCost,dest), newCost);
				}
			}
	}
	// Console.WriteLine(state.cost);
	if (states.Count == 0) break;
	state = states.Dequeue();
}
Console.WriteLine(iter);

Console.WriteLine($"{state.cost}");

bool isOrganized(Span<char> cols) {
	return cols[ 0] == 'A'
		&& cols[ 1] == 'A'
		&& cols[ 2] == 'A'
		&& cols[ 3] == 'A'
		&& cols[ 4] == 'B'
		&& cols[ 5] == 'B'
		&& cols[ 6] == 'B'
		&& cols[ 7] == 'B'
		&& cols[ 8] == 'C'
		&& cols[ 9] == 'C'
		&& cols[10] == 'C'
		&& cols[11] == 'C'
		&& cols[12] == 'D'
		&& cols[13] == 'D'
		&& cols[14] == 'D'
		&& cols[15] == 'D';
}


bool isDesitination(int i, char c) {
	return c == roomFor[i];	
}

bool canGoInRoom(char[] stat, int from, int dest) {
	if(dest > MAX_ROOM) return true; // destination is geen kamer
	char c = stat[from];
	int room = indexToRoom(dest);
	return isDesitination(room, c) && roomIsFree(stat, room);
}

int indexToRoom(int i) {
	if (i >=  0 || i <=  3) return 0;  
	if (i >=  4 || i <=  7) return 1;
	if (i >=  8 || i <= 11) return 2;
	if (i >= 12 || i <= 15) return 3;
	return -1;
}

bool validRoute(char[] state,int from, int dest) {
	if (paths[from, dest] == null) return false;
	foreach(var node in paths[from,dest]) {
		if(node == from) continue;
		if(state[node] != '.') return false;
	}
	return true;
}

bool roomIsFree (char[] stat, int room) {
	int i1 = 4*room;
	int i2 = i1 + 1;
	int i3 = i1 + 2;
	int i4 = i1 + 3;
	char goal = roomFor[room];
	return (stat[i1] == '.' || stat[i1] == goal)
		&& (stat[i2] == '.' || stat[i2] == goal)
		&& (stat[i3] == '.' || stat[i3] == goal)
		&& (stat[i4] == '.' || stat[i4] == goal);
}