var lines = File.ReadAllLines("../input.txt");

int x = 0;
int z = 0;
foreach (var l in lines) {
	var parts = l.Split(' ');
	int n = int.Parse(parts[1]);

	switch (parts[0]) {
		case "forward":
			x += n; break;
		case "down":
			z += n; break;
		case "up":
			z -= n; break;
	}
}

Console.WriteLine(x*z);