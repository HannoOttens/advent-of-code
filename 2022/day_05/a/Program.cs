var lines = File.ReadAllText("../in.txt");
var parts = lines.Split("\r\n\r\n").ToList();

// Parse the stacks
Stack<char>[] stck = new Stack<char>[9] { new(), new(), new(), new(), new(), new(), new(), new(), new() }; 
foreach(var line in parts[0].Split("\r\n").Take(8).Reverse()) {
	var blks = line.Chunk(4).ToList(); 
	for (int i = 0; i < 9; i++)  {
		if (blks[i][0] == '[') stck[i].Push(blks[i][1]);
	}
}

// Run the input
foreach(var line in parts[1].Split("\r\n")) {
	Console.WriteLine(line);
	var cmnd = line.Split(' ').Where((_, indx) => indx % 2 != 0).Select(int.Parse).ToList();
	int numb = cmnd[0];
	int from = cmnd[1] - 1;
	int to   = cmnd[2] - 1;
	while (numb > 0) {
		stck[to].Push(stck[from].Pop());
		numb--;
	}
}

for(int i = 0; i < stck.Length; i++)
	Console.Write(stck[i].Pop());