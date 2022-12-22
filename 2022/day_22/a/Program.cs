using static System.Math;
using System.Text;
using System.Text.RegularExpressions;

// Read/parse inputs
string[] mapStr = File.ReadAllLines("../map.txt");
// Get maxX/maxY
int maxY = mapStr.Length;
int maxX = 0;
for(int y = 0; y < maxY; y++) 
	maxX = Max(mapStr[y].Length, maxX);
// Fill the dict
Dictionary<Posi, char> map = new();
for(int y = 0; y < maxY; y++) 
	for(int x = 0; x < maxX; x++)
		map[new(x,y)] = mapStr[y].Length > x ? mapStr[y][x] : ' ';

// Parse commands
string instrTxt = File.ReadAllText("../in.txt");
Regex reg = new Regex(@"(\d+)|[A-Z]");
var instr = reg.Matches(instrTxt);  
Dictionary<Posi, int> retrace = new();

// Directions
var dir = new Posi[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1)};

// Initial pos
int startX = 0;
while (map[new(startX,0)] != '.') startX++;
Posi curr = new(startX-1, 0); 
int rota = 0;
Console.WriteLine($"Start: {curr}");

// Run through all commands
for (int i = 0; i < instr.Count; i++) {
	switch (instr[i].Value) {
		case "L": rota--; break;
		case "R": rota++; break;
		default:
			int steps = int.Parse(instr[i].Value);
			bool stop = false;
			while (!stop && (steps-- > 0)) {
				(stop, curr) = makeStep(curr, rota);
				retrace[curr] = rota;
			}
			break;
	} 
	rota %= dir.Length;
	if (rota < 0) rota = dir.Length-1;
	retrace[curr] = rota;
}
draw();
Console.WriteLine($"Posi: {curr}, Rota: {rota}");
Console.WriteLine((curr.PosY+1)*1000 + (curr.PosX+1)*4 + rota );


bool boundsCheck(Posi curr) 
	=> ((curr.PosX>=0) && (curr.PosY>=0) && (curr.PosX<maxX) && (curr.PosY<maxY))
	&& (map[curr] != ' ');

(bool stop, Posi posn) makeStep(Posi curr, int rota) {
	var next = curr + dir[rota];
	while (!boundsCheck(next)) { // Out of bounds -> Time to wrap!
		if (next.PosX < 0)
			next = new(maxX - 1, next.PosY);
		else if (next.PosY < 0)
			next = new(next.PosX, maxY - 1);
		else if (next.PosX >= maxX)
			next = new(0, next.PosY);
		else if (next.PosY >= maxY)
			next = new(next.PosX, 0);
		else
			next += dir[rota]; // In the void
	}
	if (map[next] == '#')
		return (true , curr);
	else 
		return (false, next);
}

void draw() {
	var sb = new StringBuilder();
	for (int y = 0; y < maxY; y++) {
		for(int x = 0; x < maxX; x++)
		{
			if (retrace.ContainsKey(new(x,y))) {
				char arr = retrace[new(x,y)] switch {
					0 => '>',
					1 => 'v',
					2 => '<',
					3 => '^',
				};
				sb.Append(arr);
			}
			else sb.Append(map[new(x,y)]);
		}
		sb.AppendLine();
	}
	Console.WriteLine(sb.ToString());
}

public record Posi(int PosX = 0, int PosY = 0)
{
	public static Posi operator +(Posi a, Posi b) => new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b) => new(a.PosX - b.PosX, a.PosY - b.PosY);
}