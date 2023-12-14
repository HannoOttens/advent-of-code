using static System.Math;

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

int[] copies = new int[cards.Count];
int index = 0;
int lastCopies = 0;
foreach((List<int> win, List<int> mine) in cards) {
	int points = 0;
	foreach (int n in win) 
		if (mine.Contains(n)) 
			points += 1;

	int currentNum = copies[index] + 1;
	if (index+1 == cards.Count) {
		lastCopies = currentNum * points;
	}
	else {
		int offset = 1;
		while (offset <= points) {
			int trgt = Min(cards.Count-1, index + offset);
			copies[trgt] += currentNum;
			offset += 1;
		}
	}

	index += 1;
}

Console.WriteLine(copies.Sum() + cards.Count() + lastCopies);