from functools import reduce

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
        [(_, s2)] = parseWS(s)
        for (r1, s3) in p1(s2):
            [(_, s4)] = parseWS(s3)
            for r2, s5 in p2(s4):
                yield (r1, r2), s5
    return f
cont = Infix(__cont)

# Cont, but drop first
def __d_cont(p1, p2):
    def f(s):
        for (res,s2) in cont(p1,p2)(s):
            yield res[1],s2
    return f
d_cont = Infix(__d_cont)

# Cont, but drop first
def __cont_d(p1, p2):
    def f(s):
        for (res,s2) in cont(p1,p2)(s):
            yield res[0],s2
    return f
cont_d = Infix(__cont_d)

def __cor(p1, p2):
    def f(s):
        for a in p1(s):
            yield a
        for b in p2(s):
            yield b
    return f
cor = Infix(__cor)

def parseChar(c):
    def f(s):
        if len(s) > 0 and s[0] == c:
            return [(c, s[1:])]
        return []
    return f

def parseNum(s):
    # First char has to be +,- or num
    if len(s) == 0 or not s[0].isdigit() \
            and not s[0] == '+' \
            and not s[0] == '-':
        return

    # Rest can only be num
    num = s[0]

    s = s[1:]
    while len(s) > 0 and s[0].isdigit():
        num += s[0]
        s = s[1:]

    yield int(num), s

def parseWord(s):
    if not s[0].isalpha():
        return []
    word = ''
    while len(s) > 0 and s[0].isalpha():
        word += s[0]
        s = s[1:]
    yield word, s

def parseText(s):
    for lst, s2 in parseList(parseWS, parseWord)(s):
        if len(lst) > 0:
            yield ' '.join(lst), s2

def parseList(parserSep, parser):
    def f(s):
        l = []
        for (item, s2) in parser(s):
            while item != None:
                l.append(item)
                for (r, s2) in cont(parserSep, parser)(s2):
                    item = r[1]
                    break
                else:
                    item = None
            yield l, s2
    return f

def parseWS(s):
    while len(s) > 0 and (s[0] == ' ' 
                       or s[0] == '\t'):
        s = s[1:]
    return [(' ', s)]

def parseNone(s):
    return [(True, s)]

def oneOf(chars):
    def f(s):
        for char in chars:
            for (c, s2) in parseChar(char)(s):
                yield c, s2
    return f

def parseToken(word):
    def f(s):
        s2 = s
        for c in word:
            res = parseChar(c)(s2)
            if len(res) > 0:
                [(_, s2)] = res 
            else:
                return []
        return [(word, s2)]
    return f

def parseNewline(s):
    if len(s) > 0 and s[0] == '\n':
        return [('\n', s[1:])]
    return []

def many(parser):
    return parseList(parseNone, parser)

def parseFail(s):
    return []