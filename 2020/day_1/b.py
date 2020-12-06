with open("input.txt", "r") as f:
    nums = list(map(int,f.readlines()))
def fun():
    for ii, i in enumerate(nums):
        for ij, j in list(enumerate(nums))[ii:]:
            for k in nums[ij:]:
                if i + j + k == 2020:
                    print(i * j * k)
                    return
fun()