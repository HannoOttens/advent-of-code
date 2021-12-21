using static System.Math;
using StrSpan = System.ReadOnlySpan<char>;

var lines = File.ReadAllLines("../input.txt");
(int pos, int score) p1 = (parsePlayer(lines[0]), 0);
(int pos, int score) p2 = (parsePlayer(lines[1]), 0);
int die = 1;
int round = 0;
while(p1.score < 1000 && p2.score < 1000) {
	if (round % 2 == 0) {
		p1.pos += die; die++;
		p1.pos += die; die++;
		p1.pos += die; die++;
		p1.pos %= 10;
		p1.score += p1.pos + 1;
	} else {
		p2.pos += die; die++;
		p2.pos += die; die++;
		p2.pos += die; die++;
		p2.pos %= 10;
		p2.score += p2.pos + 1;
	}
	round++;
}
Console.WriteLine($"{Min(p1.score,p2.score)*(die-1)}");
Console.WriteLine($"P1: {p1}");
Console.WriteLine($"P2: {p2}");
Console.WriteLine($"Round: {round}");
Console.WriteLine($"Die: {die}");

int parsePlayer(StrSpan str) {
	while(str[0] != ':') str = str.Slice(1);
	str = str.Slice(2);
	return str[0] - '0' - 1; // 0 based gaan doen
}