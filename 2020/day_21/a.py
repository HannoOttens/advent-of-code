
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

    # Count ingredients
    occor = 0
    allergen_free = list(map(lambda t: t[0], filter(lambda s: len(s[1]) == 0, ye_set.items())))
    for (ingrs, _) in dishes:
        for ingr in ingrs:
            if ingr in allergen_free:
                occor += 1
    print(occor)

# ===================
# Algebra
# ===================

def parseDish(s):
    return (many(parseWord) |cont| parens(parseToken('contains') |d_cont| parseList(parseChar(','), parseWord)))(s)


def parseDishes(s):
    return parseList(parseChar('\n'), parseDish)(s)


# call main
main()
