
from parsers import *
from collections import defaultdict
import math

# ===================
# Code
# ===================

solution_a = ([3217, 2131, 1997, 1619, 3251, 1129, 2741, 2917, 1871, 1223, 2203, 1093, 2297, 3323, 1747, 3911, 1237, 1889, 1721, 1409, 2281, 1931, 2011, 3187, 1753, 1609, 2851, 2909, 1531, 2383, 2029, 3989, 1987, 3359, 3889, 1013, 3863, 3457, 2027, 3923, 3803, 3739, 2677, 1433, 2251, 1451, 3947, 1523, 2467, 3677, 3347, 3701, 2731, 2207, 2423, 1097, 1759, 1637, 1559, 1663, 3659, 2803, 1499, 1229, 2503, 1879, 3533, 3571, 2269, 3847, 2711, 2411, 3019, 2551, 2687, 
3037, 2857, 3491, 2243, 3391, 2789, 3331, 3061, 2143, 2153, 1279, 1181, 1249, 1459, 1571, 1579, 2003, 2441, 1657, 2341, 1291, 2593, 2521, 2017, 3049, 1999, 1217, 3449, 2719, 2659, 2777, 2539, 1877, 3371, 1823, 3559, 3109, 1399, 1277, 1289, 1213, 2843, 3257, 2137, 1009, 1361, 2381, 1063, 1483, 3319, 1979, 1481, 2293, 3119, 3343, 3709, 1069, 1613, 3769, 3011, 1811, 2087, 2081, 1117, 3851, 1061, 2213, 3373, 1321], [1, 3, 1, 0, 0, 1, 1, 1, 1, 2, 2, 0, 
2, 2, 2, 2, 1, 2, 2, 1, 2, 0, 2, 0, 1, 1, 0, 0, 0, 0, 2, 1, 2, 1, 2, 0, 0, 2, 2, 3, 0, 1, 0, 3, 2, 1, 1, 2, 3, 0, 3, 1, 0, 0, 1, 2, 2, 3, 0, 3, 1, 1, 1, 0, 2, 2, 2, 2, 3, 3, 2, 1, 1, 1, 2, 2, 1, 1, 0, 2, 0, 2, 1, 1, 1, 0, 0, 0, 1, 1, 0, 3, 0, 0, 2, 0, 1, 1, 2, 0, 2, 0, 3, 0, 1, 3, 2, 0, 0, 1, 0, 0, 3, 1, 2, 3, 0, 1, 3, 2, 3, 2, 2, 2, 0, 
0, 2, 2, 1, 0, 1, 0, 1, 2, 0, 2, 2, 1, 0, 3, 2, 1, 0, 0], [1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 
0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 
1, 0, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0])

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