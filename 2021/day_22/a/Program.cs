using static System.Math;
var sr = new StreamReader("../input.txt");

var cuboidCommands = new LinkedList<CuboidCommand>();
while(sr.Peek() >= 0) cuboidCommands.AddLast(parseCuboidCommand(sr));

// Start to program!
var cuboids = new List<Cuboid>();
processCommnands(cuboidCommands, cuboids);

long on = 0;
foreach(var c in cuboids) {
	Cuboid ovlp = overlap(c, new Cuboid() { x = (-50, 50), y = (-50, 50), z = (-50, 50) });
	if(isValid(ovlp)) {
		on += Abs((ovlp.x.hi - ovlp.x.lo) * (ovlp.y.hi - ovlp.y.lo) * (ovlp.z.hi - ovlp.z.lo));
		Console.WriteLine($"{ovlp}");
	}
}
Console.WriteLine($"On: {on}");


void processCommnands(LinkedList<CuboidCommand> commands, List<Cuboid> cuboids) {
	bool first = true;
	foreach(var command in commands) {
		if (first && command.on) {
			cuboids.Add(command.cuboid);
			first = false;
		} else {
			var cubuff = new List<Cuboid>();
			var delCubes = new List<Cuboid>();
			LinkedList<CuboidCommand> extraCommands = null;
			foreach(var cube in cuboids) {
				// Commando cuboid in zit andere cube
				if(contains(cube, command.cuboid)) {
					if(!command.on)  {
						cubuff.AddRange(splitOff(cube, command.cuboid, false));
						delCubes.Add(cube);
					}
					break;
				} else if (intersect(cube, command.cuboid)) {
					if(command.on) {
						extraCommands = splitOverlap(cube, command.cuboid);
						break;
					} else {
						cubuff.AddRange(splitOff(cube, command.cuboid, false));
						delCubes.Add(cube);
					}
				} else {
					if(command.on) cubuff.Add(command.cuboid);
					break;
				}
			}
			foreach(var cube in delCubes) cuboids.Remove(cube);
			cuboids.AddRange(cubuff);
			if (extraCommands != null) processCommnands(extraCommands, cuboids);
		}
	}
}


// Split a on command in piece such that none overlap
LinkedList<CuboidCommand> splitOverlap(Cuboid cube, Cuboid offC) {
	var cubes = splitOff(offC, cube, true);
	var commands = cubes.Select(c => new CuboidCommand() { on = true, cuboid = c});
	return new LinkedList<CuboidCommand>(commands);
}

bool isValid(Cuboid cube) {
	return cube.x.lo <= cube.x.hi && cube.y.lo <= cube.y.hi && cube.z.lo <= cube.z.hi;
}

// Split it like
// AA|AA|AA
// AA|AA|AA
// AA|--|AA
// AA|BB|AA
// AA|BB|AA
// AA|--|AA
// AA|AA|AA
// AA|AA|AA
List<Cuboid> splitOff(Cuboid cube, Cuboid offC, bool alsoOverlap) {
	var splitCuboids = new List<Cuboid>();
	// Grote stukken links en rechts
	Cuboid xsp1;
	xsp1.x.lo = cube.x.lo;
	xsp1.x.hi = offC.x.lo - 1;
	xsp1.y.lo = cube.y.lo;
	xsp1.y.hi = cube.y.hi;
	xsp1.z.lo = cube.z.lo;
	xsp1.z.hi = cube.z.hi;
	if(isValid(xsp1)) splitCuboids.Add(xsp1);
	Cuboid xsp2;
	xsp2.x.lo = offC.x.hi + 1;
	xsp2.x.hi = cube.x.hi;
	xsp2.y.lo = cube.y.lo;
	xsp2.y.hi = cube.y.hi;
	xsp2.z.lo = cube.z.lo;
	xsp2.z.hi = cube.z.hi;
	if(isValid(xsp2)) splitCuboids.Add(xsp2);
	// Grote stukken onder/boven
	Cuboid zsp1;
	zsp1.x.lo = Max(cube.x.lo, offC.x.lo);
	zsp1.x.hi = Min(cube.x.hi, offC.x.hi);
	zsp1.y.lo = cube.y.lo;
	zsp1.y.hi = cube.y.hi;
	zsp1.z.lo = cube.z.lo;
	zsp1.z.hi = offC.z.lo - 1;
	if(isValid(zsp1)) splitCuboids.Add(zsp1);
	Cuboid zsp2;
	zsp2.x.lo = Max(cube.x.lo, offC.x.lo);
	zsp2.x.hi = Min(cube.x.hi, offC.x.hi);
	zsp2.y.lo = cube.y.lo;
	zsp2.y.hi = cube.y.hi;
	zsp2.z.lo = offC.z.hi + 1;
	zsp2.z.hi = cube.z.hi;
	if(isValid(zsp2)) splitCuboids.Add(zsp2);
	// Kleinste stukjes voor/achter
	Cuboid ysp1;
	ysp1.x.lo = Max(cube.x.lo, offC.x.lo);
	ysp1.x.hi = Min(cube.x.hi, offC.x.hi);
	ysp1.y.lo = cube.y.lo;
	ysp1.y.hi = offC.y.lo - 1;
	ysp1.z.lo = Max(cube.z.lo, offC.z.lo);
	ysp1.z.hi = Min(cube.z.hi, offC.z.hi);
	if(isValid(ysp1)) splitCuboids.Add(ysp1);
	Cuboid ysp2;
	ysp2.x.lo = Max(cube.x.lo, offC.x.lo);
	ysp2.x.hi = Min(cube.x.hi, offC.x.hi);
	ysp2.y.lo = offC.y.hi + 1;
	ysp2.y.hi = cube.y.hi;
	ysp2.z.lo = Max(cube.z.lo, offC.z.lo);
	ysp2.z.hi = Min(cube.z.hi, offC.z.hi);
	if(isValid(ysp2)) splitCuboids.Add(ysp2);
	if(alsoOverlap) {
		Cuboid ovlp = overlap(cube, offC);
		splitCuboids.Add(ovlp);
	}
	return splitCuboids;
}


