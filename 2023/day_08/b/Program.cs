using static System.Math;

// Parsing
var text = File.ReadAllText("../input.txt");
var things = text.Split("\r\n\r\n");
string instructions = things[0];
string[] map = things[1].Split("\r\n");
// Make a dict out of it
List<KeyValuePair<string,(string,string)>> pairs
	= map.Select(l => new KeyValuePair<string,(string,string)> (l.Substring(0, 3)
																, (l.Substring(7, 3), l.Substring(12, 3)))
				).ToList();
var dict = new Dictionary<string, (string left,string right)>(pairs);
// Select nodes ending in a
var nodes = pairs.Select(kv => kv.Key).Where(n => n[2] == 'A');

var steps = nodes.Select(FindZNode).ToArray();

long total_lcm = steps.Aggregate(1L, lcm);
Console.WriteLine(total_lcm);


string makeStep(int step, string node) =>
	(instructions[step % instructions.Length] == 'L'
		? dict[node].left
		: dict[node].right);

long FindZNode(string node) {
	int count = 0;
	while (node[2] != 'Z') {
		node = makeStep(count, node);
		count += 1;
	}
	return count;
}

long gcd(long a, long b) => (b == 0 ? a : gcd(b, a % b));
long lcm(long a, long b) => (a / gcd(a, b)) * b; 