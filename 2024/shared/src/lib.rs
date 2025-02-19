
use std::fs;
use std::env;
use std::ops::{Add, Mul, Sub, Div, Rem};

pub fn read_input(day: i32) -> String {
	let filename = format!("../inputs/{}.txt", day);
	fs::read_to_string(&filename)
		.expect("Cannot read file.")
}

pub fn read_test_input(day: i32) -> String {
	let filename = format!("../inputs/test_inputs/{}.txt", day);
	fs::read_to_string(&filename)
		.expect("Cannot read file.")
}

pub fn read_test_input_n(day: i32, n: i32) -> String {
	let filename = format!("../inputs/test_inputs/{}_{}.txt", day, n);
	fs::read_to_string(&filename)
		.expect("Cannot read file.")
}

pub fn read_challenge_input(day: i32, n: i32) -> String {
	let filename = format!("../inputs/challenge/{}_{}.txt", day, n);
	fs::read_to_string(&filename)
		.expect("Cannot read file.")
}

pub fn is_part_a () -> bool {
    let args: Vec<String> = env::args().collect();
	args.len() < 2 || args[1] == "a"
}

pub fn is_part_b () -> bool {
    let args: Vec<String> = env::args().collect();
	args.len() == 2 && args[1] == "b"
}

pub fn is_challenge_a () -> bool {
    let args: Vec<String> = env::args().collect();
	args.len() == 2 && args[1] == "ca"
}

pub fn is_challenge_b () -> bool {
    let args: Vec<String> = env::args().collect();
	args.len() == 2 && args[1] == "cb"
}

//==============================================================================
// vv Benchmarking

pub fn bench(func : fn()) {
	use std::time::Instant;
	let now = Instant::now();

	func();

    let elapsed = now.elapsed();
    println!("Elapsed: {:.2?}", elapsed);
}

//==============================================================================
// vv Point

#[derive(Debug, Copy, Clone, PartialEq, Eq, Hash)]
pub struct Point {
	pub x: i32,
	pub y:  i32
}

impl Point {
	pub fn from_usize(x:usize, y: usize) -> Point {
		Point{x : x as i32, y: y as i32}
	}

	pub fn zero() -> Point {
		Point {
			x : 0,
			y : 0,
		}
	}
}

impl Add for Point {
	type Output = Self;
	fn add(self, other : Self) -> Self {
		Point {
			x: self.x + other.x,
			y: self.y + other.y,
		}
	}
}

impl Sub for Point {
	type Output = Self;
	fn sub(self, other : Self) -> Self {
		Point {
			x: self.x - other.x,
			y: self.y - other.y,
		}
	}
}

impl Mul<i32> for Point {
	type Output = Self;
	fn mul(self, other : i32) -> Self {
		Point {
			x: self.x * (other as i32),
			y: self.y * (other as i32),
		}
	}
}

impl Div for Point {
	type Output = Self;
	fn div(self, other : Self) -> Self {
		Point {
			x: self.x / other.x,
			y: self.y / other.y,
		}
	}
}

impl Rem for Point {
	type Output = Self;
	fn rem(self, other : Self) -> Self {
		Point {
			x: self.x % other.x,
			y: self.y % other.y,
		}
	}
}

pub fn directions() -> [Point; 8] {
	[
		// Axis aligned
		Point { x: -1, y:  0 },
		Point { x:  1, y:  0 },
		Point { x:  0, y:  1 },
		Point { x:  0, y: -1 },
		// Diagonalen
		Point { x: -1, y:  1 },
		Point { x:  1, y: -1 },
		Point { x:  1, y:  1 },
		Point { x: -1, y: -1 },
	]
}

pub fn diagonals() -> [Point; 4] {
	[
		Point { x: -1, y:  1 },
		Point { x:  1, y: -1 },
		Point { x:  1, y:  1 },
		Point { x: -1, y: -1 },
	]
}

// let op: clockwise
pub fn hori_and_vert() -> [Point; 4] {
	[
		Point { x:  0, y: -1 },
		Point { x:  1, y:  0 },
		Point { x:  0, y:  1 },
		Point { x: -1, y:  0 },
	]
}

// exclusive upperbound
pub fn check_bound(point: Point, min_x : i32, min_y : i32, max_x : i32, max_y : i32) -> bool {
	   point.x >= min_x
	&& point.x <  max_x
	&& point.y >= min_y
	&& point.y <  max_y
}


