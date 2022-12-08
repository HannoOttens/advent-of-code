string[] grid = File.ReadAllLines("../in.txt");
byte[,] visi = new byte[grid.Length, grid.Length];

bool boundChk (int posX, int posY) 
	=> (posX >= 0 && posY >= 0 && posX < grid.Length && posY < grid.Length);

void markVisi (int dirX, int dirY, int posX, int posY) {
	int highest = -1;
	do {
		if ((int)grid[posX][posY] > highest)
			visi[posX, posY] = 1;
		highest = Math.Max(highest, (int)grid[posX][posY]);
		posX += dirX; 
		posY += dirY;
	} while (boundChk(posX, posY));
}

for (int indx = 0; indx < grid.Length; indx++)
{
	markVisi( 1,  0, 0            , indx);
	markVisi(-1,  0, grid.Length-1, indx);
	markVisi( 0,  1, indx, 0			);
	markVisi( 0, -1, indx, grid.Length-1);
}

Console.WriteLine(visi.Cast<byte>().Aggregate(0, (x, y) => x + y));