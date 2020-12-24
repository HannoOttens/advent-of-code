
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
    tiles = next(parseTile(rules))[0]

    tile_map = defaultdict(bool)
    for tile in tiles:
        x = 0
        y = 0
        for step in tile:
          if step == 'e':
              x += 1
          elif step == 'w':
              x -= 1
          elif step == 'ne':
              y += 1 # Only up
          elif step == 'nw':
              x -= 1 # Up and left
              y += 1
          elif step == 'se':
              x += 1
              y -= 1
          elif step == 'sw':
              y -= 1
        tile_map[(x,y)] = not tile_map[(x,y)]

    print(len(tiles))
    print(tile_map)
    print(sum(tile_map.values()))

# ===================
# Algebra
# ===================

def parseDir(s):
    return (parseToken('se') 
      |cor| parseToken('sw') 
      |cor| parseToken('nw') 
      |cor| parseToken('ne') 
      |cor| parseToken('e') 
      |cor| parseToken('w') 
     )(s)

def parseDirections(s):
    return many(parseDir)(s)

def parseTile(s):
    return parseList(parseChar('\n'), parseDirections)(s)

# call main
main()
