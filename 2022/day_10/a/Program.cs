string[] cmnds = File.ReadAllLines("../in.txt");

int varX = 1;
int wave = 0;
int clck = 1;
for (int cmnd = 0; cmnd < cmnds.Length; cmnd++)
{
	(int tick, int incr) = cmnds[cmnd][0] switch {
		'n' => (1, 0),
		'a' => (2, int.Parse(cmnds[cmnd].Split(' ')[1]))
	};

	while (tick > 0) {
		if (clck % 40 == 20 && clck <= 220) {
			Console.WriteLine($"Clock: {clck}, ValX: {varX}, Value: {clck * varX}");
			wave += clck * varX;
		}
		clck++;
		tick--;
	}

	varX += incr;
}
Console.WriteLine(wave);