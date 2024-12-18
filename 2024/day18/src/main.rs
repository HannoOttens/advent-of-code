use std::{cmp::Ordering, collections::{BinaryHeap, HashMap, HashSet}};

extern crate shared;
use shared::Point;

type Obstacles = HashMap<Point, usize>;
const DAY : i32 = 18;


fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let bytes = parse_input(content);

	if shared::is_part_a() {
		println!("{:?}", walk(&bytes, 1024, Point { x: 0, y: 0}, Point { x: 70, y: 70}));
	} else if shared::is_part_b() {
		println!("{:?}", find_first_blocking(&bytes));
	}
}

// =============================================================================
// vv part a
// =============================================================================

struct State {
    cost: usize,
    state: Point,
}
impl Eq for State {}
impl PartialEq for State {
	fn eq(&self, other: &Self) -> bool {
		self.cost == other.cost
	}
}
impl PartialOrd for State {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}
impl Ord for State {
    fn cmp(&self, other: &Self) -> Ordering {
        other.cost.cmp(&self.cost)
    }
}

// =============================================================================

const DIRS : [Point; 4]	= [
	Point { x:  0, y: -1 },
	Point { x:  1, y:  0 },
	Point { x:  0, y:  1 },
	Point { x: -1, y:  0 },
];

fn walk(obstacles : &Obstacles, time : usize, start : Point, goal : Point) -> Option<usize> {
	let mut seen = HashSet::new();
    let mut heap = BinaryHeap::new();
	heap.push(State { cost: 0, state: start });

	while let Some(State { cost, state: pos }) = heap.pop() {
		if seen.contains(&pos) { continue; }
		seen.insert(pos);

		if pos == goal {
			return Some(cost);
		}

		for dir in DIRS {
			let nxtp = pos + dir;
			if !(obstacles.contains_key(&nxtp) && obstacles[&nxtp] < time)
				&& shared::check_bound(nxtp, 0, 0, goal.x+1, goal.y+1)
			{
				heap.push(State { cost: cost + 1, state: nxtp });
			}
		}
	}

	return None;
}

// =============================================================================
// vv part b
// =============================================================================

fn find_key_for_value(map: &Obstacles, time : usize) -> Option<Point> {
    map.iter()
        .find_map(|(key, &val)| if val == time { Some(*key) } else { None })
}

fn find_first_blocking(bytes: &Obstacles) -> Point {
	let mut min = 0;
	let mut max = bytes.len();
	while min < max {
		let mid = (min + max) / 2;
		let dist = walk(&bytes, mid, Point { x: 0, y: 0}, Point { x: 70, y: 70});

		if dist.is_none() {
			max = mid - 1;
		} else {
			min = mid + 1;
		}
	}
	find_key_for_value(&bytes, min-1).unwrap()
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_button(button : &str) -> Point {
	let (x, y) = button.split_once(',').unwrap();
	Point {
		x : x.parse().unwrap(),
		y : y.parse().unwrap(),
	}
}

fn parse_input(content : String) -> Obstacles {
	content.lines()
		.map(parse_button)
		.enumerate()
		.map(|(i,v)| (v,i))
		.collect()
}

// =============================================================================
// vv tests vv
// =============================================================================

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn inputs_exists() {
		shared::read_input(DAY);
		shared::read_test_input_n(DAY, 0);
    }

	#[test]
    fn test_parse() {
		let input = shared::read_input(DAY);
		parse_input(input).len();
    }

	#[test]
	fn test_part_a() {
		let input = shared::read_test_input_n(DAY, 0);
		let bytes = parse_input(input);
		assert_eq!(22, walk(&bytes, 12, Point { x: 0, y: 0}, Point { x: 6, y: 6}).unwrap());
	}

	#[test]
	fn test_part_b() {
		let input = shared::read_test_input_n(DAY, 0);
		let bytes = parse_input(input);
		assert_eq!(Point { x: 6, y : 1 }, find_first_blocking(&bytes));
	}
}
