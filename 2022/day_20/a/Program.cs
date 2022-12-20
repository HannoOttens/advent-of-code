using static System.Math;

var input = File.ReadAllLines("../in.txt").Select(int.Parse).ToArray();
var map = Enumerable.Range(0, input.Length).ToArray();
Console.WriteLine($"Before distinct: {input.Distinct().Count()}");

int iter = 0;
int indx = 0;
while (iter < input.Length) {
	indx = Array.IndexOf(map, iter);
	int curr = input[indx];

	int newI = (indx + curr) % (input.Length-1);

	while (newI <= 0) newI = input.Length + newI - 1;
	while (newI >= input.Length) newI = newI - input.Length + 1;

	if (newI > indx) {
		Array.Copy(input, indx+1, input, indx, newI - indx);
		Array.Copy(map  , indx+1, map  , indx, newI - indx);
	}
	else if (newI < indx) {
		Array.Copy(input, newI, input, newI+1, indx - newI);
		Array.Copy(map  , newI, map  , newI+1, indx - newI);
	}
	input[newI] = curr;
	map  [newI] = iter;
	iter++;
}

int zeroIndx = Array.IndexOf(input, 0);
int a = input[(zeroIndx + 1000)%input.Length];
int b = input[(zeroIndx + 2000)%input.Length];
int c = input[(zeroIndx + 3000)%input.Length];

if (input.Length < 100) Console.WriteLine(string.Join(", ", input));
Console.WriteLine($"{(a,b,c)}, Sum: {a+b+c}");
Console.WriteLine($"Map check: {(map.Distinct().Count() == input.Length ? "OK" : "ITEMS LOST!")}");
Console.WriteLine($"After distinct: {input.Distinct().Count()}");

// unsafe 
// {
// 	Buffer buff;
// 	for (int i = 0; i < input.Length; i++)
// 		buff.buffer[i] = input[i];

//     // fixed (byte* bufferA = myStructA.buffer, bufferB = myStructB.buffer)
//     // {
//     //     *((FixedSizeBufferWrapper*)bufferA) =
//     //     *((FixedSizeBufferWrapper*)bufferB);
//     // }
// }

// public unsafe struct Buffer
// {
//     public unsafe fixed int buffer[5000];
// }

