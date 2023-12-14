var lines = File.ReadAllLines("../input.txt");


int strToDigit (string l, int index) {
	if (l.Length - index < 3) return -1;
	switch (l.Substring(index, 3)) {
		case "one": return 1;
		case "two": return 2;
		case "six": return 6;
	}
	if (l.Length - index < 4) return -1;
	switch (l.Substring(index, 4)) {
		case "four": return 4;
		case "five": return 5;
		case "nine": return 9;	
	}
	if (l.Length - index < 5) return -1;
	switch (l.Substring(index, 5)) {
		case "three": return 3;
		case "seven": return 7;
		case "eight": return 8;
	};
	return -1;
}

int lineToNumb(string l) {
	int frst = -1;
	int last = 0;
	int orLast = 0;
	for (int i = 0; i < l.Length; i++) {
		char c = l[i];
		if (char.IsDigit(c)) {
			if ((frst == -1))
				frst = int.Parse(c.ToString());
			last = int.Parse(c.ToString());
		}

		if ((frst == -1))
			frst = strToDigit(l, i);
		orLast = strToDigit(l, i);
		if (orLast != -1) last = orLast;
	}
	return frst * 10 + last;
}


var nums = lines.Select(lineToNumb);

Console.WriteLine(nums.Sum());