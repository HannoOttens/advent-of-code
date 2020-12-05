with open("a.txt", "r") as f:
    lines = f.read().splitlines()
_max = 0
for seat in lines:
    num = seat.replace('B', '1').replace('F', '0').replace('R', '1').replace('L', '0')
    _max = max(int('0b'+num, 2), _max)
print(_max)