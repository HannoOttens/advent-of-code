using static System.Math;
Dictionary<string, long> memory = new();
Dictionary<string, Monkey> forest = new();

(Func<long>,string[]) getCompute(string formula) {
	if (char.IsDigit(formula[0])) 
		return (() => (long.Parse(formula)), new string[0]);
	else {
		string[] parts = formula[5] switch {
			'*' => formula.Split(" * "),
			'/' => formula.Split(" / "),
			'+' => formula.Split(" + "),
			'-' => formula.Split(" - "),
		};
		Func<long> compute = formula[5] switch {
			'*' => () => memory[parts[0]] * memory[parts[1]],
			'/' => () => memory[parts[0]] / memory[parts[1]],
			'+' => () => memory[parts[0]] + memory[parts[1]],
			'-' => () => memory[parts[0]] - memory[parts[1]],
		};
		return (compute, parts);
	}
}

Monkey[] monkeys = File.ReadAllLines("../in.txt")
	.Select(x => x.Split(": "))
	.Select(l => {
		(Func<long> compute, string[] deps) = getCompute(l[1]);
		var m = new Monkey(l[0], compute, deps.ToList());
		forest[m.name] = m;
		return m;
	}).ToArray();

// Binary search naar het juiste getal
long minV = 0;
long maxV = long.MaxValue/1000;
memory["qmfl"] = 1;
memory["qdpj"] = 0;
while (memory["qmfl"] != memory["qdpj"]) {
	memory = new();
	// Pak curr in het midden van min/max
	long curr = (minV + maxV) / 2;
	Console.WriteLine($"Curr: {curr}, Min: {minV}, Max: {maxV}");
	memory["humn"] = curr;
	// Doorgaan tot de root berekend is
	while(!memory.ContainsKey("root")) {
		for (int m = 0; m < monkeys.Length; m++) {
			// Nog niet berekend en alle dependencies beschibaar?
			if (!memory.ContainsKey(monkeys[m].name)
				&& monkeys[m].dependsOn.Select(x => memory.ContainsKey(x)).All(x => x)) 
			{
				memory[monkeys[m].name] = monkeys[m].compute();
			}
		}
	}
	// We weten dat `humn` alleen `qmfl` beinvloed en dat deze groter wordt
	// als `humn` kleiner wordt. Pas MaxV en MinV aan op basis van dat idee.
	if (memory["qmfl"] < memory["qdpj"])
		maxV = curr;
	else
		minV = curr;
}
Console.WriteLine(memory["humn"]); // THIS ONE!

record Monkey(string name, Func<long> compute, List<string> dependsOn);