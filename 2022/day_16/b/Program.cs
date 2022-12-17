using static System.Math;
Dictionary<string, Node> cave = parseCave();
cave = simplify(cave);

// Calculate the value of open so we can cut it off too
ulong allOpen = 0;
int maxFlow = 0;
foreach (var n in cave.Values) 
	if (canOpen(allOpen, n)) {
		maxFlow += n.flowRate;
		allOpen = setBit(allOpen, n.index);
	}
int bestV = 1700;
Console.WriteLine(search("AA", "AA"));

// --------------------------------------------
// Dijkstra gejat van dag 12
// --------------------------------------------
int shortestPath (Dictionary<string, Node> cave, string from, string to) {
	var queue = new PriorityQueue<string, int>(); 
	var seen = new HashSet<string>();
	queue.Enqueue(from, 0);

	int length;
	while (queue.TryDequeue(out string curr, out length)) {
		if (curr == to) break; // Got there!
		if (seen.Contains(curr)) continue; // Al een snellere route gevonden
		seen.Add(curr);

		// Toegestane stappen enqueue
		foreach(var step in cave[curr].connected)
			queue.Enqueue(step.Item1, length + 1);
	}
	return length;
}

// --------------------------------------------
// Simplify the graph
// --------------------------------------------
Dictionary<string, Node> simplify(Dictionary<string, Node> cave) {
	var valueNodes = cave.Values.Where(x => canOpen(0, x)).ToList();
	valueNodes.Add(cave["AA"]); // Also add the starting node

	// Make all combinations of valueNodes
	Dictionary<string, Node> newCave = new();
	foreach (var node in valueNodes) {
		var temp = new Node(
			node.name,
			node.index,
			node.flowRate,
			valueNodes.Where(n2 => n2.name != node.name)
					  .Select(n2 => (n2.name, 1+shortestPath(cave, node.name, n2.name)))
					  .ToList()
		);
		newCave.Add(temp.name, temp);
	}
	
	return newCave;
}

// --------------------------------------------
// Nice and complicated search
// --------------------------------------------
int search (
	string pos, string elePos, 
	int time = 26, int flow = 0, 
	ulong open = 0,
	int currTotal = 0,
	int myBusy = 0, int elBusy = 0) 
{
	// Base condition: Running out of time
	if (time == 0) return currTotal;
	// Base condition: All valves are open
	if (open == allOpen) return currTotal + flow * time;
	// Can it still do better than best? Otherwise give up
	if (currTotal + maxFlow * time <= bestV) return 0;

	// Get nodes
	Node myNo = cave[pos];
	Node elNo = cave[elePos];

	// Make all combinations of steps
	var myBusyList = new List<(string name,int dist)> { ("BUSY", myBusy) };
	var elBusyList = new List<(string name,int dist)> { ("BUSY", elBusy) };
	var steps = 
		from me in (myBusy <= 0 ? myNo.connected : myBusyList)
		from el in (elBusy <= 0 ? elNo.connected : elBusyList)
		where (me.Item1 != el.Item1) || me.Item1 == "BUSY"
		select (me,el);

	// Get the best step to take
	int maxV = 0;
	int index = 1;
	foreach ((var mine, var elep) in steps) {
		if(time == 26) Console.WriteLine($"{index++}/{16*16}");
		string newPos = pos;
		string newElePos = elePos;
		int newFlow = flow;
		ulong newOpen = open;
		
		if (mine.dist == 1) {
			if (!getBit(newOpen, myNo.index)) {

				newOpen = setBit(newOpen, myNo.index);
				newFlow += myNo.flowRate;
			}
		}
		if (elep.dist == 1) {
			if (!getBit(newOpen, elNo.index)) {
			newOpen = setBit(newOpen, elNo.index);
			newFlow += elNo.flowRate;
			}
		}

		// Next nodes
		Node myNxt = myNo;
		Node elNxt = elNo;
		if (mine.name != "BUSY") myNxt = cave[mine.name]; 
		if (elep.name != "BUSY") elNxt = cave[elep.name]; 

		// Dont allow visiting the same node twice per layer
		if ((mine.name != "BUSY" && !canOpen(newOpen, myNxt))
			|| (elep.name != "BUSY" && !canOpen(newOpen, elNxt)))
			continue; 

		maxV = Max(maxV, search(
			myNxt.name,
			elNxt.name,
			time - 1,
			newFlow,
			newOpen,
			currTotal + flow,
			mine.dist - 1,
			elep.dist - 1
		));
		bestV = Max(maxV, bestV);
	}
	return maxV;
}
bool  getBit(ulong open, int indx) => (open & (1UL << indx)) > 0;
ulong setBit(ulong open, int indx) => (open | (1UL << indx));
// Check if it is possible to open a valve
bool canOpen(ulong open, Node n) => !getBit(open, n.index) && n.flowRate > 0;


// --------------------------------------------
// Parsing
// --------------------------------------------
string getName(string str) => str.Substring(6, 2);
int getFlowRat(string str) {
	int indx = str.IndexOf('=')+1;
	return int.Parse(str.Substring(indx, str.IndexOf(';') - indx));
}
List<(string,int)> getConn(string str) {
	List<string> list;
	if (str.IndexOf("valves ") > 0) {
		string conns = str.Substring(str.IndexOf("valves ") + "valves ".Length);
		list = conns.Split(", ").ToList();
	} else {
		string conns = str.Substring(str.IndexOf("valve ") + "valve ".Length);
		list = conns.Split(", ").ToList();
	}
	return list.Select(x => (x,0)).ToList();
}
KeyValuePair<string,Node> parseSensor(string valve, int indx) {
	var name = getName(valve);
	return new(name, new Node(name, indx, getFlowRat(valve), getConn(valve)));
}
Dictionary<string, Node> parseCave ()
	=> new Dictionary<string, Node>(File.ReadAllLines("../in.txt").Select(parseSensor));

enum Actn { Open, Move };
record Node (string name, int index, int flowRate, List<(string name, int dist)> connected);