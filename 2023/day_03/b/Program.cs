var lines = File.ReadAllLines("../input.txt");
Box2 box = new Box2(new Posi(0,0), new Posi(lines[0].Length, lines.Length));

// Globals
var offsets = new Posi[] { 
	new(-1,-1), new( 0,-1), new( 1,-1), 
	new(-1, 0),             new( 1, 0),
	new(-1, 1), new( 0, 1), new( 1, 1)
};
Posi left = new(-1, 0);
Posi rght = new( 1, 0);

int total = 0;

for(int x = box.BotL.PosX; x < box.TopR.PosX; x++)
for(int y = box.BotL.PosY; y < box.TopR.PosY; y++) {
	Posi curr = new(x,y);
	if (getC(curr) == '*') {
		HashSet<Posi> set = new();
		foreach (Posi off in offsets) {
			// find unique numbers
			Posi adj = curr + off;
			if (box.PosInBox(adj) && posDigit(adj)) {
				while (box.PosInBox(adj + left) && posDigit(adj + left))
					adj += left;
				set.Add(adj);
			}
		}
		if (set.Count() == 2) {
			total += set.Aggregate(1, (aggr, p) => aggr * readNumb(p));
		}
	}
}

Console.WriteLine(total);

bool posDigit (Posi pos) => char.IsDigit(getC(pos));
char getC(Posi pos) => lines[pos.PosY][pos.PosX];
int readNumb(Posi pos) {
	List<char> chrs = new() { getC(pos) };
	while (box.PosInBox(pos + rght) && posDigit(pos + rght)) 
	{
		pos += rght;
		chrs.Add(getC(pos));
	}
	string numb = new string(chrs.ToArray());
	return int.Parse(numb);
}

public record Posi(int PosX, int PosY)
{
	public static Posi operator +(Posi a, Posi b)
		=> new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b)
		=> new(a.PosX - b.PosX, a.PosY - b.PosY);
	public int Dist() => Math.Max(Math.Abs(PosX), Math.Abs(PosY));
}

public record Box2(Posi BotL, Posi TopR) {
	public bool PosInBox (Posi p) {
		return (BotL.PosX <= p.PosX)
			&& (BotL.PosY <= p.PosY)
			&& (TopR.PosY > p.PosX)
			&& (TopR.PosY > p.PosY);
	}
}