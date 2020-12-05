with open("b.txt", "r") as f:
    lines = f.read().splitlines()
my_seat = (888 * 889 // 2) - (88 * 89 // 2)
for seat in lines:
    num = seat.replace('B', '1').replace('F', '0').replace('R', '1').replace('L', '0')
    my_seat -= int('0b'+num, 2)
print(my_seat)