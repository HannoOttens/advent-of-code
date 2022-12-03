var lines = File.ReadAllLines("../in.txt");

int charToIndx (char c) {
	if (c >= 'a') 
		return (int)c - (int)'a';
	return (int)c - (int)'A' + 26;
}

int prio = 0;
ulong grup = ulong.MaxValue;
for (int elfI = 0; elfI < lines.Length; elfI++) {
	ulong elfB = 0;
	for (int i = 0; i < lines[elfI].Length; i++) {
		elfB |= (1UL << charToIndx(lines[elfI][i]));
	}
	grup &= elfB;

	if (elfI % 3 == 2) {
		prio += (int)Math.Log2(grup) + 1;
		grup = ulong.MaxValue;
	}
}


Console.WriteLine(prio);