
from parsers import *
from collections import defaultdict
import math
from itertools import permutations, product

_str = '872495136'
inp = []
for c in _str:
    inp.append(int(c))

current = inp[0]
for i in range(100):
    cur_idx = inp.index(current)

    # Pick up cups
    pick = []
    rem = []
    for i in range(1, 4):
        pick.append(inp[(cur_idx+i) % len(inp)])
    for i in range(4, len(inp) + 1):
        rem.append(inp[(cur_idx+i) % len(inp)])

    # Destination
    target = inp[cur_idx] - 1
    while target < min(inp) or target in pick:
        target -= 1
        if target < min(inp):
            target = max(inp)

    # Add cups
    idx = rem.index(target)
    inp = rem[:idx+1] + pick + rem[idx+1:]
    # print(inp)
    cur_idx = inp.index(current)
    current = inp[(cur_idx+ 1) % len(inp)]

one_idx = inp.index(1)
s = ''
for i in range(len(inp)):
    s += str(inp[(i + one_idx) % len(inp)])
print(s[1:])