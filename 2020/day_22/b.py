
from parsers import *
from collections import defaultdict
import math
from itertools import permutations, product
from copy import deepcopy

# ===================
# Code
# ===================


def main():
	with open("input.txt", "r") as f:
		rules = f.read()

	res = next(parsePlayers(rules), None)
	if res == None:
		raise Exception("Parsing failed")

	player1 = deepcopy(res[0][0][1])
	player2 = deepcopy(res[0][1][1])
	print('Starting hands: ')
	print (player1)
	print (player2)

	_, rnd = recursiveCombat(player1, player2)
	print()
	print('Rounds played: ' + str(rnd))
	
	score_cards = player1 if len(player1) > 0 else player2
	print(score_cards)
	
	score = 0
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

		# P1 wins immidiatly if the game state has occured before
		if game_dict[hashVal(p1) * hashVal(p2)]:
			p1 = [1]
			p2 = []
			break
		game_dict[hashVal(p1) * hashVal(p2)] = True

		if c1 <= len(p1) and c2 <= len(p2):
			(p1_wins, _) = recursiveCombat(p1[:c1], p2[:c2])
		else:
			p1_wins = (c1 > c2)

		# Base
		if p1_wins:
			p1.extend([c1,c2])
		else:
			p2.extend([c2,c1])

	# print('==========END==============')
	return (len(p1) > 0), rnd


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
