use std::collections::HashMap;

extern crate shared;

const DAY : i32 = 22;

fn main() {
	shared::bench (run);
}

fn run() {
	let content = shared::read_input(DAY);
	let monkeys = parse_input(content);

	if shared::is_part_a() {
		let total = total_number(monkeys);
		println!("{:?}", total);
	} else if shared::is_part_b() {
		let total = total_bananas(monkeys);
		println!("{:?}", total);
	}
}

// =============================================================================
// vv part a
// =============================================================================

fn prune(n : usize) -> usize {
	n % (1 << 24)
}

fn next(mut n : usize) -> usize {
	n = n ^ (n << 6);
	n = prune(n);
	n = n ^ (n >> 5);
	n = prune(n);
	n = n ^ (n << 11);
	prune(n)
}

fn monkey_number(mut time : usize, mut n : usize) -> usize {
	while time > 0 {
		n = next(n);
		time -= 1;
	}
	n
}

fn total_number(monkeys : Vec<usize>) -> usize {
	monkeys.into_iter()
		.map(|n| monkey_number(2000, n))
		.sum::<usize>()
}

// =============================================================================
// vv part b
// =============================================================================

fn monkey_price(mut n : usize) -> HashMap<(i8, i8, i8, i8), i32>
{
	let mut n2 = (n % 10) as i8; n = next(n);
	let mut n3 = (n % 10) as i8; n = next(n);
	let mut n4 = (n % 10) as i8; n = next(n);
	let mut n5 = (n % 10) as i8;

	let mut time = 2000 - 4;
	let mut map = HashMap::new();
	while time > 0 {
		let n1 = n2;
		n2 = n3;
		n3 = n4;
		n4 = n5;

		n = next(n);
		n5 = (n % 10) as i8;

		let key = ((n2 - n1), (n3 - n2), (n4 - n3), (n5 - n4));
		if !map.contains_key(&key) {
			map.insert(key, n5 as i32);
		}

		time -= 1;
	}
	map
}

// =============================================================================

fn total_bananas(monkeys : Vec<usize>) -> i32 {
	let mems : Vec<_> = monkeys.iter()
		.map(|monkey| monkey_price(*monkey))
		.collect();

	let mut money = 0;
	for c1 in -9..10 {
		for c2 in -9..10 as i8 {
			for c3 in -9..10 as i8 {
				for c4 in -9..10 as i8 {
					let curr = mems.iter()
						.map(|mem| *mem.get(&(c1, c2, c3, c4)).unwrap_or(&0))
						.sum::<i32>();
					if curr > money { money = curr; }
				}
			}
		}
	}
	money
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_input(content : String) -> Vec<usize> {
	content.lines().map(|s| s.parse().unwrap()).collect()
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
    }

	#[test]
    fn test_prune() {
		assert_eq!(prune(100000000), 16113920);
    }

	#[test]
    fn test_part_a() {
		let content = shared::read_test_input_n(DAY, 0);
		let monkeys = parse_input(content);
		assert_eq!(total_number(monkeys), 37327623);
    }
	#[test]
    fn correct_part_a() {
		let content = shared::read_input(DAY);
		let monkeys = parse_input(content);
		assert_eq!(total_number(monkeys), 21147129593);
    }

	#[test]
    fn test_part_b_123() {
		assert_eq!(*(monkey_price(123).get(&(-1, -1, 0, 2)).unwrap()), 6);
    }

	#[test]
    fn test_part_b() {
		let content = shared::read_test_input_n(DAY, 1);
		let monkeys = parse_input(content);

		assert_eq!(total_bananas(monkeys), 23);
    }
}
