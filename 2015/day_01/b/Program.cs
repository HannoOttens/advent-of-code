Console.WriteLine(
		File.ReadAllText("../input.txt")
			.Aggregate((0,0), (tuple, c) 
				=> (tuple.Item2 < 0 
					? tuple 
					: ( tuple.Item1 + 1
					  , tuple.Item2 + (c == '(' ? 1 : -1)))));