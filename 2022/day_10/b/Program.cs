string[] cmnds = File.ReadAllLines("../in.txt");
bool[,] screen = new bool[6, 40];

int varX = 1;
int clck = 0;
for (int cmnd = 0; cmnd < cmnds.Length; cmnd++)
{
	(int tick, int incr) = cmnds[cmnd][0] switch {
		'n' => (1, 0),
		'a' => (2, int.Parse(cmnds[cmnd].Split(' ')[1]))
	};

	while (tick > 0) {
		int posY = clck / 40;
		int posX = clck % 40;
		screen[posY, posX] = (posX >= varX - 1) && (posX <= varX + 1);
		clck++;  
		tick--;
	}

	varX += incr;
}

for (int y = 0; y < 6; y++){
	for (int x = 0; x < 40; x++)
		Console.Write(screen[y,x] ? '#' : '.');
	Console.WriteLine();
}