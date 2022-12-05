import numpy as np 

input = open("day2input.txt").readlines()

def has_any_twos (box_id):
    id_list = list(box_id)

    for char in id_list:
        if id_list.count(char) == 2:
            return True

    return False

def has_any_threes (box_id):
    id_list = list(box_id)

    for char in id_list:
        if id_list.count(char) == 3:
            return True

    return False

twos_count = 0
threes_count = 0

for line in input:
    if has_any_twos(line) == True:
        twos_count += 1
    if has_any_threes(line) == True:
        threes_count += 1

print(twos_count * threes_count)


all_boxes = []
for line in input:
    box_as_array = np.array(list(line), str)
    all_boxes.append(box_as_array)

print(all_boxes)

for box1 in all_boxes:
    for box2 in all_boxes:
        if box1 is box2:
            break

        letters_equal = np.core.defchararray.equal(box1, box2)

        matches = np.where(letters_equal, box1, "_")
        if np.size(np.where(matches == "_")) == 1:
            print(''.join(list(matches)))