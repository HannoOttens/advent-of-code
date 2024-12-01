use std::collections::HashMap;
extern crate shared;

const DAY : i32 = 1;

fn main() {
	let content = shared::read_input(DAY);
	let (list_a, list_b) = parse_input(&content);
	if shared::is_part_a() {
		println!("{}", list_dist(list_a, list_b));
	} else {
		println!("{}", list_similarity(list_a, list_b));
	}
}

fn parse_input(content : &str) -> (Vec<i32>, Vec<i32>) {
	let mut list_a = vec!();
	let mut list_b = vec!();

	for line in content.lines() {
		let parts : Vec<&str> = line.split("   ").collect();
		list_a.push(parts[0].parse().unwrap());
		list_b.push(parts[1].parse().unwrap());
	}
	(list_a, list_b)
}

fn list_dist(mut list_a : Vec<i32>, mut list_b : Vec<i32>) -> i32 {
	list_a.sort();
	list_b.sort();
	let mut totl = 0;
	for (a, b) in list_a.iter().zip(list_b) {
		totl += (a - b).abs();
	}
	totl
}

fn list_similarity(list_a : Vec<i32>, list_b : Vec<i32>) -> i32 {
	let occ_map = occurence_map(list_b);
	let mut totl = 0;
	for a in list_a {
		let n = occ_map.get(&a).unwrap_or(&0);
		totl += n * a;
	}
	totl
}

fn occurence_map(list : Vec<i32>) -> HashMap<i32, i32> {
	let mut map = HashMap::new();
	for a in list {
		let count = map.entry(a).or_insert(0);
		*count += 1;
	}
	map
}


#[cfg(test)]
mod tests {
    use super::*;
	const LIST_A : [i32; 6] = [3, 4, 2, 1, 3, 3];
	const LIST_B : [i32; 6] = [4, 3, 5, 3, 9, 3];

    #[test]
    fn input_exists() {
		shared::read_input(DAY);
    }

	#[test]
	fn parse() {
		let content = "\
3   4
4   3
2   5
1   3
3   9
3   3";
		let (list_a, list_b) = parse_input(content);
		assert_eq!(list_a, LIST_A);
		assert_eq!(list_b, LIST_B);
	}

    #[test]
	fn dist() {
		assert_eq!(11, list_dist(LIST_A.to_vec(), LIST_B.to_vec()));
	}

	#[test]
	fn test_occurence_map() {
		let map = occurence_map(LIST_B.to_vec());
		assert_eq!(3, *map.get(&3).unwrap());
		assert_eq!(1, *map.get(&4).unwrap());
		assert_eq!(1, *map.get(&9).unwrap());
		assert_eq!(1, *map.get(&5).unwrap());
	}

	#[test]
	fn similarity() {
		assert_eq!(31, list_similarity(LIST_A.to_vec(), LIST_B.to_vec()));
	}
}