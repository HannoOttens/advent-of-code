(char, int)[] cmnds = File.ReadAllLines("../in.txt").Select(t => (t[0], int.Parse(t[2..]))).ToArray();

HashSet<Posi> beenThereDoneThat = new();
Posi head = new(0,0);
Posi tail = new(0,0);
beenThereDoneThat.Add(tail);
foreach ((char dir, int length) in cmnds) {
	Posi step = dir switch {
		'U' => new( 0,  1),
		'D' => new( 0, -1),
		'R' => new( 1,  0),
		'L' => new(-1,  0),
	};

	for(int i = 0; i < length; i++)
	{
		head += step;
		if ((head - tail).Dist() > 1) {
			tail = head - step; // Tail always goes where the head came from
			beenThereDoneThat.Add(tail);
		}
	}
}
Console.WriteLine(beenThereDoneThat.Count);

public record Posi(int PosX, int PosY)
{
	public static Posi operator +(Posi a, Posi b)
		=> new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b)
		=> new(a.PosX - b.PosX, a.PosY - b.PosY);
	public int Dist() => Math.Max(Math.Abs(PosX), Math.Abs(PosY));
}