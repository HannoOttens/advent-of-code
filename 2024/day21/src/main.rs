use std::collections::HashMap;
extern crate shared;

const DAY : i32 = 21;

fn main() {
	shared::bench (run);
}

fn run() {
	let content = shared::read_input(DAY);
	let options = parse_input(content);
	if shared::is_part_a() {
		let total = options.iter()
			.map(|(i, code)| (*i as u64) * keypad_movement(code, 2))
			.sum::<u64>();
		println!("{:?}", total);
	} else if shared::is_part_b() {
		let total = options.iter()
			.map(|(i, code)| (*i as u64) * keypad_movement(code, 25))
			.sum::<u64>();
		println!("{:?}", total);
	}
}

// =============================================================================
// vv part a
// =============================================================================
// lekker complex de paden zoeken:

const NUM_PAD : &str = "789456123 0A";
const ARR_PAD : &str = " ^A<v>";

fn extn_path(path : &str, c : char) -> String {
	let mut p = path.to_string();
	p.push(c);
	return p;
}

fn manhat(from : i32, to : i32) -> usize {
	let from_x = from % 3;
	let from_y = from / 3;
	let to_x = to % 3;
	let to_y = to / 3;
	((to_x - from_x).abs() + (to_y - from_y).abs()) as usize
}

fn _find_path(pad : &str, from : i32, to : i32, max_l : usize, seen : usize, path : &str) -> Vec<String> {
	// Lege plek? Stoppen
	let cur_char = pad.chars().nth(from as usize).unwrap();
	if cur_char == ' ' { return vec!(); }
	// Al geweest? Stoppen
	if seen & (1 << from) > 0 { return vec!(); }
	// Langer dan manhattan distance? Stoppen
	if path.len() > max_l { return vec!(); }

	// Markeren als gezien
	let seen = seen | (1 << from);

	if from == to {
		return vec!(extn_path(path, 'A'));
	} else {
		let mut paths = vec!();
		if from + 3 < (pad.chars().count() as i32)
		{
			paths.push(_find_path(pad, from+3, to, max_l, seen, &extn_path(path, 'v')));
		}
		if from - 3 >= 0
		{
			paths.push(_find_path(pad, from - 3, to, max_l, seen, &extn_path(path, '^')));
		}
		if from == 0 || from == 1
		|| from == 3 || from == 4
		|| from == 6 || from == 7
		|| from == 9 || from == 10
		{
			paths.push(_find_path(pad, from + 1, to, max_l, seen, &extn_path(path, '>')));
		}
		if from == 2  || from == 1
		|| from == 5  || from == 4
		|| from == 8  || from == 7
		|| from == 11 || from == 10 {
			paths.push(_find_path(pad, from - 1, to, max_l, seen, &extn_path(path, '<')));
		}
		return paths.into_iter().flatten().collect();
	}
}

fn find_path(pad : &str, from : char, to : char) -> Vec<String> {
	let from = pad.find(from).unwrap() as i32;
	let to   = pad.find(to  ).unwrap() as i32;
	_find_path(pad, from, to, manhat(from, to), 0, &"")
}

fn move_time(
	mem : &mut HashMap<(char, char, usize), u64>,
	from : char,
	to : char,
	robots : usize,
	frst : bool) -> u64
{
	let m = mem.get(&(from, to, robots));
	if m.is_some() { return *m.unwrap() }

	let paths;
	if frst { paths = find_path(NUM_PAD, from, to) }
	else	{ paths = find_path(ARR_PAD, from, to) }

	if robots == 0 { return paths.iter().map(|p| p.len()).min().unwrap() as u64; }

	let min_time = paths
		.into_iter()
		.map(|path| {
			let z = String::from("A") + &path;
			z.chars().zip(path.chars())
					 .map(|(from, to)| move_time(mem, from, to, robots-1, false))
					 .sum()
		})
		.min().unwrap();
	mem.insert((from, to, robots), min_time);
	min_time
}

fn keypad_movement(code : &str, robots : usize) -> u64 {
	let mut mem = HashMap::new();

	// sub paths van eerste niveau
	let mut totl_time = 0;
	for i in 0..code.chars().count() {
		let to = code.chars().nth(i).unwrap();
		let mut from = 'A';
		if i > 0 { from = code.chars().nth(i-1).unwrap(); }
		totl_time += move_time(&mut mem, from, to, robots, true);
	}

	totl_time
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_input(content : String) -> Vec<(usize, String)> {
	content.lines()
		.map(|s| (s[0..3].parse().unwrap(), String::from(s)))
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
    }

	#[test]
    fn test_parse() {
		let input = shared::read_test_input_n(DAY, 0);
		parse_input(input);
    }
}


//^A<<^^A>>AvvvA

// theirs:

//            3                          7          9                 A
//        ^   A         <<      ^^       A     >>   A        vvv      A
//    <   A > A  v <<   AA >  ^ AA >     A  v  AA ^ A   < v  AAA >  ^ A
// <v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^    A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A
// v<<A>>^AvA^Av<<A>>^AAv<A<A>>^AAvAA<^A>Av<A>^AA<A>Av<A<A>>^AAAvA<^A>A
//    <   A > A   <   AA  v <   AA >>  ^ A  v  AA ^ A  v <   AAA >  ^ A
//        ^   A       ^^        <<       A     >>   A        vvv      A
//            3                          7          9                 A
// 789
// 456
// 123
//  0A

//  ^A
// <v>
