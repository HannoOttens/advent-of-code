extern crate shared;
use shared::{check_bound, Point};

const DAY : i32 = 10;
type Grid = Vec<Vec<u8>>;
const MAP_SIZE : usize = 57;
type TrailMap = [[(u128,u128);MAP_SIZE];MAP_SIZE];
type TrailMapB = [[u64;MAP_SIZE];MAP_SIZE];

const DIRS : [Point; 4]	= [
	Point { x:  0, y: -1 },
	Point { x:  1, y:  0 },
	Point { x:  0, y:  1 },
	Point { x: -1, y:  0 },
];

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let grid = parse_input(content);
	if shared::is_part_a() {
		let trailhead_map = find_trailheads(&grid);
		println!("{:?}", sum_trailheads(trailhead_map, grid));
	} else {
		let trailhead_map = find_trailheads_b(&grid);
		println!("{:?}", sum_trailheads_b(trailhead_map, grid));
	}
}

// =============================================================================
// vv part a

fn sum_trailheads(trail_map : TrailMap, grid : Grid) -> u32 {
	let mut totl = 0;
	for (y, row) in grid.iter().enumerate() {
		for (x, elem) in row.iter().enumerate() {
			if *elem == 0 {
				totl += trail_map[y][x].0.count_ones()
					  + trail_map[y][x].1.count_ones()
			}
		}
	}
	totl
}

fn init(trail_map : &mut TrailMap, grid : &Grid) {
	let mut nine_index = 0;
	for (y, row) in grid.iter().enumerate() {
		for (x, elem) in row.iter().enumerate() {
			if *elem == 9 {
				if nine_index < 128 {
					trail_map[y][x].0 = 1 << nine_index;
				} else {
					trail_map[y][x].1 = 1 << (nine_index-128);
				}
				nine_index += 1;
			}
		}
	}
}

fn handle_pos(trail_map : &mut TrailMap, grid : &Grid, height : u8, pos : Point) {
	for diff in DIRS {
		let next_to = pos + diff;
		if !check_bound(next_to, 0, 0, grid[0].len() as i32, grid.len() as i32) {
			continue;
		}

		let x = next_to.x as usize;
		let y = next_to.y as usize;
		if grid[y][x] == height + 1 {
			trail_map[pos.y as usize][pos.x as usize].0 |= trail_map[y][x].0;
			trail_map[pos.y as usize][pos.x as usize].1 |= trail_map[y][x].1;
		}
	}
}

fn iterate(trail_map : &mut TrailMap, grid : &Grid, height : u8) {
	for (y, row) in grid.iter().enumerate() {
		for (x, elem) in row.iter().enumerate() {
			if *elem == height {
				let p = Point::from_usize(x,y);
				handle_pos(trail_map, grid, height, p);
			}
		}
	}
}

fn find_trailheads(grid : &Grid) -> TrailMap {
	let mut trail_map = [[(0u128,0u128);MAP_SIZE];MAP_SIZE];
	init(&mut trail_map, &grid);
	for height in (0..9).rev() {
		iterate(&mut trail_map, &grid, height)
	}
	trail_map
}

// =============================================================================
// vv part b

fn sum_trailheads_b(trail_map : TrailMapB, grid : Grid) -> u64 {
	let mut totl = 0;
	for (y, row) in grid.iter().enumerate() {
		for (x, elem) in row.iter().enumerate() {
			if *elem == 0 {
				totl += trail_map[y][x]
			}
		}
	}
	totl
}

fn init_b(trail_map : &mut TrailMapB, grid : &Grid) {
	for (y, row) in grid.iter().enumerate() {
		for (x, elem) in row.iter().enumerate() {
			if *elem == 9 {
				trail_map[y][x] = 1;
			}
		}
	}
}

fn handle_pos_b(trail_map : &mut TrailMapB, grid : &Grid, height : u8, pos : Point) {
	for diff in DIRS {
		let next_to = pos + diff;
		if !check_bound(next_to, 0, 0, grid[0].len() as i32, grid.len() as i32) {
			continue;
		}

		let x = next_to.x as usize;
		let y = next_to.y as usize;
		if grid[y][x] == height + 1 {
			trail_map[pos.y as usize][pos.x as usize] += trail_map[y][x];
		}
	}
}

fn iterate_b(trail_map : &mut TrailMapB, grid : &Grid, height : u8) {
	for (y, row) in grid.iter().enumerate() {
		for (x, elem) in row.iter().enumerate() {
			if *elem == height {
				let p = Point::from_usize(x,y);
				handle_pos_b(trail_map, grid, height, p);
			}
		}
	}
}

fn find_trailheads_b(grid : &Grid) -> TrailMapB {
	let mut trail_map = [[0;MAP_SIZE];MAP_SIZE];
	init_b(&mut trail_map, &grid);
	for height in (0..9).rev() {
		iterate_b(&mut trail_map, &grid, height)
	}
	trail_map
}

// =============================================================================
// vv parse

fn parse_input(content : String) -> Grid {
	content.lines()
		.map(|line| line.chars()
			.map(|c| c.to_digit(10).unwrap() as u8)
			.collect())
		.collect()
}


// =============================================================================
// vv printing

fn print(trail_map : &TrailMap) {
	println!();
	for line in trail_map {
		println!("{line:?}");
	}
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
    fn test_a_0() {
		let content = shared::read_test_input_n(DAY, 0);
		let grid = parse_input(content);
		let trailhead_map = find_trailheads(&grid);
		assert_eq!(1, sum_trailheads(trailhead_map, grid));
    }

    #[test]
    fn test_a_1() {
		let content = shared::read_test_input_n(DAY, 1);
		let grid = parse_input(content);
		let trailhead_map = find_trailheads(&grid);
		assert_eq!(36, sum_trailheads(trailhead_map, grid));
    }


    #[test]
    fn test_b_0() {
		let content = shared::read_test_input_n(DAY, 0);
		let grid = parse_input(content);
		let trailhead_map = find_trailheads_b(&grid);
		assert_eq!(16, sum_trailheads_b(trailhead_map, grid));
    }

    #[test]
    fn test_b_1() {
		let content = shared::read_test_input_n(DAY, 1);
		let grid = parse_input(content);
		let trailhead_map = find_trailheads_b(&grid);
		assert_eq!(81, sum_trailheads_b(trailhead_map, grid));
    }
}