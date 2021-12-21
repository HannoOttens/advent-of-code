using static System.Math;
using StrSpan = System.ReadOnlySpan<char>;
// Global memory
var memory = new Dictionary<((int,int),(int,int),int,int, bool), (long,long)>();

// Readin
var lines = File.ReadAllLines("../input.txt");
(int pos, int score) p1 = (parsePlayer(lines[0]), 0);
(int pos, int score) p2 = (parsePlayer(lines[1]), 0);
(long wins1, long wins2) = start(p1,p2);

Console.WriteLine($"W1: {wins1}");
Console.WriteLine($"W2: {wins2}");

(long w1, long w2) start((int,int) p1, (int,int) p2) {
	// Split the universe
	(long w1_1, long w2_1) = step(p1, p2, 1, 2, true);
	(long w1_2, long w2_2) = step(p1, p2, 2, 2, true);
	(long w1_3, long w2_3) = step(p1, p2, 3, 2, true);

	// Sum up scores and save
	long w1 = w1_1 + w1_2 + w1_3;
	long w2 = w2_1 + w2_2 + w2_3;

	return (w1, w2);
}


(long w1, long w2) step(
	(int pos, int score) p1,
	(int pos, int score) p2,
	int die,
	int throwsLeft,
	bool p1Plays
) {
	var key = (p1,p2,die,throwsLeft,p1Plays);
	if(memory.ContainsKey(key)) return memory[key];

	// Make a step
	if(p1Plays)
		p1.pos += die;
	else
		p2.pos += die;

	// Switch player?
	if (throwsLeft == 0) {
		if(p1Plays) {
			p1.pos %= 10;
			p1.score += p1.pos + 1; 
		} else {
			p2.pos %= 10;
			p2.score += p2.pos + 1;
		}
		p1Plays = !p1Plays; 
		throwsLeft = 3; 
	}

	// Bottom cases
	if (p1.score >= 21) return (1, 0);
	if (p2.score >= 21) return (0, 1);

	// Split the universe
	(long w1_1, long w2_1) = step(p1, p2, 1, throwsLeft-1, p1Plays);
	(long w1_2, long w2_2) = step(p1, p2, 2, throwsLeft-1, p1Plays);
	(long w1_3, long w2_3) = step(p1, p2, 3, throwsLeft-1, p1Plays);

	// Sum up scores and save
	long w1 = w1_1 + w1_2 + w1_3;
	long w2 = w2_1 + w2_2 + w2_3;
	memory[key] = (w1,w2);
	return (w1,w2);
}



int parsePlayer(StrSpan str) {
	while(str[0] != ':') str = str.Slice(1);
	str = str.Slice(2);
	return str[0] - '0' - 1; // 0 based gaan doen
}
