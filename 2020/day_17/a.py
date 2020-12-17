
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    # Parse 2x
    (cubes, _) = parseCubes(rules)

    active = defaultdict(bool)
    for x in range(len(cubes)):
        for y in range(len(cubes)):
            if cubes[x][y] == '#':
                active[(x,y,0)] = True
    # Evolve!
    for _ in range(6):
        new_active = defaultdict(bool)
        inactive_neighbours = set()

        for (x,y,z) in list(active.keys()):
            # Rule 1
            active_around = total_active(active, neighbours(x,y,z))
            if active_around == 2 or active_around == 3:
                new_active[(x,y,z)] = True

            # Collect inactive neighbours
            for n in neighbours(x,y,z):
                if not active[n]:
                    inactive_neighbours.add(n)

        # Rule 2
        for (x,y,z) in inactive_neighbours:
            if total_active(active, neighbours(x,y,z)) == 3:
                new_active[(x,y,z)] = True

        active = new_active

    # Output
    print(sum(active.values()))

# Precompute directions
directions = [(dx-1, dy-1, dz-1) for dx in range(3)
              for dy in range(3)
              for dz in range(3)
              if not (dx == 1 and dy == 1 and dz == 1)]
def neighbours(x, y, z):
    for (dx, dy, dz) in directions:
        yield (x + dx, y + dy, z + dz)

def total_active(active, ns):
    return sum(map(lambda c: active[(c[0],c[1],c[2])], ns))

# ===================
# Algebra
# ===================


def parseLine(s):
    return (parseList(parseNone, parseChar('.') | cor | parseChar('#')))(s)


def parseCubes(s):
    return parseList(parseChar('\n'), parseLine)(s)


# call main
main()
