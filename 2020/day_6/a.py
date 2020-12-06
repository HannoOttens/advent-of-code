with open("input.txt", "r") as f:
    file = f.read() 

new_group = '\n\n'
groups = file.split(new_group)

total_questions = 0
for group in groups:
    group = group.replace('\n','')
    total_questions += len(set(group))
print(total_questions)