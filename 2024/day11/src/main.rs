use std::collections::HashMap;

extern crate shared;
const DAY : i32 = 11;
type Stones = Vec<u32>;
type Mem = HashMap<(u32, u32), u64>;

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let stones = parse_input(content);
	if shared::is_part_a() {
		println!("{:?}", map_blink(stones, 25));
	} else {
		println!("{:?}", map_blink(stones, 75));
	}
}

// =============================================================================
// vv part a/b

fn count_digits(stone : u32) -> u32 {
	if stone < 10           { return 1; }
	if stone < 100          { return 2; }
	if stone < 1000         { return 3; }
	if stone < 10000        { return 4; }
	if stone < 100000       { return 5; }
	if stone < 1000000      { return 6; }
	if stone < 10000000     { return 7; }
	if stone < 100000000    { return 8; }
	if stone < 1000000000   { return 9; }
	10
}

fn split_stone(digits : u32, stone : u32) -> (u32, u32) {
	let power = 10u32.pow(digits as u32 / 2);
	(stone / power, stone % power)
}

fn blink(mem : &mut Mem, blinks : u32, stone : u32) -> u64 {
	if blinks == 0 { return 1; }

	if let Some(totl) = mem.get(&(blinks, stone)) {
		return *totl;
	}

	let total;
	if stone == 0 {
		total = blink(mem, blinks-1, 1);
	} else {
		let digits = count_digits(stone);
		if (digits & 1) == 0 {
			let (p1, p2) = split_stone(digits, stone);
			total = blink(mem, blinks-1, p1) + blink(mem, blinks-1, p2);
		} else {
			total = blink(mem, blinks-1, stone * 2024);
		}
	}

	mem.insert((blinks, stone), total);
	total
}

fn map_blink(stones : Stones, blinks : u32) -> u64 {
	let mut mem = HashMap::new();
	stones.into_iter()
		.map(|s| blink(&mut mem, blinks, s))
		.sum()
}

// =============================================================================
// vv parse

fn parse_input(content : String) -> Stones {
	content.split_whitespace()
		.map(|x| x.parse().unwrap())
		.collect()
}

// =============================================================================
// vv tests vv

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn inputs_exists() {
		shared::read_input(DAY);
		shared::read_test_input_n(DAY, 0);
		shared::read_test_input_n(DAY, 1);
    }

	#[test]
    fn test_blink_stone_zero() {
		assert_eq!(blink(&mut HashMap::new(), 1, 0), 1);
    }

	#[test]
    fn test_split() {
		assert_eq!(split_stone(6, 123456), (123,456));
    }

	#[test]
    fn test_blink_stone_zero_to_one_to_2024_to_two_stones() {
		assert_eq!(blink(&mut HashMap::new(), 3, 0), 2);
    }

	#[test]
    fn test_blink_182_i_mean_blink_25() {
		let content = shared::read_test_input_n(DAY, 1);
		let stones = parse_input(content);
		assert_eq!(map_blink(stones, 25), 55312);
    }
}