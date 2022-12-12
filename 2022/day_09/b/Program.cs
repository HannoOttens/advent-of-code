(char, int)[] cmnds = File.ReadAllLines("../in.txt").Select(t => (t[0], int.Parse(t[2..]))).ToArray();

HashSet<(int x, int y)> beenThereDoneThat = new();
(int x, int y)[] rope = Enumerable.Range(0, 10).Select(_ => (0,0)).ToArray();
beenThereDoneThat.Add(rope[9]);
foreach ((char dir, int length) in cmnds) {
	for(int i = 0; i < length; i++)
	{
		rope[0] = dir switch {
			'U' => (rope[0].x    , rope[0].y + 1),
			'D' => (rope[0].x    , rope[0].y - 1),
			'R' => (rope[0].x + 1, rope[0].y    ),
			'L' => (rope[0].x - 1, rope[0].y    ),
		};
		for (int knot = 1; knot < 10; knot++) {
			(int x, int y) vect = (rope[knot-1].x - rope[knot].x
								 , rope[knot-1].y - rope[knot].y); 
			if (Math.Max(Math.Abs(vect.x), Math.Abs(vect.y)) < 2) break;
			rope[knot] = (rope[knot].x + Math.Sign(vect.x), rope[knot].y + Math.Sign(vect.y));
		}
		beenThereDoneThat.Add(rope[9]); // Add the final one to the list
	}
}
Console.WriteLine(beenThereDoneThat.Count);