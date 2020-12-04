with open("a.txt", "r") as f:
    file = f.read() 

new_passport = '\n\n'
identifiers = ['byr', 'iyr','eyr','hgt','hcl','ecl','pid']

passports = file.split(new_passport)

valid_passports = 0
for passport in passports:
    passport = passport.replace(' ', '\n')
    tokens = passport.split('\n')

    validation_map = dict(map(lambda t: (t, False), identifiers))
    for token in tokens:
        _id = token.split(':')[0]
        validation_map[_id] = True

    if all(validation_map.values()):
        valid_passports += 1

print(valid_passports)