// Check if c1 contains c2 (c2 is fully contained in c1 if true)
bool contains(in Cuboid c1, in Cuboid c2) {
	return (c1.x.lo >= c2.x.lo && c1.x.hi <= c2.x.hi) 
		&& (c1.y.lo >= c2.y.lo && c1.y.hi <= c2.y.hi)
		&& (c1.z.lo >= c2.z.lo && c1.z.hi <= c2.z.hi);
}

// Check if two cuboids intersect
bool intersect(in Cuboid c1, in Cuboid c2) {
	Cuboid ovlp = overlap(c1,c2);
	return isValid(ovlp);
}


Cuboid overlap(in Cuboid c1, in Cuboid c2) {
	Cuboid ovlp;
	ovlp.x.lo = Max(c1.x.lo, c2.x.lo);
	ovlp.x.hi = Min(c1.x.hi, c2.x.hi);
	ovlp.y.lo = Max(c1.y.lo, c2.y.lo);
	ovlp.y.hi = Min(c1.y.hi, c2.y.hi);
	ovlp.z.lo = Max(c1.z.lo, c2.z.lo);
	ovlp.z.hi = Min(c1.z.hi, c2.z.hi);
	return ovlp;
}
	
/////////////
// Parsing //
CuboidCommand parseCuboidCommand(StreamReader sr) {
	CuboidCommand cc;
	cc.on = parseSwitch(sr, "on", "off");
	parseChar(sr, ' ');
	cc.cuboid.x = parseBounds(sr, 'x');
	parseChar(sr, ',');
	cc.cuboid.y = parseBounds(sr, 'y');
	parseChar(sr, ',');
	cc.cuboid.z = parseBounds(sr, 'z');
	parseNewLine(sr);
	return cc;
}

// Lekker overkill
bool parseSwitch(StreamReader sr, string trueStr, string falseStr) {
	int i = 0;
	while(i < trueStr.Length && trueStr[i] == (char)sr.Peek()) {
		sr.Read(); i++;
	}
	if (i == trueStr.Length) return true;
	while(i < falseStr.Length && falseStr[i] == (char)sr.Peek()) {
		sr.Read(); i++;
	}
	if (i == falseStr.Length) return false;
	char c = (char)sr.Peek();
	throw new Exception($"Expected '{trueStr}' or '{falseStr}' but got '{(char)sr.Peek()}'");
}

(int lo, int hi) parseBounds(StreamReader sr, char axis) {
	parseChar(sr, axis);
	parseChar(sr, '=');
	int lo = parseInt(sr);
	parseChar(sr, '.');
	parseChar(sr, '.');
	int hi = parseInt(sr);
	return (lo, hi);
}

// Generic stuff
void parseString(StreamReader sr, string str) {
	foreach(char c in str) parseChar(sr, c);
}
void parseChar(StreamReader sr, char c) {
	if (sr.Peek() != c) 
		throw new Exception($"Expected '{c}' but got '{(char)sr.Peek()}'");
	sr.Read();
}
int parseInt(StreamReader sr) {
	int i = 0;
	Span<char> nmbr = stackalloc char[11];
	while(char.IsDigit((char)sr.Peek()) || (sr.Peek() == '-')) {
		if(i>=11) throw new Exception($"Number too large for an integer: '{nmbr}...'");
		nmbr[i++] = (char)sr.Read();	
	} 
	int n = int.Parse(nmbr.Slice(0,i));
	return n;
}
void parseNewLine(StreamReader sr) {
	parseChar(sr, '\r');
	parseChar(sr, '\n');
}
void AddAllBefore<T>(LinkedList<T> b, LinkedList<T> a) {
	foreach(var item in b) a.AddFirst(item);
}

// Classes
struct CuboidCommand {
	public bool on;
	public Cuboid cuboid;
}

struct Cuboid {
	public (int lo, int hi) x;
	public (int lo, int hi) y;
	public (int lo, int hi) z;

	public override string ToString() {
		return $"x={x.lo}..{x.hi},\ty={y.lo}..{y.hi},\tz={z.lo}..{z.hi}";
	}
}
