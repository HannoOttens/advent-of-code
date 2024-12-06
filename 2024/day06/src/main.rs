extern crate shared;
use shared::*;

const DAY : i32 = 6;

fn main() {
	shared::bench (run);
}

fn run () {
	let (p,  mut grid) = parse_input(shared::read_input(DAY));
	if is_part_a() {
		println!("{}", start_walking(p, &grid));
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

fn start_walking(start : Point, grid : &Vec<Vec<char>>) -> usize
{
	let mut seen = grid.iter()
		.map(|l| l.iter().map(|_| false).collect())
		.collect();
	let directions = shared::hori_and_vert();

	let mut next_pos = Some(start);
	let mut dir = 0;
	while let Some(pos) = next_pos {
		set_seen(&mut seen, pos);
		let next = pos + directions[dir];
		let chr = get_pos(grid, next);
		match chr {
			None      => next_pos = None,
			Some('#') => dir = (dir + 1) % directions.len(),
			Some(_)   => next_pos = Some(next),
		}
	}
	seen.iter()
		.map(|l| l.iter().filter(|b| **b).count())
		.sum()
}

// =============================================================================
// vv part b

fn set_pos(grid : &mut Vec<Vec<char>>, p : Point, chr : char){
	grid[p.y as usize][p.x as usize] = chr;
}

fn set_seen_dir(seen : &mut Vec<Vec<Vec<bool>>>, p : Point, dir : usize) {
	seen[p.y as usize][p.x as usize][dir] = true;
}

fn has_seen_dir(seen : &Vec<Vec<Vec<bool>>>, p : Point, dir : usize) -> bool {
	seen[p.y as usize][p.x as usize][dir]
}

fn find_loop(start : Point, grid : &Vec<Vec<char>>) -> bool
{
	let mut seen = grid.iter()
		.map(|l| l.iter().map(|_| [false,false,false,false].to_vec()).collect())
		.collect();
	let directions = shared::hori_and_vert();

	let mut next_pos = Some(start);
	let mut dir = 0;
	while let Some(pos) = next_pos {
		if has_seen_dir(&seen, pos, dir) {
			return true
		}
		set_seen_dir(&mut seen, pos, dir);

		let next = pos + directions[dir];
		let chr = get_pos(grid, next);
		match chr {
			None      => next_pos = None,
			Some('#') => dir = (dir + 1) % directions.len(),
			Some(_)   => next_pos = Some(next),
		}
	}
	false
}

fn find_all_loop(start : Point,
	grid : &mut Vec<Vec<char>>) -> usize
{
	let mut totl = 0;
	for y in 0..grid.len() {
		for x in 0..grid[0].len() {
			let p = Point::from_usize(x, y);
			match get_pos(grid, p) {
				Some('#') => {},
				Some('^') => {},
				Some(_) => {
					set_pos(grid, p, '#');
					if find_loop(start, grid) {
						totl += 1;
					}
					set_pos(grid, p, '.');
				},
				_ => panic!("AAAAAAAAAAAAAAAAAAAA!")
			}
		}
	}
	totl
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
		assert_eq!(41, start_walking(p, &grid));
	}

	#[test]
	fn test_loops() {
		let (p,  mut grid) = parse_input(shared::read_test_input(DAY));
		assert_eq!(6, find_all_loop(p, &mut grid));
	}
}