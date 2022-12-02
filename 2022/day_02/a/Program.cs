var lines = File.ReadAllLines("../in.txt");

int[,] scor = new int[,] {
	//X  Y  Z
	{ 3, 6, 0 }, // A (Rock)
	{ 0, 3, 6 }, // B (Paper)
	{ 6, 0, 3 }, // C (Scissors)
};
int[] valu = new int[] { 1, 2, 3 };

int score = 0;
foreach (var l in lines) {
	int oppn = (int)l[0] - (int)'A';
	int mine = (int)l[2] - (int)'X';
	score += valu[mine];
	score += scor[oppn, mine];
}

Console.WriteLine(score);