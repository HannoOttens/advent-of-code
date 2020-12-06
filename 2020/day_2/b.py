with open("input.txt", "r") as f:
    lines = f.readlines()
valid = 0
for line in lines:
    [policy, pw] = line.split(': ')
    [rnge, ch] = policy.split(" ")
    [mn, mx] = list(map(int,rnge.split("-")))
    if (pw[mn - 1] == ch) ^ (pw[mx - 1] == ch):
        valid += 1
print(valid)