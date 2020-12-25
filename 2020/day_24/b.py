
from parsers import *
from collections import defaultdict
import math
from itertools import permutations, product

# ===================
# Code
# ===================

dirs = ['w', 'e', 'ne', 'nw', 'se', 'sw']

def main():
    with open("input.txt", "r") as f:
        rules = f.read()
    tiles = next(parseTile(rules))[0]

    # Get initial tiles
    tile_map = defaultdict(bool)
    for tile in tiles:
        x = 0
        y = 0
        for step in tile:
            x, y = make_step(step, x, y)
        tile_map[(x, y)] = not tile_map[(x, y)]

    # Evolve!
    active = tile_map
    for gen in range(1, 101):
        new_active = defaultdict(bool)
        inactive_neighbours = set()

        for (x,y) in list(active.keys()):
            # Rule 1
            active_around = total_active(active, neighbours(x,y))
            if active[(x,y)] and (active_around == 1 or active_around == 2):
                new_active[(x,y)] = True

            # Collect inactive neighbours
            for n in neighbours(x,y):
                if not active[n]:
                    inactive_neighbours.add(n)

        # Rule 2
        for (x,y) in inactive_neighbours:
            if not active[x,y] and total_active(active, neighbours(x,y)) == 2:
                new_active[(x,y)] = True

        active = new_active
    print(f'Day {gen}: {len(active)}')


def neighbours(x,y):
    return list(map(lambda st: make_step(st, x, y), dirs))

def total_active(active, nbs):
    return sum(map(lambda n: active[n], nbs))

def make_step(step, x, y):
    if step == 'e':
        x += 1
    elif step == 'w':
        x -= 1
    elif step == 'ne':
        y += 1  # Only up
    elif step == 'nw':
        x -= 1  # Up and left
        y += 1
    elif step == 'se':
        x += 1
        y -= 1
    elif step == 'sw':
        y -= 1
    return x,y
# ===================
# Algebra
# ===================

def parseDir(s):
    return (parseToken('se')
            | cor | parseToken('sw')
            | cor | parseToken('nw')
            | cor | parseToken('ne')
            | cor | parseToken('e')
            | cor | parseToken('w')
            )(s)


def parseDirections(s):
    return many(parseDir)(s)


def parseTile(s):
    return parseList(parseChar('\n'), parseDirections)(s)


# call main
main()
