Console.WriteLine(((Func<string, int>)(input 
	=> (input.Select((((x, indx) 
		=> (indx >= 14) && (
				// Vergelijken met
				((Func<Func<int,int,bool>,bool>)(f1 =>
					   f1(indx-00, 13)
					&& f1(indx-01, 12)
					&& f1(indx-02, 11)
					&& f1(indx-03, 10)
					&& f1(indx-04, 09)
					&& f1(indx-05, 08)
					&& f1(indx-06, 07)
					&& f1(indx-07, 06)
					&& f1(indx-08, 05)
					&& f1(indx-09, 04)
					&& f1(indx-10, 03)
					&& f1(indx-11, 02)
					&& f1(indx-12, 01))
				// f1
				)((Func<int,int,bool>)((curr, dpth) =>
					((input[curr] != input[curr - 01]) || (dpth < 01))
				 && ((input[curr] != input[curr - 02]) || (dpth < 02)) 
				 && ((input[curr] != input[curr - 03]) || (dpth < 03)) 
				 && ((input[curr] != input[curr - 04]) || (dpth < 04)) 
				 && ((input[curr] != input[curr - 05]) || (dpth < 05)) 
				 && ((input[curr] != input[curr - 06]) || (dpth < 06)) 
				 && ((input[curr] != input[curr - 07]) || (dpth < 07)) 
				 && ((input[curr] != input[curr - 08]) || (dpth < 08)) 
				 && ((input[curr] != input[curr - 09]) || (dpth < 09)) 
				 && ((input[curr] != input[curr - 10]) || (dpth < 10)) 
				 && ((input[curr] != input[curr - 11]) || (dpth < 11)) 
				 && ((input[curr] != input[curr - 12]) || (dpth < 12)) 
				 && ((input[curr] != input[curr - 13]) || (dpth < 13)))))))).ToList().IndexOf(true)+1))
)(File.ReadAllText("../in.txt")));