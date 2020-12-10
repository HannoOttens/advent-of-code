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
    
    # Find target
    for index in range(26, len(code)):
        target = code[index]
        sum_list = code[index-25:index]
        if not check_target(target, sum_list):
            break
        index += 1
    
    # Save target
    target = code[index]
    
    # Find sum
    for index in range(len(code)):
        _min = 9999999999999999999999999
        _max = 0
        _sum = 0
        
        i = index
        while _sum < target:
            _min = min(_min, code[i])
            _max = max(_max, code[i])
            _sum += code[i]
            i += 1
        
        if _sum == target:
            break

    print(f'Target: {target}, Index: {index}, I: {i}')
    print(f'Min: {_min}, Max: {_max}, Sum: {_min+_max}')


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