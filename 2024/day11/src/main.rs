use std::collections::HashMap;

extern crate shared;
const DAY : i32 = 11;
type Stones = Vec<u64>;
type Mem = HashMap<(u32, u64), u64>;

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

fn count_digits(mut stone : u64) -> u64 {
	let mut digits = 0;
	while stone > 0 {
		digits += 1;
		stone /= 10;
	}
	digits
}

fn split_stone(mut stone : u64) -> (u64, u64) {
	let digs = count_digits(stone);
	let mut p1 = 0;
	let mut indx = 0;
	while indx * 2 < digs {
		p1 += (stone % 10) * 10u64.pow(indx as u32);
		stone /= 10;
		indx += 1
	}
	(stone, p1)
}

fn blink(mem : &mut Mem, blinks : u32, stone : u64) -> u64 {
	if blinks == 0 {
		return 1;
	}
	if mem.contains_key(&(blinks, stone)) {
		return mem[&(blinks, stone)];
	}

	let total;
	if stone == 0 {
		total = blink(mem, blinks-1, 1);
	} else if (count_digits(stone) & 1) == 0 {
		let (p1, p2) = split_stone(stone);
		total = blink(mem, blinks-1, p1) + blink(mem, blinks-1, p2);
	} else {
		total = blink(mem, blinks-1, stone * 2024);
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