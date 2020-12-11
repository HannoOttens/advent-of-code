
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()
    
    # Parse 2x
    (seat_plan, _) = parseSeatPlan(rules)
    seat_plan_a = border(seat_plan)
    seat_plan_b = border(seat_plan)
    print_plan(seat_plan)

    # Evolve!
    generation = 0
    while evolve(seat_plan_a, seat_plan_b):
        t = seat_plan_b
        seat_plan_b = seat_plan_a
        seat_plan_a = t
        generation += 1
        print(generation)

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
    for x in range(len(old_plan))[1:-1]:
        for y in range(len(old_plan[0]))[1:-1]:
            oa = occupied_arround(old_plan, x, y)
            if old_plan[x][y] == '.':
                continue
            if live(old_plan, x, y, oa):
                changed = True
                new_plan[x][y] = '#'
            elif dead(old_plan, x, y, oa): 
                changed = True
                new_plan[x][y] = 'L'
            else:
                new_plan[x][y] = old_plan[x][y]
    return changed

def free(seat):
    return seat == '.' or seat == 'L'

def taken(seat):
    return seat == '#'

def live(old_plan, x, y, oa):
    return (old_plan[x][y] == 'L' and oa == 0)

def dead(old_plan, x, y, oa):
    return (old_plan[x][y] == '#' and oa >= 5)

def directions():
    return [(dx-1, dy-1) for dx in range(3) for dy in range(3) if not (dx == 1 and dy == 1)]


def occupied_arround(old_plan, x, y):
    h = len(old_plan)
    w = len(old_plan[0])
    
    occupied_arround = 0
    for (dx,dy) in directions():
        for d in range(1, 1000000):
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

def border(plan):
    l = len(plan[0]) + 2
    new_plan = [l * ['.']]
    for row in plan:
        new_row = ['.'] + row + ['.']
        new_plan.append(new_row)
    new_plan.append(l * ['.'])
    return new_plan
    
def print_plan(plan):
    list(map(lambda s: print(''.join(s)), plan))
    print()

# ===================
# Algebra
# ===================

def parseLine(s):
    return (parseList(parseNone, parseChar('L') <<cor>> parseChar('.') <<cor>> parseChar('#')))(s)

def parseSeatPlan(s):
    return parseList(parseChar('\n'), parseLine)(s)

# call main
main()