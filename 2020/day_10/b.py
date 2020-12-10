
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    # Parse
    with open("input.txt", "r") as f:
        rules = f.read()
    (jolts, rest) = parseCode(rules)
    if rest != '':
        raise "Parsing error! Remaining input: " + rest

    # Sort
    jolts.sort()

    # Find where the we have sets of consecutive 1-diffs
    one_diff = [1] if jolts[0] == 1 else [0]
    for idx in range(len(jolts))[1:]:
        if jolts[idx] - jolts[idx - 1] == 1:
            one_diff[-1] += 1
        else:
            one_diff.append(0)

    # I dont feel like coding this in, deal with it. Calculated by hand.
    l_options = {1: 1, 2: 2, 3: 4, 4: 7}

    # Compute number of possible options
    options = 1
    for l in filter(lambda l: l > 0, one_diff):
        options *= l_options[l]

    # Output
    print(options)

# ===================
# Algebra
# ===================

def parseCode(s):
    return (parseList(parseChar('\n'), parseNum))(s)


# call main
main()
