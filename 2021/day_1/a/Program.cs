// See https://aka.ms/new-console-template for more information

var lines = File.ReadAllLines("../input.txt");

int prev = int.MaxValue;
int incr = 0;
foreach (var l in lines) {
	int curr = int.Parse(l);
	if(curr > prev) incr++; 
	prev = curr;
}

Console.WriteLine(incr);