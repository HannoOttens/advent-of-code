
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    # Parse
    (numbers, rest) = parseNums(rules)
    if rest != "":
        raise Exception("Remaining input in parser:\n" + rest)
    
    d = defaultdict(lambda: [])
    for i, n in enumerate(numbers):
        d[n].append(i)

    n = numbers[-1]
    for i in range(len(numbers), 2020):
        if len(d[n]) <= 1:
            n2 = 0
        else:
            n2 = d[n][-1] - d[n][-2]
        d[n2].append(i)
        n = n2

        
    print(n)
# ===================
# Algebra
# ===================

def parseNums(s):
    return (parseList(parseChar(','), parseNum))(s)

# Call main
main()
