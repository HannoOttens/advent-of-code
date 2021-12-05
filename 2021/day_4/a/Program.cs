using System.Diagnostics;
var lines = File.ReadAllLines("../input.txt");
var stopwatch = new Stopwatch();
stopwatch.Start();

(int, int[,]) parseBoards() {
	int nBoards = (lines.Length - 1) / 6;
	var rslt = new int[nBoards,25];
	
	for (int i = 1; i < lines.Length; i++) {	
		// Skip emptyline
		if ((i - 1) % 6 != 0) { 
			int board = (i - 2) / 6;
			int row = (i - 2) % 6;
			var nums = lines[i]
				.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(n => int.Parse(n))
				.ToList();
			for(int j = 0; j < 5; j++) 
				rslt[board,5*row+j] = nums[j];	
		}
	}
	return (nBoards, rslt);
}

void printBoard(int[,] boards, int[] bits, int board) {
	for(int i = 0; i < 5; i++) {
		for(int j = 0; j < 5; j++) {
			int cell = 5*i+j;
			if(checkField(bits,board,cell))
				Console.Write("X ,");
			else
				Console.Write($"{boards[board,cell]:D2},");

		}
		Console.WriteLine();
	}
	Console.WriteLine();
}

void setField(int[] bits, int board, int bit) {
	bits[board] |= 1 << bit;
}

bool checkField(int[] bits, int board, int bit) {
	return (bits[board] & (1 << bit)) > 0;
}

int boardScore(int[,] boards, int[] bits, int winner, int n)
{
	int sum = 0;
	for (int cell = 0; cell < 25; cell++)
		if (!checkField(bits, winner, cell)) 
			sum += boards[winner, cell];
	return sum * n;
}

bool checkWin(int[] bits, int board) {
	int rowMask = 0b11111;
	for(int row = 0; row < 5; row++){
		if ((bits[board] & rowMask) == rowMask) return true;
		rowMask <<= 5;
	}
	int colMask = 0b0000100001000010000100001;
	for(int col = 0; col < 5; col++){
		if ((bits[board] & colMask) == colMask) return true;
		colMask <<= 1;
	}
	return false;
}

var nums = lines[0].Split(',')
				.Select(n => int.Parse(n))
				.ToList();
(int nBoards, var boards) = parseBoards();
var bits = new int[nBoards];
int n = -1;
int winner = -1;
while(winner < 0) {
	n++;
	for (int board = 0; board < nBoards; board++) {
		for (int cell = 0; cell < 25; cell++)
			if(boards[board,cell] == nums[n])
				setField(bits, board, cell);

		if(checkWin(bits, board)) {
			winner = board;
			break;
		}
	}
}

if (winner > 0) {
	int winningScore = boardScore(boards, bits, winner, nums[n]);
	stopwatch.Stop();

	Console.WriteLine($"Score: {winningScore}");
	Console.WriteLine($"Winning number: {nums[n]}");
	Console.WriteLine($"Board number: {winner}");
	Console.WriteLine($"Board:");

	printBoard(boards, bits, winner);
	Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds/1000.0}s");
} else {
	Console.WriteLine("Oops!");
}
