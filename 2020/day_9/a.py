
from parsers import *

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()
    
    (code, rest) = parseCode(rules)
    if rest != '':
        raise "Parsing error! Remaining input: " + rest
    
    index = 26
    while index < len(code):
        target = code[index]
        sum_list = code[index-25:index]
        if not check_target(target, sum_list):
            break
        index += 1
    print(code[index])

def check_target(target, sum_list):
    for idx1, e1 in enumerate(sum_list): 
        for idx2, e2 in enumerate(sum_list): 
            if idx1 == idx2 or idx2 < idx1:
                continue
            if e1 + e2 == target:
                return True
    return False

# ===================
# Algebra
# ===================

def parseCode(s):
    return (parseList(parseChar('\n'), parseNum))(s)
    
# call main
main()