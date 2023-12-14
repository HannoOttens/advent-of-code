var lines = File.ReadAllLines("../input.txt");

var nums = lines.Select(l => {
	int frst = -1;
	int last = 0;
	for (int i = 0; i < l.Length; i++) {
		char c = l[i];
		if (char.IsDigit(c)) {
			if ((frst == -1)) 
				frst = int.Parse(c.ToString());
			last = int.Parse(c.ToString());
		}
	}
	return frst * 10 + last;
});

Console.WriteLine(nums.Sum());