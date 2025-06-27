use std::collections::HashMap;

extern crate shared;

#[derive(Debug)]
#[derive(Clone)]
#[derive(PartialEq, Eq)]
enum Operation {
	AND,
	OR,
	XOR
}

#[derive(Debug)]
#[derive(Clone)]
struct Gate<'a> {
	in_a : &'a str,
	in_b : &'a str,
	out : &'a str,
	op : Operation,
}

type Error = String;
// type Error = (String, Option<(&str, &str)>);

const DAY : i32 = 24;

fn main() {
	shared::bench (run);
}

fn run() {
	let content = shared::read_input(DAY);
	let (mut wires, gates) = parse_input(&content);

	if shared::is_part_a() {
		run_computer(&mut wires, gates);
		println!("{:?}", number_value('z', wires));
	} else if shared::is_part_b() {
		println!("{:?}", validate_addition(gates));
	}
}

// =============================================================================
// vv part a
// =============================================================================

fn run_computer<'a>(wires : &mut HashMap<&'a str, bool>, mut gates : Vec<Gate<'a>>) {
	let mut i = 0;
	while gates.len() > 0 {
		i = i % gates.len();
		let gate = &gates[i];
		if wires.contains_key(gate.in_a)
			&& wires.contains_key(gate.in_b)
		{
			wires.insert(
				gate.out,
				match &gate.op {
					Operation::AND => wires[gate.in_a] && wires[gate.in_b],
					Operation::OR  => wires[gate.in_a] || wires[gate.in_b],
					Operation::XOR => wires[gate.in_a] ^  wires[gate.in_b],
				}
			);
			gates.remove(i);
		}
		else {
			i += 1;
		}
	}
}

fn number_value(c : char, wires: HashMap<&str, bool>) -> u64 {
	let zs : Vec<_> = wires.into_iter()
		.filter(|(k,_)| k.starts_with(c))
		.collect();

	let mut n = 0;
	for (gate, value) in zs {
		let gate = &gate[1..];
		if value {
			n += 1 << gate.parse::<usize>().unwrap();
		}
	}
	n
}

// =============================================================================
// vv part b
// =============================================================================
// a full adder circuit contains 1 OR, 2 XOR and 2 AND gates
// a half adder contains one XOR and one AND gate
// the input has 44 OR gates, 89 AND gates and 89 XOR gates.
// This is enough for exactly 44 full adders and one half adder (the first bit does not have a carry in)

// The answer was: bmn,jss,mvb,rds,wss,z08,z18,z23
// Found by running the program and dealing wiht the errors.
// Curious if we can auto-fix it...

fn find_by_inputs<'a>(
	gates : &'a Vec<Gate<'a>>,
	in_a : &str,
	in_b : &str,
	op : Operation
) -> Result<&'a Gate<'a>, Error>
{
	if let Some(gate) = gates.iter().find(|g| (g.in_a == in_a && g.in_b == in_b) && g.op == op) {
		return Ok(gate)
	}

	gates.iter().find(|g| (g.in_a == in_b && g.in_b == in_a) && g.op == op)
		.ok_or(format!("Expected {:?} gate with inputs '{}' and '{}'", op, in_a, in_b))
}

fn validate_half_adder(gates : Vec<Gate>, carry_out: &str) -> Result<(), Error> {
	let x_0 = "x00";
	let y_0 = "y00";
	let z_0 = "z00";

	let xor_input = find_by_inputs(&gates, &x_0, &y_0, Operation::XOR)?;
	if xor_input.out != z_0 {return  Err("x_0 and y_0 do not output to z_0".to_string())}
	let and_input = find_by_inputs(&gates, &x_0, &y_0, Operation::AND)?;
	if and_input.out != carry_out {return Err(format!("Carry out of half adder should be '{}' but was '{}'", and_input.out, carry_out))}

	Ok(())
}

