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

int routes = explore(new Stack<Cave>(), caves[index["start"]]);
Console.WriteLine($"Routes: {routes}");

int explore(Stack<Cave> seen, Cave cave) {
	// Einde gevonden, 1 stap dus
	if(cave.name == "end") return 1;
	// Anders alleen door als groot of niet gezien
	if(!cave.big && seen.Contains(cave)) return 0;

	if(!cave.big) seen.Push(cave);
	int result = 0;
	foreach(var child in cave.connectedTo)
		result += explore(seen, child);
	if(!cave.big) seen.Pop();

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

	public Cave(string name) {
		this.big = char.IsUpper(name[0]);
		this.name = name;
		this.connectedTo = new List<Cave>();
	}
}