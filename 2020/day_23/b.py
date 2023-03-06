_str = '872495136'
inp = []
for c in _str:
    inp.append(int(c))

# Make dict of cup to next cup
d = dict()
for i in range(1000000):
    d[i] = i+1
for k, v in zip(inp, inp[1:] + [len(inp) + 1]):
    d[k] = v
d[1000000] = inp[0]

# Iterate!
mn = min(inp)
mx = 1000000
current = inp[0]
for i in range(10000000):
    p1 = d[current]
    p2 = d[p1]
    p3 = d[p2]

    # Find target
    target = current - 1
    while target < mn or target in [p1,p2,p3]:
        target = target - 1
        if target < mn: # Loop back to max
            target = mx 

    gap_r = d[p3]
    r_join_r = d[target]

    d[current] = gap_r  
    d[target] = p1    
    d[p3] = r_join_r    

    current = d[current]

# Print result
c1 = d[1]
c2 = d[c1]
print(c1*c2)