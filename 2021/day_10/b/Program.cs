using System.Diagnostics;
using static System.Math;

// Globals
var scoreTable = new Dictionary<char,int>();
scoreTable.Add('(', 1);
scoreTable.Add('[', 2);
scoreTable.Add('{', 3);
scoreTable.Add('<', 4);
var reverse = new Dictionary<char,char>();
reverse.Add(')', '(');
reverse.Add(']', '[');
reverse.Add('}', '{');
reverse.Add('>', '<');


// Readin
var lines = File.ReadAllLines("../input.txt");
var scores = new List<long>();
foreach(var line in lines) {
	(bool valid, Stack<char> stack) = isValid(line);
	if(valid && stack.Count > 0) {
		long lineScore = 0;
		while(stack.Count > 0) lineScore = lineScore * 5 + scoreTable[stack.Pop()];
		scores.Add(lineScore);
	}
}
scores.Sort();
Console.WriteLine($"Badness score: {scores[(scores.Count/2)]}");

(bool, Stack<char>) isValid(string line) {
	var charStck = new Stack<char>();
	for(int i = 0; i < line.Length; i++) {
		char c = line[i];
		if (isOpen(c)) 
			charStck.Push(c);
		else if (isCLose(c)) {
			if (charStck.Peek() == reverse[c])
				charStck.Pop();
			else
				return (false, charStck);
		}
	}
	return (true, charStck);
}

bool isOpen(char c) {
	return (c == '(')
		|| (c == '[')
		|| (c == '{')
		|| (c == '<');
}

bool isCLose(char c) {
	return (c == ')')
		|| (c == ']')
		|| (c == '}')
		|| (c == '>');
}
