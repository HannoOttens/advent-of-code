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

HashSet<Posi> set = new();

for(int x = box.BotL.PosX; x < box.TopR.PosX; x++)
for(int y = box.BotL.PosY; y < box.TopR.PosY; y++) {
	Posi curr = new(x,y);
	if (posMarkr(curr)) {
		foreach (Posi off in offsets) {
			Posi adj = curr + off;
			if (box.PosInBox(adj) && posDigit(adj)) {
				while (box.PosInBox(adj + left) && posDigit(adj + left))
					adj += left;
				set.Add(adj);
			}
		}
	}
}

int total = 0;
foreach (Posi p in set) {
	Posi trgt = p;
	List<char> chrs = new() { getC(trgt) };
	while (box.PosInBox(trgt + rght) && posDigit(trgt + rght)) 
	{
		trgt += rght;
		chrs.Add(getC(trgt));
	}
	string numb = new string(chrs.ToArray());
	Console.WriteLine(int.Parse(numb));
	total += int.Parse(numb);
}
Console.WriteLine(total);


bool posDigit (Posi pos) => char.IsDigit(getC(pos));
bool posMarkr (Posi pos) => !posDigit(pos) && getC(pos) != '.';
char getC(Posi pos) => lines[pos.PosY][pos.PosX];


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