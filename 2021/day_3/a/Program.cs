var lines = File.ReadAllLines("../input.txt");

var zero = new int[lines[0].Length];
var ones = new int[lines[0].Length];
var actv = new bool[lines.Length];

foreach (var l in lines) 
	for(int i = 0; i < ones.Length; i++) 
		if (l[i] == '0') zero[i]++; else ones[i]++;

int gamma = 0;
int epsil = 0;
int b = (1 << ones.Length - 1);
for(int i = 0; i < ones.Length; i++) {
	gamma += b * (ones[i] > zero[i] ? 1 : 0);
	epsil += b * (ones[i] < zero[i] ? 1 : 0);
	b /= 2;
}

Console.WriteLine(gamma * epsil);