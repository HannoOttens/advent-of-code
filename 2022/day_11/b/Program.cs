
Monkey[] monkeys = parseMonkeys();
ulong[] buisiness = new ulong[monkeys.Length];

ulong divNum = 1;
foreach(Monkey m in monkeys) divNum *= m.testNum;

for (int round = 0; round < 10_000 * monkeys.Length; round++) {
	int indx = round % monkeys.Length;
	var monkey = monkeys[indx];
	buisiness[indx] += (ulong)monkey.items.Count;
	while (monkey.items.Count > 0) {
		ulong newI = monkey.op(monkey.items.Dequeue()) % divNum;
		int target = newI % monkey.testNum == 0
				   ? monkey.trueMonkey
				   : monkey.falseMonkey;
		monkeys[target].items.Enqueue(newI);
	}
}
Array.Sort(buisiness);
Console.WriteLine(buisiness[monkeys.Length-1]
				* buisiness[monkeys.Length-2]);

Monkey[] parseMonkeys () {
	List<Monkey> monkeys = new();
	string input = File.ReadAllText("../in.txt");
	foreach(string monkeyStr in input.Split("\r\n\r\n")) {
		string[] monkey = monkeyStr.Split("\r\n");
		monkeys.Add(new(
			new Queue<ulong>(monkey[0].Split(", ").Select(ulong.Parse)),
			parseExpr(monkey[1]),
			ulong.Parse(monkey[2]),
			int.Parse(monkey[3]),
			int.Parse(monkey[4])
		));
	}
	return monkeys.ToArray();
} 

Func<ulong,ulong> parseExpr(string expr) {
	string[] parts = expr.Split(' ');
	bool oldR = (parts[2] == "old");
	ulong valu = 0;
	if (!oldR) valu = ulong.Parse(parts[2]);
	return (parts[1], oldR) switch {
		("*", true ) => x => x * x,
		("*", false) => x => x * valu,
		("+", true ) => x => x + x,
		("+", false) => x => x + valu,
	};
}

record Monkey (
	Queue<ulong> items, 
	Func<ulong,ulong> op, 
	ulong testNum, 
	int trueMonkey, 
	int falseMonkey);