
use std::fs;
use std::env;
use std::ops::Add;
use std::ops::Mul;

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

pub fn is_part_a () -> bool {
    let args: Vec<String> = env::args().collect();
	args.len() == 1
}

//==============================================================================
// vv Point

#[derive(Debug)]
#[derive(Copy)]
#[derive(Clone)]
pub struct Point {
	pub x: i32,
	pub y:  i32
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

impl Mul<i32> for Point {
	type Output = Self;
	fn mul(self, other : i32) -> Self {
		Point {
			x: self.x * (other as i32),
			y: self.y * (other as i32),
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

// exclusive upperbound
pub fn check_bound(point: Point, min_x : i32, min_y : i32, max_x : i32, max_y : i32) -> bool {
	   point.x >= min_x
	&& point.x <  max_x
	&& point.y >= min_y
	&& point.y <  max_y
}

//==============================================================================
// vv tests

#[cfg(test)]
mod tests {
    use super::*;

	#[test]
	fn test_bound () {
		assert!( check_bound(Point{x:  1, y: 10}, 0, 0, 10, 10));
		assert!(!check_bound(Point{x:  1, y: 11}, 0, 0, 10, 10));
		assert!(!check_bound(Point{x:  1, y: -1}, 0, 0, 10, 10));

		assert!( check_bound(Point{x: 10, y:  0}, 0, 0, 10, 10));
		assert!(!check_bound(Point{x: 11, y:  0}, 0, 0, 10, 10));
		assert!(!check_bound(Point{x: -1, y:  0}, 0, 0, 10, 10));
	}
}
