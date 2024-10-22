using static System.Math;

var groups = File.ReadAllLines("../input.txt").Select(l => l.Split(' ').Select(int.Parse).ToArray());

int multiply = 1;
foreach(var group in groups) {
	int time = group[0];
	int recordDist = group[1];

	int better = 0;
	for(int hold = 0; hold < time; hold++) {
		int distance = (time - hold) * hold;
		if (distance > recordDist) 
			better += 1;
	}

	multiply *= better;
}

Console.WriteLine(multiply);
