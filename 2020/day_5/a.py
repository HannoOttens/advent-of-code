with open("a.txt", "r") as f:
    lines = f.read().splitlines()
n = 1023
my_seat = n * (n + 1) / 2
_max = 0
for seat in lines:
    num = seat.replace('B', '1').replace('F', '0').replace('R', '1').replace('L', '0')
    n = int('0b'+num, 2)
    _max = max(n, _max)
print(_max)
