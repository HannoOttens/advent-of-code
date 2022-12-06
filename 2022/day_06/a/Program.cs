Console.WriteLine(((Func<string, int>)((input) => (input.Select((x, indx) => (indx >= 4) 
				&& input[indx  ] != input[indx-1]
				&& input[indx  ] != input[indx-2]
				&& input[indx  ] != input[indx-3]
				&& input[indx-1] != input[indx-2]
				&& input[indx-1] != input[indx-3]
				&& input[indx-2] != input[indx-3]).ToList().IndexOf(true)+1)))(File.ReadAllText("../in.txt")));