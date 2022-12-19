using static System.Math;
// Parsing
var cubes = File.ReadAllLines("../in.txt")
				.Select(x => x.Split(',').Select(int.Parse).ToArray())
				.Select(p => new Posi(p[0],p[1],p[2]))
				.ToArray();
				
// Sides of the cube
Posi[] steps = new Posi[] { new(-1,0,0), new(0,-1,0), new(0,0,-1)
					 	  , new( 1,0,0), new(0, 1,0), new(0,0, 1)};

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
Console.WriteLine(visi);

public record Posi (int X, int Y, int Z) {
	public static Posi operator +(Posi a, Posi b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	public static Posi operator -(Posi a, Posi b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
}