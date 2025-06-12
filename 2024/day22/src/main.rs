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

fn monkey_price(
	mut time : usize, monkey : usize,
	c1 : i32, c2 : i32, c3 : i32, c4 : i32
) -> i32 {
	let mut n = monkey;

	// Genereer eerste 4 changes
	let mut n2 = (n % 10) as i32;
	n = next(n);
	let mut n3 = (n % 10) as i32;
	n = next(n);
	let mut n4 = (n % 10) as i32;
	n = next(n);
	let mut n5 = (n % 10) as i32;
	time = time - 4;

	while time > 0 {
		let n1 = n2;
		n2 = n3;
		n3 = n4;
		n4 = n5;
		n = next(n);
		n5 = (n % 10) as i32;

		if (c1 == (n2 - n1))
		&& (c2 == (n3 - n2))
		&& (c3 == (n4 - n3))
		&& (c4 == (n5 - n4)) {
			return n5;
		}

		time -= 1;
	}
	0
}

fn monkey_bananas(
	monkeys : &Vec<usize>,
	best_money : i32,
	c1 : i32, c2 : i32, c3 : i32, c4 : i32
) -> i32 {
	// test op onmogelijke sequences
	if (c1 + c2).abs() > 9 { return 0; }
	if (c2 + c3).abs() > 9 { return 0; }
	if (c3 + c4).abs() > 9 { return 0; }
	if (c1 + c2 + c3).abs() > 9 { return 0; }
	if (c2 + c3 + c4).abs() > 9 { return 0; }
	if (c1 + c2 + c3 + c4).abs() > 9 { return 0; }

	let mut curr_money = 0;
	let mut curr_index = 0;
	for monkey in monkeys {
		curr_money = curr_money + monkey_price(2000, *monkey, c1, c2, c3, c4);
		curr_index += 1;
		if curr_money < best_money - ((monkeys.len() - curr_index) as i32 * 9) {
			return 0;
		}
	}
	curr_money
}

// =============================================================================

fn total_bananas(monkeys : Vec<usize>) -> i32 {
	let mut money = 250;
	for c1 in -9..10 {
		for c2 in -9..10 {
			for c3 in -9..10 {
				for c4 in -9..10 {
					let curr = monkey_bananas(&monkeys, money,  c1, c2, c3, c4);
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
		assert_eq!(monkey_price(10, 123,-1, -1, 0, 2), 6);
    }

	#[test]
    fn test_part_b() {
		let content = shared::read_test_input_n(DAY, 1);
		let monkeys = parse_input(content);

		assert_eq!(total_bananas(monkeys), 23);
    }
}
