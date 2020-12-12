
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

    y = 1
    x = 10
    psy = 0
    psx = 0
    for (c, n) in steps:
        if c == 'N':
            y += n
        elif c == 'E':
            x += n
        elif c == 'S':
            y -= n
        elif c == 'W':
            x -= n

        elif c == 'F':
            for _ in range(n):
                psy += y 
                psx += x
        elif c == 'R':
            x, y = turn(n, x, y)
        elif c == 'L':
            x, y = turn(360 - n, x, y)
        else:
            raise Exception("Unknown command: " + c)

    print(abs(psx) + abs(psy))


def turn(deg, x, y):
    steps = deg // 90
    for _ in range(steps):
        y = -x
        x = y
    return x, y


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
