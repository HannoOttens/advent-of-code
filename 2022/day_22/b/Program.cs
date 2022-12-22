using static System.Math;
using System.Text;
using System.Text.RegularExpressions;

var connected = new int[6,4];
const int TOP = 3, RGH = 0, BOT = 1, LFT = 2;
connected[0, TOP] = 5; 
connected[0, LFT] = 3; 

connected[1, TOP] = 5; 
connected[1, RGH] = 4; 
connected[1, BOT] = 2; 

connected[2, RGH] = 1; 
connected[2, LFT] = 3; 

connected[3, TOP] = 2; 
connected[3, LFT] = 0; 

connected[4, RGH] = 1; 
connected[4, BOT] = 5; 

connected[5, RGH] = 4; 
connected[5, BOT] = 1; 
connected[5, LFT] = 0; 

// Read/parse inputs
string[] mapStr = File.ReadAllLines("../map.txt");

int blockSize;
if (mapStr[0].Length < 50)
	blockSize = 4; // Demo demo
else
	blockSize = 50; // Real deal!

// Get maxX/maxY
int maxY = mapStr.Length;
int maxX = 0;
for(int y = 0; y < maxY; y++) 
	maxX = Max(mapStr[y].Length, maxX);

// Fill the dict
Dictionary<Posi, char> map = new();
for(int y = 0; y < maxY; y++) 
for(int x = 0; x < maxX; x++)
	map[new(x,y)] = mapStr[y].Length > x ? mapStr[y][x] : ' ';


// Find faces
Posi[] faces = new Posi[6];
Dictionary<Posi, int> faceMap = new ();
int faceIndx = 0;
for(int bY = 0; bY < maxY/blockSize; bY++) 
for(int bX = 0; bX < maxX/blockSize; bX++)
{
	Posi facePos = new(bX, bY);
	faceMap[facePos] = -1;
	if (map[new(bX*blockSize, bY*blockSize)] != ' ') {
		faces[faceIndx] = facePos;
		faceMap[facePos] = faceIndx;
		faceIndx++;
	} 
}

// Parse commands
string instrTxt = File.ReadAllText("../in.txt");
Regex reg = new Regex(@"(\d+)|[A-Z]");
var instr = reg.Matches(instrTxt);  
Dictionary<Posi, int> retrace = new();

// Directions
var dir = new Posi[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1)};

// Initial pos
int startX = 0;
while (map[new(startX,0)] != '.') startX++;
Posi curr = new(startX, 0); 
int rota = 0;
Console.WriteLine($"Start: {curr}");

// Run through all commands
for (int i = 0; i < instr.Count; i++) {
	switch (instr[i].Value) {
		case "L": rota--; break;
		case "R": rota++; break;
		default:
			int steps = int.Parse(instr[i].Value);
			bool stop = false;
			while (!stop && (steps-- > 0)) {
				(stop, curr, rota) = makeStep(curr, rota);
				retrace[curr] = rota;
			}
			break;
	} 
	rota %= dir.Length;
	if (rota < 0) rota = dir.Length-1;
	retrace[curr] = rota;
}
draw();
Console.WriteLine($"Posi: {curr}, Rota: {rota}");
Console.WriteLine((curr.PosY+1)*1000 + (curr.PosX+1)*4 + rota );
// HOGER DAN: 46208

bool boundsCheck(Posi curr) 
	=> ((curr.PosX>=0) && (curr.PosY>=0) && (curr.PosX<maxX) && (curr.PosY<maxY))
	&& (map[curr] != ' ');

(bool stop, Posi posn, int rota) makeStep(Posi curr, int rota) {
	var next = curr + dir[rota];
	int nRot = rota;
	if (!boundsCheck(next)) { // Out of bounds -> Time to wrap!
		Posi fPos = new(curr.PosX/blockSize, curr.PosY/blockSize);

		// Where did we come from?
		int currFace = faceMap[fPos];
		int nextIndx = connected[currFace, rota];
		Posi nextFace = faces[nextIndx]; 

		// Bereken lokale positie van curr op face
		Posi locl = new(curr.PosX%blockSize, curr.PosY%blockSize);

		// Berekenen lokale positie op nextFace
		(Posi nLcl, nRot) = ((currFace, rota) switch {
			(0, TOP) => (new Posi(0, locl.PosX   ), RGH),
			(0, LFT) => (new Posi(0, 49-locl.PosY), RGH),
		
			(1, TOP) => (new Posi(locl.PosX, 49   ), TOP), // 1->5
			(1, RGH) => (new Posi(49, 49-locl.PosY), LFT), // 1->4
			(1, BOT) => (new Posi(49, locl.PosX   ), LFT), // 1->2
		
			(2, LFT) => (new Posi(locl.PosY, 0 ), BOT), // 2->3
			(2, RGH) => (new Posi(locl.PosY, 49), TOP), // 2->1
		
			(3, LFT) => (new Posi(0 , 49-locl.PosY), RGH), // 3->0
			(3, TOP) => (new Posi(0 , locl.PosX   ), RGH), // 3->2
			
			(4, RGH) => (new Posi(49, 49-locl.PosY), LFT), // 4->1
			(4, BOT) => (new Posi(49, locl.PosX   ), LFT), // 4->5
			
			(5, LFT) => (new Posi(locl.PosY, 0 ), BOT), // 4->1
			(5, BOT) => (new Posi(locl.PosX, 0 ), BOT), // 4->5
			(5, RGH) => (new Posi(locl.PosY, 49), TOP), // 4->5
		});
		// Van local naar global
		next = new(
			nLcl.PosX + nextFace.PosX * blockSize,
			nLcl.PosY + nextFace.PosY * blockSize
		);
	}

	if (map[next] == '#')
		return (true , curr, rota);
	else 
		return (false, next, nRot);
}

void draw() {
	var sb = new StringBuilder();
	for (int y = 0; y < maxY; y++) {
		for(int x = 0; x < maxX; x++)
		{
			if (retrace.ContainsKey(new(x,y))) {
				char arr = retrace[new(x,y)] switch {
					0 => '>',
					1 => 'v',
					2 => '<',
					3 => '^',
				};
				sb.Append(arr);
			}
			else sb.Append(map[new(x,y)]);
		}
		sb.AppendLine();
	}
	Console.WriteLine(sb.ToString());
}

public record Posi(int PosX = 0, int PosY = 0)
{
	public static Posi operator +(Posi a, Posi b) => new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b) => new(a.PosX - b.PosX, a.PosY - b.PosY);
}