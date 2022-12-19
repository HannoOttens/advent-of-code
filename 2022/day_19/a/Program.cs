using static System.Math;

// The effect of buying index n
var store = new Vec[4] {
	new(1, 0, 0, 0),
	new(0, 1, 0, 0),
	new(0, 0, 1, 0),
	new(0, 0, 0, 1),
};

// Determine if this solution is worse at the same time with the same inventory
bool objectivlyWorse((int, Vec) prev, (int, Vec) curr) {
	// Minder tijd over of later met een slechtere inventory...
	return curr.Item1 <= prev.Item1
		&& (curr.Item2.KleinerGelijkAan(prev.Item2));
}

// Hoe lang tot we cost kunnen betalen?
int timeUntilBuy(Vec invn, Vec robo, Vec cost) {
	int time = 1;
	while(!invn.canBuy(cost)) {
		time++;
		invn += robo;
	}
	return time;
}

// Heeft het nog nut om meer van deze te hebben?
bool stillNeeded(Blueprint bp, Vec robo, Robot rob)
	=> rob switch {
		Robot.Ore  => robo.ore < bp.maxCost.ore,
		Robot.Clay => robo.cly < bp.maxCost.cly,
		Robot.Obsi => robo.obs < bp.maxCost.obs,
		Robot.Geod => true,
	};

int maxGeo = 0;
Dictionary<(int, Vec, Vec), int> memory = new();
Dictionary<Vec, (int, Vec)> cutters = new();
Robot[] robos = new Robot[] { Robot.Ore, Robot.Clay, Robot.Obsi, Robot.Geod };
int gauss(int n) => (n * (n-1)) / 2;

int openGeodes(Blueprint bp, Vec invn, Vec robo, int time = 24) {
	if (invn.geo > maxGeo) {
		maxGeo = invn.geo;
		Console.WriteLine(maxGeo);
	}
	if (invn.geo + time*robo.geo + gauss(time-1) < maxGeo) return 0;

	if (time == 0) return invn.geo;

	if (cutters.ContainsKey(robo) && objectivlyWorse(cutters[robo], (time, invn))) return 0;
	if (memory.ContainsKey((time, invn, robo))) return memory[(time, invn, robo)];

	cutters[robo] = (time, invn);

	int maxV = 0;
	bool boughtAny = false;
	foreach (Robot rob in robos) {
		if ((invn + 100*robo).canBuy(bp.costs[(int)rob])
			&& stillNeeded(bp, robo, rob)) 
		{
			int timeNeeded = timeUntilBuy(invn, robo, bp.costs[(int)rob]);
			if (timeNeeded <= time) {
				boughtAny = true;
				maxV = Max(maxV, openGeodes(bp, invn + timeNeeded*robo - bp.costs[(int)rob], robo + store[(int)rob], time - timeNeeded));
			}
		}
	}
	if (!boughtAny) maxV = Max(maxV, openGeodes(bp, invn + time*robo, robo, 0));

	memory[(time, invn, robo)] = maxV;
	return maxV;
}

var blueprints = File.ReadAllLines("../in.txt")
				.Select(x  => x.Split('\t').Select(int.Parse).ToArray())
				.Select(p  => new Blueprint(p[0],p[1],p[2],p[3],p[4],p[5],p[6]));
int value =0;
foreach (var bp in blueprints) {
	memory = new();
	cutters = new();
	maxGeo = 0;
	Console.WriteLine($"| {bp.index}");
	value += bp.index * openGeodes(bp, new(0,0,0,0), new(1,0,0,0));
}
Console.WriteLine(value);

enum Robot { Ore = 0, Clay = 1, Obsi = 2, Geod = 3 }
public class Blueprint
{
	public int index;
	public Vec[] costs = new Vec[4];
	public Vec maxCost;

	public Blueprint (
		int index, 
		int orOreCost, 					// Ore robot cost
		int clOreCost, 					// Clay robot cost
		int obOreCost, int obClayCost,  // Obsidian robot cost
		int geOreCost, int geObsCost) 	// Geode robot cost
	{
		this.index = index;
		costs[(int)Robot.Ore ] = new(orOreCost, 0         , 0        , 0);
		costs[(int)Robot.Clay] = new(clOreCost, 0         , 0        , 0);
		costs[(int)Robot.Obsi] = new(obOreCost, obClayCost, 0        , 0);
		costs[(int)Robot.Geod] = new(geOreCost, 0		  , geObsCost, 0);

		maxCost = new Vec(
			Max(orOreCost, Max(clOreCost, Max(obOreCost, geOreCost))),
			obClayCost,
			geObsCost,
			int.MaxValue // Altijd nuttig om meer geos te hebben!
		);
	}
}
public record Vec (int ore, int cly, int obs, int geo) {
	public static Vec operator +(Vec a, Vec b) => new(a.ore + b.ore, a.cly + b.cly, a.obs + b.obs, a.geo + b.geo);
	public static Vec operator -(Vec a, Vec b) => new(a.ore - b.ore, a.cly - b.cly, a.obs - b.obs, a.geo - b.geo);
	public static Vec operator *(int a, Vec b) => new(a * b.ore, a * b.cly, a * b.obs, a * b.geo);
	
	public bool KleinerGelijkAan (Vec other) {
		return ore <= other.ore
			&& cly <= other.cly
			&& obs <= other.obs
			&& geo <= other.geo;
	}
	public bool canBuy (Vec cost) {
		var rem = this - cost;
		return (rem.ore >= 0)
			&& (rem.cly >= 0)
			&& (rem.obs >= 0)
			&& (rem.geo >= 0);
	}
}