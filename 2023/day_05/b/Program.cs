using static System.Math;

var text = File.ReadAllText("../input.txt");
var groups = text.Split("\r\n\r\n");
var seeds = groups[0].Substring("seeds: ".Length).Split(" ").Select(long.Parse).ToArray();
var maps = groups.Skip(1).Select(
				map => map.Split("\r\n")
						  .Skip(1)
						  .Select(nums => nums.Split(" ")
						  					  .Select(long.Parse)
											  .ToArray()).ToArray()).ToArray();


bool inSourceRange (long[] range, long value) {
	return (value >= range[1]) && (value < (range[1] + range[2]));
}

long sourceToTarget(long[] range, long value) {
	return range[0] + (value - range[1]);
}

bool rangeInVal(long[] range, (long low,long high) val) {
	return (val.low < range[1]) && (val.high > range[1] + range[2]);
}

((long,long)[],(long,long)) rangeToTarget(long[] range, (long low,long high) val) {
	long newLow = range[1] + range[2];
	long newHigh = range[1] -1;
	
	if (inSourceRange(range, val.low) && inSourceRange(range, val.high))
		return (new (long,long)[] {}
								, (val.low, val.high));
	else if (inSourceRange(range, val.low))
		return (new (long,long)[] {(newLow, val.high)}
								, (val.low, newLow - 1));
	else if (inSourceRange(range, val.high))
		return (new (long,long)[] {(val.low, newHigh)}
								, (newHigh + 1, val.high));
	else if (rangeInVal(range, val)) 
		return (new (long,long)[]  {(val.low, newHigh), (newLow, val.high)}
								, (newHigh+1, newLow-1));
	else
		return (new (long,long)[] {val}
								, (-1L, -1L));
}

long lowest = long.MaxValue;

// Check every seed range and record the lowest result
for(long rIdx = 0; rIdx < seeds.Length; rIdx += 2) {
	long l = seeds[rIdx];
	long h = seeds[rIdx] + seeds[rIdx+1] - 1;
	var origRanges = new List<(long,long)>() { (l,h) };

	// Loop the seed range through all the conversions
	for (long mapIndex = 0; mapIndex < maps.Length; mapIndex++) {
		var newRanges = new List<(long,long)>();

		// Loop over all current ranges (will become more once the ranges get split)
		foreach(var origRange in origRanges) {
			// Keep a list of remainders, will grow when a range is split through the middle
			(long low, long high)[] remainders = new (long,long)[] { origRange };

			// Loop the remainers through all the ranges in the map
			foreach(long[] range in maps[mapIndex]) {
				List<(long low, long high)> newRemainders = new List<(long low, long high)>();

				foreach (var remain in remainders) {
					((long low, long high)[] newRemain, (long,long) transferred) = rangeToTarget(range, remain);
					newRemainders.AddRange(newRemain);

					if (transferred.Item1 != -1L) {
						transferred.Item1 = sourceToTarget(range, transferred.Item1);
						transferred.Item2 = sourceToTarget(range, transferred.Item2);
						newRanges.Add(transferred);
					}
				}

				// no parts left, break
				remainders = newRemainders.ToArray();
				if(remainders.Length == 0) break; 		
			}

			newRanges.AddRange(remainders);
		}

		origRanges = newRanges;
	}

	foreach ((long low, long high) in origRanges) 
		lowest = Min(low, lowest);
}

Console.WriteLine(lowest);