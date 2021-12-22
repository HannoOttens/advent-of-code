using static System.Math;
using StrSpan = System.ReadOnlySpan<char>;

string input = File.ReadAllText("../input.txt");
StrSpan str = input.AsSpan();
var bounds = parseBounds(ref str);
Console.WriteLine($"{bounds}");

// Toch gewoon lekker simuleren!
int dxMin = (int)Ceiling(0.5 * (Sqrt(8 * bounds.xMin + 1) - 1));
int dxMax = bounds.xMax;
int dyMin = bounds.yMin;
int dyMax = -bounds.yMin;

int options = 0;
for(int dx = dxMin; dx <= dxMax;  dx++)
for(int dy = dyMin; dy <= dyMax;  dy++) {
	int x = 0, y = 0, tdx = dx, tdy = dy;
	while (y >= bounds.yMin && x <= bounds.xMax)
	{
		x += tdx;
		y += tdy;
		tdx = Max(tdx - 1, 0);
		tdy = tdy - 1;

		if  (x >= bounds.xMin && y <= bounds.yMax 
			&& x <= bounds.xMax && y >= bounds.yMin) 
		{
			options++;
			break;
		} 
	}

}
Console.WriteLine($"Options {options}");

(int xMin, int xMax, int yMin, int yMax) parseBounds(ref StrSpan str) {
	int i = 0;
	// Text voor dubble punt
	while(str[0] != ':') str = str.Slice(1);
	// ': x='
	str = str.Slice(4);
	int xMin = parseInt(ref str);
	// '..'
	str = str.Slice(2);
	int xMax = parseInt(ref str);
	// ', y='
	str = str.Slice(4);
	int yMin = parseInt(ref str);
	// '..'
	str = str.Slice(2);
	int yMax = parseInt(ref str);
	return (xMin, xMax, yMin, yMax);
}

int parseInt(ref StrSpan str) {
	int i = 0;
	while(char.IsDigit(str[i]) || (str[i] == '-')) i++;	
	var t = str.Slice(0, i);
	int n = int.Parse(str.Slice(0, i));
	str = str.Slice(i);
	return n;
}