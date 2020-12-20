
from parsers import *
from collections import defaultdict
import math

# ===================
# Code
# ===================

solution_a = ([2797, 2719, 1583, 3187, 3533, 3851, 2089, 3643, 3371, 1321, 3779, 3167, 1291, 2711, 2539, 1097, 3529, 3203, 1051, 3169, 2087, 3917, 3109, 2707, 3637, 1399, 3797, 2801, 3803, 3413, 3259, 1531, 1031, 2593, 1367, 1249, 1447, 3853, 1759, 1049, 2837, 3727, 2003, 2081, 3001, 3911, 1523, 2927, 1361, 3761, 2741, 2141, 1481, 1667, 2113, 2617, 2441, 3719, 1747, 1259, 2659, 3659, 2803, 2699, 3463, 3307, 3079, 2953, 1511, 1699, 3457, 1171, 2521, 2897, 3251, 2503, 1549, 3527, 1129, 3701, 2473, 1429, 1483, 2039, 3583, 1471, 1237, 2339, 2621, 3623, 2179, 1871, 2633, 3361, 3061, 2459, 1103, 2309, 3923, 2957, 1787, 2917, 2243, 3877, 
3617, 1499, 2693, 3559, 2543, 1789, 1373, 1181, 2591, 2273, 1721, 3067, 3319, 2437, 2857, 1601, 2129, 1231, 1013, 3613, 1777, 1163, 1753, 2029, 1657, 1297, 3221, 2161, 3593, 3467, 2383, 2239, 2371, 3331, 1093, 1877, 3019, 2647, 3389, 3517], [0, 2, 2, 0, 3, 1, 0, 3, 3, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 3, 1, 1, 0, 3, 1, 3, 1, 3, 0, 0, 1, 1, 0, 3, 3, 3, 2, 3, 2, 3, 0, 2, 0, 1, 3, 3, 1, 0, 2, 1, 0, 2, 1, 2, 0, 3, 3, 1, 1, 0, 1, 1, 3, 0, 3, 1, 2, 3, 0, 2, 1, 0, 0, 2, 0, 3, 2, 1, 1, 0, 1, 1, 0, 1, 3, 3, 0, 3, 2, 3, 2, 2, 1, 2, 2, 1, 0, 1, 1, 2, 2, 1, 3, 0, 3, 0, 0, 1, 0, 2, 1, 1, 0, 3, 3, 0, 0, 3, 3, 1, 1, 3, 3, 1, 0, 3, 
3, 1, 1, 2, 1, 0, 1, 1, 3, 0, 1, 0, 0, 1, 0, 2], [0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1])
sea_monster = [
    '                  # ',
    '#    ##    ##    ###',
    ' #  #  #  #  #  #   '
]
def main():
    with open("input.txt", "r") as f:
        rules = f.read()
    res = next(parseTiles(rules), None)
    if res == None:
        raise Exception("Parsing failed")

    for flp in range(2):
        for rot in range(4):
            # Recover image from solution
            images = dict(res[0])
            size = round(math.sqrt(len(res[0])))
            image = construct_image(size, solution_a, images)
            image_grid = list(map(list, image.split('\n')))
            image_grid = flip(flp, image_grid)
            image_grid = rotate(rot, image_grid)

            # Find sea monster #
            sea_monsters = set()
            for x in range(len(image_grid)):
                for y in range(len(image_grid)):
                    monster_locs = check_monster(image_grid, x, y)
                    for l in monster_locs:
                        sea_monsters.add(l)

            if len(sea_monsters) > 0:
                print(sum(map(lambda c: c == '#', image)) - len(sea_monsters))
                return

def check_monster(image_grid, x, y):
    monster_locs = []
    for i, row in enumerate(sea_monster):
        for j, c in enumerate(list(row)):
            if c != '#':
                continue
            lx = x + j
            ly = y + i
            if lx >= len(image_grid) or ly >= len(image_grid):
                return []
            if image_grid[ly][lx] != c:
                return []
            if c == '#':
                monster_locs.append((lx, ly))
    return monster_locs

def construct_image(size, sol, images):
    full_image = []
    for i, (img_id, rot, flp) in enumerate(zip(sol[0], sol[1], sol[2])):
        # Start new row
        if i % size == 0:
            full_image.append([])

        # Transform
        img = images[img_id]
        img = strip_border(img)
        img = flip(flp, img)
        img = rotate(rot, img)

        full_image[-1].append(img)

    full_grid = ''
    for row in full_image:
        for i in range(len(row[0])):
            for image in row:
                full_grid += ''.join(image[i])
            full_grid += '\n'
    return full_grid[0:-1]

def flip(flp, image):
    if flp == 0:
        return image
    # Horizontal
    if flp == 1:
        return list(map(rev, image))

def rotate(r, l):
    for n in range(r):
        l = [list(reversed(x)) for x in zip(*l)]
    return l

def strip_border(image):
    image = image[1:-1]
    return list(map(lambda l: l[1:-1], image))

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