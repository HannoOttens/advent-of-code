
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    (alg,   rest) = parseAlgebra(rules)
    (_,     rest) = (parseNewline |cont| parseNewline)(rest)
    (lines, rest) = parseLines(rest)
    if len(rest) > 0:
        raise "Parsing failed"
    parser = makeParser(dict(alg))
    
    valid = 0
    for l in lines:
        _, rest = parser(l)
        if len(rest) == 0:
            valid += 1
    print(valid)

def makeParser(alg):
    d = dict()
    for key, options in reversed(alg.items()):
        parser = parseFail
        for option in options:
            parser = parser |cor| parserFor(d, option)
        d[key] = parser

    # Change 8 and 11
    def p8(s):
        return (d[42] |cont| (d[8] |cor| parseNone))(s)
    d[8] = p8
    
    def p11(s):
        return (d[42] |cont| ((d[11] |cont| d[31]) |cor| d[31]))(s)
    d[11] = p11

    return d[0]

def parserFor(d, option):
    def f(s):
        if type(option) == str:
            return parseChar(option)(s)
        parser = parseNone
        for step in option:
            parser = parser |cont| d[step]
        return parser(s)
    return f

# ===================
# Algebra
# ===================

def parseOption(s):
    return parseList(parseWS, parseNum)(s)

def parseTerm(s):
    return (parseChar('"') 
  |d_cont| (oneOf(['a', 'b']) 
  |cont_d| parseChar('"')))(s)

def parseRule(s):
    return (parseNum 
    |cont| (parseChar(':') 
  |d_cont| parseList(parseChar('|'), parseTerm |cor| parseOption)))(s)

def parseAlgebra(s):
    return parseList(parseChar('\n'), parseRule)(s)

def parseLines(s):
    return parseList(parseChar('\n'), parseWord)(s)


# call main
main()

# ((1, +, 2), *, 3)