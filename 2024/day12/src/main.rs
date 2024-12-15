extern crate shared;
use std::collections::VecDeque;

use shared::{check_bound, Point};

const DAY : i32 = 12;
type Grid = Vec<Vec<char>>;
type Seen = Vec<Vec<bool>>;

const DIRS : [Point;4] = [
	Point { x:  0, y: -1 },
	Point { x:  1, y:  0 },
	Point { x:  0, y:  1 },
	Point { x: -1, y:  0 },
];

const DIAGS : [Point;4] = [
	Point { x: -1, y:  1 },
	Point { x:  1, y: -1 },
	Point { x:  1, y:  1 },
	Point { x: -1, y: -1 },
];

fn main() {
	shared::bench (run);
}

fn run () {
	if shared::is_part_a() {
		let content = shared::read_input(DAY);
		let grid = parse_input(content);
		println!("{:?}", find_farm_perim_costs(&grid));
	} else if shared::is_part_b() {
		let content = shared::read_input(DAY);
		let grid = parse_input(content);
		println!("{:?}", find_farm_sides_costs(&grid));
	} else if shared::is_challenge_a() {
		let content = shared::read_challenge_input(DAY, 0);
		let grid = parse_input(content);
		println!("Challenge A");
		println!("Input size: {:?}", grid.len());
		println!("Perimiter: {:?}", find_farm_perim_costs(&grid));
		println!("Sides: {:?}", find_farm_sides_costs(&grid));
	}
	else if shared::is_challenge_b() {
		let content = shared::read_challenge_input(DAY, 1);
		let grid = parse_input(content);
		println!("Challenge B");
		println!("Input size: {:?}", grid.len());
		println!("Perimiter: {:?}", find_farm_perim_costs(&grid));
		println!("Sides: {:?}", find_farm_sides_costs(&grid));
	}
}

// =============================================================================
// vv part a

fn new_seen(grid : &Grid) -> Seen {
	grid.iter()
		.map(|l| l.iter().map(|_| false).collect())
		.collect()
}

fn get_seen(seen : &Seen, p : &Point) -> bool {
	seen[p.y as usize][p.x as usize]
}

fn set_seen(seen : &mut Seen, p : &Point) {
	seen[p.y as usize][p.x as usize] = true;
}

fn find_neigh(grid : &Grid, p : Point) -> Vec<Point> {
	let mut neigh = vec!();
	for dp in DIRS {
		let nxtp = p + dp;
		if check_bound(nxtp, 0, 0, grid[0].len() as i32, grid.len() as i32)
			&& (grid[nxtp.y as usize][nxtp.x as usize] == grid[p.y as usize][p.x as usize])
		{
			neigh.push(nxtp);
		}
	}
	neigh
}

fn farm_cost(seen : &mut Seen, grid : &Grid, x : usize, y : usize) -> usize {
	let mut queue = VecDeque::new();
	queue.push_back(Point::from_usize(x, y));

	let mut perim = 0;
	let mut area = 0;
	while let Some(p) = queue.pop_front() {
		if get_seen(seen, &p) { continue; }
		set_seen(seen, &p);

		let neigh = find_neigh(grid, p);
		area += 1;
		perim += 4 - neigh.len();

		for p in neigh {
			queue.push_back(p);
		}
	}
	perim * area
}

fn find_farm_perim_costs(grid : &Grid) -> usize {
	let mut seen = new_seen(&grid);
	let mut totl = 0;
	for (y, line) in grid.iter().enumerate() {
		for (x, _) in line.iter().enumerate() {
			if !get_seen(&seen, &Point::from_usize(x,y)) {
				totl += farm_cost(&mut seen, &grid, x, y);
			}
		}
	}
	totl
}

// =============================================================================
// vv part b

// assumes p1 in bounds. p2 is different if out of bounds.
fn grid_equ(grid : &Grid, p1 : Point, p2 : Point) -> bool {
	check_bound(p2, 0, 0, grid[0].len() as i32, grid.len() as i32)
		&& grid[p1.y as usize][p1.x as usize] == grid[p2.y as usize][p2.x as usize]
}

