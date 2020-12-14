
from parsers import *
import math
from collections import defaultdict

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
            mem[mem_idx] = apply_mask(mask, value) 
        else:
            raise "Unknown instruction: " + instr

    print(sum(mem.values()))

def apply_mask(mask, value):
    # Strip '0b'
    value_bits = list(bin(value))[2:]
    value_bits = (len(mask) - len(value_bits)) * ['0'] + value_bits

    for idx, mask_bit in enumerate(reversed(mask)):
        if mask_bit == '1':
            value_bits[-(idx+1)] = '1'
        elif mask_bit == '0':
            value_bits[-(idx+1)] = '0'

    # Convert back to int
    return int('0b' + ''.join(value_bits), 2)

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
