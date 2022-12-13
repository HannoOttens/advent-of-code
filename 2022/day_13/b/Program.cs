// vind het einde van een item in de list
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
// compare met char spans zodat we nooit een kopie maken (alles gaat op de input string)
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
// Init functie omdat span<T> niet als type argument gegeven mag worden
int strCmpr(string a, string b) => compair(a.AsSpan(), b.AsSpan());

List<string> packets = File.ReadAllText("../in.txt")
		.Replace("\r\n\r\n", "\r\n")
		.Split("\r\n")
		.ToList();
packets.Add("[[2]]");
packets.Add("[[6]]");
packets.Sort(strCmpr);

Console.WriteLine((packets.IndexOf("[[2]]")+1) * (packets.IndexOf("[[6]]")+1));
