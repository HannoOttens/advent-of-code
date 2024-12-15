extern crate shared;
use std::{thread, time::Duration};

use shared::Point;

const DAY : i32 = 14;
type Robot = (Point, Point);
type Robots = Vec<Robot>;

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let robots = parse_input(content);
	if shared::is_part_a() {
		let (q1,q2,q3,q4) = walk_robots(101, 103, &robots, 100);
		println!("{:?}", q1*q2*q3*q4);
	} else if shared::is_part_b() {
		println!("{:?}", print_robots(&robots));
	}
}

// =============================================================================
// vv part a

type Quadrant = u8;
fn determine_quadrant (w : i32, h : i32, x : i32, y : i32) -> Option<Quadrant> {
	let mid_w = (w - 1) / 2;
	let mid_h = (h - 1) / 2;
	if x > mid_w {
		if y > mid_h { Some(4) } else if y < mid_h { Some(2) } else { None }
	} else if x < mid_w {
		if y > mid_h { Some(3) } else if y < mid_h { Some(1) } else { None }
	} else {
		None
	}
}

fn walk_robot(w : usize, h : usize, (pos, vel) : &Robot, seconds : i32) -> Option<Quadrant> {
	let infi = *pos + *vel * seconds;
	let final_x = infi.x.rem_euclid(w as i32);
	let final_y = infi.y.rem_euclid(h as i32);
	determine_quadrant(w as i32, h as i32, final_x, final_y)
}

fn walk_robots(w : usize, h : usize, robots : &Robots, seconds : i32) -> (i32,i32,i32,i32) {
	robots.into_iter()
		.map(|r| walk_robot(w, h, r, seconds))
		.flatten()
		.fold((0,0,0,0), |(q1,q2,q3,q4), q|
			match q {
				1 => (q1+1,q2,q3,q4),
				2 => (q1,q2+1,q3,q4),
				3 => (q1,q2,q3+1,q4),
				4 => (q1,q2,q3,q4+1),
				_ => panic!("Nope")
			})
}

// =============================================================================
// vv part b

fn calc_robot(w : usize, h : usize, (pos, vel) : &Robot, seconds : i32) -> Point {
	let infi = *pos + *vel * seconds;
	let x = infi.x.rem_euclid(w as i32);
	let y = infi.y.rem_euclid(h as i32);
	Point { x, y }
}

fn print_robots(robots : &Robots) {
	const W : usize = 101;
	const H : usize = 103;
	let mut grid = [' ';W*H];

	let mut step = 7603;
		grid.fill(' ');
		print!("{}[2J", 27 as char);

		for robot in robots {
			let p = calc_robot(W, H, robot, step);
			grid[p.x as usize + (p.y as usize) * W] = 'X';
		}
		let mut str = String::new();
		for y in 0..H {
			str.push_str(&grid[y*W..(y+1)*W].iter().collect::<String>());
			str.push('\n');
		}
		str.push_str(&format!("\n{}", step));
		println!("{}", str);

		thread::sleep(Duration::from_millis(250));

		step += 1;
}

// =============================================================================
// vv parse

fn prase_pos(pos : &str) -> Point {
	let (posx, posy) = pos.split_once(",").unwrap();
	Point {
		x : posx.parse().unwrap(),
		y : posy.parse().unwrap(),
	}
}

fn prase_robot(pos : &str) -> Robot {
	let (pos, vel) = pos.split_once(" ").unwrap();
	(
		prase_pos(&pos[2..]),
		prase_pos(&vel[2..])
	)
}

fn parse_input(content : String) -> Robots {
	content.lines()
		.map(prase_robot)
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
    fn test_determine_quadrant() {
		assert_eq!(Some(3), determine_quadrant(11, 7, 3, 5));
		assert_eq!(None, determine_quadrant(11, 7, 5, 4));
		assert_eq!(Some(2), determine_quadrant(11, 7, 9, 0));
    }


	#[test]
    fn test_part_a_solo_robo() {
		let input = shared::read_test_input_n(DAY, 0);
		let robots = parse_input(input);
		let (q1,q2,q3,q4) = walk_robots(101, 103, &robots, 100);
		assert_eq!(0, q1*q2*q3*q4);
    }

	#[test]
    fn test_part_a() {
		let input = shared::read_test_input_n(DAY, 1);
		let robots = parse_input(input);
		let (q1,q2,q3,q4) = walk_robots(101, 103, &robots, 100);
		assert_eq!(12, q1*q2*q3*q4);
    }


	#[test]
    fn test_part_b() {
		let input = shared::read_test_input_n(DAY, 1);
		let robots = parse_input(input);
		print_robots(&robots);
    }
}
