extern crate shared;

const DAY : i32 = 17;

type Registers = (u64, u64, u64);
type Program = Vec<u64>;

#[derive(Debug)]
enum Command {
	ADV (u64), // OP 0: A = A / 2^x
	BXL (u64), // OP 1: B = B xor x
	BST (u64), // OP 2: B = x mod 8
	JNZ (u64), // OP 3: if x == 0 { noop } else { jump x } (let op x/2 vanwege onze parsing) als opcode niet aanwezig -> exit
	BXC (()),  // OP 4: ignore x, B = B xor C
	OUT (u64), // OP 5: print (x mod 8)
	BDV (u64), // OP 6: B = A / 2^x
	CDV (u64), // OP 7: C = A / 2^x
}

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let (regs, program) = parse_input(content).unwrap();

	if shared::is_part_a() {
		println!("{:?}", exec(&program, regs)); // 5,1,4,0,5,1,0,2,6
	} else if shared::is_part_b() {
		println!("{:?}", solve(&program, (0,0,0), 0));
	}
}

// =============================================================================
// vv part a
// =============================================================================

fn op_val(regs : &Registers, operand : u64) -> u64 {
	match operand {
		0 => 0,
		1 => 1,
		2 => 2,
		3 => 3,
		4 => regs.0,
		5 => regs.1,
		6 => regs.2,
		7 => panic!("Combo operand 7 is reserved but not used in valid programs"),
		_ => panic!("Invalid operand"),
	}
}

fn parse_instruction(op : u64, combo : u64) -> Command {
	match op {
		0 => Command::ADV(combo),
		1 => Command::BXL(combo),
		2 => Command::BST(combo),
		3 => Command::JNZ(combo),
		4 => Command::BXC(()),
		5 => Command::OUT(combo),
		6 => Command::BDV(combo),
		7 => Command::CDV(combo),
		_ => panic!("Invalid operation: {op}")
	}
}

fn exec(program : &Program, mut regs : Registers) -> Vec<u64> {
	let mut program_output = Vec::new();
	let mut sp = 0;
	while sp < program.len() {
		let comd = parse_instruction(program[sp], program[sp+1]);
		let out = exec_comd(comd, &mut sp, &mut regs);
		program_output.push(out);
	}
	program_output.into_iter().flatten().collect()
}

fn exec_comd(comd: Command, sp: &mut usize, regs: &mut Registers) -> Option<u64> {
	*sp += 2;
	match comd {
		Command::ADV (x) => { regs.0 = div(regs, regs.0, x); None }, // OP 0: A = A / 2^x
		Command::BXL (x) => { regs.1 = x ^ regs.1          ; None }, // OP 1: B = B xor x
		Command::BST (x) => { regs.1 = op_val(regs, x) % 8 ; None }, // OP 2: B = x mod 8
		Command::JNZ (x) => { jnz(regs.0, x, sp)           ; None }, // OP 3: if A == 0 { noop } else { jump x } (let op x/2 vanwege onze parsing) als opcode niet aanwezig -> exit
		Command::BXC (_) => { regs.1 = regs.1 ^ regs.2     ; None }, // OP 4: ignore x, B = B xor C
		Command::OUT (x) => { Some(op_val(regs, x) % 8)           }, // OP 5: print (x mod 8)
		Command::BDV (x) => { regs.1 = div(regs, regs.0, x); None }, // OP 6: B = A / 2^x
		Command::CDV (x) => { regs.2 = div(regs, regs.0, x); None }, // OP 7: C = A / 2^x
	}
}

fn jnz(condition : u64, new_sp : u64, sp : &mut usize) {
	if condition != 0 {
		*sp = new_sp as usize;
	}
}

fn div(regs : &Registers, val : u64, operand : u64) -> u64 {
	val >> op_val(regs, operand)
}

// =============================================================================
// vv part b
// =============================================================================

fn solve(program : &Program, regs : Registers, index : usize) -> Option<Registers> {
	if index == program.len() {
		return Some(regs);
	}

	let trgt = program.len() - index - 1;
	for i in 0..8 {
		let mut cur_reg = regs;
		cur_reg.0 |= i << (trgt * 3);

		let output = exec(&program, cur_reg);
		if output.len() > trgt && output[trgt] == program[trgt] {
			if let Some(solution) = solve(program, cur_reg, index+1) {
				return Some(solution);
			}
		}
	}
	None
}

// =============================================================================
// vv parse
// =============================================================================

fn parse_register(reg : &str) -> Option<u64> {
	let (_, num) = reg.split_once(": ")?;
	num.parse().ok()
}

fn parse_program(program : &str) -> Option<Program> {
	let (_, prog) = program.split_once(": ")?;
	prog.split(',').map(|x| x.parse().ok()).collect()
}

fn parse_input(content : String) -> Option<(Registers, Program)> {
	let (regs, program) = content.split_once("\r\n\r\n")?;

	let regs = regs.lines().collect::<Vec<_>>();
	let reg_a = parse_register(regs[0])?;
	let reg_b = parse_register(regs[1])?;
	let reg_c = parse_register(regs[2])?;
	let program = parse_program(program)?;

	Some(((reg_a, reg_b, reg_c), program))
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
		shared::read_test_input(DAY);
    }

	#[test]
    fn test_parse() {
		let input = shared::read_test_input(DAY);
		assert!(parse_input(input).is_some());
    }

	#[test]
    fn test_part_a() {
		let input = shared::read_test_input(DAY);
		let (regs, program) = parse_input(input).unwrap();
		assert_eq!([4,6,3,5,6,3,5,2,1,0].to_vec(), exec(&program, regs));
    }

	#[test]
    fn test_part_a_1() {
		let input = shared::read_test_input_n(DAY, 1);
		let (regs, program) = parse_input(input).unwrap();
		assert_eq!([4,2,5,6,7,7,7,7,3,1,0].to_vec(), exec(&program, regs));
    }
}
