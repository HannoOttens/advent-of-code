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
var ledger = new Dictionary<char, int>(); 
for(int i = 0; i < polymer.Length; i++) ledger.Increase(polymer[i]);

// Expand all combinations
for(int i = 0; i < polymer.Length - 1; i++) {
	expand(0, polymer[i], polymer[i+1], ledger);
}
int quantDiff = (
	ledger.Max(pair => pair.Value) - ledger.Min(pair => pair.Value)
);
Console.WriteLine($"Difference: {quantDiff}");

void expand(int depth, char c1, char c2, Dictionary<char, int> legder) {
	if (depth == 10) return;

	if(polymerTemplate.ContainsKey((c1,c2))) {
		char c3 = polymerTemplate[(c1,c2)];
		ledger.Increase(c3);
		expand(depth + 1, c1, c3, ledger);
		expand(depth + 1, c3, c2, ledger);
	}
}

// Extend dict class with 'increase' method
public static class Extensions
{
    public static void Increase(
		this Dictionary<char, int> dictionary, 
		char key)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key]++;
        else
            dictionary.Add(key, 1);
    }
}
