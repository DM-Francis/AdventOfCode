import numpy as np

def expand_in_all_directions(array):
    new_array = np.zeros((array.shape[0] + 2, array.shape[1] + 2, array.shape[2] + 2, array.shape[3] + 2))
    new_array[1:-1,1:-1,1:-1,1:-1] = array
    return new_array

def count_active_neighbors(array):
    # Plan is to create 80 arrays, where each is a copy of the original,
    # but shifted in 1 direction of the 80 directions, and without wrapping around.
    # We can use numpy.roll, roll does wrap around, so we need to ensure the edges are all reset to 0 after rolling.
    # Although, if we assume that 'array' always has zeros on the edges, we don't need to worry about about wrapping

    directions = np.indices((3,3,3,3)) - 1
    directions = np.moveaxis(directions, 0, 4)
    directions = np.reshape(directions, (-1,4))

    num_array = array.astype(int)

    neighbor_count = -num_array  # ensure we only count neigbors, not the original square
    for i in np.arange(directions.shape[0]):
        neighbor_array = np.roll(num_array, (directions[i][0],directions[i][1],directions[i][2],directions[i][3]), (0,1,2,3))
        neighbor_count += neighbor_array

    return neighbor_count

def run_cycle(array):
    new_array = expand_in_all_directions(array)
    active_neighbors = count_active_neighbors(new_array)

    remained_active = np.logical_and(new_array == True, np.logical_or(active_neighbors == 2, active_neighbors == 3))
    new_active = np.logical_and(new_array == False, active_neighbors == 3)

    all_active = np.logical_or(remained_active, new_active)

    return all_active

def render_array(array):
    print(np.where(array, np.full_like(array, '#', dtype=str), np.full_like(array, '.', dtype=str)))

if __name__ == "__main__":
    input_file = open("input.txt")
    input_lines = input_file.readlines()

    input_array = np.full((1, 1, len(input_lines), len(input_lines[0]) - 1), '.', dtype=str)

    for i in range(len(input_lines)):
        for j in range(len(input_lines[0]) - 1):
            input_array[0,0,i,j] = input_lines[i][j]

    initial_state = input_array == '#'
    state = initial_state
    for i in range(6):
        state = run_cycle(state)

    render_array(state)

    print(np.sum(state))