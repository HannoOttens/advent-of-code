extern crate shared;
use shared::*;

const DAY : i32 = 4;
const XMAS : [char;4] = ['X','M','A','S'];
const MAS  : [char;3] = ['M','A','S'];

fn main() {
	let input = shared::read_input(DAY);
	let input = parse_input(&input);
	println!("{}",check_input(is_part_a(), &input));
}

fn check_input (part_a : bool, input: &Vec<Vec<char>>) -> i32 {
	let mut totl = 0;
	for x in 0..(input[0].len() as i32) {
		for y in 0..(input.len() as i32) {
			if part_a {
				totl += count_xmas(input, Point{x,y})
			} else {
				totl += count_mas(input, Point{x,y});
			}
		}
	}
	totl
}

// =============================================================================
// vv part a

fn count_xmas(input: &Vec<Vec<char>>, p : Point) -> i32 {
	directions().into_iter()
		.map(|dir| check_xmas(input, p, dir) as i32)
		.sum()
}

fn check_xmas(input: &Vec<Vec<char>>, p : Point, dir : Point) -> bool {
	let max_y = input.len() as i32;
	let max_x = input[0].len() as i32;

	let mut ok = true;
	for step in 0..4 {
		let posn = p + (dir * step);
		ok = ok && check_bound(posn, 0, 0, max_x, max_y)
				&& (XMAS[step as usize] == input[posn.y as usize][posn.x as usize]);
	}
	ok
}

// =============================================================================
// vv part b

fn count_mas(input: &Vec<Vec<char>>, p : Point) -> i32 {
	let totmas : i32 = diagonals().into_iter()
		.map(|dir| check_mas(input, p, dir) as i32)
		.sum();
	if totmas == 2 { 1 } else { 0 }
}

fn check_mas(input: &Vec<Vec<char>>, p : Point, dir : Point) -> bool {
	let max_y = input.len() as i32;
	let max_x = input[0].len() as i32;

	let mut ok = true;
	for step in 0..3 {
		let posn = p + (dir * (step - 1));
		ok = ok && check_bound(posn, 0, 0, max_x, max_y)
				&& (MAS[step as usize] == input[posn.y as usize][posn.x as usize]);
	}
	ok
}

// =============================================================================
// vv parse

fn parse_input(content : &str) -> Vec<Vec<char>> {
	content.lines().map(|line| line.chars().collect()).collect()
}

// =============================================================================
// vv tests vv

#[cfg(test)]
mod tests {
    use super::*;

	const TEST_INPUT : &str = "\
MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

    #[test]
    fn input_exists() {
		shared::read_input(DAY);
    }

	#[test]
	fn find_xmas() {
		assert_eq!(1, count_xmas(&parse_input(TEST_INPUT), Point{x:5, y:0}));
		assert_eq!(2, count_xmas(&parse_input(TEST_INPUT), Point{x:9, y:3}));
	}

	#[test]
	fn no_xmas() {
		assert_eq!(0, count_xmas(&parse_input(TEST_INPUT), Point{x:0, y:0}));
	}

	#[test]
	fn xmas_in_one_direction() {
		assert_eq!(1, check_xmas(&parse_input(TEST_INPUT)
									, Point{x:5, y:0}
									, Point{x:1, y:0}));
	}

	#[test]
	fn xmas_diagonal() {
		assert_eq!(1, check_xmas(&parse_input(TEST_INPUT)
									, Point{x:9, y:3}
									, Point{x:-1, y:1}));
	}

	#[test]
	fn test_case_a () {
		assert_eq!(18, check_input(true, &parse_input(TEST_INPUT)));
	}

	#[test]
	fn test_case_b () {
		assert_eq!(9, check_input(false, &parse_input(TEST_INPUT)));
	}

	#[test]
	fn one_mas() {
		assert_eq!(1, count_mas(&parse_input(TEST_INPUT), Point{x:2, y:1}));
	}
}