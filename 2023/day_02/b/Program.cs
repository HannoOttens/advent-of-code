using static System.Math;

var lines = File.ReadAllLines("../input.txt");

int total = 0;
for (int i = 1; i <= lines.Count(); i++) {
	Game game = parseGame(i, lines[i-1]);
	int power = powerGame(game);
	total += power;
}
Console.WriteLine(total);


(int,int,int) maxTup ((int,int,int) a, (int,int,int) b) {
	return (
		Math.Max(a.Item1, b.Item1),
		Math.Max(a.Item2, b.Item2),
		Math.Max(a.Item3, b.Item3)
	);
}
(int r, int g, int b) powerDice(string color, int n) {
	return color switch {
		"red"   => (n, 0, 0),
		"green" => (0, n, 0),
		"blue"  => (0, 0, n),
		_ => (0,0,0)
	};
}
(int r,int g,int b) powerRound(Round r) {
	return r.cubes.Aggregate((0,0,0), (c,cube) => maxTup(c, powerDice(cube.color, cube.n)));
};
int powerGame(Game g) { 
	(int r,int g,int b) tup = g.rounds.Aggregate((0,0,0), (c,roun) => maxTup(c, powerRound(roun)));
	return tup.r * tup.g * tup.b;
}

Game parseGame(int index, string line) {
	Game game = new Game();
	game.id = index;
	game.rounds = line.Split(';').Select(round => {
		Round r = new Round();
		var cubes = round.Split(',');
		r.cubes = cubes.Select(cbs => (cbs.Split(' ')[1], int.Parse(cbs.Split(' ')[0]))).ToList();
		r.total = r.cubes.Aggregate(0, (c, cube) => c + cube.n);
		return r;
	}).ToList();
	return game;
}

class Round {
	public int total;
	public List<(string color, int n)> cubes = new();
}

class Game {
	public int id;
	public List<Round> rounds = new();
}
