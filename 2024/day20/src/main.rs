
use std::{cmp::Ordering, collections::{BinaryHeap, HashMap}};

extern crate shared;
use shared::Point;

const DAY : i32 = 20;

const LEFT  : Point = Point { x: -1, y:  0 };
const RIGHT : Point = Point { x:  1, y:  0 };
const UP    : Point = Point { x:  0, y: -1 };
const DOWN  : Point = Point { x:  0, y:  1 };
const DIRS : [Point;4] = [ UP, RIGHT, DOWN, LEFT ];

type Grid = Vec<Vec<char>>;
type StepsFrom = HashMap<Point, usize>;

fn main() {
	shared::bench (run);
}

fn run() {
	let content = shared::read_input(DAY);
	let grid = parse_input(content);
	if shared::is_part_a() {
		println!("{:?}", find_cheat_routes(&grid, 100));
	} else if shared::is_part_b() {
		println!("{:?}", find_cheat_routes_n(&grid, 100, 20));
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

fn is_free(grid : &Grid, pos : Point) -> bool {
	grid[pos.y as usize][pos.x as usize] == '.'
	  || grid[pos.y as usize][pos.x as usize] == 'E'
	  || grid[pos.y as usize][pos.x as usize] == 'S'
}

fn find_start(grid : &Grid) -> Point {
	shared::find_char(grid, 'S')
}

fn find_end(grid : &Grid) -> Point {
	shared::find_char(grid, 'E')
}

fn find_cheat_routes(grid : &Grid, min_save : usize) -> usize {
	find_cheat_routes_n(grid, min_save, 2)
}

// =============================================================================
// vv part b
// =============================================================================

fn manhat(pos : Point, goal : Point) -> usize {
	((pos.x - goal.x).abs() + (pos.y - goal.y).abs()) as usize
}

fn walk_record_from_point(grid : &Grid, pos : Point) -> StepsFrom {
	let mut map = HashMap::new();
    let mut heap = BinaryHeap::new();
	heap.push(State { cost: 0, state: pos });

	while let Some(State { cost, state }) = heap.pop() {
		if map.contains_key(&state) { continue; }
		map.insert(state, cost);

		for dir in DIRS {
			let nxtp = state + dir;
			if is_free(grid, nxtp) {
				heap.push(State { cost: cost + 1, state: nxtp });
			}
		}
	}
	map
}

fn find_cheat_routes_n(grid : &Grid, min_save : usize, cheat_steps : i32) -> usize {
	let start = find_start(&grid);
	let goal = find_end(&grid);
	let steps_from_start = walk_record_from_point(grid, start);
	let steps_from_goal  = walk_record_from_point(grid, goal);
	let fastest = steps_from_start[&goal];

	let mut cheat_routes = 0;
	for (y, line) in grid.iter().enumerate() {
		for (x, _) in line.iter().enumerate().filter(|(_,c)| **c != '#') {
			for dy in -cheat_steps..cheat_steps+1 {
				for dx in -cheat_steps..cheat_steps+1 {
					if (dy == 0) && (dx == 0) { continue; }

					let start_cheat = Point::from_usize(x, y);
					let end_cheat = Point { x : x as i32 + dx, y: y as i32 + dy };
					if !shared::check_bound(end_cheat, 0, 0, grid[0].len() as i32, grid.len() as i32) {
						continue;
					}
					if !is_free(grid, end_cheat) { continue; }

					let cheated_steps = manhat(start_cheat, end_cheat);
					if cheated_steps > cheat_steps as usize { continue; }

					let start_dist = steps_from_start[&start_cheat];
					let end_dist = steps_from_goal[&end_cheat];
					let total_time = start_dist + cheated_steps + end_dist;
					if total_time <= fastest - min_save {
						cheat_routes += 1;
					}
				}
			}
		}
	}
	cheat_routes
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_input(content : String) -> Grid {
	content.lines()
		.map(|line| line.chars().collect())
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
    }

	#[test]
    fn test_parse() {
		let input = shared::read_test_input_n(DAY, 0);
		parse_input(input);
    }

	#[test]
    fn test_a_fastest_route() {
		let input = shared::read_test_input_n(DAY, 0);
		let grid = parse_input(input);
		let start = find_start(&grid);
		let end = find_end(&grid);
		assert_eq!(84, walk(&grid, start, end));
    }

	#[test]
    fn test_a() {
		let input = shared::read_test_input_n(DAY, 0);
		let grid = parse_input(input);
		assert_eq!(5+3+2+4+2+14+14, find_cheat_routes(&grid, 1));
    }

	#[test]
    fn test_b() {
		let input = shared::read_test_input_n(DAY, 0);
		let grid = parse_input(input);
		assert_eq!(32+31+29+39+25+23+20+19+12+14+12+22+4+3, find_cheat_routes_n(&grid, 50, 20));
    }
}
