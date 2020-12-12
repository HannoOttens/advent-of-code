
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

    n = 1
    e = 10
    psy = 0
    psx = 0
    for (c, num) in steps:
        if c == 'N':
            n += num
        elif c == 'E':
            e += num
        elif c == 'S':
            n -= num
        elif c == 'W':
            e -= num

        elif c == 'F':
            for _ in range(num):
                psy += n 
                psx += e
        elif c == 'R':
            e, n = turn(num, e, n)
        elif c == 'L':
            e, n = turn(360 - num, e, n)
        else:
            raise Exception("Unknown command: " + c)

    print(abs(psx) + abs(psy))


def turn(deg, e, n):
    steps = deg // 90
    for _ in range(steps):
        n2 = n
        n = -e
        e = n2
    return e, n


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
