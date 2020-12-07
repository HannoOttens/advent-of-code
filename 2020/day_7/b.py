
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

    print(dfs('shiny gold', bag_dict))

# ===================
# DFS
# ===================
def dfs(bag, bags_dict):
    s = 0
    for count, c_bag in bags_dict[bag]:
        s += count + count * dfs(c_bag, bags_dict)
    return s

# ===================
# Fancy infix stuff
# ===================

class Infix:
    def __init__(self, function):
        self.function = function
    def __ror__(self, other):
        return Infix(lambda x, self=self, other=other: self.function(other, x))
    def __or__(self, other):
        return self.function(other)
    def __rlshift__(self, other):
        return Infix(lambda x, self=self, other=other: self.function(other, x))
    def __rshift__(self, other):
        return self.function(other)
    def __call__(self, value1, value2):
        return self.function(value1, value2)

# ===================
# Paser lib stuff
# ===================
def __cont(p1, p2):
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
cont = Infix(__cont)

# Cont, but drop first
def ___cont(p1, p2):
    def f(s):
        res,s2 = cont(p1,p2)(s)
        if res != None:
            return res[1],s2
        return None, s
    return f
_cont = Infix(___cont)

def __cor(p1, p2):
    def f(s):
        r1, s2 = p1(s)
        if r1 == None:
            return p2(s)
        return r1, s2
    return f
cor = Infix(__cor)

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
    bag = (parseWord <<cont>> (parseWord <<cont>> parseWord))(s)
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
    r = (parseBag <<cont>> (parseContain 
                  <<_cont>> (parseNoOther 
                            <<cor>> 
                            parseList(parseChar(','), 
                                      parseNum <<cont>> parseBag))))(s)
    return r


# call main
main()