with open("input.txt", "r") as f:
    lines = f.read().splitlines() 
w = len(lines[0])
h = len(lines)
dx = 3
dy = 1
px = py = trees = 0
while py < h:
    if lines[py][px%w] == '#':
        trees += 1
    px += dx
    py += dy
print(trees)