fn validate_full_adder<'a>(gates : &Vec<Gate<'a>>, bit : usize, carry_out: &str) -> Result<&'a str, Error> {
	let x_n = format!("x{:02}", bit);
	let y_n = format!("y{:02}", bit);
	let z_n = format!("z{:02}", bit);

	// addition
	// x_n xor y_n -> a_n
	let xor_input = find_by_inputs(gates, &x_n, &y_n, Operation::XOR)?;
	let a_n = xor_input.out;
	// a_n xor c_n -> z_n
	let xor_carry = gates.iter()
		.find(|g| (g.in_a == a_n || g.in_b == a_n) && g.op == Operation::XOR)
		.ok_or(format!("Could not find XOR gate with '{}' as input. Swap '{}' and '{}'?", a_n, a_n, z_n))?;
	if xor_carry.out != z_n { return Err(format!("Expected output of XOR gate to be '{}', but found gate with '{}' instead.", z_n, xor_carry.out)); }

	// bepaal de carry-in wire
	let c_n : &str;
	if xor_carry.in_a == a_n {
		c_n = xor_carry.in_b
	} else {
		c_n = xor_carry.in_a
	}

	// carry circuit
	// a_n and c_n -> c1_n
	let and_carry = find_by_inputs(gates, &a_n, &c_n, Operation::AND)?;
	// x_n and y_n -> c2_n
	let and_input = find_by_inputs(gates, &x_n, &y_n, Operation::AND)?;
	// c1_n or c2_n -> cout
	let or_carry = find_by_inputs(gates, and_carry.out, and_input.out, Operation::OR)?;

	if or_carry.out != carry_out { return Err("err".to_string()) }
	Ok(c_n)
}

fn validate_addition(gates : Vec<Gate>) -> Result<(), Error> {
	let mut carry_out = "z45";
	let mut bit = 44;
	while bit > 0 {
		carry_out = validate_full_adder(&gates, bit, carry_out)?;
		bit -= 1;
	}
	validate_half_adder(gates, carry_out)
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_wire(wire : &str) -> (&str, bool) {
	let (name, value) = wire.split_once(": ").unwrap();
	let mut b = false;
	if value == "1" { b = true; }
	(name, b)
}

fn parse_gate(wire : &str) -> Gate {
	let (logic, out) = wire.split_once(" -> ").unwrap();

	if let Some((in_a, in_b)) = logic.split_once(" XOR ") {
		return Gate { in_a, in_b, out, op: Operation::XOR };
	}

	if let Some((in_a, in_b)) = logic.split_once(" OR ") {
		return Gate { in_a, in_b, out, op: Operation::OR };
	}

	let (in_a, in_b) = logic.split_once(" AND ").unwrap();
	Gate { in_a, in_b, out, op: Operation::AND }
}

fn order_gate(mut gate : Gate) -> Gate {
	if gate.in_b.starts_with("x") {
		let temp = gate.in_b;
		gate.in_b = gate.in_a;
		gate.in_a = temp;
	}
	gate
}

fn parse_input(content : &String) -> (HashMap<&str, bool>, Vec<Gate>) {
	let (wires, gates) = content.split_once("\r\n\r\n").unwrap();
	let wires = wires.lines().map(parse_wire).collect();
	let gates = gates.lines().map(parse_gate).map(order_gate).collect();
	(wires, gates)
}

// =============================================================================
// vv tests vv

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn inputs_exists() {
		shared::read_test_input_n(DAY, 0);
		shared::read_test_input_n(DAY, 1);
		shared::read_input(DAY);
    }

	#[test]
    fn test_parse() {
		let input = shared::read_test_input_n(DAY, 0);
		let (wires, _gates) = parse_input(&input);

		assert!(wires["x00"]);
		assert!(wires["x01"]);
		assert!(wires["x02"]);
		assert!(!wires["y00"]);
		assert!(wires["y01"]);
		assert!(!wires["y02"]);
    }
}
