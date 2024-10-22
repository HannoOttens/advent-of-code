using static System.Math;

var text = File.ReadAllText("../input.txt");
var things = text.Split("\r\n\r\n");
string instructions = things[0];
string[] map = things[1].Split("\r\n");

List<KeyValuePair<string,(string,string)>> pairs
	= map.Select(l => new KeyValuePair<string,(string,string)> (l.Substring(0, 3)
																, (l.Substring(7, 3), l.Substring(12, 3)))
				).ToList();
var dict = new Dictionary<string, (string left,string right)>(pairs);


string node = "AAA";
int count = 0;
while (node != "ZZZ") {
	if (instructions[count % instructions.Length] == 'L')
		node = dict[node].left;
	else
		node = dict[node].right;
	count += 1;
}

Console.WriteLine(count);