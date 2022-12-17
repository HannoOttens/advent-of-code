using static System.Math;
using System.Text;

string[][] blocks = new string[][] {
	new string[] { "####" },
	new string[] { ".#.",
				   "###",
				   ".#." },
	new string[] { "..#",
				   "..#",
				   "###" },
	new string[] { "#",
				   "#",
				   "#",
				   "#" },
	new string[] { "##",
				   "##" },
};

// Build the map
char[,] map = new char[9, 12_000];
for(int x =  0; x < 9; x++) {
	for (int y = 0; y < 12_000; y++)
		map[x,y] = (x == 0 || x == 8 ? '|' : '.');
	map[x,0] = '-';
}

// Load the wind directions
string wind = File.ReadAllLines("../in.txt")[0];

bool collides (string[] block, Posi bb, Posi posn) {
	for(int locX = 0; locX < bb.PosX; locX++)
	for(int locY = 0; locY < bb.PosY; locY++) {
		if (block[locY][locX] == '#' && map[posn.PosX+locX, posn.PosY-locY] != '.')
			return true;
	}
	return false;
}

void placeBlock(string[] block, Posi bb, Posi posn) {
	for(int locX = 0; locX < bb.PosX; locX++)
	for(int locY = 0; locY < bb.PosY; locY++) {
		if (block[locY][locX] == '#') 
			map[posn.PosX+locX, posn.PosY-locY] = '#';
	}
}

// Tetris!
int maxY = 0;
int i = 0;
int prevMaxY = 0;
int prevI = 0;
int yet = 0;
while(i < 1714+1446) {
	if (yet == 0 && i > 0) {
		Console.WriteLine((i - prevI, maxY - prevMaxY));
		prevMaxY = maxY;
		prevI = i;
	}
	string[] block = blocks[i % blocks.Length];
	Posi bb = new(block[0].Length, block.Length); // Bounding box (always local from 0,0)
	bool stopped = false;

	Posi posn = new(3, maxY + bb.PosY + 3);
	while (!stopped) {
		// Push by yet (TODO: Program in the collisions)
		Posi newP = posn.Copy();
		newP.PosX = (wind[yet]) switch {
			'>' => posn.PosX + 1,
			'<' => posn.PosX - 1,
		};
		yet = (yet + 1) % wind.Length;
		// Accept the newP if not collides
		if (!collides(block, bb, newP)) 
			posn = newP.Copy(); // Pushed into something

		// Fall down
		newP = posn.Copy();
		newP.PosY--;
		if (collides(block, bb, newP)) { 
			newP = posn.Copy(); // Pushed into something (not a wall)
			maxY = Max(newP.PosY, maxY);
			placeBlock(block, bb, newP);
			stopped = true;
		}
		posn = newP.Copy();
	}
	i++;
}
Console.WriteLine((i - prevI, maxY - prevMaxY));


Console.WriteLine("| First stack  ");
Console.WriteLine("| 	Added height = 2685");
Console.WriteLine("|	Used blocks = 1714");
Console.WriteLine("| Second stack ");
Console.WriteLine("|	Added height = 2702");
Console.WriteLine("|	Used blocks = 1720");
Console.WriteLine("| Second block repeats forever..");
Console.WriteLine("| So! We can stack the stack this many times");
ulong startStackBlocks = 1714;
ulong startStackHeight = 2685;
ulong repeatedStackBlocks = 1720;
ulong repeatedStackHeight = 2702;
ulong repeatedStacks = (1_000_000_000_000UL - startStackBlocks) / repeatedStackBlocks;
Console.WriteLine(repeatedStacks);
Console.WriteLine("| But we need to stack the remainder on top!");
Console.WriteLine((1_000_000_000_000UL - startStackBlocks) % repeatedStackBlocks);
Console.WriteLine("| This adds the height of 2303 to the top.");
ulong endStackHeight = 2303;
Console.WriteLine("| We can now calculate the total:");
Console.WriteLine(startStackHeight + repeatedStacks * repeatedStackHeight + endStackHeight);

public record Posi {
	public int PosX;
	public int PosY;
	public Posi (int x, int y) => (PosX, PosY) = (x,y); 
	public Posi Copy() => new Posi(PosX, PosY);
}