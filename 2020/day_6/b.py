import string

with open("input.txt", "r") as f:
    file = f.read() 

new_group = '\n\n'
groups = file.split(new_group)
letters = string.ascii_lowercase[:26]

total_questions = 0
for group in groups:
    persons = group.split('\n')

    letter_set = set(string.ascii_lowercase[:26])
    for person in persons:
        letter_set = letter_set.intersection(set(person))
    total_questions += len(letter_set)
print(total_questions)