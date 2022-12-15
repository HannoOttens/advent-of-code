using static System.Math;
Sensor[] sensors = File.ReadAllLines("../in.txt").Select(parseSensor).ToArray();

int maxCo = 4_000_000;

Posi rslt = new();
for (int line = 0; line < maxCo; line++) {
	int posi = findBeac(line, true);
	if (posi > 0) {
		rslt = new(posi, line);
	} 
}
Console.WriteLine((long)rslt.PosX*maxCo + (long)rslt.PosY);

int findBeac(int line, bool overY) {
	List<(int minX, int maxX)> noBeacon = new();
	foreach(var sensor in sensors) {
		int range = sensor.posi.ManHat(sensor.beac);
		int yDist = Abs(sensor.posi.PosY - line);
		int remainingRange = range - yDist;
		// Bereik over de lijn?
		if (remainingRange >= 0) {
			noBeacon.Add((sensor.posi.PosX - remainingRange
						, sensor.posi.PosX + remainingRange));
		}
	}
	// Find the gap
	noBeacon = noBeacon.OrderBy(x => x.minX).ToList();
	int processedX = noBeacon[0].minX;
	foreach((int minX, int maxX) in noBeacon){
		if (minX > processedX + 1) return processedX + 1;
		processedX = Max(maxX, processedX);
	}
	return -1;
}



// Parsing
Posi getPosi(string str) {
	string xyStr = str.Substring(str.IndexOf('x'));
	var parts = xyStr.Split(", ");
	return new Posi(int.Parse(parts[0].Substring(2))
				  , int.Parse(parts[1].Substring(2)));
}
Sensor parseSensor(string sensor) {
	string[] parts = sensor.Split(':');
	return new Sensor(getPosi(parts[0]), getPosi(parts[1]));
}

// Records
record Sensor (Posi posi, Posi beac);
public record Posi(int PosX = 0, int PosY = 0)
{
	public static Posi operator +(Posi a, Posi b) => new(a.PosX + b.PosX, a.PosY + b.PosY);
	public static Posi operator -(Posi a, Posi b) => new(a.PosX - b.PosX, a.PosY - b.PosY);
	public int ManHat (Posi b) => (Abs(PosX - b.PosX)) + (Abs(PosY - b.PosY));
}