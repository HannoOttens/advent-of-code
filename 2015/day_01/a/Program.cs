﻿Console.WriteLine(File.ReadAllText("../input.txt").Aggregate(0, (acc, c) => acc + (c == '(' ? 1 : -1)));