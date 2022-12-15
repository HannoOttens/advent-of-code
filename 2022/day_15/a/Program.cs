using static System.Math;
Sensor[] sensors = File.ReadAllLines("../in.txt").Select(parseSensor).ToArray();

int targY = 2_000_000;
List<(int minX, int maxX)> noBeacon = new();

foreach(var sensor in sensors) {
	int range = sensor.posi.ManHat(sensor.beac);
	int yDist = Abs(sensor.posi.PosY - targY);
	int remainingRange = range - yDist;
	// Bereik over de lijn?
	if (remainingRange >= 0) {
		noBeacon.Add((sensor.posi.PosX - remainingRange
					, sensor.posi.PosX + remainingRange));
	}
}

noBeacon = noBeacon.OrderBy(x => x.minX).ToList();

int cannotBeThere = 0;
int processedX = int.MinValue;
foreach((int minX, int maxX) in noBeacon){
	int trueMinX = Max(minX, processedX);
	cannotBeThere += Max(0, maxX - trueMinX);
	processedX = Max(maxX, processedX);
}
Console.WriteLine(cannotBeThere);

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