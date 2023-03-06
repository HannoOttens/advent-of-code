
def main():
    public_card = 2084668
    public_door = 3704642
    private_door = crack(public_door, 7)
    print(encrypt(private_door, public_card))

def encrypt(loop_size, subject):
    n = 1
    for i in range(loop_size):
        n *= subject
        n = n % 20201227
    return n

def crack(public_key, subject):
    n = 1
    loop_size = 1
    while True:
        n *= subject
        n = n % 20201227
        if n == public_key:
            return loop_size
        if n == 1:
            raise Exception("Invalid public key")
        loop_size += 1
    return n

main()