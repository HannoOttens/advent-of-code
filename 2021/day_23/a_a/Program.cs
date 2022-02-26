var lines = File.ReadAllLines("input.txt");

// #############
// #89.A.B.C.DE#
// ###0#2#4#6###
//   #1#3#5#7#
//   #########

const int MAX_ROOM = 7;

sbyte[,] stepCost = new sbyte[,] {
	// 0,  1,  2,  3,  4,  5,  6,  7,  8,  9,  A,  B,  C,  D,  E
	{ -1,  1, -1, -1, -1, -1, -1, -1, -1,  2,  2, -1, -1, -1, -1 }, // 0
	{  1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }, // 1
	{ -1, -1, -1,  1, -1, -1, -1, -1, -1, -1,  2,  2, -1, -1, -1 }, // 2
	{ -1, -1,  1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }, // 3
	{ -1, -1, -1, -1, -1,  1, -1, -1, -1, -1, -1,  2,  2, -1, -1 }, // 4
	{ -1, -1, -1, -1,  1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }, // 5
	{ -1, -1, -1, -1, -1, -1, -1,  1, -1, -1, -1, -1,  2,  2, -1 }, // 6
	{ -1, -1, -1, -1, -1, -1,  1, -1, -1, -1, -1, -1, -1, -1, -1 }, // 7
	{ -1, -1, -1, -1, -1, -1, -1, -1, -1,  1, -1, -1, -1, -1, -1 }, // 8
	{  2, -1, -1, -1, -1, -1, -1, -1, -1, -1,  2, -1, -1, -1, -1 }, // 9
	{  2, -1,  2, -1, -1, -1, -1, -1, -1,  2, -1,  2, -1, -1, -1 }, // A
	{ -1, -1,  2, -1,  2, -1, -1, -1, -1, -1,  2, -1,  2, -1, -1 }, // B
	{ -1, -1, -1, -1,  2, -1,  2, -1, -1, -1, -1,  2, -1,  2, -1 }, // C
	{ -1, -1, -1, -1, -1, -1,  2, -1, -1, -1, -1, -1,  2, -1,  1 }, // D
	{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,  1, -1 }  // E
};
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

var cols = new char[15];
cols[0]  = lines[2][3];
cols[1]  = lines[3][3];
cols[2]  = lines[2][5];
cols[3]  = lines[3][5];
cols[4]  = lines[2][7];
cols[5]  = lines[3][7];
cols[6]  = lines[2][9];
cols[7]  = lines[3][9];
cols[8]  = '.';
cols[9]  = '.';
cols[10] = '.';
cols[11] = '.';
cols[12] = '.';
cols[13] = '.';
cols[14] = '.';

(char[] arr, int cost, int lastToMove) state = (cols,0,-1);
var states = new PriorityQueue<(char[],int,int), int>();
while(!isOrganized(state.arr)) {
	string stateStr = new string(state.arr);
	if(!visited.ContainsKey(stateStr)) {
		visited[stateStr] = true;
		for(int from = 0; from < 15; from++)
		for(int dest = 0; dest < 15; dest++)
		if(stepCost[from,dest] > 0 
			&& state.arr[from] != '.' 
			&& state.arr[dest] == '.'
			&& canGoInRoom(state.arr, from, dest))
			// && ifStopOnlyToRoom(state.arr, from, dest, state.lastToMove))
		{
			char[] newS = new char[15];
			for(int c = 0; c < 15; c++) newS[c] = state.arr[c];
			newS[dest] = state.arr[from];
			newS[from] = '.';
			// Console.WriteLine($"From {from}");
			// showState(state.arr);
			// Console.WriteLine();
			// Console.WriteLine($"To {dest}");
			// showState(newS);
			// Console.WriteLine();
			if(!visited.ContainsKey(new string(newS))) {
				var newCost = state.cost + stepCost[from,dest] * costMultiplier[state.arr[from]];
				states.Enqueue((newS,newCost,dest), newCost);
			}
		}
	}
	// Console.WriteLine(state.cost);
	state = states.Dequeue();
}

Console.WriteLine($"{state.cost}");

bool isOrganized(Span<char> cols) {
	return cols[0] == 'A'
		&& cols[1] == 'A'
		&& cols[2] == 'B'
		&& cols[3] == 'B'
		&& cols[4] == 'C'
		&& cols[5] == 'C'
		&& cols[6] == 'D'
		&& cols[7] == 'D';
}


bool isDesitination(int i, char c) {
	return c == roomFor[i];	
}

bool canGoInRoom(char[] stat, int from, int dest) {
	if(dest <= MAX_ROOM && from <= MAX_ROOM) return true; // Binnen een kamer mag bewogen worden
	if(dest > MAX_ROOM) return true;
	char c = stat[from];
	int room = indexToRoom(dest);
	return isDesitination(room, c) && roomIsFree(stat, room);
}

bool ifStopOnlyToRoom(char[] stat, int from, int dest, int lastToMove) {
	if (lastToMove == from) return true; // Nog aan t bewegen, toestaan
	if (from > MAX_ROOM && dest > MAX_ROOM) return false; // Van hal naar hal, mag niet
	return true; // Rest vd gevallen mag wel
}

int indexToRoom(int i) {
	if (i == 0 || i == 1) return 0;  
	if (i == 2 || i == 3) return 1;
	if (i == 4 || i == 5) return 2;
	if (i == 6 || i == 7) return 3;
	return -1;
}

bool roomIsFree (char[] stat, int room) {
	int i1 = 2*room;
	int i2 = i1 + 1;
	char goal = roomFor[room];
	return (stat[i1] == '.' || stat[i1] == goal)
		&& (stat[i2] == '.' || stat[i2] == goal);

}


void showState(char[] state) {
	Console.WriteLine("#############");

	Console.Write("#");
	Console.Write(state[8]);
	Console.Write(state[9]);
	Console.Write(".");
	Console.Write(state[10]);
	Console.Write(".");
	Console.Write(state[11]);
	Console.Write(".");
	Console.Write(state[12]);
	Console.Write(".");
	Console.Write(state[13]);
	Console.Write(state[14]);
	Console.WriteLine("#");

	Console.Write("###");
	Console.Write(state[0]);
	Console.Write("#");
	Console.Write(state[2]);
	Console.Write("#");
	Console.Write(state[4]);
	Console.Write("#");
	Console.Write(state[6]);
	Console.WriteLine("###");

	Console.Write("  #");
	Console.Write(state[1]);
	Console.Write("#");
	Console.Write(state[3]);
	Console.Write("#");
	Console.Write(state[5]);
	Console.Write("#");
	Console.Write(state[7]);
	Console.WriteLine("#  ");

	Console.WriteLine("  #########  ");
}