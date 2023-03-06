with open("input.txt", "r") as f:
	lines = f.read().splitlines()
# gauss: sum of all ticket numbers, minus the missing ones at the start
my_seat = ((994 * 995) // 2) - ((60 * 61) // 2)
for seat in lines:
	num = seat.replace('B', '1').replace('F', '0').replace('R', '1').replace('L', '0')
	my_seat -= int('0b'+num, 2)
print(my_seat)