
use std::fs;
use std::env;

pub fn read_input(day: i32) -> String {
	let filename = format!("../inputs/{}.txt", day);

	fs::read_to_string(&filename)
		.expect("Cannot read file.")
}


pub fn is_part_a () -> bool {
    let args: Vec<String> = env::args().collect();
	args.len() == 1
}

#[cfg(test)]
mod tests {
    use super::*;
}
