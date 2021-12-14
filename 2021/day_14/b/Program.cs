// Memory
var memory = new Dictionary<(int,char,char),Dictionary<char, long>>();

// Readin
var lines = File.ReadAllLines("../input.txt");
string polymer = lines[0];

// Read polymer template
var polymerTemplate = new Dictionary<(char,char), char>();
for(int l = 2; l < lines.Length; l++) {
	polymerTemplate.Add(
		(lines[l][0], lines[l][1]),
		lines[l][6]
	);
}

// Init ledger
var ledger = new Dictionary<char, long>(); 
for(int i = 0; i < polymer.Length; i++) ledger.Increase(polymer[i]);

// Expand all combinations
for(int i = 0; i < polymer.Length - 1; i++) {
	Console.WriteLine(i);
	ledger.Increase(expand(0, polymer[i], polymer[i+1]));
}
long quantDiff = (
	ledger.Max(pair => pair.Value) - ledger.Min(pair => pair.Value)
);
Console.WriteLine($"Difference: {quantDiff}");

Dictionary<char, long> expand(int depth, char c1, char c2) {
	var key = (depth,c1,c2);
	if(memory.ContainsKey(key))
		return memory[key];

	var ledger = new Dictionary<char, long>();
	if (depth == 40) return ledger;

	if(polymerTemplate.ContainsKey((c1,c2))) {
		char c3 = polymerTemplate[(c1,c2)];
		ledger.Increase(c3);
		ledger.Increase(expand(depth + 1, c1, c3));
		ledger.Increase(expand(depth + 1, c3, c2));
	}

	memory.Add(key, ledger);
	return ledger;
}

// Extend dict class with 'increase' method
public static class Extensions
{
    public static void Increase(
		this Dictionary<char, long> dictionary, 
		char key)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key]++;
        else
            dictionary.Add(key, 1);
    }

	public static void Increase(this Dictionary<char, long> target, Dictionary<char, long> source)
	{
		foreach(var element in source)
			if (target.ContainsKey(element.Key))
				target[element.Key] += element.Value;
			else
				target.Add(element.Key, element.Value);
	}
}