
from parsers import *
import math

# ===================
# Code
# ===================


def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    # Parse
    (steps, rest) = parseSteps(rules)
    if rest != "":
        raise Exception("Remaining input in parser: " + rest)

    x = 0
    y = 0
    dx = 1
    dy = 0
    for (c, n) in steps:
        print(f'{c}{n}')
        if c == 'N':
            y += n
        elif c == 'E':
            x += n
        elif c == 'S':
            y -= n
        elif c == 'W':
            x -= n
        elif c == 'F':
            x += dx * n
            y += dy * n
        elif c == 'R' or c == 'L':
            dx, dy = turn(c, n, dx, dy)
            print(dx, dy)
        else:
            raise Exception("Unknown command: " + c)
        print(x, y)

    print(abs(x) + abs(y))


def turn(_dir, deg, dx, dy):
    if deg == 180:
        return (-dx, -dy)

    if deg == 270:
        deg = 90
        _dir = 'L' if _dir == 'R' else 'R'

    if _dir == 'R':
        if dx == 0:
            return (dy, 0)
        if dy == 0:
            return (0, -dx)
    if _dir == 'L':
        if dx == 0:
            return (-dy, 0)
        if dy == 0:
            return (0, dx)


    # # Normalize, always turn right
    # if _dir == 'R':
    #     deg = -deg

    # sdx = math.acos(dx)
    # rdeg = deg * (math.pi / 180)
    # print(rdeg)
    # print(sdx)

    # dx2 = math.cos(sdx + rdeg)
    # dy2 = math.sin(sdx + rdeg)

    # return round(dx2), round(dy2)


# ===================
# Algebra
# ===================

def parseStep(s):
    return (oneOf(['N', 'W', 'S', 'E', 'R', 'L', 'F']) << cont >> parseNum)(s)


def parseSteps(s):
    return parseList(parseChar('\n'), parseStep)(s)


# call main
main()
