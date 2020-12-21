
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
    image_borders = list(map(binaryBorder, images))

    # Size of grid
    s = round(math.sqrt(len(images)))
    solution = solve(s, dict(image_borders), image_borders[:])
    print(solution[0][0] * solution[0][11] * solution[0][132] * solution[0][143])

# Run solver
def solve(s, image_borders, rem, pos=[], rot=[], flp=[]):
    if len(rem) == 0:
        return pos, rot, flp

    # All options
    for idx, image_border in enumerate(rem):
        for r in range(4):
            for f in range(2):
                # Setup
                del rem[idx]
                # Recurse
                if match(s, image_borders, pos + [image_border[0]], rot + [r], flp + [f]):
                    res = solve(s, image_borders, rem, pos + [image_border[0]], rot + [r], flp + [f])
                    if res != None:
                        return res
                # Restore
                rem.insert(idx, image_border)
    return None

# Match all borders
def match(s, image_borders, pos, rot, flp):
    for idx in range(len(pos)):
        for side, n_idx in neighbours(s, idx, pos):
            if (image_borders[pos[idx]][flp[idx]][rot[idx]][side] \
                    != image_borders[pos[n_idx]][flp[n_idx]][rot[n_idx]][(side + 2) % 4]):
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
def binaryBorder(image):
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
    return list(map(lambda c: list(map(toBinary, c)), rots))

def toBinary(l):
    s = ''.join(l)
    return int('0b' + s.replace('.', '0').replace('#', '1'), 2)


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
