
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    (exprs, rest) = parseExprs(rules.replace(' ', ''))
    if len(rest) > 0:
        print(exprs)
        raise Exception("Parser failed, remaining input:\n"+rest)
    
    for expr in exprs:
        print(expr)

def evalExpr(expr):
    # Base case
    if type(expr) == int:
        return expr
    
    # Operators
    lhs, op, rhs = expr
    if op == '+':
        return evalExpr(lhs) + evalExpr(rhs)
    if op == '*':
        return evalExpr(lhs) * evalExpr(rhs)

# ===================
# Algebra
# ===================

# expr ::= n | expr * expr | expr + expr | (expr)

def parseOp(s):
    return (parseChar('+') |cor| parseChar('*'))(s)

def parseExpr(s):
    pExpr = ((parseExpr |cont| parseOp |cont| parseExpr) 
        |cor| parens(parseExpr))
    print(s)
    return (parseNum |cont| pExpr)(s)

def parseExprs(s):
    return parseList(parseChar('\n'), parseExpr)(s)

# call main
main()

# ((1, +, 2), *, 3)