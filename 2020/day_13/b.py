from parsers import parseNum, parseChar, parseList, cor, d_cont
import math

# ===================
# Code
# ===================

def main():
    # Parse
    with open("input.txt", "r") as f:
        rules = f.read()
    (busses, rest) = parsePlan(rules)
    if rest != "":
        raise Exception("Remaining input in parser: " + rest)

    # Tuple with index (i.e. offset) and filter out non-constraints
    constraints = list(filter(lambda t: t[1] != 'x', enumerate(busses)))

    # Start at the non-zero take off of first buss
    t = constraints[0][1]

    # Start by increasing t with the RTT of the first bus
    adder = t

    # Incorperate all contraints
    for (offset, bus_id) in constraints[1:]:
        # Find where the next bus arrives at 't + offset'
        t_bus = t + offset
        rem = t_bus % bus_id
        while rem != 0:
            t_bus += adder
            rem = t_bus % bus_id
        t = t_bus - offset

        # Multiply the adder to make sure all constraints
        # are met when finding the 't' the for next bus
        adder *= bus_id

    print(t)

# ===================
# Algebra
# ===================

def parsePlan(s):
    return (parseNum 
  |d_cont| (parseChar('\n')
  |d_cont| parseList(parseChar(','), parseNum |cor| parseChar('x'))))(s)

# Call main
main()
