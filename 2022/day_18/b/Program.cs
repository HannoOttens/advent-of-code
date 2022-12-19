using static System.Math;
// Parsing
var cubes = File.ReadAllLines("../in.txt")
				.Select(x => x.Split(',').Select(int.Parse).ToArray())
				.Select(p => new Posi(p[0],p[1],p[2]))
				.ToArray();
// Make a hashset
HashSet<Posi> hashed = new(cubes);
// Sides of the cube
Posi[] steps = new Posi[] { new(-1,0,0), new(0,-1,0), new(0,0,-1)
					 	  , new( 1,0,0), new(0, 1,0), new(0,0, 1)};
// Bounding box
int maxX = cubes.Select(p => p.X).Max();
int minX = cubes.Select(p => p.X).Min();
int maxY = cubes.Select(p => p.Y).Max();
int minY = cubes.Select(p => p.Y).Min();
int maxZ = cubes.Select(p => p.Z).Max();
int minZ = cubes.Select(p => p.Z).Min();
Console.WriteLine($"Bounding box: {(minX, minY, minZ)} to {(maxX, maxY, maxZ)}");

// Find a way to the edge from any position
bool aWayOut(Posi strt) {
	if (hashed.Contains(strt)) return true;

	var queue = new PriorityQueue<Posi, int>(); 
	bool[,,] seen = new bool[maxX, maxY, maxZ];
	queue.Enqueue(strt, 0);

	while (queue.TryDequeue(out Posi curr, out int length)) {
		// Got to the edge?
		if ( curr.X == minX || curr.X == maxX
		  || curr.Y == minY || curr.Y == maxY
		  || curr.Z == minZ || curr.Z == maxZ) return true; 
		// Al-geweest stuff
		if (seen[curr.X, curr.Y, curr.Z]) continue;
		seen[curr.X, curr.Y, curr.Z] = true;
		// Toegestane stappen enqueue
		foreach(var step in steps) {
			var newP = curr + step;
			if (!hashed.Contains(newP))
				queue.Enqueue(newP, length + 1);
		}
	}
	return false;
}

// Find airpockets
List<Posi> pocketses = new();
for(int X = minX; X < maxX; X++)
for(int Y = minY; Y < maxY; Y++)
for(int Z = minZ; Z < maxZ; Z++)
	if (!aWayOut(new(X, Y, Z)))
		pocketses.Add(new(X, Y, Z));
cubes = pocketses.ToArray();

// Calculate the sides of airpockets
int visi = cubes.Length * 6;
HashSet<Posi> list = new();
foreach (var cube in cubes) {
	foreach(var step in steps) {
		var side = cube + step;
		if (list.Contains(side))
			visi -= 2;
	}
	list.Add(cube);
}

// Substract from the answer from problem 1
Console.WriteLine(4310 - visi);

public record Posi (int X, int Y, int Z) {
	public static Posi operator +(Posi a, Posi b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	public static Posi operator -(Posi a, Posi b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
}