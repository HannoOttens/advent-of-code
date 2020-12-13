
from parsers import parseNum, parseChar, parseList, cor, d_cont, cont
import math
from operator import itemgetter

# ===================
# Code
# ===================


def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    # Parse
    ((time, busses), rest) = parsePlan(rules)
    if rest != "":
        raise Exception("Remaining input in parser: " + rest)

    busses = list(filter(lambda c: c != 'x', busses))
    wait_times = map(lambda bus: bus - (time % bus), busses)
    (idx, wait_time) = min(enumerate(wait_times), key=itemgetter(1)) 

    print(busses[idx] * wait_time)

# ===================
# Algebra
# ===================

def parsePlan(s):
    return (parseNum <<cont>> (parseChar('\n') 
      <<d_cont>> parseList(parseChar(','), parseNum 
                                           <<cor>> 
                                           parseChar('x'))))(s)


# call main
main()
