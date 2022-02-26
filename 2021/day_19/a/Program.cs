using StrSpan = System.ReadOnlySpan<char>;

var text = File.ReadAllText("../input.txt");
var span = text.AsSpan();
parseScanners(ref span);


List<Scanner> parseScanners(ref StrSpan str) {
	var scanners = new List<Scanner>();
	while(str.Length > 0) {
		scanners.Add(parseScanner(ref str));
	}
	return scanners;
}

Scanner parseScanner(ref StrSpan str) {
	Scanner s;
	str.Slice(3); //---
	str.Slice(" scanner ".Length);
	s.id = parseInt(ref str);
	str.Slice(4); //---\n
	s.beacons = new List<(int x, int y, int z)>();
	while(str[0] != '\n') {
		s.beacons.Add(parseBeacon(ref str));
	}
	str.Slice(2); //\n\n
	return s;
}

(int,int,int) parseBeacon(ref StrSpan str) {
	int x = parseInt(ref str);
	str = str.Slice(1); // ,
	int y = parseInt(ref str);
	str = str.Slice(1); // ,
	int z = parseInt(ref str);
	str = str.Slice(1); // \n
	return (x,y,z);
}

int parseInt(ref StrSpan str) {
	int i = 0;
	while(char.IsDigit(str[i]) || (str[i] == '-')) i++;	
	var t = str.Slice(0, i);
	int n = int.Parse(str.Slice(0, i));
	str = str.Slice(i);
	return n;
}


struct Scanner {
	public int id;
	public List<(int x, int y, int z)> beacons;
}