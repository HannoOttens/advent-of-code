using static System.Math;
Dictionary<string, long> memory = new();

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
		return new Monkey(l[0], compute, deps.ToList());
	}).ToArray();


while(!memory.ContainsKey("root")) {
	for (int m = 0; m < monkeys.Length; m++) {
		if (memory.ContainsKey(monkeys[m].name)) continue;
		if (monkeys[m].dependsOn.Select(x => memory.ContainsKey(x)).All(x => x)) {
			memory[monkeys[m].name] = monkeys[m].compute();
		}
	}
}
Console.WriteLine(memory["root"]);

record Monkey(string name, Func<long> compute, List<string> dependsOn);