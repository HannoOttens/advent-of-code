
from parsers import *
from collections import defaultdict

# Precompute directions
directions = [(dx-1, dy-1) for dx in range(3) for dy in range(3) if not (dx == 1 and dy == 1)]

# ===================
# Code
# ===================


def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    # Parse 2x
    (seat_plan_a, _) = parseSeatPlan(rules)
    (seat_plan_b, _) = parseSeatPlan(rules)
    print_plan(seat_plan_a)

    # Evolve!
    while evolve(seat_plan_a, seat_plan_b):
        t = seat_plan_b
        seat_plan_b = seat_plan_a
        seat_plan_a = t

    # Output
    print_plan(seat_plan_b)
    occupied = 0
    for row in seat_plan_b:
        for seat in row:
            if seat == '#':
                occupied += 1
    print(occupied)


def evolve(old_plan, new_plan):
    changed = False
    for x in range(len(old_plan)):
        for y in range(len(old_plan[0])):
            oa = occupied_arround(old_plan, x, y)
            if old_plan[x][y] == '.':
                continue
            if old_plan[x][y] == 'L' and oa == 0:
                changed = True
                new_plan[x][y] = '#'
            elif old_plan[x][y] == '#' and oa >= 5:
                changed = True
                new_plan[x][y] = 'L'
            else:
                new_plan[x][y] = old_plan[x][y]
    return changed

def occupied_arround(old_plan, x, y):
    h = len(old_plan)
    w = len(old_plan[0])

    occupied_arround = 0
    for (dx, dy) in directions:
        for d in range(1, max(w,h)):
            x2 = x + dx * d
            y2 = y + dy * d
            if x2 < 0 or x2 >= h or y2 < 0 or y2 >= w:
                break
            if old_plan[x2][y2] == '#':
                occupied_arround += 1
                break
            if old_plan[x2][y2] == 'L':
                break
    return occupied_arround


def print_plan(plan):
    list(map(lambda s: print(''.join(s)), plan))
    print()

# ===================
# Algebra
# ===================


def parseLine(s):
    return (parseList(parseNone, parseChar('L') << cor >> parseChar('.') << cor >> parseChar('#')))(s)


def parseSeatPlan(s):
    return parseList(parseChar('\n'), parseLine)(s)


# call main
main()
