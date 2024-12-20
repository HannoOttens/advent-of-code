use std::collections::HashMap;

extern crate shared;

type Towels = Vec<String>;
type Designs = Vec<String>;
const DAY : i32 = 19;

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let (towels, designs) = parse_input(content);

	if shared::is_part_a() {
		println!("{:?}", check_ok(&towels, &designs));
	} else if shared::is_part_b() {
		println!("{:?}", check_total(&towels, &designs));
	}
}

// =============================================================================
// vv part a
// =============================================================================

fn check_design(towels: &Towels, design: &str) -> bool {
	if design.len() == 0 {
		return true
	}

	for towel in towels {
		if design.starts_with(towel) &&
			check_design(towels, &design[towel.len()..])
		{
			return  true;
		}
	}

	false
}

fn check_ok(towels: &Towels, designs: &Designs) -> usize {
	designs.into_iter()
		.filter(|design| check_design(towels, design))
		.count()
}

// =============================================================================
// vv part b
// =============================================================================

fn check_design_options(mem : &mut HashMap<String, usize>, towels: &Towels, design: &str) -> usize {
	if let Some(n) = mem.get(design) {
		return *n;
	}

	if design.len() == 0 {
		return 1
	}

	let n = towels.into_iter()
		.filter(|towel| design.starts_with(*towel))
		.map(|towel| check_design_options(mem, towels, &design[towel.len()..]))
		.sum();
	mem.insert(String::from(design), n);
	n
}

fn check_total(towels: &Towels, designs: &Designs) -> usize {
	let mut mem = HashMap::new();

	designs.into_iter()
		.map(|design| check_design_options(&mut mem, towels, design))
		.sum()
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_input(content : String) -> (Towels, Designs) {
	let (towels, designs) = content.split_once("\r\n\r\n").unwrap();
	(
		towels.split(", ").map(|s| String::from(s)).collect(),
		designs.lines().map(|s| String::from(s)).collect(),
	)
}

// =============================================================================
// vv tests vv
// =============================================================================

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
		let input = shared::read_input(DAY);
		parse_input(input);
    }


	#[test]
    fn test_part_a() {
		let input = shared::read_test_input_n(DAY, 0);
		let (towels, designs) = parse_input(input);
		assert_eq!(6, check_ok(&towels, &designs));
    }
}
