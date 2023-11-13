from typing import List
import numpy as np
import matplotlib as mpl
from matplotlib import pyplot as plt
from scipy.spatial.transform import Rotation


class Scanner:
    def __init__(self, scanner_id: int, beacons: np.ndarray):
        self.id = scanner_id
        self.beacons = beacons
        self.rotation = Rotation.identity()
        self.offset = np.array([0, 0, 0])
        self.finalized = False

    def get_final_beacons(self):
        return np.round(self.rotation.apply(self.beacons) + self.offset)


def create_scanner_from_raw(raw: str) -> Scanner:
    scanner_lines = raw.splitlines()
    scanner_id = int(scanner_lines[0].split(' ')[2])
    beacons_array = np.genfromtxt(scanner_lines[1:], delimiter=',')
    return Scanner(scanner_id, beacons_array)


def load_all_scanners(raw: str) -> List[Scanner]:
    raw_split = raw.split('\n\n')
    result = []
    for raw_scanner in raw_split:
        scanner_object = create_scanner_from_raw(raw_scanner)
        result.append(scanner_object)

    return result


def match_scanners(a: Scanner, b: Scanner) -> bool:
    print(f'Comparing scanners {a.id} and {b.id}.')
    max_matches = 0
    for rotation in all_rotations:
        rotated_beacons = rotation.apply(b.beacons).round()
        a_beacons = a.beacons[:, np.newaxis, :]
        b_beacons = rotated_beacons[np.newaxis, :, :]
        rel_offsets = a_beacons - b_beacons
        rel_offsets = np.reshape(rel_offsets, (-1, 3))

        unique_offsets, counts = np.unique(rel_offsets, axis=0, return_counts=True)
        valid_offsets = unique_offsets[np.nonzero(counts >= 12)]
        max_matches = max(max_matches, counts.max())

        if len(valid_offsets) > 0:
            b.rotation = a.rotation * rotation
            b.offset = np.round(a.rotation.apply(valid_offsets[0]) + a.offset)
            b.finalized = True
            return True

    print(f'Highest number of matches: {max_matches}')
    return False


def plot_scanners(scanners: List[Scanner]):
    fig = plt.figure()
    ax = fig.add_subplot(projection='3d')

    for i, scanner in enumerate(scanners):
        beacons = scanner.get_final_beacons()
        xs = beacons[:, 0]
        ys = beacons[:, 1]
        zs = beacons[:, 2]
        ax.scatter(xs, ys, zs, c=f'C{i}', marker='o')
        ax.scatter(scanner.offset[0], scanner.offset[1], scanner.offset[2], c=f'C{i}', marker='^')

    # ax.scatter(1105, -1205, 1229, c='black', marker='^')
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')

    plt.show()


def main():
    with open('example-input') as f:
        raw_input = f.read()

    print(len(all_rotations))

    scanners = load_all_scanners(raw_input)

    first = scanners[0]
    first.finalized = True  # Set scanner 0 as the canonical coordinates
    combined_scanner = Scanner(-1, first.beacons.copy())

    queue = [s for s in scanners if (not s.finalized)]
    previous = None

    while len(queue) > 0:
        current = queue.pop(0)
        if current == previous:
            print(f"Couldn't match scanner {current.id}")
            break

        # Compare current to the 'combined scanner' and check for overlaps
        print(f'Scanner id: {current.id}. Beacons: {current.beacons.shape[0]}')
        matched = match_scanners(combined_scanner, current)
        if matched:
            print(f'Matched {current.id} with offset {current.offset} and rotation {current.rotation.as_rotvec(degrees=True)}')
            transformed_beacons = current.get_final_beacons()
            new_beacon_set = np.concatenate((combined_scanner.beacons, transformed_beacons))
            combined_scanner.beacons = np.unique(new_beacon_set, axis=0)
        else:
            previous = current
            queue.append(current)

    total_beacon_count = combined_scanner.beacons.shape[0]
    uniques = np.unique(combined_scanner.beacons, axis=0)

    print(f'Final beacons: {combined_scanner.beacons}')
    print(f'Total number of beacons: {total_beacon_count}')
    # plot_scanners([s for s in scanners if s.finalized])


all_rotations = Rotation.create_group('O')
if __name__ == '__main__':
    main()
