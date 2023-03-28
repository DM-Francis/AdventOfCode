use std::{fs, collections::HashMap};

#[derive(PartialEq, Eq, Hash, Clone, Copy)]
struct Position(i32, i32);

fn main() {
    let input = fs::read_to_string("input.txt").expect("Could not find input.txt");

    let mut current = Position(0,0);
    let mut visited = HashMap::from([(current, 1)]);

    for command in input.chars() {
        current = move_position(current, command);

        *visited.entry(current).or_default() += 1;
    }

    println!("Visited at least once: {}", visited.len());

    // Part 2
    let mut current_santa = Position(0,0);
    let mut current_robo_santa = Position(0,0);
    let mut visited_2 = HashMap::from([(current_santa, 2)]);

    for (i, command) in input.char_indices() {
        if i % 2 == 0 {
            current_santa = move_position(current_santa, command);
            *visited_2.entry(current_santa).or_default() += 1;
        }
        else {
            current_robo_santa = move_position(current_robo_santa, command);
            *visited_2.entry(current_robo_santa).or_default() += 1;
        }
    }
    
    println!("Visited at least once (with robo): {}", visited_2.len());
}

fn move_position(position: Position, command: char) -> Position {
        return match command {
            '^' => Position(position.0, position.1 + 1),
            'v' => Position(position.0, position.1 - 1),
            '>' => Position(position.0 + 1, position.1),
            '<' => Position(position.0 - 1, position.1),
            _ => panic!("Invalid character found")
        };
}
