use std::sync::{Arc, Mutex};
use std::thread;

extern crate shared;
use shared::*;

const DAY : i32 = 6;

fn main() {
	shared::bench (run);
}

fn run () {
	let (p,  mut grid) = parse_input(shared::read_input(DAY));
	if is_part_a() {
		println!("{}", count_walked(p, &grid));
	} else {
		println!("{}", find_all_loop(p, &mut grid));
	}
}

// =============================================================================
// vv part a

fn get_pos(grid : &Vec<Vec<char>>, p : Point) -> Option<&char> {
	grid.get(p.y as usize)?.get(p.x as usize)
}

fn set_seen(seen : &mut Vec<Vec<bool>>, p : Point) {
	seen[p.y as usize][p.x as usize] = true;
}

fn go_next (grid : &Vec<Vec<char>>, pos : Point, dir : usize) -> (usize, Option<Point>){
	let directions = shared::hori_and_vert();
	let next = pos + directions[dir];
	let chr = get_pos(grid, next);
	match chr {
		None      => (dir, None),
		Some('#') => ((dir + 1) % directions.len(), Some(pos)),
		Some(_)   => (dir, Some(next)),
	}
}

fn start_walking(start : Point, grid : &Vec<Vec<char>>) -> Vec<Vec<bool>>
{
	let mut seen = grid.iter()
		.map(|l| l.iter().map(|_| false).collect())
		.collect();

	let mut next_pos = Some(start);
	let mut dir = 0;
	while let Some(pos) = next_pos {
		set_seen(&mut seen, pos);
		(dir, next_pos) = go_next(grid, pos, dir)
	}
	seen
}

fn count_walked(start : Point, grid : &Vec<Vec<char>>) -> usize {
	let seen = start_walking(start, grid);
	seen.iter()
		.map(|l| l.iter().filter(|b| **b).count())
		.sum()
}

// =============================================================================
// vv part b

fn set_seen_dir(seen : &mut Vec<Vec<Vec<bool>>>, p : Point, dir : usize) {
	seen[p.y as usize][p.x as usize][dir] = true;
}

fn has_seen_dir(seen : &Vec<Vec<Vec<bool>>>, p : Point, dir : usize) -> bool {
	seen[p.y as usize][p.x as usize][dir]
}

fn go_next_blockade(grid : &Vec<Vec<char>>, blockade : Point, pos : Point, dir : usize) -> (usize, Option<Point>){
	let directions = shared::hori_and_vert();
	let next = pos + directions[dir];

	if next == blockade {
		((dir + 1) % directions.len(), Some(pos))
	} else {
		let chr = get_pos(grid, next);
		match chr {
			None      => (dir, None),
			Some('#') => ((dir + 1) % directions.len(), Some(pos)),
			Some(_)   => (dir, Some(next)),
		}
	}
}

fn check_loop(start : Point, grid : &Vec<Vec<char>>, blockade : Point) -> bool
{
	let mut seen = grid.iter()
		.map(|l| l.iter().map(|_| [false,false,false,false].to_vec()).collect())
		.collect();

	let mut next_pos = Some(start);
	let mut dir = 0;
	while let Some(pos) = next_pos {
		if has_seen_dir(&seen, pos, dir) {
			return true
		}
		set_seen_dir(&mut seen, pos, dir);
		(dir, next_pos) = go_next_blockade(grid, blockade, pos, dir)
	}
	false
}

fn find_all_loop(start : Point,
	grid : &Vec<Vec<char>>) -> usize
{
	// Alleen blockade plaatsen waar we standaard lopen
	let seen = start_walking(start, grid);
	let seen_ref = &seen;

	// Multithreaded zoeken
    let counter = Arc::new(Mutex::new(0));
	thread::scope(|s| {
		for y in 0..grid.len() {
			let counter = Arc::clone(&counter);
			let y_ref = y;

			s.spawn(move || {
				for x in 0..grid[0].len() {
					if seen_ref[y][x] {
						let blockade = Point::from_usize(x, y_ref);
						match get_pos(grid, blockade) {
							Some('#') => {},
							Some('^') => {},
							Some(_) => {
								if check_loop(start, grid, blockade) {
									let mut n = counter.lock().unwrap();
									*n += 1;
								}
							},
							_ => panic!("AAAAAAAAAAAAAAAAAAAA!")
						}
					}
				}
			});
		}
	});
	let x = *counter.lock().unwrap();
	x
}


// =============================================================================
// vv parse

fn parse_input(content : String) -> (Point, Vec<Vec<char>>) {
	let grid : Vec<Vec<char>> = content.lines()
		.map(|line| line.chars().collect())
		.collect();

	for y in 0..grid.len() {
		for x in 0..grid[0].len() {
			if grid[y][x] == '^' {
				return (Point::from_usize(x,y), grid);
			}
		}
	}
	(Point{x:0,y:0}, grid)
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
		let (_,  grid) = parse_input(shared::read_input(DAY));
		assert_eq!(130, grid.len());
	}

	#[test]
	fn test_walk() {
		let (p,  grid) = parse_input(shared::read_test_input(DAY));
		assert_eq!(41, count_walked(p, &grid));
	}

	#[test]
	fn test_loops() {
		let (p,  mut grid) = parse_input(shared::read_test_input(DAY));
		assert_eq!(6, find_all_loop(p, &mut grid));
	}
}