using static System.Math;

// Possible steps and where we start to check
//   N 
// W   E
//   S

Posi[][] steps = new Posi[][] {
	new Posi[] { new( 0, -1), new( 1, -1), new(-1, -1) }, // N
	new Posi[] { new( 0,  1), new( 1,  1), new(-1,  1) }, // S
	new Posi[] { new(-1,  0), new(-1, -1), new(-1,  1) }, // W
	new Posi[] { new( 1,  0), new( 1, -1), new( 1,  1) }, // E
};
Dictionary<Posi, List<Posi>> planner;
List<Posi> elves = new();
HashSet<Posi> elfMap = new();

// Read in current state
string[] map = File.ReadAllLines("../in.txt");
for(int y = 0; y < map.Length; y++) 
for(int x = 0; x < map[0].Length; x++)
	if(map[y][x] == '#')
		elves.Add(new(x,y));

// Iterate!
Console.WriteLine($"Elves before: {elves.Count}");
int iter = 0;
bool oneMoved = true;
while (oneMoved) {
	oneMoved = false;
	planner = new();
	elfMap = new(elves);
	// draw();

	foreach(var elf in elves) {
		if(shouldMove(elf)) {
			planMove(elf);
			oneMoved = true;
		}
		else
			addToPlanner(elf, elf);
	}

	elves = new();
	foreach(var kv in planner) {
		if (kv.Value.Count > 1) 
			elves.AddRange(kv.Value);
		else
			elves.Add(kv.Key);
	}

	iter++;
}
elfMap = new(elves);
Console.WriteLine($"{iter}");

// Staat er nog een elf in de buurt van deze elf?
bool shouldMove(Posi elf) {
	for(int y = -1; y <= 1; y++) 
	for(int x = -1; x <= 1; x++) {
		if (x == 0 && y == 0) continue;
		Posi step = new(x,y);
		if (elfMap.Contains(elf + step)) 
			return true;
	}
	return false;
}

// Staat er nog een elf in de buurt van deze elf?
void planMove(Posi elf) {
	for (int chck = 0; chck < 4; chck++) {
		int wind = (iter + chck) % 4;
		// Check if all is clear in that direction
		bool shouldGo = true;
		foreach(var step in steps[wind])
			shouldGo &= !elfMap.Contains(elf + step);
		// If it is okay then go and stop
		if (shouldGo) {
			var step = steps[wind][0];
			addToPlanner(elf, elf + step);
			return;
		}
	}
	addToPlanner(elf, elf);
}

// Add a elfs next position to the planner
void addToPlanner(Posi curr, Posi next) {
	planner.TryGetValue(next, out List<Posi> list);
	if (list == null) list = new();
	list.Add(curr);
	planner[next] = list;
}

// Make bounding box
(Posi minP, Posi maxP) boundingBox() {
	Posi minP = new(int.MaxValue,int.MaxValue);
	Posi maxP = new(int.MinValue,int.MinValue);
	foreach(var elf in elves) {
		minP = new( Min(minP.PosX, elf.PosX)
			 	  , Min(minP.PosY, elf.PosY));
		maxP = new( Max(maxP.PosX, elf.PosX)
				  , Max(maxP.PosY, elf.PosY));
	}
	return (minP, maxP);
}

void draw() {
	Console.WriteLine($"== Current: {iter} ==");
	(Posi minP, Posi maxP) = boundingBox();
	for(int y = minP.PosY; y <= maxP.PosY; y++) {
		for(int x = minP.PosX; x <= maxP.PosX; x++)
			Console.Write(elfMap.Contains(new(x,y)) ? '#' : '.');
		Console.WriteLine();
	}
	Console.WriteLine();
}

public record Posi(int PosX = 0, int PosY = 0)
{
	public static Posi operator +(Posi a, Posi b) => new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b) => new(a.PosX - b.PosX, a.PosY - b.PosY);
}