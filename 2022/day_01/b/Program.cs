var lines = File.ReadAllLines("../in.txt");

int[] maxC = new int[3] { 0, 0, 0};

int lowIndx() {
	if (maxC[0] <= maxC[1] && maxC[0] <= maxC[2])
		return 0;
	if (maxC[1] <= maxC[2] && maxC[1] <= maxC[0])
		return 1;
	return 2;
}

int curC = 0;
int indx = 0;
foreach (var l in lines) {
	if (string.IsNullOrEmpty(l)) {
		indx = lowIndx();
		maxC[indx] = Math.Max(maxC[indx], curC);
		curC = 0;
	} else {
		int curr = int.Parse(l);
		curC += curr;
	}
}
indx = lowIndx();
maxC[indx] = Math.Max(maxC[indx], curC);


Console.WriteLine(maxC.Sum());