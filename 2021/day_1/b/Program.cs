// See https://bka.ms/new-console-template for more information

var lines = File.ReadAllLines("../input.txt");

int incr = 0;
int iter = 0;
int currWind = 0;
var windows = new int[]{ 0,0,0,0 };
foreach (var l in lines) {
	if (iter > 3) {
		int nextWind = (currWind + 1) % 4;
		if (windows[currWind] < windows[nextWind]) 
			incr++;
		windows[currWind] = 0;
	}
	
	int curr = int.Parse(l);
	if (iter % 4 != 3) windows[0] += curr; 
	if (iter % 4 != 0) windows[1] += curr; 
	if (iter % 4 != 1) windows[2] += curr; 
	if (iter % 4 != 2) windows[3] += curr; 
	currWind = (currWind + 1) % 4;
	
	iter++;
}

Console.WriteLine(incr);