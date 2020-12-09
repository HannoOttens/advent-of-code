
# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read().splitlines()

    bag_dict = dict()
    for rule in rules:
        res, s = parseRule(rule)
        if s != '.':
            raise "Unknown bag definition: " + s
        (bag, contains) = res
        bag_dict[bag] = contains

    containers = 0
    for bag in bag_dict.keys():
        if 'shiny gold' in bag:
            continue
        if dfs(bag, bag_dict):
            containers += 1
            
    print(containers) 

# ===================
# DFS
# ===================
def dfs(bag, bags_dict):
    if 'shiny gold' in bag:
        return True

    for c_bag in bags_dict[bag]:
        if dfs(c_bag, bags_dict):
            return True
                
    return False

# ===================
# Paser lib stuff
# ===================
def cont(p1, p2):
    def f(s):
        s2 = parseWS(s)
        r1, s3 = p1(s2)
        if r1 != None:
            s4 = parseWS(s3)
            r2, s5 = p2(s4)
            return (r1, r2), s5
        else:
            return None, s
    return f

# Cont, but drop first
def _cont(p1, p2):
    def f(s):
        res,s2 = cont(p1,p2)(s)
        if res != None:
            return res[1],s2
        return None, s
    return f

def cor(p1, p2):
    def f(s):
        r1, s2 = p1(s)
        if r1 == None:
            return p2(s)
        return r1, s2
    return f

def parseChar(c):
    def f(s):
        if s[0] == c:
            return c, s[1:]
        return None, s
    return f

def parseNum(s):
    if not s[0].isdigit():
        return None, s
    num = ''
    while s[0].isdigit():
        num += s[0]
        s = s[1:]
    return int(num), s

def parseWord(s):
    if not s[0].isalpha():
        return None, s
    word = ''
    while s[0].isalpha():
        word += s[0]
        s = s[1:]
    return word, s

def parseList(parserSep, parser):
    def f(s):
        l = []
        item, s2 = parser(s)
        while item != None:
            l.append(item)
            r, s2 = cont(parserSep, parser)(s2)
            if r != None:
                item = r[1]
            else:
                item = None
        return l, s2
    return f

def parseWS(s):
    while s[0] == ' ':
        s = s[1:]
    return s

# ===================
# Algebra
# ===================

def parseBag(s):
    bag = cont(parseWord,
               cont(parseWord,parseWord))(s)
    if bag != None:
        [tint, [color, tag]], s2 = bag
        if tag == 'bag' or tag == 'bags':
            return f'{tint} {color}', s2
    return None, s

def parseNoOther(s):
    if s.startswith('no other bags'):
        return [], s.replace('no other bags', '')
    return None, s


def parseContain(s):
    [contain, *rest] = s.split(' ')
    if contain == 'contain':
        return contain, ' '.join(rest)
    return None, s

def parseRule(s):
    r = cont(parseBag, 
             _cont(parseContain, cor(parseNoOther, 
                                     parseList(parseChar(','), 
                                               _cont(parseNum, parseBag)))))(s)
    return r


# call main
main()

