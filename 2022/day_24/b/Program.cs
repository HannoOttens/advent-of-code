using static System.Math;
Posi[] steps = new Posi[] { new (1, 0), new (0, 1), new (0,0), new (-1, 0), new (0, -1)};

string[] map = File.ReadAllLines("../in.txt");

int maxX = map[0].Length-2;
int maxY = map.Length-2;
int repeatsEvery = Lcm(maxX, maxY);

// Toegstane steps
Posi strt = new Posi(0, 0);
Posi goal = new Posi(maxX-1, maxY-1);

// Too low 331
int timeAB = shortestPath(1, strt, goal);
Console.WriteLine($"a->b: {timeAB}");
int timeBA = shortestPath(timeAB+1, goal, strt);
Console.WriteLine($"b->a: {timeBA}");
Console.WriteLine(shortestPath(timeBA+1, strt, goal));

int shortestPath (int startTime, Posi from, Posi to) {
	while (containsBlizz(startTime, from)) startTime++;

	var seen = new HashSet<(int,Posi)>();
	var queue = new PriorityQueue<Posi, int>(); 
	queue.Enqueue(from, startTime);

	int time;
	while (queue.TryDequeue(out Posi curr, out time)) {
		if (curr == to) break; // Got there!

		var key = (time % repeatsEvery, curr);
		if (seen.Contains(key)) continue; // Al een snellere route gevonden
		seen.Add(key);

		// Toegestane stappen enqueue
		foreach(var step in steps) {
			var next = curr + step;
			if (boundChk(next) && !containsBlizz(time+1, next))
				queue.Enqueue(next, time + 1);
		}
	}

	return time+1;
}

bool boundChk (Posi posn) 
	=> (posn.PosX >= 0 && posn.PosY >= 0 && posn.PosX < maxX && posn.PosY < maxY);
bool containsBlizz(int time, Posi posn) {
	int xSub = mod(posn.PosX - time, maxX);
	int xAdd = mod(posn.PosX + time, maxX);
	int ySub = mod(posn.PosY - time, maxY);
	int yAdd = mod(posn.PosY + time, maxY);
	return map[posn.PosY+1][xSub+1] == '>'
		|| map[posn.PosY+1][xAdd+1] == '<'
		|| map[ySub+1][posn.PosX+1] == 'v'
		|| map[yAdd+1][posn.PosX+1] == '^';
}

static int Lcm(int a, int b) => (a / Gfc(a, b)) * b;
static int Gfc(int a, int b)
{
    while (b != 0) {
		(a, b) = (b, a % b);
	}
    return a;
}
int mod(int x, int m) {
    return (x%m + m)%m;
}

public record Posi(int PosX = 0, int PosY = 0)
{
	public static Posi operator +(Posi a, Posi b) => new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b) => new(a.PosX - b.PosX, a.PosY - b.PosY);
}