
from parsers import *
from collections import defaultdict

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    # Parse
    (numbers, rest) = parseTickets(rules)
    if rest != "":
        raise Exception("Remaining input in parser:\n" + rest)
    
    ranges = flatten(numbers[0])
    tickets = numbers[1]
    my_ticket = tickets[0]
    nearby_tickets = tickets[1]

    ticket_scanning_error = 0
    for ticket in nearby_tickets:
        for n in ticket:
            if not in_ranges(ranges, n):
                ticket_scanning_error += n
    print(ticket_scanning_error)

    
def in_ranges(ranges, n):
    for (mn, mx) in ranges:
        if mn <= n <= mx:
            return True
    return False

def flatten(arr):
    res = []
    for lst in arr:
        for item in lst:
            res.append(item)
    return res

# ===================
# Algebra
# ===================

def parseRange(s):
    return (parseNum |cont| (parseChar('-') |d_cont| parseNum))(s)

def parseField(s):
    return (parseList(parseWS, parseWord) 
   |d_cont| (parseChar(':') 
   |d_cont| (parseRange 
     |cont| (parseToken('or') 
   |d_cont| parseRange))))(s)

def parseFields(s):
    return parseList(parseChar('\n'), parseField)(s)

def parseNums(s):
    return (parseList(parseChar(','), parseNum))(s)

def parseTickets(s):
    return (parseFields
       |cont| (parseNewlines 
     |d_cont| (parseToken('your ticket:')
     |d_cont| (parseNewline 
     |d_cont| (parseNums 
       |cont| (parseNewlines 
     |d_cont| (parseToken('nearby tickets:')
     |d_cont| (parseNewline 
     |d_cont| (parseList(parseChar('\n'), parseNums))))))))))(s)

# Call main
main()
