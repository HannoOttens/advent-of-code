using StrSpan = System.ReadOnlySpan<char>;

var decoder = new Dictionary<char, string>() {
	{ '0', "0000"},
	{ '1', "0001"},
	{ '2', "0010"},
	{ '3', "0011"},
	{ '4', "0100"},
	{ '5', "0101"},
	{ '6', "0110"},
	{ '7', "0111"},
	{ '8', "1000"},
	{ '9', "1001"},
	{ 'A', "1010"},
	{ 'B', "1011"},
	{ 'C', "1100"},
	{ 'D', "1101"},
	{ 'E', "1110"},
	{ 'F', "1111"}
};

var input = File.ReadAllLines("../input.txt")[0];
var decodedInput = input.Select(c => decoder[c]).Aggregate((s, r) => s + r);
StrSpan span = decodedInput.ToCharArray();
var rslt = readPacket(ref span);
Console.WriteLine($"{rslt.rslt}");				 


(int vers, int type, int rslt) readPacket(ref StrSpan s) {
	int vers = readVersion(ref s);
	int type = readType(ref s);
	int rslt = vers;
	if (type == 4) {
		// Literals
		int dumm = readLiteral(ref s);
	} else {
		// Operators
		int lngT = lengthTypeId(ref s);
		if (lngT == 0) {
			// Number of bits
			int lgth = length(ref s);
			int strt = s.Length;
			while(strt - s.Length < lgth) {
				(int v1, int t1, int r1) = readPacket(ref s);
				rslt += r1;
			}
		} else {
			// Number of packets
			int subPacks = subPackets(ref s);
			while(subPacks-- > 0) {
				(int v1, int t1, int r1) = readPacket(ref s);
				rslt += r1;
			}
		}
	}
	return (vers, type, rslt);
}

int readVersion(ref StrSpan s) => takeInt(ref s, 3);
int readType(ref StrSpan s) => takeInt(ref s, 3);
int lengthTypeId(ref StrSpan s) => takeInt(ref s, 1);
int length(ref StrSpan s) => takeInt(ref s, 15);
int subPackets(ref StrSpan s) => takeInt(ref s, 11);

int readLiteral(ref StrSpan s) {
	int indx = 0;
	Span<char> nmbr = stackalloc char[64];
	StrSpan last;
	do {
			last = takeN(ref s, 1);
		var part = takeN(ref s, 4);
		part.CopyTo(nmbr.Slice(indx, 4));
		indx += 4;
	} while(last[0] == '1');
	nmbr = nmbr.Slice(0, indx);
	return Convert.ToInt32(nmbr.ToString(), 2);
}

int takeInt(ref StrSpan s, int count) {
	var bits = takeN(ref s, count);
	var n = Convert.ToInt32(bits.ToString(), 2);
	return n;
}

StrSpan takeN(ref StrSpan s, int count) {
	var bits = s.Slice(start: 0, length: count); 
	s = s.Slice(start: count);
	return bits;
}
