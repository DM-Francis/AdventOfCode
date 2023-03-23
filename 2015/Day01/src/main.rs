use std::fs;

fn main() {
    let input = fs::read_to_string("input.txt").expect("Input not found");

    let mut floor = 0;
    for (i, char) in input.chars().enumerate() {

        match char {
            '(' => floor += 1,
            ')' => floor -= 1,
            _ => panic!("Invalid character")
        };

        if floor == -1 {
            println!("{}", i + 1);
            break;
        }
    }

    println!("{}", floor);
}