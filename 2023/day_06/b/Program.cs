using static System.Math;

var groups = File.ReadAllLines("../input.txt").Select(l => l.Split(' ').Select(int.Parse).ToArray());

long time = 44707080;
long recordDist = 283113411341491;

long better = 0;
for(long hold = 0; hold < time; hold++) {
	long distance = (time - hold) * hold;
	if (distance > recordDist) 
		better += 1;
}

Console.WriteLine(better);
