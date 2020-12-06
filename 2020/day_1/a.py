with open("input.txt", "r") as f:
    nums = list(map(int,f.readlines()))
def fun():
    for i in nums:
        for j in nums:
            if i + j == 2020:
                print(i * j)
                return
fun()