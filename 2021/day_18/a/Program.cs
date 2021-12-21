using static System.Math;
using StrSpan = System.ReadOnlySpan<char>;

var lines = File.ReadAllLines("../input.txt");
var val1 = parseSnailNumbers(lines[0].AsSpan());
for(int i = 1; i < lines.Length; i++) {
	var val2 = parseSnailNumbers(lines[i].AsSpan());
	val1 = add(val1, val2);
	reduce(val1);
}

Console.WriteLine($"{val1}");
Console.WriteLine($"{magnitude(val1)}");


Snail add(Snail s1, Snail s2) {
	Snail addSnail = new Snail();
	addSnail.snailL = s1;
	s1.up = addSnail;
	addSnail.snailR = s2;
	s2.up = addSnail;
	incDepth(addSnail);
	return addSnail;
}

void incDepth(Snail snail) {
	snail.D += 1;
	if(snail.snailL != null) incDepth(snail.snailL);
	if(snail.snailR != null) incDepth(snail.snailR);
}

void reduce(Snail s) {
	bool didAThing;
	do {
		Console.WriteLine($"{s}");
 		didAThing = false;
		int dumL = -9999, dumR = -9999;
		didAThing = explode(ref dumL, s, ref dumR);
		if(!didAThing) 
			didAThing = split(s);

	} while (didAThing);
}

bool explode(ref int L, Snail snail, ref int R) {
	bool didExplode = false;
	// Diepte groter dan 4 en geen geneste snails?
	if (snail.D > 4 
		&& snail.snailL == null 
		&& snail.snailR == null) 
	{
		L += snail.L;
		R += snail.R;

		if(snail.up.snailL == snail) {
			snail.up.snailL = null;
			snail.up.L = 0;
		} else {
			snail.up.snailR = null;
			snail.up.R = 0;
		}
		didExplode = true;
		Console.WriteLine("BOOM");
	} else {
		// Recurse!
		if(snail.snailL != null && snail.snailR != null) {
			var target = leftMost(snail.snailR);
			didExplode = explode(ref L, snail.snailL, ref target.L);
			if(!didExplode) {
				target = rightMost(snail.snailL);
				didExplode = explode(ref target.R, snail.snailR, ref R);
			}
		}
		else if(snail.snailL != null) 
			didExplode = explode(ref L, snail.snailL, ref snail.R);
		else if(snail.snailR != null) 
			didExplode = explode(ref snail.L, snail.snailR, ref R);
	}
	return didExplode;
}

Snail leftMost(Snail s) {
	if (s.snailL == null) 
		return s;
	else 
		return leftMost(s.snailL);
}

Snail rightMost(Snail s) {
	if (s.snailR == null) 
		return s;
	else 
		return rightMost(s.snailR);
}


bool split(Snail snail) {
	bool didSplit = false;
	if (!didSplit && snail.snailL != null) didSplit = split(snail.snailL);
	if (!didSplit && snail.L > 9) {
		snail.snailL = splitPart(snail, snail.L);
		snail.L = -1;
		didSplit = true;
	} 
	if (!didSplit && snail.snailR != null) didSplit = split(snail.snailR);
	if (!didSplit && snail.R > 9) {
		snail.snailR = splitPart(snail, snail.R);
		snail.R = -1;
		didSplit = true;
	}
	return didSplit;
}

Snail splitPart(
	Snail s, int n
) {
	var newSnail = new Snail();
	newSnail.D = s.D + 1;
	newSnail.L = (int)Round(Floor  (n / 2.0));
	newSnail.R = (int)Round(Ceiling(n / 2.0));
	newSnail.up = s;
	return newSnail;
} 


Snail parseSnailNumbers(StrSpan str) {
	Snail snail = null;
	Snail last = null;
	int d = 0;
	while(str.Length > 0) {
		switch (str[0]) {
			case '[': 
				d++;
				var s2 = new Snail();
				s2.up = snail;
				s2.D = d;
				if (snail != null)
					if(snail.snailL == null && snail.L < 0) {
						snail.snailL = s2;
					} else if (snail.snailR == null && snail.R < 0) {
						snail.snailR = s2;
					}

				snail = s2;
				break;
			case ']': 
				last = snail;
				snail = snail.up;
				d--;	
				break;
			case ',': break;
			default:
				int n = str[0] - '0';
				if (str[1] == ',') {
					snail.L = n;
				} else {
					snail.R = n;
				}
				break;
		}
		str = str.Slice(1);
	}
	return last;
}

long magnitude(Snail s) {
	long m = 0;
	if (s.snailL != null) {
		m += 3 * magnitude(s.snailL);
	} else {
		m += 3 * s.L;
	}
	if (s.snailR != null) {
		m += 2 * magnitude(s.snailR);
	} else {
		m += 2 * s.R;
	}
	return m;
}

// Classes
class Snail {
	public Snail up;
	public Snail snailL;
	public Snail snailR;
	public int L = -1;
	public int R = -1;
	public int D;

	public override string ToString()
	{
		string rslt = "[";
		if(snailL != null) {
			rslt += snailL.ToString();
		} else {
			rslt += L;
		}
		rslt += ",";
		if(snailR != null) {
			rslt += snailR.ToString();
		} else {
			rslt += R;
		}
		rslt += "]";
		return rslt;
	}
}
