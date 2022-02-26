var input = File.ReadAllLines("../input.txt");

int width = input[0].Length;
int height = input.Length;
char[,] grid = new char[width, height];
char[,] buff = new char[width, height];
for(int x = 0; x < width; x++) 
for(int y = 0; y < height; y++) {
	buff[x,y] = input[y][x];
	grid[x,y] = '.';
}

const char EASTERN = '>';
const char SOUTHERN = 'v';

int day = 0;
while(Change(buff, grid)) {
	Console.WriteLine($"Day {day}:");
	for(int y = 0; y < height; y++) {
		for(int x = 0; x < width; x++) {
			Console.Write(buff[x,y]);
		}
		Console.WriteLine();
	}
	Console.WriteLine();


	day++;
	// Copy buff to grid
	for(int x = 0; x < width; x++) 
	for(int y = 0; y < height; y++) {
		grid[x,y] = buff[x,y];
		buff[x,y] = '.';
	}

	// Move eastern
	for(int x = 0; x < width; x++) 
	for(int y = 0; y < height; y++) {
		if(grid[x,y] == EASTERN) {
			int nextX = (x + 1) % width;
			if (grid[nextX,y] == '.') 
				buff[nextX,y] = EASTERN;
			else
				buff[x,y] = EASTERN;
		}
	}

	// Move southern
	for(int x = 0; x < width; x++) 
	for(int y = 0; y < height; y++) {
		if(grid[x,y] == SOUTHERN) {
			int nextY = (y + 1) % height;
			if (grid[x,nextY] != SOUTHERN && buff[x,nextY] == '.') 
				buff[x,nextY] = SOUTHERN;
			else
				buff[x,y] = SOUTHERN;
		}
	}
}
Console.WriteLine($"Day {day}:");
	for(int y = 0; y < height; y++) {
		for(int x = 0; x < width; x++) {
			Console.Write(buff[x,y]);
		}
		Console.WriteLine();
	}
	Console.WriteLine();

bool Change(char[,] a, char[,] b) {
	for(int x = 0; x < width; x++) 
	for(int y = 0; y < height; y++) {
		if (a[x,y] != b[x,y]) return true;
	}
	return false;
}