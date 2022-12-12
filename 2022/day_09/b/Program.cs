(char, int)[] cmnds = File.ReadAllLines("../in.txt").Select(t => (t[0], int.Parse(t[2..]))).ToArray();

HashSet<Posi> beenThereDoneThat = new();
Posi[] rope = new Posi[10] { new(), new(), new(), new(), new(), new(), new(), new(), new(), new()};
beenThereDoneThat.Add(rope[9]);
foreach ((char dir, int length) in cmnds) {
	for(int i = 0; i < length; i++)
	{
		rope[0] += dir switch {
			'U' => new( 0,  1),
			'D' => new( 0, -1),
			'R' => new( 1,  0),
			'L' => new(-1,  0),
		};
		for (int knot = 1; knot < 10; knot++) {
			var vect = rope[knot-1] - rope[knot]; 
			if (vect.Dist() < 2) break;
			rope[knot] += vect.Uno();
		}
		beenThereDoneThat.Add(rope[9]); // Add the final one to the list
	}
}
Console.WriteLine(beenThereDoneThat.Count);

public record Posi(int PosX = 0, int PosY = 0)
{
	public static Posi operator +(Posi a, Posi b) => new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b) => new(a.PosX - b.PosX, a.PosY - b.PosY);
	public int Dist() => Math.Max(Math.Abs(PosX), Math.Abs(PosY));
	// Max it maximum 1/-1 in both X and
	public Posi Uno() => new (Math.Sign(PosX), Math.Sign(PosY));
}