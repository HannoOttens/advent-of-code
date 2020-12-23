# Source: https://github.com/pvillano/advent2020/blob/main/day_23_3_hash_and_links.py#L1
import time

start_time = time.time()

data = """..."""
test_data = """389125467"""

cups_list = list(map(int, list(test_data)))

cups_map = list(range(1,1000000+2))

for k, v in zip(cups_list, cups_list[1:] + [len(cups_list) + 1]):
    cups_map[k] = v

cups_map[1000000] = cups_list[0]

cur_cup = cups_list[0]

for i in range(10*1000000):

    r1 = cups_map[cur_cup]
    r2 = cups_map[r1]
    r3 = cups_map[r2]
    dest_cup = (cur_cup - 2) % 1000000 + 1
    while dest_cup == r3 or dest_cup == r2 or dest_cup == r1:
        dest_cup = (dest_cup - 2) % 1000000 + 1

    gap_r = cups_map[r3]
    r_join_r = cups_map[dest_cup]

    cups_map[cur_cup] = gap_r  # gap = (cur_cup, cups_map[r3])
    cups_map[dest_cup] = r1    # l_join = (dest_cup, r1)
    cups_map[r3] = r_join_r    # r_join = (r3, cups_map[dest_cup])

    cur_cup = cups_map[cur_cup]

c1 = cups_map[1]
c2 = cups_map[c1]
end_time = time.time()
print(c1, c2, c1*c2, end_time-start_time)