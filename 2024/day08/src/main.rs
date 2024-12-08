use std::collections::HashMap;
extern crate shared;
use shared::Point;

const DAY : i32 = 8;
type Grid = Vec<Vec<char>>;
type SatMap = HashMap<char, Vec<Point>>;

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let input = parse_input(content);
	let satmap = find_satilites(&input);
	if shared::is_part_a() {
		println!("{}", find_antinodes(&satmap, input[0].len() as i32, input.len() as i32));
	} else {
		println!("{}", find_resonant_antinodes(&satmap, input[0].len() as i32, input.len() as i32));
	}
}

// =============================================================================
// vv part a

fn find_satilites(grid : &Grid) -> SatMap {
	let mut map = SatMap::new();
	for y in 0..grid.len() {
		for x in 0..grid[0].len() {
			let chr = grid[y][x];
			if chr == '.' {
				continue;
			}

			let list = map.entry(chr).or_insert(vec!());
			list.push(Point::from_usize(x,y));
		}
	}
	map
}

fn find_antinodes(map : &SatMap, width : i32, height : i32) -> usize {
	let mut seen = [[false; 50]; 50];

	for (_, posns) in map {
		for (indx, p1) in posns.iter().enumerate() {
			for p2 in &posns[indx+1..] {
				let diff = *p1 - *p2;
				let anti_a = *p1 + diff;
				let anti_b = *p2 - diff;

				if shared::check_bound(anti_a, 0, 0, width, height) {
					seen[anti_a.y as usize][anti_a.x as usize] = true;
				}
				if shared::check_bound(anti_b, 0, 0, width, height) {
					seen[anti_b.y as usize][anti_b.x as usize] = true;
				}
			}
		}
	}

	seen.into_iter()
		.flatten()
		.filter(|b| *b)
		.count()
}

// =============================================================================
// vv part b

fn find_antinodes(map : &SatMap, width : i32, height : i32) -> usize {
	let mut seen = [[false; 50]; 50];

	for (_, posns) in map {
		for (indx, p1) in posns.iter().enumerate() {
			seen[p1.y as usize][p1.x as usize] = true;

			for p2 in &posns[indx+1..] {
				let diff = *p1 - *p2;

				let mut anti_a = *p1 + diff;
				while shared::check_bound(anti_a, 0, 0, width, height) {
					seen[anti_a.y as usize][anti_a.x as usize] = true;
					anti_a = anti_a + diff;
				}

				let mut anti_b = *p2 - diff;
				while shared::check_bound(anti_b, 0, 0, width, height) {
					seen[anti_b.y as usize][anti_b.x as usize] = true;
					anti_b = anti_b - diff;
				}
			}
		}
	}

	seen.into_iter()
		.flatten()
		.filter(|b| *b)
		.count()
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
		shared::read_test_input(DAY);
    }

	#[test]
    fn test_parse() {
		let content = shared::read_test_input(DAY);
		let input = parse_input(content);
		let input = find_satilites(&input);
		assert!(input.contains_key(&'A'));
		assert!(input[&'A'].len() == 3);
	}

	#[test]
	fn test_find_antinodes () {
		let content = shared::read_test_input(DAY);
		let input = parse_input(content);
		let satmap = find_satilites(&input);
		assert_eq!(14, find_antinodes(&satmap, input[0].len() as i32, input.len() as i32));
	}

	#[test]
	fn test_find_resonant_antinodes () {
		let content = shared::read_test_input(DAY);
		let input = parse_input(content);
		let satmap = find_satilites(&input);
		assert_eq!(34, find_resonant_antinodes(&satmap, input[0].len() as i32, input.len() as i32));
	}
}