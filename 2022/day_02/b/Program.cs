var lines = File.ReadAllLines("../in.txt");

int[,] outc = new int[,] { // Translate the outcome en oppentents play to my play
	//X  Y  Z
	{ 2, 0, 1 }, // A (Rock)
	{ 0, 1, 2 }, // B (Paper)
	{ 1, 2, 0 }, // C (Scissors)
};
int[] valu = new int[] { 1, 2, 3 };

int score = 0;
foreach (var l in lines) {
	int oppn = (int)l[0] - (int)'A';
	int goal = (int)l[2] - (int)'X';
	int mine = outc[oppn, goal];
	score += valu[mine];
	score += goal * 3; // The goal has already been determined :D
}

Console.WriteLine(score);