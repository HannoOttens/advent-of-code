using static System.Math;
Dictionary<string, Node> cave = parseCave();
Console.WriteLine(search("AA"));

int search (string pos, int time = 30, int flow = 0, ulong open = 0, ulong visited = 0) {
	Node node = cave[pos];
	if (getBit(visited, node.index))
		return flow * time; // Return same flow till end of time
	if (time == 0) return 0;

	// Huidige valve nog niet open?
	int maxV = 0;
	if (node.flowRate > 0 && !getBit(open, node.index)) {
		maxV = Max(maxV, search(
			pos,
			time - 1,
			flow + node.flowRate,
			setBit(open, node.index),
			0 // Mag weer naar alles
		));
	}
	// Ook steps maken
	foreach(var next in node.connected) {
		var nxtN = cave[next];
		maxV = Max(maxV, search(
			next,
			time - 1,
			flow,
			open,
			setBit(visited, node.index)
		));
	}
	return maxV + flow;
}
bool  getBit(ulong open, int indx) => (open & (1UL << indx)) > 0;
ulong setBit(ulong open, int indx) => (open | (1UL << indx));

// Parsing
string getName(string str) => str.Substring(6, 2);
int getFlowRat(string str) {
	int indx = str.IndexOf('=')+1;
	return int.Parse(str.Substring(indx, str.IndexOf(';') - indx));
}
string[] getConn(string str) {
	if (str.IndexOf("valves ") > 0) {
		string conns = str.Substring(str.IndexOf("valves ") + "valves ".Length);
		return conns.Split(", ");
	} else {
		string conns = str.Substring(str.IndexOf("valve ") + "valve ".Length);
		return conns.Split(", ");
	}
}
KeyValuePair<string,Node> parseSensor(string valve, int indx) {
	var name = getName(valve);
	return new(name, new Node(name, indx, getFlowRat(valve), getConn(valve)));
}
Dictionary<string, Node> parseCave ()
	=> new Dictionary<string, Node>(File.ReadAllLines("../in.txt").Select(parseSensor));

record Node (string name, int index, int flowRate, string[] connected);