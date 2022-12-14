using System.Text;

string[] polylines = File.ReadAllLines("../in.txt");

// Maak de kaart
Dictionary<Posi, char> map = new();
for (int y = 0; y <= 200; y++) 
for (int x = 0; x <= 800; x++) map[new(x,y)] = ' ';

// Lees gesteente in
int maxY = 0;
foreach(string polyline in polylines) {
	Posi[] points = polyline.Split(" -> ")
							.Select(p => new Posi(int.Parse(p.Split(',')[0])
												, int.Parse(p.Split(',')[1])))
							.ToArray();
	// Diepste stuk steen bepalen
	foreach (Posi p in points) maxY = Math.Max(maxY, p.PosY);
	// Lijnen trekken
	for (int indx = 1; indx < points.Length; indx++) {
		var vect = (points[indx] - points[indx-1]).Uno();
		while (points[indx-1] != points[indx])
		{
			map[points[indx-1]] = '#';
			points[indx-1] += vect;
		}
		map[points[indx-1]] = '#';
	}
}

// Toegestane steps
Posi[] steps = new Posi[] { new(0, 1), new(-1, 1), new(1, 1)}; 

// Simuleer!
int stuck = 0;
bool done = false;
while(!done) {
	// Simuleer een sandparticle
	Posi SPos = new(500, 0);
	bool canStep = true;
	while(canStep && SPos.PosY <= maxY) {
		draw(SPos);

		canStep = false;
		Posi NPos = new();
		foreach(var step in steps) {
			NPos = SPos + step;
			if (map[NPos] == ' ') {
				canStep = true;
				break;
			};
		}
		if (canStep) SPos = NPos;
	}

	stuck++;
	map[SPos] = 'O';
	Thread.Sleep(100);

	if (SPos.PosY == 0 && SPos.PosX == 500) done = true;
}
Console.WriteLine(stuck);

void draw(Posi curr) {
	map[curr] = 'V';
	StringBuilder sb = new();
	int yMin = Math.Max(curr.PosY - 20, 0);
	for (int y = yMin; y <= yMin + 30; y++) {
		for (int x = 425; x <= 575; x++) sb.Append(map[new(x,y)]);
		sb.Append('\n');
	}
	Console.SetCursorPosition(0,0);
	Console.WriteLine(sb.ToString());
	Thread.Sleep(10);
	map[curr] = ' ';
}


public record Posi(int PosX = 0, int PosY = 0)
{
	public static Posi operator +(Posi a, Posi b) => new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b) => new(a.PosX - b.PosX, a.PosY - b.PosY);
	public Posi Uno () => new(Math.Sign(PosX), Math.Sign(PosY));
}