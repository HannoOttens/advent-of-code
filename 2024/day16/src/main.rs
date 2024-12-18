use std::{cmp::Ordering, collections::{BinaryHeap, HashMap, HashSet}};

extern crate shared;
use shared::Point;

const DAY : i32 = 16;

const LEFT  : Point = Point { x: -1, y:  0 };
const RIGHT : Point = Point { x:  1, y:  0 };
const UP    : Point = Point { x:  0, y: -1 };
const DOWN  : Point = Point { x:  0, y:  1 };
const DIRS : [Point;4] = [ UP, RIGHT, DOWN, LEFT ];

type Grid = Vec<Vec<char>>;
type History = HashMap<(Point, usize, usize), (Point, usize, usize)>;

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let grid = parse_input(content);
	let start = find_start(&grid);
	let end = find_end(&grid);

	if shared::is_part_a() {
		println!("{:?}", walk(&grid, start, end)); // too high: 87480
	} else if shared::is_part_b() {
		println!("{:?}", walk(&grid, start, end));
	}
}

// =============================================================================
// vv part a
// =============================================================================

struct State {
    cost: usize,
    state: (Point, usize),
	prev: Option<(Point, usize, usize)>
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
}

fn find_start(grid : &Grid) -> Point {
	shared::find_char(grid, 'S')
}

fn find_end(grid : &Grid) -> Point {
	shared::find_char(grid, 'E')
}

// =============================================================================

fn turn_left(dir : usize) -> usize {
	(dir as i32 - 1).rem_euclid(4) as usize
}

fn turn_right(dir : usize) -> usize {
	(dir + 1) % 4
}

// =============================================================================

fn walk(grid : &Grid, start : Point, goal : Point) -> Option<(usize, usize)> {
	let mut seen = shared::new_seen_dir(grid);
    let mut heap = BinaryHeap::new();
	let mut hist = HashMap::new();
	heap.push(State { cost: 0, state: (start, 1), prev: None });

	while let Some(State { cost, state: (pos, dir), prev }) = heap.pop() {
		if shared::get_seen_dir(&seen, pos, dir) { continue; }
		shared::set_seen_dir(&mut seen, pos, dir);

		if let Some(prev) = prev {
			hist.insert((pos,dir,cost), prev);
		}

		if pos == goal {
			return Some((cost, find_visited(hist, start, goal, dir, cost)));
		}

		let prev = Some((pos, dir, cost));
		heap.push(State { prev, cost: cost+1000, state: (pos, turn_left(dir))});
		heap.push(State { prev, cost: cost+1000, state: (pos, turn_right(dir))});
		if is_free(grid, pos + DIRS[dir]) {
			heap.push(State { prev, cost: cost+1, state: (pos + DIRS[dir], dir) });
		}
	}
	return None;
}

fn find_visited(hist: History, start : Point, mut goal : Point, mut dir : usize, mut cost : usize) -> usize {
	let mut green_nodes = HashSet::new();
	green_nodes.insert((start, 1, 0));
	green_nodes.insert((goal, dir, cost));
	while let Some((prev, pdir, pcost)) = hist.get(&(goal, dir, cost)) {
		green_nodes.insert((*prev, *pdir, *pcost));
		dir = *pdir;
		goal = *prev;
		cost = *pcost;
	}

	let mut prev_size = 0;
	while green_nodes.len() != prev_size {
		prev_size = green_nodes.len();
		for (pos, dir, cost) in hist.keys() {
			if with_one_step(*pos, *dir, *cost, &green_nodes) {
				green_nodes.insert((*pos, *dir, *cost));
			}
		}
	}

	// tel unieke punten
	green_nodes.into_iter()
		.map(|(p,_,_)| p)
		.collect::<HashSet<_>>()
		.len()
}

fn with_one_step(pos: Point, dir: usize, cost: usize, green_nodes: &HashSet<(Point, usize, usize)>) -> bool {
	let opt_a = (pos, turn_left(dir) , cost+1000);
	let opt_b = (pos, turn_right(dir), cost+1000);
	let opt_c = (pos + DIRS[dir], dir, cost+1);

	green_nodes.contains(&opt_a)
	|| green_nodes.contains(&opt_b)
	|| green_nodes.contains(&opt_c)
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
		shared::read_test_input_n(DAY, 1);
    }

	#[test]
    fn test_parse() {
		let input = shared::read_test_input_n(DAY, 1);
		parse_input(input);
    }

	#[test]
    fn test_a_0() {
		let input = shared::read_test_input_n(DAY, 0);
		let grid = parse_input(input);
		let start = find_start(&grid);
		let end = find_end(&grid);
		assert_eq!(7036, walk(&grid, start, end).unwrap().0);
    }

	#[test]
    fn test_a_1() {
		let input = shared::read_test_input_n(DAY, 1);
		let grid = parse_input(input);
		let start = find_start(&grid);
		let end = find_end(&grid);
		assert_eq!(11048, walk(&grid, start, end).unwrap().0);
    }
}
