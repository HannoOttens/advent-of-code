extern crate shared;

use std::cmp::Ordering;

use shared::*;

const DAY : i32 = 5;
type OrderMap = [u128;100];

fn main() {
	shared::bench (run);
}

fn run () {
	let input = shared::read_input(DAY);
	let (ordering, updates) = parse_input(&input);
	if is_part_a() {
		println!("{}", sum_valid_mids(&ordering, updates));
	} else {
		println!("{}", sum_invalid_mids(&ordering, updates));
	}
}

// =============================================================================
// vv part a

fn sum_valid_mids(ordering : &OrderMap, updates : Vec<Vec<usize>>) -> usize {
	updates.iter()
		.filter(|update| validate(ordering, update))
		.map(|update| update[update.len()/2])
		.sum()
}

fn order_contains(ordering : &OrderMap, pair: &(usize,usize)) -> bool {
	(ordering[pair.0] & (1 << pair.1)) > 0
}

fn validate(ordering : &OrderMap, update : &Vec<usize>) -> bool {
	update.is_sorted_by(|a,b| order_contains(ordering, &(*a,*b)))
}

// =============================================================================
// vv part b

fn order_compare(ordering : &OrderMap, pair: &(usize,usize)) -> Ordering {
	if order_contains(ordering, pair) {
		Ordering::Less
	} else {
		Ordering::Greater
	}
}

fn sum_invalid_mids(ordering : &OrderMap, updates : Vec<Vec<usize>>) -> usize {
	let mut totl = 0;
	for mut update in updates {
		if !validate(ordering, &update) {
			update.sort_by(|a,b| order_compare(ordering, &(*a, *b)));
			totl += update[update.len() / 2];
		}
	}
	totl
}

// =============================================================================
// vv parse

fn parse_input(content : &str) -> (OrderMap, Vec<Vec<usize>>) {
	let (ordering, updates) = content.split_once("\r\n\r\n").unwrap();
	let ordering = ordering.lines()
		.map(|x| x.split_once('|').unwrap())
		.map(|(l,r)| (l.parse::<usize>().unwrap(), r.parse::<usize>().unwrap()));

	let mut order_map : OrderMap = [0; 100];
	for (l,r) in ordering {
		order_map[l] |= 1 << r;
	}

	let updates = updates.lines()
		.map(|l| l.split(',').map(|x| x.parse::<usize>().unwrap()).collect())
		.collect();
	(order_map, updates)
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
	fn parse_test() {
		let (ordering, updates) = parse_input(&shared::read_test_input(DAY));
		assert_eq!(6, updates.len());

		assert!(order_contains(&ordering, &(53, 13)));
	}

    #[test]
	fn test_validate_base() {
		let (ordering, _) = parse_input(&shared::read_test_input(DAY));
		assert!(validate(&ordering, &[53, 13].to_vec()));
	}

    #[test]
	fn test_validate() {
		let (ordering, updates) = parse_input(&shared::read_test_input(DAY));
		assert!(validate(&ordering, &updates[0]));
	}

    #[test]
	fn test_sum_valid_mids() {
		let (ordering, updates) = parse_input(&shared::read_test_input(DAY));
		assert_eq!(143, sum_valid_mids(&ordering, updates));
	}

    #[test]
	fn test_sum_invalid_mids() {
		let (ordering, updates) = parse_input(&shared::read_test_input(DAY));
		assert_eq!(123, sum_invalid_mids(&ordering, updates));
	}

}


// Dit was mijn originele oplossing voor part a, maar had door dat dit gewoon
//   uiteindelijk gewoon sorteren is
//
// fn validate(map : &OrderMap, update : &Vec<usize>) -> bool {
// 	_validate(map, update, 0, update.len()-1)
// }

// // we gaan uit van een totale ordering
// fn _validate(map : &OrderMap, update : &Vec<usize>, left : usize, right : usize) -> bool {
// 	if left >= right {
// 		return true;
// 	}

// 	let mut ok = true;
// 	let mid : usize = left + ((right - left) / 2);

// 	// validate left side
// 	let mut i = 0;
// 	while ok && (left + i < mid) {
// 		let pair = (update[left + i], update[mid]);
// 		ok &= map.contains_key(&pair);
// 		i += 1;
// 	}
// 	// validate right side
// 	i = 0;
// 	while ok && (right - i > mid) {
// 		let pair = (update[mid], update[right-i]);
// 		ok &= map.contains_key(&pair);
// 		i += 1;
// 	}
// 	//recurse
// 	ok  && ((mid == 0) || _validate(&map, update, left, mid-1))
// 		&& _validate(&map, update, mid+1, right)
// }
