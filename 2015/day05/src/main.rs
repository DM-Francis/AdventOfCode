use std::{fs, collections::HashSet};

fn main() {
    let input = fs::read_to_string("input.txt")
            .expect("Could not find input.txt");
    let strings = input
            .split_terminator('\n');
    let mut nice_string_count = 0;
    let mut nice_string_count_2 = 0;

    for string in strings {
        if contains_3_vowels(&string) && contains_double_letter(&string) && does_not_contain_restricted_substrings(&string) {
            nice_string_count += 1;
        }

        if is_nice_part_2(&string) {
            nice_string_count_2 += 1;
        }
    }

    println!("{}", nice_string_count);
    println!("{}", nice_string_count_2);
}

fn contains_3_vowels(string: &str) -> bool {
    let vowels = ['a','e','i','o','u'];
    let mut vowel_count = 0;
    for char in string.chars() {
        if vowels.contains(&char) {
            vowel_count += 1;
        }
    }

    return vowel_count >= 3;
}

fn contains_double_letter(string: &str) -> bool {
    let mut previous = '0';
    for char in string.chars() {
        if char == previous {
            return true;
        }

        previous = char;
    }

    return false;
}

fn does_not_contain_restricted_substrings(string: &str) -> bool {
    let restricted_substrings = ["ab", "cd", "pq", "xy"];
    for substring in restricted_substrings {
        if string.contains(substring) {
            return false;
        }
    }

    return true;
}

fn is_nice_part_2(string: &str) -> bool {
    let first_pair = &string[..2];
    let mut existing_pairs = HashSet::<&str>::from([first_pair]);
    let mut has_surrounded_letter = false;
    let mut has_double_pair = false;
    let mut previous_pair = first_pair;

    for window in char_windows(string, 3) {
        if window.chars().nth(0) == window.chars().nth(2) {
            has_surrounded_letter = true;
        }

        let new_pair: &str =  &window[1..];

        if new_pair == previous_pair {
            previous_pair = "";
            continue; // Must be overlap
        }

        if existing_pairs.contains(&new_pair) {
            has_double_pair = true;
        } 
        else {
            existing_pairs.insert(new_pair);
            previous_pair = new_pair;
        }

        if has_surrounded_letter && has_double_pair {
            break;
        }
    }

    return has_double_pair && has_surrounded_letter;
}

fn char_windows<'a>(src: &'a str, win_size: usize) -> impl Iterator<Item = &'a str> {
    src.char_indices()
        .flat_map(move |(from, _)| {
            src[from ..].char_indices()
                .skip(win_size - 1)
                .next()
                .map(|(to, c)| {
                    &src[from .. from + to + c.len_utf8()]
                })
    })
}