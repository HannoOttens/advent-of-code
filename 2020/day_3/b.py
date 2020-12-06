with open("input.txt", "r") as f:
    lines = f.read().splitlines() 
w = len(lines[0])
h = len(lines)
total = 1
slopes = [(1,1),(3,1),(5,1),(7,1),(1,2)]
for (dx,dy) in slopes:
    px = py = trees = 0
    while py < h:
        if lines[py][px%w] == '#':
            trees += 1
        px += dx
        py += dy
    print(f'Slope: {dx,dy}, Trees: {trees}')
    total *= trees
print(f'Total: {total}')