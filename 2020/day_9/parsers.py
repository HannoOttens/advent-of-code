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
            if r1 != None:
                return (r1, r2), s5
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
        if len(s) > 0 and s[0] == c:
            return c, s[1:]
        return None, s
    return f


def parseNum(s):
    # First char has to be +,- or num
    if len(s) == 0 or not s[0].isdigit() \
            and not s[0] == '+' \
            and not s[0] == '-':
        return None, s
    # Rest can only be num
    num = s[0]
    s = s[1:]
    while len(s) > 0 and s[0].isdigit():
        num += s[0]
        s = s[1:]
    return int(num), s

def parseWord(s):
    if not s[0].isalpha():
        return None, s
    word = ''
    while len(s) > 0 and s[0].isalpha():
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
    while len(s) > 0 and s[0] == ' ':
        s = s[1:]
    return s
