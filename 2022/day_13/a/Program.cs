int itemEnd(ReadOnlySpan<char> list) {
	int depth = 0;
	int indx = 0;
	while (indx < list.Length) {
		if (depth == 0 && list[indx] == ',') break;
		if (list[indx] == '[') depth++;
		if (list[indx] == ']') depth--;
		indx++;
	}
	return indx;
}

int compair(ReadOnlySpan<char> left, ReadOnlySpan<char> rght) {
	if (left.Length == 0) return -1;
	if (rght.Length == 0) return 1;

	// Both lists
	if (left[0] == '[' && rght[0] == '[') {
		var L = left.Slice(1, left.Length-2);
		var R = rght.Slice(1, rght.Length-2);
		while (L.Length > 0) {
			if (R.Length == 0) return 1; // Right ran out
			int endL = itemEnd(L);
			int endR = itemEnd(R);
			int compr = compair(L.Slice(0, endL), R.Slice(0, endR));
			if (compr != 0) 
				return compr;

			// Naar volgende item
			L = endL == L.Length ? "".AsSpan() : L.Slice(endL+1, L.Length-endL-1);
			R = endR == R.Length ? "".AsSpan() : R.Slice(endR+1, R.Length-endR-1);
		}
		return R.Length == 0 ? 0 : -1;
	// Mismatch, pretend L is a list
	} else if (char.IsDigit(left[0]) && rght[0] == '[') {
		var L = $"[{left}]".AsSpan();
		return compair(L, rght);
	// Mismatch, pretend R is a list
	} else if (left[0] == '[' && char.IsDigit(rght[0])) {
		var R = $"[{rght}]".AsSpan();
		return compair(left, R);
	// Integers!
	} else {
		return (int.Parse(left).CompareTo(int.Parse(rght)));
	}
}

Console.WriteLine( 
	File.ReadAllText("../in.txt")
		.Split("\r\n\r\n")
		.Select(pair => (pair.Split("\r\n")[0], pair.Split("\r\n")[1]))
		.Select((pair, indx) => compair(pair.Item1.AsSpan(), pair.Item2.AsSpan()) <= 0 ? indx + 1 : 0)
		.Sum());