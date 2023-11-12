import numpy as np
from scipy.spatial.transform import Rotation


class Scanner:
    def __init__(self, scanner_id: int, beacons: np.ndarray):
        self.id = scanner_id
        self.beacons = beacons
        self.rotation = Rotation.identity()
        self.offset = np.array([0, 0, 0])
        self.finalized = False


def create_scanner_from_raw(raw_scanner: str) -> Scanner:
    scanner_lines = raw_scanner.splitlines()
    scanner_id = int(scanner_lines[0].split(' ')[2])
    beacons_array = np.genfromtxt(scanner_lines[1:], delimiter=',')
    return Scanner(scanner_id, beacons_array)


if __name__ == '__main__':
    # read lines from file into array
    with open('input') as f:
        raw_input = f.read()

    raw_input = """--- scanner 0 ---
-1,-1,1
-2,-2,2
-3,-3,3
-2,-3,1
5,6,-4
8,0,7
"""
    raw_scanners = raw_input.split('\n\n')
    scanners = []
    for scanner in raw_scanners:
        scanner_object = create_scanner_from_raw(scanner)
        scanners.append(scanner_object)

    all_rotations = Rotation.create_group('O')
    print(len(all_rotations))
    for scanner in scanners:
        print(f'Scanner id: {scanner.id}. Beacon count: {scanner.beacons.shape[0]}. Last beacon: {scanner.beacons[-1]}')
        for rotation in all_rotations:
            rotated_beacons = rotation.apply(scanner.beacons)
            print(rotated_beacons)
