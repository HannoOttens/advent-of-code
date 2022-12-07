string[] cmds = File.ReadAllLines("../in.txt");


Node root = new();
root.Name = "/";
Node curr = root;

// Build tree
foreach(string comd in cmds) {
	var parts = comd.Split(' ');
	switch (parts[0] + " " + parts[1]) {
		case "$ cd":
			curr = parts[2] switch {
				".." => curr.Up,
				"/"  => root,
				_    => curr.Chld.Find((x) => x.Name == parts[2])
			};
			break;
		case "$ ls":
			break;
		default:
			Node n = new();
			n.Up = curr;
			n.Name = parts[1];
			if (int.TryParse(parts[0], out int Size))
				n.Size = Size;
			if (!curr.Chld.Any((x) => x.Name == n.Name))
				curr.Chld.Add(n);
			break;
	}
}

// Traverse tree en set dir sizes
int dirSizes (Node n) {
	int size = 0;
	foreach(Node node in n.Chld) 
		size += dirSizes(node);
	n.Size += size;
	return n.Size;
}
dirSizes(root);

// Sum up all directory sizes
int dir100k (Node n) {
	int size = 0;
	foreach(Node node in n.Chld) 
		size += dir100k(node);
	if (n.Chld.Count > 0 && n.Size < 100_000)
		size += n.Size;
	return size;
}
Console.WriteLine(dir100k(root));

class Node {
	public string Name;
	public int Size = 0;
	public Node Up;
	public List<Node> Chld = new();
}
