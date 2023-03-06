using static System.Math;
long deci = File.ReadAllLines("../in.txt").Select(x => toDec(x)).Sum();
Console.WriteLine(deci);
Console.WriteLine(deci == toDec("2-2--02=1---1200=0-1"));
			//                    ^^^^^^^
long toDec(string snafu) {
	long totl = 0;
	for (int p = 0; p < snafu.Length; p++) {
		int indx = snafu.Length - 1 - p;

		totl += snafu[indx] switch {
			'=' => -2 * ((long)Pow(5, p)),
			'-' => -1 * ((long)Pow(5, p)),
			'0' =>  0,
			'1' =>  1 * ((long)Pow(5, p)),
			'2' =>  2 * ((long)Pow(5, p)),
		};
	}
	return totl;
}