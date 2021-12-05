// See https://bka.ms/new-console-template for more information

var lines = File.ReadAllLines("../input.txt");

int x = 0;
int a = 0;
int d = 0;
foreach (var l in lines) {
	var parts = l.Split(' ');
	int n = int.Parse(parts[1]);

	switch (parts[0]) {
		case "forward":
			x += n; 
			d += n * a;
			break;
		case "down":
			a += n; break;
		case "up":
			a -= n; break;
	}
}

Console.WriteLine(x*d);