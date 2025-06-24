use std::collections::{HashMap, HashSet};

extern crate shared;

const DAY : i32 = 23;

fn main() {
	shared::bench (run);
}

fn run() {
	let content = shared::read_input(DAY);
	let network = parse_input(&content);

	if shared::is_part_a() {
		// 2406: too high!?
		println!("{}", sets_of_three(network));
	} else if shared::is_part_b() {
		let max_clique = max_cliques(network);
		println!("{:?}", max_clique);

		let content = shared::read_input(DAY);
		let network = parse_input(&content);
		for n1 in &max_clique {
			for n2 in &max_clique {
				assert!(n1 == n2 || network[n1].contains(n2));
			}
		}
		println!("{:?}", max_clique.into_iter().fold(String::new(), |a,b| a + "," + b));
	}
}

// =============================================================================
// vv part a
// =============================================================================

fn sort<'a>(
	mut n1 : &'a str,
	mut n2 : &'a str,
	mut n3 : &'a str
) -> (&'a str, &'a str, &'a str)
{
	if n2 > n3 {
		let t = n3;
		n3 = n2;
		n2 = t;
	}
	if n1 > n2 {
		let t = n2;
		n2 = n1;
		n1 = t;
	}
	if n2 > n3 {
		let t = n3;
		n3 = n2;
		n2 = t;
	}
	assert!(n1 <= n2);
	assert!(n2 <= n3);
	(n1, n2, n3)
}

fn sets_of_three(network : HashMap<&str, HashSet<&str>>) -> usize {
	let mut set = HashSet::new();

	for (n1, connected) in &network {
		for n2 in connected {
			for n3 in connected {
				if (n1 != n2) && (n2 != n3) && (n1 != n3)
				&& network[n2].contains(n3)
				&& ((n1.starts_with("t"))
				 || (n2.starts_with("t"))
				 || (n3.starts_with("t"))) {
					set.insert(sort(n1, n2, n3));
				}
			}
		}
	}
	set.len()
}

// =============================================================================
// vv part b
// =============================================================================

fn bron_kerbosch<'a>(
    r: &HashSet<&'a str>,
    p: &mut HashSet<&'a str>,
    x: &mut HashSet<&'a str>,
    g: &HashMap<&'a str, HashSet<&'a str>>,
    cliques: &mut Vec<HashSet<&'a str>>,
) {
    if p.is_empty() && x.is_empty() {
        if r.len() > 2 {
            cliques.push(r.clone());
        }
        return;
    }

    // Choose a pivot with the maximum degree in P ∪ X
    let pivot = p.union(x).max_by_key(|v| g[*v].len()).unwrap();

	let candidates : Vec<_> = p.difference(&g[pivot]).cloned().collect();
	for v in candidates {
		// New R is R ∪ {v}
		let mut new_r = r.clone();
		new_r.insert(v);

		// New P is P ∩ N(v)
		let mut new_p = p.intersection(&g[v]).map(|v| *v).collect();
		// New X is X ∩ N(v)
		let mut new_x = x.intersection(&g[v]).map(|v| *v).collect();
		// Recursive call
		bron_kerbosch(&new_r, &mut new_p, &mut new_x, g, cliques);

		// Move v from P to X
		p.remove(v);
		x.insert(v);
	}
}

fn max_cliques<'a>(network : HashMap<&'a str, HashSet<&'a str>>) -> Vec<&'a str> {
	let r: HashSet<&str> = HashSet::new();
    let mut p: HashSet<&str> = network.keys().cloned().collect();
    let mut x: HashSet<&str> = HashSet::new();

    // Collect cliques
    let mut cliques = Vec::new();
    bron_kerbosch(&r, &mut p, &mut x, &network, &mut cliques);

	println!("{:?}", cliques);
	let mut result = cliques.into_iter()
		.fold(HashSet::new(), |max:HashSet<&str>, c| if c.len() > max.len() {c} else {max})
		.into_iter()
		.collect::<Vec<_>>();
	result.sort();
	result
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_input(content : &String) -> HashMap<&str, HashSet<&str>> {
	content.lines()
		.map(|s : &str| {
			let mut split = s.split('-');
			(split.next().unwrap(), split.next().unwrap())
		})
		.fold(HashMap::new(),
			|mut m, (a, b)| {
				m.entry(a).or_default().insert(b);
				m.entry(b).or_default().insert(a);
				m
			})
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
    fn test_part_a() {
		let content = shared::read_test_input_n(DAY, 0);
		let network = parse_input(&content);
		assert_eq!(sets_of_three(network), 7);
    }
}
