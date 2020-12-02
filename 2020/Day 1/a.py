with open("a.txt", "r") as f:
    nums = list(map(int,f.readlines()))
def f():
    for i in nums:
        for j in nums:
            if i + j == 2020:
                print(i * j)
                return
f()