//==============================================================================
// vv Point

#[derive(Debug)]
#[derive(Copy)]
#[derive(Clone)]
#[derive(PartialEq)]
pub struct Point64 {
	pub x: i64,
	pub y:  i64
}

impl Point64 {
	pub fn from_usize(x:usize, y: usize) -> Point64 {
		Point64{x : x as i64, y: y as i64}
	}

	pub fn zero() -> Point64 {
		Point64 {
			x : 0,
			y : 0,
		}
	}
}

impl Add for Point64 {
	type Output = Self;
	fn add(self, other : Self) -> Self {
		Point64 {
			x: self.x + other.x,
			y: self.y + other.y,
		}
	}
}

impl Sub for Point64 {
	type Output = Self;
	fn sub(self, other : Self) -> Self {
		Point64 {
			x: self.x - other.x,
			y: self.y - other.y,
		}
	}
}

impl Mul<i64> for Point64 {
	type Output = Self;
	fn mul(self, other : i64) -> Self {
		Point64 {
			x: self.x * (other as i64),
			y: self.y * (other as i64),
		}
	}
}

impl Div for Point64 {
	type Output = Self;
	fn div(self, other : Self) -> Self {
		Point64 {
			x: self.x / other.x,
			y: self.y / other.y,
		}
	}
}

impl Rem for Point64 {
	type Output = Self;
	fn rem(self, other : Self) -> Self {
		Point64 {
			x: self.x % other.x,
			y: self.y % other.y,
		}
	}
}

//==============================================================================
// vv number stuff

pub fn gcd(mut n : i64, mut m : i64) -> i64 {
	while m != 0 {
		if m < n {
			std::mem::swap(&mut m, &mut n);
		}
		m %= n;
	}
	n
}

pub fn lcm(a : i64, b : i64) -> i64 {
    return a*(b/gcd(a,b));
}

//==============================================================================
// vv grid stuff

pub fn find_char(grid : &Vec<Vec<char>>, chr : char) -> Point {
	for y in 0..grid.len() {
		for x in 0..grid[0].len() {
			if grid[y][x] == chr {
				return Point::from_usize(x,y);
			}
		}
	}
	panic!("No '{chr}' found!");
}

pub fn new_seen(grid : &Vec<Vec<char>>) -> Vec<Vec<bool>> {
	grid.iter()
		.map(|l| l.iter().map(|_| false).collect())
		.collect()
}

pub fn set_seen(seen : &mut Vec<Vec<bool>>, p : Point) {
	seen[p.y as usize][p.x as usize] = true;
}

pub fn get_seen(seen : &Vec<Vec<bool>>, p : &Point) -> bool {
	seen[p.y as usize][p.x as usize]
}

pub fn set_seen_dir(seen : &mut Vec<Vec<[bool;4]>>, p : Point, dir : usize) {
	seen[p.y as usize][p.x as usize][dir] = true;
}

pub fn get_seen_dir(seen : &Vec<Vec<[bool;4]>>, p : Point, dir : usize) -> bool {
	seen[p.y as usize][p.x as usize][dir]
}

pub fn new_seen_dir(grid : &Vec<Vec<char>>) -> Vec<Vec<[bool;4]>> {
	grid.iter()
		.map(|l| l.iter().map(|_| [false,false,false,false]).collect())
		.collect()
}

//==============================================================================
// vv print

pub fn print_char_grid(grid : &Vec<Vec<char>>) {
	for y in 0..grid.len() {
		println!("{}", &grid[y].iter().collect::<String>());
	}
}

//==============================================================================
// vv tests

#[cfg(test)]
mod tests {
    use super::*;

	#[test]
	fn test_bound () {
		assert!( check_bound(Point{x:  1, y:  9}, 0, 0, 10, 10));
		assert!(!check_bound(Point{x:  1, y: 11}, 0, 0, 10, 10));
		assert!(!check_bound(Point{x:  1, y: -1}, 0, 0, 10, 10));

		assert!( check_bound(Point{x:  9, y:  0}, 0, 0, 10, 10));
		assert!(!check_bound(Point{x: 11, y:  0}, 0, 0, 10, 10));
		assert!(!check_bound(Point{x: -1, y:  0}, 0, 0, 10, 10));
	}
}
