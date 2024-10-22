using static System.Math;

const int FIVE_SAME = 0x6_00000;
const int FOUR_SAME = 0x5_00000;
const int FULL_HOUS = 0x4_00000;
const int THRE_SAME = 0x3_00000;
const int TWO_PAIRS = 0x2_00000;
const int ONE_PAIRS = 0x1_00000;
const int HIGH_CARD = 0x0_00000;

int charHex(char c) {
	return c switch {
		'2' => 0x2,
		'3' => 0x3,
		'4' => 0x4,
		'5' => 0x5,
		'6' => 0x6,
		'7' => 0x7,
		'8' => 0x8,
		'9' => 0x9,
		'T' => 0xA,
		'J' => 0xB,
		'Q' => 0xC,
		'K' => 0xD,
		'A' => 0xE
	};
}

int handScore (string hand) {
	int score = 0;

	Dictionary<char, int> dict = new();
	for(int index = 1; index <= hand.Length; index++) {
		// Char to dict
		var c = hand[index-1];
		if (dict.ContainsKey(c))
			dict[c] += 1;
		else
			dict.Add(c, 1);

		// Add high-card value
		int cVal = charHex(c);
		score += cVal << 4 * (hand.Length - index);
	}

	var values = dict.Values.ToList();
	values.Sort();
	values.Reverse();
	
	if (values[0] == 5)
		score += FIVE_SAME;
	else if (values[0] == 4)
		score += FOUR_SAME;
	else if ((values[0] == 3) && (values[1] == 2))
		score += FULL_HOUS;
	else if (values[0] == 3)
		score += THRE_SAME;
	else if ((values[0] == 2) && (values[1] == 2))
		score += TWO_PAIRS;
	else if (values[0] == 2)
		score += ONE_PAIRS;
	else 
		score += HIGH_CARD;

	return score;
}

var groups = File.ReadAllLines("../input.txt").Select(l => l.Split(' ').ToArray());
var valueHands = groups.Select(g => (handScore(g[0]), int.Parse(g[1])));
var orderdHands = valueHands.OrderBy(vh => vh.Item1).ToList();

long totalWinnings = 0;
for (int i = 1; i <= orderdHands.Count; i++) {
	totalWinnings += orderdHands[i-1].Item2 * i;
}
Console.WriteLine(totalWinnings);