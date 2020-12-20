
from parsers import *
from collections import defaultdict
import math
from itertools import permutations, product

# ===================
# Code
# ===================


def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    res = list(parseTiles(rules))
    if len(res) == 0:
        raise Exception("Parsing failed")
    images = res[0][0]
    image_borders = list(map(gitBinBorder, images))

    # Size of grid
    s = round(math.sqrt(len(images)))
    solution = solve(s, dict(image_borders), image_borders[:])
    #solution = ([2797, 2719, 1583, 3187, 3533, 3851, 2089, 3643, 3371, 1321, 3779, 3167, 1291, 2711, 2539, 1097, 3529, 3203, 1051, 3169, 2087, 3917, 3109, 2707, 3637, 1399, 3797, 2801, 3803, 3413, 3259, 1531, 1031, 2593, 1367, 1249, 1447, 3853, 1759, 1049, 2837, 3727, 2003, 2081, 3001, 3911, 1523, 2927, 1361, 3761, 2741, 2141, 1481, 1667, 2113, 2617, 2441, 3719, 1747, 1259, 2659, 3659, 2803, 2699, 3463, 3307, 3079, 2953, 1511, 1699, 3457, 1171, 2521, 2897, 3251, 2503, 1549, 3527, 1129, 3701, 2473, 1429, 1483, 2039, 3583, 1471, 1237, 2339, 2621, 3623, 2179, 1871, 2633, 3361, 3061, 2459, 1103, 2309, 3923, 2957, 1787, 2917, 2243, 3877, 3617, 1499, 2693, 3559, 2543, 1789, 1373, 1181, 2591, 2273, 1721, 3067, 3319, 2437, 2857, 1601, 2129, 1231, 1013, 3613, 1777, 1163, 1753, 2029, 1657, 1297, 3221, 2161, 3593, 3467, 2383, 2239, 2371, 3331, 1093, 1877, 3019, 2647, 3389, 3517], [0, 2, 2, 0, 3, 1, 0, 3, 3, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 3, 1, 1, 0, 3, 1, 3, 1, 3, 0, 0, 1, 1, 0, 3, 1, 3, 2, 3, 2, 3, 0, 2, 0, 1, 3, 3, 1, 0, 2, 1, 0, 2, 1, 2, 0, 3, 3, 1, 1, 0, 1, 1, 3, 0, 3, 1, 2, 3, 0, 2, 1, 0, 0, 2, 0, 3, 2, 1, 1, 0, 1, 1, 0, 1, 3, 3, 0, 3, 2, 3, 2, 2, 1, 2, 2, 1, 0, 1, 1, 2, 2, 1, 3, 0, 3, 0, 0, 1, 0, 2, 1, 1, 0, 3, 3, 0, 0, 3, 3, 1, 1, 3, 3, 1, 0, 3, 3, 1, 1, 2, 1, 0, 1, 1, 3, 0, 1, 0, 0, 1, 0, 0], [0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 2, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2])
    print(solution)
    print(solution[0][0] * solution[0][11] * solution[0][132] * solution[0][143])

# Run solver
def solve(s, image_borders, rem, pos=[], rot=[], flp=[]):
    if len(rem) == 0:
        return pos, rot, flp

    # All options
    for idx, image_border in enumerate(rem):
        if len(rem) == len(image_borders):
            print(idx)

        for r in range(4):
            for f in range(2):
                # Setup
                del rem[idx]
                pos.append(image_border[0])
                rot.append(r)
                flp.append(f)
                # Recurse
                if match(s, image_borders, pos, rot, flp):
                    res = solve(s, image_borders, rem, pos, rot, flp)
                    if res != None:
                        return res
                # Restore
                rem.insert(idx, image_border)
                pos.pop()
                rot.pop()
                flp.pop()
    return None

# Match all borders
def match(s, image_borders, pos, rot, flp):
    for idx in range(len(pos)):
        p = pos[idx]
        f = flp[idx]
        r = rot[idx]
        for side, n_idx in neighbours(s, idx, pos):
            n_p = pos[n_idx]
            n_f = flp[n_idx]
            n_r = rot[n_idx]
            if not eq(image_borders[p][f][r][side], image_borders[n_p][n_f][n_r][(side + 2) % 4]):
                return False
    return True

def eq(a, b):
    for (pa, pb) in zip(a, b):
        if pa != pb:
            return False
    return True

# Get neighboring fields
def neighbours(s, idx, pos):
    if idx == 0:
        return []
    if idx % s == 0 and idx >= s :
        return [(0, idx-s)]
    if idx < s:
        return [(3, idx-1)]
    return [(3, idx-1), (0, idx-s)]

# Convert to binary
def gitBinBorder(image):
    (_id, img) = image
    borders = [img[0]
             , list(map(lambda row: row[-1], img))
             , img[-1]
             , list(map(lambda row: row[0], img))]
    # Flip vertical
    borders_fv = [rev(borders[0])
               ,  borders[3]
               ,  rev(borders[2])
               ,  borders[1]]
    return (_id, [rotations(borders), rotations(borders_fv)])

def rotations(b):
    rots = [b]
    for _ in range(1, 4):
        rots.append([
                rev(rots[-1][3]),
                rots[-1][0],
                rev(rots[-1][1]),
                rots[-1][2],
            ])
    return rots


def rev(l):
    return list(reversed(l))

# ===================
# Algebra
# ===================


def parseImage(s):
    return (parseList(parseChar('\n'), many(oneOf(['.', '#']))))(s)


def parseTile(s):
    return (parseToken('Tile')
        |d_cont| (parseNum
        |cont|   (parseChar(':')
        |d_cont| (parseNewline
        |d_cont| parseImage))))(s)


def parseTiles(s):
    return parseList(parseChar('\n') |cont| parseChar('\n'), parseTile)(s)


# call main
main()
