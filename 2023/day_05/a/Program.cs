using static System.Math;

var text = File.ReadAllText("../input.txt");
var groups = text.Split("\r\n\r\n");
var seeds = groups[0].Substring("seeds: ".Length).Split(" ").Select(long.Parse);
var maps = groups.Skip(1).Select(
				map => map.Split("\r\n")
						  .Skip(1)
						  .Select(nums => nums.Split(" ")
						  					  .Select(long.Parse)
											  .ToArray()).ToArray()).ToArray();


bool inSourceRange (long[] range, long value) {
	return (value > range[1]) && (value < (range[1] + range[2]));
}

long sourceToTarget(long[] range, long value) {
	return range[0] + (value - range[1]);
}

long lowest = long.MaxValue;
foreach(long seed in seeds) {
	long result = seed;
	for (long mapIndex = 0; mapIndex < maps.Length; mapIndex++) {
		foreach(long[] range in maps[mapIndex]) {
			if (inSourceRange(range, result))
			{
				result = sourceToTarget(range, result);
				break;
			}	
		}
	}

	lowest = Min(result, lowest);
}

Console.WriteLine(lowest);