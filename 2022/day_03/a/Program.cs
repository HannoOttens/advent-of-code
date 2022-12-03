var lines = File.ReadAllLines("../in.txt");

int charToIndx (char c) {
	if (c >= 'a') 
		return (int)c - (int)'a';
	return (int)c - (int)'A' + 26;
}


int prio = 0;
ulong cntA;
ulong cntB;
foreach (var l in lines) {
	cntA = 0;
	cntB = 0;

	int splt = l.Length / 2;
	var sckA = l.Substring(0, splt);
	var sckB = l.Substring(splt, splt);

	for (int i = 0; i < splt; i++) {
		cntA |= (1UL << charToIndx(sckA[i]));
		cntB |= (1UL << charToIndx(sckB[i]));
	}

	ulong same = cntA & cntB;
	prio += (int)Math.Log2(same) + 1;
}
Console.WriteLine(prio);