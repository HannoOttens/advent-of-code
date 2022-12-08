string[] grid = File.ReadAllLines("../in.txt");

bool boundChk (int posX, int posY) 
	=> (posX >= 0 && posY >= 0 && posX < grid.Length && posY < grid.Length);

int scneScor (int dirX, int dirY, int posX, int posY) {
	char hght  = grid[posX][posY];
	int scor = 0;

	posX += dirX; 
	posY += dirY;
	while (boundChk(posX, posY)) {
		scor += 1;
		if (grid[posX][posY] >= hght) return scor;
		posX += dirX; 
		posY += dirY;
	}
	return scor;
}

int mxSc = 0;
for (int xPos = 0; xPos < grid.Length; xPos++)
for (int yPos = 0; yPos < grid.Length; yPos++)
{
	mxSc = Math.Max(mxSc,
		  scneScor( 1,  0, xPos, yPos)
		* scneScor(-1,  0, xPos, yPos)
		* scneScor( 0,  1, xPos, yPos)
		* scneScor( 0, -1, xPos, yPos)
	);
}

Console.WriteLine(mxSc);