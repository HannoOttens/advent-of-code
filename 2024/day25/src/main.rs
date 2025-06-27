
extern crate shared;

type Pins = (u8, u8, u8, u8, u8);
const DAY : i32 = 25;

fn main() {
	shared::bench (run);
}

fn run() {
	let content = shared::read_input(DAY);
	let (locks, keys) = parse_input(&content);
	println!("{:?}", locks.len());
	println!("{:?}", keys .len());
	println!("{:?}", matches(locks, keys));
}

// =============================================================================
// vv part a
// =============================================================================

fn test_fit(lock : Pins, key : Pins) -> bool {
	   (lock.0 + key.0 < 6)
	&& (lock.1 + key.1 < 6)
	&& (lock.2 + key.2 < 6)
	&& (lock.3 + key.3 < 6)
	&& (lock.4 + key.4 < 6)
}

fn matches(locks : Vec<Pins>, keys : Vec<Pins>) -> usize {
	keys.into_iter()
		.map(|key| locks.iter().filter(|lock| test_fit(**lock, key)).count())
		.sum()
}

// =============================================================================
// vv parse
// =============================================================================

fn max_pin(rows: &Vec<Vec<usize>>, is_key: bool, column : usize) -> u8 {
	rows.iter().map(|x| x[column]).sum::<usize>() as u8 - 1
}

fn parse_pins(pins: &str) -> (bool, Pins) {
	let is_key = pins.starts_with('.');
	let rows = pins.split("\r\n")
		.map(|l| l.chars().map(|c| if c == '#' { 1 } else { 0 }).collect())
		.collect();
	(
		is_key,
		(
			max_pin(&rows, is_key, 0),
			max_pin(&rows, is_key, 1),
			max_pin(&rows, is_key, 2),
			max_pin(&rows, is_key, 3),
			max_pin(&rows, is_key, 4)
		)
	)
}

fn parse_input(content : &String) -> (Vec<Pins>, Vec<Pins>) {
	let keys_and_locks : Vec<_> =
		content.split("\r\n\r\n")
		.map(parse_pins)
		.collect();

	let keys  = keys_and_locks.iter().filter(|(is_key,_)|  *is_key).map(|(_,p)| *p).collect();
	let locks = keys_and_locks.iter().filter(|(is_key,_)| !*is_key).map(|(_,p)| *p).collect();
	(locks, keys)
}

// =============================================================================
// vv tests vv

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn inputs_exists() {
		shared::read_input(DAY);
    }

	#[test]
    fn test_parse() {
		let input = shared::read_input(DAY);
		let (locks, keys) = parse_input(&input);
    }
}
