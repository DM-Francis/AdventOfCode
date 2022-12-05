import numpy as np

input = open("Day1Input.txt").readlines()

total = 0
existingtotals = []

freqBase = np.empty(len(input), int)

counter = 0
for line in input:
    total += int(line)
    freqBase[counter] = total
    counter += 1

all_freqs = np.copy(freqBase)
baseLast = freqBase[-1]

finished = False
counter = 0
while finished == False:
    current_freqs = freqBase + (2 * counter + 1) * baseLast
    
    any_matches = np.isin(current_freqs, all_freqs)
    if np.any(any_matches):
        matches = np.nonzero(np.where(any_matches, current_freqs, 0))
        print(current_freqs[matches][0])
        finished = True
    else:
        all_freqs = np.append(all_freqs, current_freqs)

    counter += 1
