Console.WriteLine(((Func<string, int>)(input 
	=> (input.Select((((x, indx) 
		=> (indx >= 14) && (
				// Vergelijken met
				((Func<Func<int,int,bool>,bool>)(f1 => Enumerable.Range(0, 12).Select((x) => f1(indx-x, 13-x)).All(x => x))
				// f1
				)((Func<int,int,bool>)((curr, dpth) =>
					Enumerable.Range(1, 13).Select((x) => (input[curr] != input[curr - x]) || (dpth < x)).All(x => x)))))))
	.ToList().IndexOf(true)+1))
)(File.ReadAllText("../in.txt")));