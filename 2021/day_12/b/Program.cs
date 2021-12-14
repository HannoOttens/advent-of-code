using System.Diagnostics;
using static System.Math;

// Globals

// Readin
var caves = new List<Cave>();
var index = new Dictionary<string, int>();
var lines = File.ReadAllLines("../input.txt");
foreach(var line in lines) {
	string[] parts = line.Split('-');
	int idxA = addCave(caves, index, parts[0]);
	int idxB = addCave(caves, index, parts[1]);
	caves[idxA].connectedTo.Add(caves[idxB]);
	caves[idxB].connectedTo.Add(caves[idxA]);
}

int routes = explore(caves[index["start"]]);
Console.WriteLine($"Routes: {routes}");

int explore(Cave cave, bool didTwice=false) {
	// Einde gevonden, 1 stap dus
	if(cave.name == "end") return 1;
	// Anders alleen door als groot of niet gezien
	bool didVisit = (cave.seen > 0 && didTwice) 
				 || (cave.seen > 0 && cave.name == "start");
	if(!cave.big && didVisit) return 0;

	if (!cave.big) cave.seen += 1;
	if (cave.seen == 2) didTwice = true;
	
	int result = 0;
	foreach(var child in cave.connectedTo)
		result += explore(child, didTwice);
	
	if(!cave.big) cave.seen -= 1;

	return result;
}

int addCave(
	List<Cave> caves, 
	Dictionary<string, int> index, 
	string name) 
{
	if(!index.ContainsKey(name)) {
		index.Add(name, caves.Count);
		caves.Add(new Cave(name));
	}
	return index[name];
}

class Cave {
	public bool big;
	public string name;
	public List<Cave> connectedTo;
	public int seen;

	public Cave(string name) {
		this.big = char.IsUpper(name[0]);
		this.name = name;
		this.connectedTo = new List<Cave>();
		this.seen = 0;
	}
}