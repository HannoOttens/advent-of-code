extern crate shared;
use shared::Point;

const DAY : i32 = 15;

type Grid = Vec<Vec<char>>;
type Instructions = Vec<char>;

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let (mut grid, instructions) = parse_input(content);
	let mut robot = find_robot(&grid);
	if shared::is_part_a() {
		follow_instructions(&mut grid, &mut robot, &instructions);
		println!("{:?}", calc_gps(&grid));
	} else if shared::is_part_b() {
		let mut grid = widen_grid(grid);
		follow_wide_instructions(&mut grid, &mut robot, &instructions);
		println!("{:?}", calc_wide_gps(&grid));
	}
}

// =============================================================================
// vv part a
// =============================================================================

fn find_robot(grid : &Grid) -> Point {
	for y in 0..grid.len() {
		for x in 0..grid[0].len() {
			if grid[y][x] == '@' {
				return Point::from_usize(x,y);
			}
		}
	}
	panic!("No robot found");
}

const LEFT : Point = Point { x: -1, y:  0 };
const RIGHT : Point = Point { x: 1, y:  0 };

fn instruction_dir(instruction : char) -> Point {
	match instruction {
		'^' => Point { x:  0, y: -1 },
		'>' => RIGHT,
		'v' => Point { x:  0, y:  1 },
		'<' => LEFT,
		_ => panic!("Unkown instruction")
	}
}

fn calc_gps(grid : &Grid) -> usize {
	let mut totl = 0;
	for (y, line) in grid.iter().enumerate() {
		for (x, chr) in line.iter().enumerate() {
			if *chr == 'O' {
				totl += x + y*100;
			}
		}
	}
	totl
}

// =============================================================================
// vv grid check/modifie

fn move_char(grid : &mut Grid, from : Point, to : Point) {
	assert_eq!(grid[to.y as usize][to.x as usize], '.');
	grid[to.y as usize][to.x as usize] = grid[from.y as usize][from.x as usize];
	remove_at_pos(grid, from);
}

fn remove_at_pos(grid : &mut Grid, robot : Point) {
	assert_ne!(grid[robot.y as usize][robot.x as usize], '.');
	grid[robot.y as usize][robot.x as usize] = '.'
}

fn set_box(grid : &mut Grid, pos : Point) {
	grid[pos.y as usize][pos.x as usize] = 'O'
}

fn is_box(grid : &Grid, pos : Point) -> bool {
	grid[pos.y as usize][pos.x as usize] == 'O'
}

fn is_free(grid : &Grid, pos : Point) -> bool {
	grid[pos.y as usize][pos.x as usize] == '.'
}

// =============================================================================
// vv check if can exec and exec

fn can_exec(grid : &Grid, robot : &Point, instruction : char) -> bool {
	let dir = instruction_dir(instruction);
	let mut until_unpushable = 1;
	while is_box(grid, *robot + dir * until_unpushable) {
		until_unpushable += 1;
	}
	is_free(grid, *robot + dir * until_unpushable)
}

fn exec(grid : &mut Grid, robot : &mut Point, instruction : char) {
	let dir = instruction_dir(instruction);
	let mut until_unpushable = 1;
	while is_box(grid, *robot + dir * until_unpushable) {
		until_unpushable += 1;
	}

	if until_unpushable > 1 {
		remove_at_pos(grid, *robot + dir);
		set_box(grid, *robot + dir * until_unpushable);
	}

	*robot = *robot + dir;
}

// =============================================================================
// vv main loop

fn follow_instructions(grid : &mut Grid, robot : &mut Point, instructions : &Instructions) {
	remove_at_pos(grid, *robot);
	for instruction in instructions {
		if can_exec(grid, robot, *instruction) {
			exec(grid, robot, *instruction)
		}
	}
}

// =============================================================================
// vv part b
// =============================================================================
// vv check if we can push a box

fn is_left_box(grid : &Grid, pos : Point) -> bool {
	grid[pos.y as usize][pos.x as usize] == '['
}

fn is_right_box(grid : &Grid, pos : Point) -> bool {
	grid[pos.y as usize][pos.x as usize] == ']'
}

fn is_wide_box(grid : &Grid, pos : Point) -> bool {
	is_left_box(grid, pos) || is_right_box(grid, pos)
}

fn can_push_part_ud(grid : &Grid, pos : Point, dir : Point) -> bool {
	let nxtp = pos + dir;
	if is_wide_box(grid, nxtp) {
		can_push_box(grid, nxtp, dir)
	} else {
		is_free(grid, nxtp)
	}
}

fn can_push_box(grid : &Grid, pos : Point, dir : Point) -> bool {
	if (dir == LEFT || dir == RIGHT) && is_wide_box(grid, pos) {
		let nxtp = pos + dir * 2;
		is_free(grid, nxtp) || (is_wide_box(grid, nxtp) && can_push_box(grid, nxtp, dir))
	} else if is_left_box(grid, pos) {
		let rpos = Point { x: pos.x + 1, y: pos.y };
		assert!(is_right_box(grid, rpos));
		can_push_part_ud(grid, pos, dir) && can_push_part_ud(grid, rpos, dir)
	} else if is_right_box(grid, pos) {
		let lpos = Point { x: pos.x - 1, y: pos.y };
		assert!(is_left_box(grid, lpos));
		can_push_part_ud(grid, lpos, dir) && can_push_part_ud(grid, pos, dir)
	} else {
		panic!("Unexpected: Asked to check if we can push a box that does not exist");
	}
}

