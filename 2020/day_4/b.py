import re

# Passport validators
def checkHeight(v):
    if v.endswith('cm'):
        v = v.replace('cm','')
        return v.isdigit() & (150 <= int(v) <= 193)
    elif v.endswith('in'):
        v = v.replace('in','')
        return v.isdigit() & (59 <= int(v) <= 76)
    return False
validator = {'byr': lambda v: 1920 <= int(v) <= 2002,
             'iyr': lambda v: 2010 <= int(v) <= 2020,
             'eyr': lambda v: 2020 <= int(v) <= 2030,
             'hgt': checkHeight,
             'hcl': lambda v: len(re.findall("#[0-9a-f]{6}", v)) > 0,
             'ecl': lambda v: v in ['amb', 'blu', 'brn', 'gry', 'grn', 'hzl', 'oth'],
             'pid': lambda v: len(v) == 9 and v.isdigit()}

# Open file
with open("input.txt", "r") as f:
    file = f.read()

# Passport separtor
new_passport = '\n\n'

# Validate passports
passports = file.split(new_passport)
valid_passports = 0
for passport in passports:
    passport = passport.replace(' ', '\n')
    tokens = passport.split('\n')

    # Validator properties
    validation_map = dict(map(lambda t: (t, False), validator.keys()))
    for token in tokens:
        [_id, v] = token.split(':')
        if _id == 'cid':
            continue
        validation_map[_id] = validator[_id](v)

    # All properties are valid
    if all(validation_map.values()):
        valid_passports += 1

print(valid_passports)
