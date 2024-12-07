extern crate shared;

const DAY : i32 = 7;
type Equa = (u64, Vec<u64>);

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let input = parse_input(content);
	if shared::is_part_a() {
		println!("{}", find_valid_equas(input));
	} else {
		println!("{}", find_valid_concat_equas(input));
	}
}

// =============================================================================
// vv part a

fn exec_equa(equa : &Equa, muls : u64) -> u64 {
	let mut totl = equa.1[0];
	for i in 1..equa.1.len() {
		if (muls & (1 << i)) == 0 {
			totl *= equa.1[i];
		} else {
			totl += equa.1[i];
		}
	}
	totl
}

fn validate_equa(equa : &Equa) -> bool {
	let mut muls = 0;
	while muls < (1 << equa.1.len()) {
		if equa.0 == exec_equa(equa, muls) {
			return true
		}
		muls += 1;
	}
	false
}

fn find_valid_equas(equas : Vec<Equa>) -> u64 {
	equas.into_iter()
		.filter(validate_equa)
		.map(|x| x.0)
		.sum()
}

// =============================================================================
// vv part b

fn concat(mut a : u64, b : u64) -> u64 {
	let mut temp = b;
	while temp > 0 {
		a *= 10;
		temp /= 10;
	}
	a + b
}

fn exec_concat_equa(equa : &Equa, muls : u64, concats : u64) -> u64 {
	let mut totl = equa.1[0];
	for i in 1..equa.1.len() {
		if muls & (1 << (i - 1)) > 0 {
			totl *= equa.1[i];
		} else if concats & (1 << (i - 1)) > 0 {
			totl = concat(totl, equa.1[i]);
		} else {
			totl += equa.1[i];
		}
	}
	totl
}

fn validate_concat_equa(equa : &Equa) -> bool {
	let mut muls = 0;
	while muls < (1 << equa.1.len()-1) {
		let mut concats = 0;
		while concats < (1 << equa.1.len()-1) {
			if (muls & concats == 0)
				&& (equa.0 == exec_concat_equa(equa, muls, concats))
			{
				return true
			}
			concats += 1;
		}
		muls += 1;
	}
	false
}

fn find_valid_concat_equas(equas : Vec<Equa>) -> u64 {
	equas.into_iter()
		.filter(validate_concat_equa)
		.map(|x| x.0)
		.sum()
}

// =============================================================================
// vv parse

fn parse_equa(content : &str) -> Equa {
	let (totl, equa) = content.split_once(": ").unwrap();
	(
		totl.parse().unwrap(),
		equa.split(" ")
			.map(|x| x.parse())
			.flatten()
			.collect()
	)
}

fn parse_input(content : String) -> Vec<Equa> {
	content.lines().map(parse_equa).collect()
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
		assert_eq!(190, input[0].0);
		assert_eq!(10, input[0].1[0]);
		assert_eq!(19, input[0].1[1]);
	}

	#[test]
	fn test_validate_equa() {
		let content = shared::read_test_input(DAY);
		let input = parse_input(content);
		assert_eq!(9, input.len());
		assert!(validate_equa(&input[0]));
		assert!(validate_equa(&input[1]));
		assert!(!validate_equa(&input[2]));
		assert!(!validate_equa(&input[3]));
		assert!(!validate_equa(&input[5]));
		assert!(!validate_equa(&input[6]));
		assert!(!validate_equa(&input[7]));
		assert!(validate_equa(&input[8]));
	}

	#[test]
    fn test_part_a() {
		let content = shared::read_test_input(DAY);
		let input = parse_input(content);
		assert_eq!(3749, find_valid_equas(input));
	}

	#[test]
    fn test_concat() {
		assert_eq!(12346, concat(123,46));
	}

	#[test]
	fn test_validate_concat_equa() {
		let content = shared::read_test_input(DAY);
		let input = parse_input(content);
		assert_eq!(9, input.len());
		assert!(validate_concat_equa(&input[0]));
		assert!(validate_concat_equa(&input[1]));
		assert!(!validate_concat_equa(&input[2]));
		assert!(validate_concat_equa(&input[3]));
		assert!(!validate_concat_equa(&input[5]));
		assert!(validate_concat_equa(&input[6]));
		assert!(!validate_concat_equa(&input[7]));
		assert!(validate_concat_equa(&input[8]));
	}


	#[test]
    fn test_part_b() {
		let content = shared::read_test_input(DAY);
		let input = parse_input(content);
		assert_eq!(11387, find_valid_concat_equas(input));
	}
}