// =============================================================================
// vv push a box

fn push_part_box_ud(grid : &mut Grid, pos : Point, dir : Point) {
	let nxtp = pos + dir;
	if is_wide_box(grid, nxtp) {
		push_box(grid, nxtp, dir)
	}
	move_char(grid, pos, nxtp);
}

fn push_box_lr(grid : &mut Grid, pos : Point, dir : Point) {
	let nxt_box = pos + dir * 2;
	if is_wide_box(grid, nxt_box) {
		push_box(grid, nxt_box, dir)
	}
	move_char(grid, pos+dir, nxt_box);
	move_char(grid, pos, pos+dir);
}

fn push_box(grid : &mut Grid, pos : Point, dir : Point) {
	if (dir == LEFT || dir == RIGHT) && is_wide_box(grid, pos) {
		push_box_lr(grid, pos, dir);
	} else if is_left_box(grid, pos) {
		let rpos = Point { x: pos.x + 1, y: pos.y };
		assert!(is_right_box(grid, rpos));
		push_part_box_ud(grid, pos, dir);
		push_part_box_ud(grid, rpos, dir);
	} else if is_right_box(grid, pos) {
		let lpos = Point { x: pos.x - 1, y: pos.y };
		assert!(is_left_box(grid, lpos));
		push_part_box_ud(grid, lpos, dir);
		push_part_box_ud(grid, pos, dir);
	} else {
		panic!("Unexpected: Asked to push a box that does not exist");
	}
}

// =============================================================================

fn widen_grid(grid : Grid) -> Grid {
	let mut wide_grid = Vec::new();
	for line in grid {
		let mut wide_line = Vec::new();
		for chr in line {
			if chr == 'O' {
				wide_line.push('[');
				wide_line.push(']');
			}
			else if chr == '@' {
				wide_line.push('.');
				wide_line.push('.');
			} else {
				wide_line.push(chr);
				wide_line.push(chr);
			}
		}
		wide_grid.push(wide_line);
	}
	wide_grid
}

// =============================================================================
// vv check if can exec and exec

fn can_exec_wide(grid : &Grid, robot : &Point, instruction : char) -> bool {
	let dir = instruction_dir(instruction);
	let nxtp = *robot + dir;
	is_free(grid, nxtp)
		|| (is_wide_box(grid, nxtp) && can_push_box(grid, nxtp, dir))
}

fn exec_wide(grid : &mut Grid, robot : &mut Point, instruction : char) {
	let dir = instruction_dir(instruction);
	if is_wide_box(grid, *robot + dir) {
		push_box(grid, *robot + dir, dir);
	}
	*robot = *robot + dir;
}

// =============================================================================
// vv main loop

fn calc_wide_gps(grid : &Grid) -> usize {
	let mut totl = 0;
	for (y, line) in grid.iter().enumerate() {
		for (x, chr) in line.iter().enumerate() {
			if *chr == '[' {
				totl += x + y*100;
			}
		}
	}
	totl
}

fn follow_wide_instructions(grid : &mut Grid, robot : &mut Point, instructions : &Instructions) {
	*robot = Point { x: robot.x * 2, y: robot.y };
	for instruction in instructions {
		if can_exec_wide(&grid, robot, *instruction) {
			exec_wide(grid, robot, *instruction);
		}
	}
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_input(content : String) -> (Grid, Instructions) {
	let (grid, instructions) = content.split_once("\r\n\r\n").unwrap();
	let grid = grid.lines()
		.map(|line| line.chars().collect())
		.collect();
	let instructions = instructions.lines()
		.map(|line| line.chars().collect::<Vec<_>>())
		.flatten()
		.collect();
	(grid, instructions)
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
	fn test_part_a_0() {
		let content = shared::read_test_input_n(DAY, 0);
		let (mut grid, instructions) = parse_input(content);
		let mut robot = find_robot(&grid);
		follow_instructions(&mut grid, &mut robot, &instructions);
	    assert_eq!(2028, calc_gps(&grid));
	}

	#[test]
	fn test_part_a_1() {
		let content = shared::read_test_input_n(DAY, 1);
		let (mut grid, instructions) = parse_input(content);
		let mut robot = find_robot(&grid);
		follow_instructions(&mut grid, &mut robot, &instructions);
	    assert_eq!(10092, calc_gps(&grid));
	}

	#[test]
	fn test_part_b_1() {
		let content = shared::read_test_input_n(DAY, 1);
		let (grid, instructions) = parse_input(content);
		let mut robot = find_robot(&grid);
		let mut grid = widen_grid(grid);
		follow_wide_instructions(&mut grid, &mut robot, &instructions);
	    assert_eq!(9021, calc_wide_gps(&grid));
	}
}
