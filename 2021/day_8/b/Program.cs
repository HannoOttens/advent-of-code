using System.Diagnostics;
using static System.Math;

// A bevat segmenten B
bool Contains(string a, string b) {
	foreach(char c in b) {
		if (!a.Contains(c)) return false;
	}
	return true;
}

// Zet in A de segmenten van B uit
string Disable(string a, string b) {
	string rslt = "";
	foreach(char c in a) {
		if (!b.Contains(c)) rslt += c;
	}
	return rslt;
}

string Sort(string a) {
	var chars = a.ToCharArray();
	Array.Sort(chars);
	return new string(chars);
}

// Start
var lines = File.ReadAllLines("../input.txt");

var codes = new List<List<string>>();
var numbs = new List<List<string>>();
for(int l = 0; l < lines.Length; l++) {
	codes.Add(lines[l].Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
	numbs.Add(lines[l].Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
}

var stopwatch = new Stopwatch();
stopwatch.Start();

int total = 0;
for(int i = 0; i < numbs.Count; i++) {
	// Decode
	string one   = "";
	string four  = "";
	string seven = "";
	string eight = "";
	for(int d = 0; d < codes[i].Count; d++) {
		switch (codes[i][d].Length) {
			case 2: one   = codes[i][d]; break;
			case 4: four  = codes[i][d]; break;
			case 3: seven = codes[i][d]; break;
			case 7: eight = codes[i][d]; break;
		}
	}

	var decoder = new Dictionary<string, int>();
	decoder.Add(Sort(one  ), 1);
	decoder.Add(Sort(four ), 4);
	decoder.Add(Sort(seven), 7);
	decoder.Add(Sort(eight), 8);
	for(int d = 0; d < codes[i].Count; d++) {
		switch (codes[i][d].Length) {
			case 5: // 2, 3, 5
				if (Contains(codes[i][d], one)) {
					decoder.Add(Sort(codes[i][d]), 3);
				} else if (Contains(codes[i][d], Disable(four, one))) {
					decoder.Add(Sort(codes[i][d]), 5);
				} else {
					decoder.Add(Sort(codes[i][d]), 2);
				}
				break;
			case 6: // 0, 6, 9
				if (!Contains(codes[i][d], seven)) { // 6
					decoder.Add(Sort(codes[i][d]), 6);
				} else if(Contains(codes[i][d], seven) && Contains(codes[i][d], four)) {
					decoder.Add(Sort(codes[i][d]), 9);
				} else {
					decoder.Add(Sort(codes[i][d]), 0);
				}
				break;
		}
	}
	// Add to result
	for(int d = 0; d < numbs[i].Count; d++) {
		total += decoder[Sort(numbs[i][d])] * (int)Pow(10, 3 - d);
	}
}

stopwatch.Stop();
Console.WriteLine($"Total unique: {total}");
// drawScreen(screen);
Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds/1000.0}s");