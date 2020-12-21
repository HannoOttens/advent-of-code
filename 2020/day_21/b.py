
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

    res = next(parseDishes(rules), None)
    if res == None:
        raise Exception("Parsing failed")
    dishes = res[0]
    dishes = list(map(lambda t: (set(t[0]), set(t[1])), dishes))
    
    # Collect all ingredients and allergens
    allergens = set()
    ingredients = set()
    for (ingrs, allrs) in dishes:
        allergens = allergens.union(allrs)
        ingredients = ingredients.union(ingrs)

    # Find which allergen can be combined with which ingredient
    no_set = defaultdict(lambda: set())
    ye_set = defaultdict(lambda: set(allergens))
    for (ingrs, allrs) in dishes:
        for ingr in ingrs:
            ye_set[ingr] = ye_set[ingr].union(allrs)
        for ingr in ingredients - ingrs:
            no_set[ingr] = no_set[ingr].union(allrs)
    for key, value in no_set.items():
        ye_set[key] = ye_set[key].difference(value)

    dangerous_ingrs = list(filter(lambda s: len(s[1]), ye_set.items()))
    matches = []
    while len(dangerous_ingrs) > 0:
        for idx, (ingr, allrs) in enumerate(dangerous_ingrs):
            if len(allrs) == 1:
                allergen = list(allrs)[0]
                matches.append((ingr, allergen))
                dangerous_ingrs = list(map(lambda s: (s[0], s[1].difference(allrs)), dangerous_ingrs))
                dangerous_ingrs = list(filter(lambda s: len(s[1]), dangerous_ingrs))
                break

    # Sort by thing
    matches = sorted(matches, key=lambda x: x[1])
    s = ''
    for (ingr,_) in matches:
        s += ingr + ','
    print(s[0:-1])

# ===================
# Algebra
# ===================

def parseDish(s):
    return (many(parseWord) |cont| parens(parseToken('contains') |d_cont| parseList(parseChar(','), parseWord)))(s)


def parseDishes(s):
    return parseList(parseChar('\n'), parseDish)(s)


# call main
main()
