import string

with open("input.txt", "r") as f:
    file = f.read() 

new_group = '\n\n'
groups = file.split(new_group)
letters = string.ascii_lowercase[:26]

total_questions = 0
for group in groups:
    persons = group.split('\n')

    validation_map = dict(map(lambda t: (t, False), letters))
    for person in persons:
        for l in person:
            validation_map[l] = True
    total_questions += sum(validation_map.values())
print(total_questions)