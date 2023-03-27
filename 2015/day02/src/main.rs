use std::fs;

fn main() {
    let input = fs::read_to_string("input.txt").expect("Input not found");
    let lines = input.split_terminator("\n");
    let mut total_paper = 0;
    let mut total_ribbon = 0;
    for line in lines
    {
        let dimensions = line.split("x");
        let mut dimensions: Vec<i32> = dimensions.into_iter().map(|x| x.parse().unwrap()).collect();
        dimensions.sort();
        let x = dimensions[0];
        let y = dimensions[1];
        let z = dimensions[2];

        let paper_needed = 2*x*y + 2*x*z + 2*y*z + x*y;
        let ribbon_needed = 2*x + 2*y + x*y*z;
        total_paper += paper_needed;
        total_ribbon += ribbon_needed;
    }

    println!("Total paper needed: {}", total_paper);
    println!("Total ribbon needed: {}", total_ribbon);
}
