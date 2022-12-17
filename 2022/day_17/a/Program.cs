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
Dictionary<Posi, char> map = new();
for(int x =  0; x < 9; x++) {
	for (int y = 0; y < 5000; y++)
		map[new(x,y)] = (x == 0 || x == 8 ? '|' : '.');
	map[new(x,0)] = '-';
}

// Load the wind directions
int yet = 0;
string wind = File.ReadAllLines("../in.txt")[0];

bool collides (string[] block, Posi bb, Posi posn) {
	for(int locX = 0; locX < bb.PosX; locX++)
	for(int locY = 0; locY < bb.PosY; locY++) {
		Posi globPos = new(posn.PosX+locX, posn.PosY-locY);
		if (block[locY][locX] == '#' && map[globPos] != '.')
			return true;
	}
	return false;
}

void placeBlock(string[] block, Posi bb, Posi posn) {
	for(int locX = 0; locX < bb.PosX; locX++)
	for(int locY = 0; locY < bb.PosY; locY++) {
		Posi globPos = new(posn.PosX+locX, posn.PosY-locY);
		if (block[locY][locX] == '#') map[globPos] = '#';
	}
}

// Tetris!
int maxY = 0;
for (int i = 0; i < 2022; i++) {
	draw();
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
}
Console.WriteLine(maxY);


void draw () {
	// StringBuilder sb = new();
	// for (int y = maxY+2; y >= 0; y--) {
	// 	for (int x = 0; x < 9; x++)	
	// 		sb.Append(map[new(x,y)]);
	// 	sb.AppendLine();
	// }
	// Console.SetCursorPosition(0,0);
	// Console.WriteLine(sb.ToString());
	// Thread.Sleep(5_000);
}

public record Posi {
	public int PosX;
	public int PosY;
	public Posi (int x, int y) => (PosX, PosY) = (x,y); 
	public Posi Copy() => new Posi(PosX, PosY);
}