fn main() {
    let key = "bgvyzdsv";
    let mut i = 0;

    loop {
        i += 1;
        let input = key.to_string() + &i.to_string(); 
        let digest = md5::compute(input);

        let output = format!("{:x}", digest);
        if &output[..6] == "000000" {
            break;
        }
    }

    println!("{}", i);
}
