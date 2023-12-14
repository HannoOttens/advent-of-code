var lines = File.ReadAllLines("../input.txt");

int total = 0;
for (int i = 1; i <= lines.Count(); i++) {
	Game game = parseGame(i, lines[i-1]);
	Console.WriteLine(game.id);
	bool valid = validGame(game);
	if (valid) {
		total += game.id;
	}
}
Console.WriteLine(total);

const int max_red   = 12;
const int max_green = 13;
const int max_blue  = 14;
bool validDice(string color, int n) {
	Console.WriteLine((color, n));
	return color switch {
		"red"   => n <= max_red,
		"green" => n <= max_green,
		"blue"  => n <= max_blue,
		_ => false
	};
}
bool validRound(Round r) => r.cubes.All(x => validDice(x.color, x.n));
bool validGame(Game g) => g.rounds.All(validRound);

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
