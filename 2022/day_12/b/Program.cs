string[] input = File.ReadAllLines("../in.txt");

// Find start and goal and change them to he correct value
Posi strt = new();
Posi goal = new();
int sizeX = input[0].Length;
int sizeY = input.Length;
int[,] map = new int[sizeX, sizeY];
for (int y = 0; y < sizeY; y++)
for (int x = 0; x < sizeX; x++) {
	char c = input[y][x];
	if (c == 'S') { 
		strt = new (x, y);
		c = 'a';
	}
	if (c == 'E') {
		goal = new (x, y);
		c = 'z';
	} 
	map[x,y] = (int)c - (int)'a';
}

// Movement rules
Posi[] steps = new Posi[] { new (-1, 0), new (1, 0), new (0, -1), new (0, 1)};
bool allowed (Posi from, Posi to) 
	=> (to.PosX >= 0 && to.PosX < sizeX && to.PosY >= 0 && to.PosY < sizeY)
	&& (map[from.PosX, from.PosY] + 1 >= map[to.PosX, to.PosY]);

int shortestPath = int.MaxValue;
for (int y = 0; y < sizeY; y++)
for (int x = 0; x < sizeX; x++)
{
	if (map[x,y] != 0) continue;

	// Priority queue and Dikke van Dijkstra
	var queue = new PriorityQueue<Posi, int>(); 
	var seen = new bool[input[0].Length, input.Length];
	queue.Enqueue(new(x, y), 0);

	int length = int.MaxValue;
	bool cont = true, sccs = false;
	while (cont) {
		cont = queue.TryDequeue(out Posi curr, out length);
		if (cont) {
			if (curr == goal) { // Got there!
				cont = false;
				sccs = true;
			}
			if (seen[curr.PosX, curr.PosY]) continue; // Al een snellere route gevonden
			seen[curr.PosX, curr.PosY] = true;
		}

		if (cont) { // Toegestane stappen enqueue
			foreach(var step in steps) {
				var newP = curr + step;
				if (allowed(curr, newP))
					queue.Enqueue(newP, length + 1);
			}
		}
	}
	if (sccs) shortestPath = Math.Min(shortestPath, length);
}
Console.WriteLine(shortestPath);

public record Posi(int PosX = 0, int PosY = 0)
{
	public static Posi operator +(Posi a, Posi b) => new(a.PosX + b.PosX, a.PosY + b.PosY);
}