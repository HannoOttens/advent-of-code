use std::cmp::min;

use shared::{Point, Point64};

extern crate shared;

const DAY : i32 = 13;
type Machine = (Point64, Point64, Point64);
type Machines = Vec<Machine>;

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let machines = parse_input(content);
	if shared::is_part_a() {
		println!("{:?}", sum_prizes(&machines));
	} else if shared::is_part_b() {
		println!("{:?}", sum_prizes_b(&machines));
	}
}

// =============================================================================
// vv part a

fn find_prize((ba, bb, prize) : &Machine) -> Option<i64> {
	for use_a in 0..100000 {
		let left = *prize - (*ba * use_a);
		if (left.x < 0) || (left.y < 0) {
			break;
		}

		let use_b = left.x / bb.x;
		if left == (*bb * use_b)
		{
			return Some(3 * use_a + use_b);
		}
	}
	None
}

fn sum_prizes(machines : &Machines) -> i64 {
	machines.iter()
		.map(find_prize)
		.flatten()
		.sum()
}

// =============================================================================
// vv part b

fn multipliers(ba : Point64, bb : Point64) -> Option<(i64, i64, i64)> {
	for use_a in 0..100 {
		for use_b in 0..100 {
			if (use_a == 0) && (use_b == 0) {
				continue;
			}

			let sump = (ba * use_a) + (bb * use_b);
			if sump.x == sump.y {
				return Some((sump.x, use_a, use_b));
			}
		}
	}
	None
}

const A_LIL_OFF : i64 = 10000000000000;
fn find_prize_b((ba, bb, prize) : &Machine) -> Option<i64> {
	let prize = Point64 { x : prize.x + A_LIL_OFF
						, y : prize.y + A_LIL_OFF };

	let (step, step_a, step_b) = multipliers(*ba, *bb)?;
	let start_step = min(prize.x / step, prize.y / step) - 100;

	let rest_prize = Point64 { x : prize.x - start_step*step
							 , y : prize.y - start_step*step };

	let cost = find_prize(&(*ba, *bb, rest_prize))?;
	Some(cost + 3*step_a*start_step + step_b*start_step)
}

fn sum_prizes_b(machines : &Machines) -> i64 {
	machines.iter()
		.map(find_prize_b)
		.flatten()
		.sum()
}

// =============================================================================
// vv parse

fn parse_button(button : &str) -> Point64 {
	Point64 {
		x : button[12..14].parse().unwrap(),
		y : button[18..20].parse().unwrap(),
	}
}

fn parse_prize(prize : &str) -> Point64 {
	let prize = &prize[7..];
	let (posx, posy) = prize.split_once(", ").unwrap();
	let (posx, posy) = (&posx[2..], &posy[2..]);

	Point64 {
		x : posx.parse().unwrap(),
		y : posy.parse().unwrap(),
	}
}

fn parse_machine(machine : &str) -> Machine {
	let parts : Vec<&str> = machine.lines().collect();
	(
		parse_button(parts[0]),
		parse_button(parts[1]),
		parse_prize(parts[2]),
	)
}

fn parse_input(content : String) -> Machines {
	content.split("\r\n\r\n")
		.map(parse_machine)
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
		shared::read_test_input(DAY);
    }

	#[test]
    fn test_parse() {
		let machines = shared::read_test_input(DAY);
		let machines = parse_input(machines);
		assert_eq!(machines.len(), 4);
		assert_eq!(machines[3].2.y, 10279);
		assert_eq!(machines[1].1.x, 67);
		assert_eq!(machines[1].1.y, 21);

		let machines = shared::read_input(DAY);
		let machines = parse_input(machines);
		assert_eq!(machines.len(), 320);
    }

	#[test]
	fn test_machines() {
		let machines = shared::read_test_input(DAY);
		let machines = parse_input(machines);
		assert_eq!(find_prize(&machines[0]), Some(280));
		assert_eq!(find_prize(&machines[1]), None);
		assert_eq!(find_prize(&machines[2]), Some(200));
		assert_eq!(find_prize(&machines[3]), None);
	}

	#[test]
	fn test_sum() {
		let machines = shared::read_test_input(DAY);
		let machines = parse_input(machines);
		assert_eq!(sum_prizes(&machines), 480);
	}

	#[test]
	fn test_machines_b() {
		let machines = shared::read_test_input(DAY);
		let machines = parse_input(machines);
		assert!(!find_prize_b(&machines[0]).is_some());
		assert!( find_prize_b(&machines[1]).is_some());
		assert!(!find_prize_b(&machines[2]).is_some());
		assert!( find_prize_b(&machines[3]).is_some());
	}
}