fn inside_corner(grid : &Grid, p : Point, i : usize) -> bool {
	// inside corner
	// ?A. | .A? | ??? | ???
	// ?AA | AA? | AA? | ?AA
	// ??? | ??? | .A? | ?A.
	let dp = DIAGS[i];
	let diff = p + dp;
	let same_a = p + Point { x: dp.x, y: 0    };
	let same_b = p + Point { x:    0, y: dp.y };

	!grid_equ(grid, p, diff)
		&& grid_equ(grid, p, same_a)
		&& grid_equ(grid, p, same_b)
}

fn outside_corner(grid : &Grid, p : Point, i : usize) -> bool {
	// outside corner
	// ?.? | ??? | ?.? | ???
	// .A? | .A? | ?A. | ?A.
	// ??? | ?.? | ??? | ?.?
	let diff_a = p + DIRS[(i + 2) % 4];
	let diff_b = p + DIRS[(i + 3) % 4];

	!grid_equ(grid, p, diff_a)
		&& !grid_equ(grid, p, diff_b)
}

fn find_corners(grid : &Grid, p : Point) -> usize {
	let mut corners = 0;
	for i in 0..4 {
		if outside_corner(grid, p, i) {
			corners += 1;
		}
		if inside_corner(grid, p, i) {
			corners += 1;
		}
	}
	corners
}

fn farm_cost_sides(seen : &mut Seen, grid : &Grid, x : usize, y : usize) -> usize {
	let mut queue = VecDeque::new();
	queue.push_back(Point::from_usize(x, y));

	let mut corners = 0;
	let mut area = 0;
	while let Some(p) = queue.pop_front() {
		if get_seen(seen, &p) { continue; }
		set_seen(seen, &p);

		let neigh = find_neigh(grid, p);
		area += 1;
		corners += find_corners(grid, p);

		for p in neigh {
			queue.push_back(p);
		}
	}
	corners * area
}

fn find_farm_sides_costs(grid : &Grid) -> usize {
	let mut seen = new_seen(&grid);
	let mut totl = 0;
	for (y, line) in grid.iter().enumerate() {
		for (x, _) in line.iter().enumerate() {
			if !get_seen(&seen, &Point::from_usize(x,y)) {
				totl += farm_cost_sides(&mut seen, grid, x, y);
			}
		}
	}
	totl
}

// =============================================================================
// vv parse

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
	fn test_input_a_0() {
		let input = shared::read_test_input_n(DAY, 0);
		let grid = parse_input(input);
		let totl = find_farm_perim_costs(&grid);
		assert_eq!(totl, 140);
	}

	#[test]
	fn test_input_a_1() {
		let input = shared::read_test_input_n(DAY, 1);
		let grid = parse_input(input);
		let totl = find_farm_perim_costs(&grid);
		assert_eq!(totl, 772);
	}

	#[test]
	fn test_input_a_2() {
		let input = shared::read_test_input_n(DAY, 2);
		let grid = parse_input(input);
		let totl = find_farm_perim_costs(&grid);
		assert_eq!(totl, 1930);
	}

		#[test]
	fn test_input_b_0() {
		let input = shared::read_test_input_n(DAY, 0);
		let grid = parse_input(input);
		let totl = find_farm_sides_costs(&grid);
		assert_eq!(totl, 80);
	}

	#[test]
	fn test_input_b_1() {
		let input = shared::read_test_input_n(DAY, 1);
		let grid = parse_input(input);
		let totl = find_farm_sides_costs(&grid);
		assert_eq!(totl, 436);
	}

	#[test]
	fn test_input_b_2() {
		let input = shared::read_test_input_n(DAY, 2);
		let grid = parse_input(input);
		let totl = find_farm_sides_costs(&grid);
		assert_eq!(totl, 1206);
	}
}
