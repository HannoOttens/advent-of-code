// Failed idea: Linked list
// Je hebt de links/rechts info nodig
using static System.Math;
using StrSpan = System.ReadOnlySpan<char>;
using Snails = System.Collections.Generic.LinkedList<(int n, int d, Side s)>;
using SnailNode = System.Collections.Generic.LinkedListNode<(int n, int d, Side s)>;
using System.Text;

var lines = File.ReadAllLines("../input.txt");

var val1 = parseSnailNumbers(lines[0].AsSpan());
for(int i = 1; i < lines.Length; i++) {
	var val2 = parseSnailNumbers(lines[i].AsSpan());
	val1 = add(val1, val2);
	reduce(val1);
}

Console.WriteLine($"{snailsToStr(val1)}");


Snails add(Snails val1, Snails val2) {
	// Sticht together
	foreach(var v in val2) val1.AddLast(v);

	// Increase depth
	for (var node = val1.First;
		node != null;
		node = node.Next)
	{
		var v = node.Value;
		v.d++;
		node.Value = v;
	}

	return val1;
}


void reduce(Snails snails) {
	bool cont;
	do {
		cont = false;

		for (var snailNodeL = snails.First;
			snailNodeL.Next != null;
			snailNodeL = snailNodeL.Next)
		{
			var snailNodeR = snailNodeL.Next;
			var snailL = snailNodeL.Value;
			var snailR = snailNodeR.Value;
			// Is it a pair?
			if (snailL.d == snailR.d 
				&& snailL.s == Side.Left
				&& snailR.s == Side.Right) 
			{
				if (snailL.d > 4) {
					explode(snailNodeL, snailNodeR);
					cont = true;
				} else { 
					// Too big?
					if (snailL.n > 9) { split(snailNodeL); cont = true; }
					if (snailR.n > 9) { split(snailNodeR); cont = true; }
				}
			}
			if(cont) break; // Break out of linked list iteration if we did something
		}
		
		Console.WriteLine($"{snailsToStr(val1)}");

	} while (cont);
} 


void explode(
	SnailNode snailNodeL, 
	SnailNode snailNodeR
) {
	var snail = snailNodeL.Previous.Value;
	snail.n = 0;
	snail.d--;
	snailNodeL.List.AddAfter(snailNodeL, snail.Add);

	if (snailNodeL.Previous != null) {
		var v = snailNodeL.Previous.Value;
		v.n += snailNodeL.Value.n;
		snailNodeL.Previous.Value = v;
		snailNodeL.List.Remove(snailNodeL);
	} else {
		var v = snailNodeL.Value;
		v.d--;
		v.n = 0;
		snailNodeL.Value = v;
	}

	if (snailNodeR.Next != null) {
		var v = snailNodeR.Next.Value;
		v.n += snailNodeR.Value.n;
		snailNodeR.Next.Value = v;
		snailNodeR.List.Remove(snailNodeR);
	} else {
		var v = snailNodeR.Value;
		v.d--;
		v.n = 0;
		snailNodeR.Value = v;
	}
} 


void split(
	SnailNode snailNode
) {
	
	// Make new nodes
	var vL = snailNode.Value; 
	vL.d = snailNode.Value.d + 1;
	vL.s = Side.Left;
	vL.n = (int)Floor  (snailNode.Value.n / 2.0);
	var snailNodeL = new SnailNode(vL);
	var vR = snailNode.Value; 
	vR.d = snailNode.Value.d + 1;
	vR.s = Side.Right;
	vR.n = (int)Ceiling(snailNode.Value.n / 2.0);
	var snailNodeR = new SnailNode(vR);

	// [N, R]
	snailNode.List.AddAfter(snailNode, snailNodeR);
	// [N, L, R]
	snailNode.List.AddAfter(snailNode, snailNodeL);
	// [L , R]
	snailNode.List.Remove(snailNode);
} 

Snails parseSnailNumbers(StrSpan str) {
	int d = 0;
	var snailNumbers = new Snails(); 
	while(str.Length > 0) {
		switch (str[0]) {
			case '[': d++;	break;
			case ']': d--;	break;
			case ',': 		break;
			default:
				Side side;
				if(str[1] == ',') 
					side = Side.Left;
				else
					side = Side.Right;

				snailNumbers.AddLast((str[0] - '0', d, side));
				break;
		}
		str = str.Slice(1);
	}
	return snailNumbers;
}

string snailsToStr(Snails snails) {
	StringBuilder rslt = new StringBuilder();
	
	int lastDepth = 0;
	char lastC = 'x';
	foreach(var snail in snails) {
		if (snail.s == Side.Left) {
			if (lastC == ']') {
				rslt.Append(',');
				rslt.Append('[');
			}


			while (snail.d > lastDepth) {
				lastDepth++;
				rslt.Append('[');
			}
			rslt.Append(snail.n.ToString());
			lastC = 'x';
		} else {
			rslt.Append(',');
			rslt.Append(snail.n.ToString());
			rslt.Append(']');
			lastC = ']';
		}
	}
	return rslt.ToString();
}

enum Side { Left, Right };