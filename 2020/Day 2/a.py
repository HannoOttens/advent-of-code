with open("a.txt", "r") as f:
    lines = f.readlines()

valid = 0
for line in lines:
    [policy, pw] = line.split(':')
    [rnge, ch] = policy.split(" ")
    [mn, mx] = list(map(int,rnge.split("-")))
    if mn <= pw.count(ch) <= mx:
        valid += 1
print(valid)