using System.Diagnostics;
using static System.Math;

// Globals
var scoreTable = new Dictionary<char,int>();
scoreTable.Add(')', 3);
scoreTable.Add(']', 57);
scoreTable.Add('}', 1197);
scoreTable.Add('>', 25137);
var reverse = new Dictionary<char,char>();
reverse.Add(')', '(');
reverse.Add(']', '[');
reverse.Add('}', '{');
reverse.Add('>', '<');


// Readin
var lines = File.ReadAllLines("../input.txt");
int badness = 0;
foreach(var line in lines) badness += checkLine(line);
Console.WriteLine($"Badness score: {badness}");

int checkLine(string line) {
	var charStck = new Stack<char>();
	for(int i = 0; i < line.Length; i++) {
		char c = line[i];
		if (isOpen(c)) 
			charStck.Push(c);
		else if (isCLose(c)) {
			if (charStck.Peek() == reverse[c])
				charStck.Pop();
			else {
				return scoreTable[c];
			}
		}
	}
	return 0;
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
