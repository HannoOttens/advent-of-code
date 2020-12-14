
from parsers import *
import math
from collections import defaultdict
from operator import itemgetter
from itertools import combinations, chain

# ===================
# Code
# ===================

def main():
    with open("input.txt", "r") as f:
        rules = f.read()

    # Parse
    (code, rest) = parseCode(rules)
    if rest != "":
        raise Exception("Remaining input in parser:\n" + rest)

    mask = None
    mem = defaultdict(int)
    for (instr, code) in code:
        if instr == 'mask':
            mask = code
        elif instr == 'mem':
            (mem_idx, value) = code
            floating_addr = apply_mask(mask, mem_idx)
            assign_to_floating(mem, floating_addr, value)
        else:
            raise "Unknown instruction: " + instr

    print(sum(mem.values()))

def apply_mask(mask, value):
    # Strip '0b'
    value_bits = list(bin(value))[2:]
    value_bits = (len(mask) - len(value_bits)) * ['0'] + value_bits

    # Apply mask
    for idx, mask_bit in enumerate(reversed(mask)):
        if mask_bit == '1':
            value_bits[-(idx+1)] = '1'
        elif mask_bit == 'X':
            value_bits[-(idx+1)] = 'X'

    return value_bits

def assign_to_floating(mem, floating_addr, value):
    # Get all indexes of Xs
    floats = list(map(itemgetter(0), filter(lambda t: t[1] == 'X', enumerate(floating_addr))))
    # Set all combinations to one
    for ones in powerset(floats):
        addr_bits = floating_addr[:]
        # Set ones
        for one in ones:
            addr_bits[one] = '1'
        # Set remaning to 0
        for idx in range(len(addr_bits)):
            if addr_bits[idx] == 'X':
                addr_bits[idx] = '0'
        # Assign value to address
        address = int('0b' + ''.join(addr_bits), 2)
        mem[address] = value

def powerset(s):
    return chain.from_iterable(combinations(s, r) for r in range(len(s)+1))

# ===================
# Algebra
# ===================

def parseMask(s):
    return (parseToken('mask') 
     |cont| (parseChar('=') 
   |d_cont| parseList(parseNone, parseChar('X') 
                           |cor| parseChar('1') 
                           |cor| parseChar('0'))))(s)

def parseAlloc(s):
    # mem
    return (parseToken('mem')
    # [x]
    |cont| (parseChar('[') |d_cont| (parseNum |cont| (parseChar(']')
    # = y
    |d_cont| (parseChar('=') |d_cont| parseNum)))))(s) 

def parseCode(s):
    return (parseList(parseChar('\n'), parseMask |cor| parseAlloc))(s)

# Call main
main()
