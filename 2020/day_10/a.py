
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()
    
    # Parse
    (jolts, rest) = parseCode(rules)
    if rest != '':
        raise "Parsing error! Remaining input: " + rest
    
    # Sort
    jolts.sort()
    
    # Compute
    diffs = defaultdict(int)
    diffs[jolts[0]] += 1 # Diff of 0 to first
    diffs[3] += 1 # Diff of last adapter to device
    for idx in range(len(jolts))[:-1]:
        diffs[jolts[idx + 1] - jolts[idx]] += 1  
    
    # Output
    print(diffs)
    print(diffs[1] * diffs[3])

# ===================
# Algebra
# ===================

def parseCode(s):
    return (parseList(parseChar('\n'), parseNum))(s)
    
# call main
main()