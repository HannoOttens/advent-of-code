using System.Diagnostics;

var lines = File.ReadAllLines("../input.txt");

var stopwatch = new Stopwatch();
stopwatch.Start();

var actv = new bool[lines.Length];

// Count active bits
(int zero, int ones) BitCount(int bit) {
	int ones = 0;
	int zero = 0;
	for (int j = 0; j < lines.Length; j++)
	{
		if (actv[j])
			if (lines[j][bit] == '0') ones++; else zero++;
	}
	return (ones,zero);
}

// Filter numbers on bit
void Filter(int bit, char val) {
	for (int j = 0; j < lines.Length; j++)
		if (lines[j][bit] != val) actv[j] = false;
}

// Find last number
(bool last, string numm) Find(int bit) {
	int actC = 0;
	string actN = "";
	for (int j = 0; j < lines.Length; j++)
		if (actv[j]) {
			actN = lines[j];
			actC++;
		}
	return (actC == 1, actN);
}

string oxy = "";
for (int i = 0; i < lines.Length; i++)
	actv[i] = true;
for (int bit = 0; bit < lines[0].Length; bit++) {
	// Vind MCB 
	(int zero, int ones) = BitCount(bit);
	Filter(bit, ones >= zero ? '1': '0');
	
	// Stop bij 1
	(bool hasLast, oxy) = Find(bit);
	if (hasLast) break;
}

// Copypasta omdat lui
string co2 = "";
for (int i = 0; i < lines.Length; i++)
	actv[i] = true;
for (int bit = 0; bit < lines[0].Length; bit++) {
	// Vind MCB 
	(int zero, int ones) = BitCount(bit);
	Filter(bit, ones < zero ? '1': '0');
	
	// Stop bij 1
	(bool hasLast, co2) = Find(bit);
	if (hasLast) 
		break;
}


int oxyN = 0;
int co2N = 0;
int b = (1 << oxy.Length - 1);
for(int i = 0; i < oxy.Length; i++) {
	oxyN += b * (oxy[i] == '1' ? 1 : 0);
	co2N += b * (co2[i] == '1' ? 1 : 0);
	b /= 2;
}

stopwatch.Stop();

Console.WriteLine(oxyN * co2N);
Console.WriteLine($"{stopwatch.ElapsedMilliseconds/1000.0}s");
