var lines = File.ReadAllLines("../in.txt");

bool overlap (int a1, int a2, int b1, int b2) 
	=> (a1 >= b1 && a1 <= b2) 
	|| (a2 >= b1 && a2 <= b2)
	|| (b1 >= a1 && b1 <= a2)
	|| (b2 >= a1 && b2 <= a2);

int totl = 0;
foreach (var l in lines) {
	var pairs = l.Split(',').Select((x) => x.Split('-').Select(int.Parse).ToList()).ToList();
	totl += (overlap(pairs[0][0], pairs[0][1], pairs[1][0], pairs[1][1]) ? 1 : 0);
}
Console.WriteLine(totl);