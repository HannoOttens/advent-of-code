extern crate shared;

const DAY : i32 = 2;

fn main() {
	let content = shared::read_input(DAY);
	let input = parse_input(&content);

	if shared::is_part_a() {
		println!("{}", count_safe(input));
	} else {
		println!("{}", count_dampend_safe(input));
	}
}

fn parse_input(content : &str) -> Vec<Vec<i32>> {
	let mut result = Vec::new();

	for line in content.lines() {
		let parts = line.split(" ");
		result.push(parts.map(|x| x.parse().unwrap()).collect());
	}
	result
}

fn count_safe(input : Vec<Vec<i32>>) -> usize {
	input.into_iter()
		.map(|report: Vec<i32>| safe_report(&report))
		.filter(|b| *b)
		.count()
}

fn count_dampend_safe(input : Vec<Vec<i32>>) -> usize {
	input.into_iter()
		.map(dampend_safe_report)
		.filter(|b| *b)
		.count()
}

fn validate_diff(report : &Vec<i32>, a : usize, b : usize) -> bool {
	   ((report[a] - report[b]).abs() <= 3)
	&& ((report[a] - report[b]).abs() >= 1)
}

fn safe_report(report : &Vec<i32>) -> bool {
	let mut diff = true;
	let mut decr = true;
	let mut incr = true;
	for n in 1..report.len() {
		diff = diff && validate_diff(report, n, n-1);
		decr = decr && (report[n-1] > report[n]);
		incr = incr && (report[n-1] < report[n]);
	}
	diff && (decr || incr)
}

fn dampend_safe_report(report : Vec<i32>) -> bool {
	if safe_report(&report) {
		return true;
	}

	for n in 0..report.len() {
		let mut rep = report.clone();
		rep.remove(n);
		if safe_report(&rep) {
			return true;
		}
	}

	false
}

#[cfg(test)]
mod tests {
    use super::*;

	const MATRIX : [[i32; 5]; 6] = [
		[7, 6, 4, 2, 1],
		[1, 2, 7, 8, 9],
		[9, 7, 6, 2, 1],
		[1, 3, 2, 4, 5],
		[8, 6, 4, 4, 1],
		[1, 3, 6, 7, 9],
	];

	const CONTENT : &str = "\
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";

    #[test]
    fn input_exists() {
		shared::read_input(DAY);
    }

	#[test]
	fn parse() {
		assert_eq!(parse_input(CONTENT), MATRIX);
	}

    #[test]
	fn test_safe() {
		assert!( safe_report(&MATRIX[0].to_vec()));
		assert!(!safe_report(&MATRIX[1].to_vec()));
		assert!(!safe_report(&MATRIX[2].to_vec()));
		assert!(!safe_report(&MATRIX[3].to_vec()));
		assert!(!safe_report(&MATRIX[4].to_vec()));
		assert!( safe_report(&MATRIX[5].to_vec()));
	}

    #[test]
	fn test_dampend_safe() {
		assert!( dampend_safe_report(MATRIX[0].to_vec()));
		assert!(!dampend_safe_report(MATRIX[1].to_vec()));
		assert!(!dampend_safe_report(MATRIX[2].to_vec()));
		assert!( dampend_safe_report(MATRIX[3].to_vec()));
		assert!( dampend_safe_report(MATRIX[4].to_vec()));
		assert!( dampend_safe_report(MATRIX[5].to_vec()));
	}

    #[test]
	fn test_sum_safe() {
		assert_eq!(2, count_safe(parse_input(CONTENT)));
	}
}