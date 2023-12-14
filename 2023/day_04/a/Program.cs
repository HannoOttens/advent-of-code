var text = File.ReadAllText("../input.txt");
text = text.Replace("  ", " ");
text = text.Replace(" | ", "|");
var lines = text.Split('\n');
lines = lines.Select(l => l.Substring(l.IndexOf(": ")+2)).ToArray();
List<(List<int>,List<int>)> cards = lines.Select(l => {
	var parts = l.Split("|"); 
	return (parts[0].Split(' ').Select(int.Parse).ToList()
		  , parts[1].Split(' ').Select(int.Parse).ToList());
}
).ToList();

int total = 0;
foreach((List<int> win, List<int> mine) in cards) {
	int points = 1;
	foreach(int n in win)
		if (mine.Contains(n))
			points <<= 1;
	if (points > 1) total += points / 2;
}

Console.WriteLine(total);