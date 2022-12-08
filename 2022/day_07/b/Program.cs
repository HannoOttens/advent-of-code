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
			curr.Chld.Add(new Node() {
				Up = curr,
				Name = parts[1],
				Size = (int.TryParse(parts[0], out int fSiz) ? fSiz : 0),
			});
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

// Find smallest directory that frees up enough space
int free = 70_000_000 - root.Size;
int need = Math.Abs(free - 30_000_000);

int smallestDir = int.MaxValue;
void dirDelete (Node n) {
	if (n.Chld.Count > 0 && n.Size > need)
		smallestDir = Math.Min(smallestDir, n.Size);
	foreach(Node node in n.Chld) 
		dirDelete(node);
}

dirDelete(root);
Console.WriteLine(smallestDir);

class Node {
	public string Name = "";
	public int Size = 0;
	public Node? Up;
	public List<Node> Chld = new();
}
