
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    (alg,   rest) = list(parseAlgebra(rules))[0]
    (_,     rest) = list((parseNewline |cont| parseNewline)(rest))[0]
    (lines, rest) = list(parseLines(rest))[0]
    if len(rest) > 0:
        raise "Parsing failed"

    parser = makeParser(dict(alg))

    valid = 0
    for l in lines:
        for (_, rem) in list(parser(l)):
            if rem == '':
                valid += 1
                break
    print(valid)

def makeParser(alg):
    d = dict()
    for key, options in alg.items():
        parser = parserFor(d, options[0])
        for option in options[1:]:
            parser = parser |cor| parserFor(d, option) 
        d[key] = parser

    # Change 8 and 11
    d[8]  = d[42] |cont| ( parserFor(d, [8])  |cor| parseNone)
    d[11] = d[42] |cont| ((parserFor(d, [11]) |cor| parseNone) |cont| d[31])

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
    return parseList(parseNone, parseNum)(s)

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