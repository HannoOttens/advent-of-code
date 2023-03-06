
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

    res = next(parsePlayers(rules), None)
    if res == None:
        raise Exception("Parsing failed")
    print(res[0])

    player1 = res[0][0][1]
    player2 = res[0][1][1]
    _, rnd = recursiveCombat(player1, player2)
    print(rnd)
    score = 0
    score_cards = player1 if len(player1) > 0 else player2
    print(score_cards)
    for idx, scr in enumerate(reversed(score_cards)):
        score += (idx + 1) * scr
    print(score)


def recursiveCombat(p1,p2):
    game_dict = defaultdict(bool)
    rnd = 0
    while len(p1) > 0 and len(p2) > 0:
        rnd += 1
        c1 = p1.pop(0)
        c2 = p2.pop(0)

        # print()
        # print(f'-- Round {rnd} --')
        # print(f'Deck P1: {p1}')
        # print(f'Deck P2: {p2}')
        # print(c1)
        # print(c2)

        # Game check
        if game_dict[hashVal(p1) * hashVal(p2)]:
            # print('BIN THERE!')
            p1 = (p1 + p2) + lrs([c1,c2])
            p2 = []
            break
        game_dict[hashVal(p1) * hashVal(p2)] = True

        # Recurse
        if c1 <= len(p1) and c2 <= len(p2):
            # print('!!!!!!!SUBGAME!!!!!!!!')

            (res_p1, _) = recursiveCombat(p1[:c1], p2[:c2])
            if len(res_p1) > 0:
                p2.extend([c2,c1])
            else:
                p1.extend([c1,c2]) 
            continue

        # Base
        if c1 > c2:
            p1.extend([c1,c2])
        elif c2 > c1:
            p2.extend([c2,c1])
        else:
            raise "Whut"

    # print('==========END==============')
    return (p1, p2), rnd


def hashVal(l):
    return hash(tuple(l))

def lrs(l):
    l.sort()
    return list(l)

# ===================
# Algebra
# ===================

def parsePlayer(s):
    return (parseToken('Player') |d_cont| (parseNum |cont| (parseChar(':') |d_cont| (parseNewline |d_cont| parseList(parseChar('\n'), parseNum)))))(s)

def parsePlayers(s):
    return parseList(parseChar('\n') |cont| parseChar('\n'), parsePlayer)(s)


# call main
main()
