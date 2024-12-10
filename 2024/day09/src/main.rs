use std::cmp::min;
extern crate shared;

const DAY : i32 = 9;
type Mem = Vec<(i32, i32)>;

fn main() {
	shared::bench (run);
}

fn run () {
	let content = shared::read_input(DAY);
	let memory = parse_input(content);
	if shared::is_part_a() {
		let arranged = arrange_memory(memory);
		println!("{:?}", generate_checksum(arranged));
	} else {
		let arranged = arrange_memory_nofrag(memory);
		println!("{:?}", generate_checksum(arranged));
	}
}

fn generate_checksum(memory : Mem) -> u64 {
	let mut index = 0;
	let mut checksum = 0;
	for (iden, mut size) in memory {
		if iden == -1 {
			index += size;
		} else {
			while (size > 0) && (iden >= 0) {
				checksum += (iden * index) as u64;
				index += 1;
				size -= 1;
			}
		}
	}
	checksum
}

// =============================================================================
// vv part a

fn fill_free(memory : &mut Mem, reallocator : &mut usize, index : usize, size : i32) -> Mem {
	let mut left = size as i32;
	let mut new_mem = vec!();
	while (left > 0) && (*reallocator > index) {
		let (iden, bsiz) = memory.get_mut(*reallocator).unwrap();

		// Vrije blokken hoeven we niet te reallocaten
		if *iden < 0 {
			*reallocator -= 1;
			continue;
		}

		new_mem.push((*iden, min(left, *bsiz)));

		left -= *bsiz;
		if left < 0 {
			*bsiz = left.abs(); // nog over, blok verkleinen
		} else {
			// blok leeg, opruimen
			*bsiz = 0;
			*iden = -1;
			*reallocator -= 1;
		}
	}
	new_mem
}

fn arrange_memory(mut memory : Mem) -> Mem {
	let mut new_mem = vec!();
	let mut reallocator = memory.len() - 1;

	for indx in 0..memory.len() {
		let (iden, size) = memory[indx];
		match iden {
			-1 => new_mem.extend(fill_free(&mut memory, &mut reallocator, indx, size)),
			_  => new_mem.push((iden, size)),
		}
	}
	new_mem
}

// =============================================================================
// vv part b

fn mem_remove(memory : &mut Mem, iden : i32) {
	let index = memory.iter().position(|(i,_)| *i == iden).unwrap();
	memory[index].0 = -1;
}

fn mem_find(memory : &Mem, iden : i32) -> usize {
	memory.iter()
		.position(|(i,_)| (*i == iden))
		.unwrap()
}

fn mem_check_free(memory : &Mem, iden : i32, size : i32) -> Option<usize> {
	let free = memory.iter()
		.position(|(i,s)| (*i == -1) && (*s >= size))?;
	if free < mem_find(memory, iden) {
		Some(free) // Positie is voor huidige positie van file
	} else {
		None // Positie na, niet verplaatsen
	}
}

fn mem_insert(memory : &mut Mem, index : usize, iden : i32, size : i32) {
	let (bidn, bsiz) = memory.get_mut(index).unwrap();
	*bidn = iden;
	if *bsiz != size {
		let rem = *bsiz - size;
		*bsiz = size;
		memory.insert(index+1, (-1, rem));
	}
}

fn arrange_memory_nofrag(memory : Mem) -> Mem {
	let mut new_mem = memory.clone();
	for (iden, size) in memory.into_iter()
							.filter(|(i,_)| *i != -1)
							.rev()
	{
		if let Some(index) = mem_check_free(&new_mem, iden, size) {
			mem_remove(&mut new_mem, iden);
			mem_insert(&mut new_mem, index, iden, size);
		}
	}
	new_mem
}

// =============================================================================
// vv parse

fn parse_input(content : String) -> Mem {
	let mut memory = vec!();
	let mut iden = 0;
	let mut block_mode = true;
	for n in content.chars().map(|x| x.to_digit(10).unwrap()) {
		if block_mode {
			memory.push((iden, n as i32));
			iden += 1;
		} else if n > 0 {
			memory.push((-1, n as i32));
		}
		block_mode = !block_mode;
	}
	memory
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
		let content = shared::read_test_input_n(DAY, 0);
		let memory = parse_input(content);
		assert_eq!(memory[0].1, 1);
		assert_eq!(memory[2].1, 3);
		assert_eq!(memory[4].1, 5);
    }

	#[test]
    fn test_fill() {
		let mut mem = vec!((-1, 5), (0, 5));
		let mut reallocator = 1;
		let new_mem = fill_free(&mut mem, &mut reallocator, 0, 5);

		println!("Memory: {:?}", mem);
		println!("New memory: {:?}", new_mem);

		assert_eq!(reallocator, 0);
		assert_eq!(mem[1].0, -1, "Block ID should cleared");
		assert_eq!(mem[1].1, 0, "Block size should be emptied");
		assert_eq!(new_mem[0].0, 0);
		assert_eq!(new_mem[0].1, 5);
    }

	#[test]
    fn test_arrange() {
		let memory = parse_input(shared::read_test_input_n(DAY, 1));
		let arranged = arrange_memory(memory);
		println!("{arranged:?}");
		assert!(arranged.iter().all(|(iden,_)| *iden != -1));
		assert_eq!(1928, generate_checksum(arranged));
	}

	#[test]
    fn test_arrange_no_frag() {
		let memory = parse_input(shared::read_test_input_n(DAY, 1));
		let arranged = arrange_memory_nofrag(memory);
		println!("{arranged:?}");
		assert_eq!(2858, generate_checksum(arranged));
	}
}