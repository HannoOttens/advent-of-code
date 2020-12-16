
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
    
    # Give meaningful names
    fields = numbers[0]
    tickets = numbers[1]
    my_ticket = tickets[0]
    nearby_tickets = tickets[1]

    # Discard invalid tickets
    ranges = flatten(fields)
    valid_tickets = [my_ticket]
    for ticket in nearby_tickets:
        for n in ticket:
            if not in_ranges(ranges, n):
                break
        else: # Dirty python trick, for-else, else when break is not triggered
            valid_tickets.append(ticket)
    
    # Cross check numbers against field ranges
    d = defaultdict(lambda: defaultdict(lambda: True))
    for ticket in valid_tickets:
        for idx, n in enumerate(ticket):
            for field, ranges in fields:
                d[idx][field] &= in_ranges(ranges, n)

    # Solve constraints
    assignment = dict()
    while len(assignment.keys()) < len(fields):
        for field_idx, options in d.items():
            # Check if there is only one possible option for this field
            if sum(options.values()) == 1:
                field = key_for_value(options, True)
                assignment[field_idx] = field
                # Field no longer an option for all other idxs
                for opts in d.values():
                    opts[field] = False
    
    # Multiply motherfucker
    n = 1
    for idx, field in assignment.items():
        if field.startswith('departure '):
            n *= my_ticket[idx]
    print(n)


def key_for_value(d, v):
    for key, value in d.items():
        if v == value:
            return key
    return None

        
def in_ranges(ranges, n):
    for (mn, mx) in ranges:
        if mn <= n <= mx:
            return True
    return False


def flatten(arr):
    res = []
    for (_, (a,b))  in arr:
        res.append(a)
        res.append(b)
    return res

# ===================
# Algebra
# ===================

def parseRange(s):
    return (parseNum |cont| (parseChar('-') |d_cont| parseNum))(s)

def parseField(s):
    return  (parseText 
     |cont| (parseChar(':') 
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