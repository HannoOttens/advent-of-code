extern crate shared;

const DAY : i32 = 3;

#[derive(Debug)]
enum Lang {
	Mul(i32, i32),
	Do,
	Dont
}

#[derive(Debug)]
#[derive(PartialEq)]
#[derive(Clone)]
enum Token {
	Num (i32),
	Do,
	Dont,
	Mul,
	BracketOpen,
	BracketClose,
	Comma,
	Corrupted
}

fn main() {
	let content = shared::read_input(DAY);
	let tokens = tokenize(&content);
	let statements = parse(tokens);
	let (totl, _) = statements.into_iter().fold((0, true), eval);
	println!("{totl}");
}

// =============================================================================
// vv evaluator vv

fn eval(state : (i32, bool), statement : Lang) -> (i32, bool) {
	let (curr, mut enab) = state;
	enab = enab || shared::is_part_a();

	match statement {
		Lang::Mul(x, y) => if enab { (curr + x * y, enab) } else {(curr, enab)},
		Lang::Do        => (curr, true),
		Lang::Dont      => (curr, false),
	}
}

// =============================================================================
// vv parser vv

fn parse(tokens : Vec<Token>) -> Vec<Lang> {
	let mut i = 0;
	let mut statements = Vec::new();
	while i < tokens.len() {
		match &tokens[i..] {
			[
				Token::Mul,
				Token::BracketOpen,
				Token::Num(x),
				Token::Comma,
				Token::Num(y),
				Token::BracketClose,
				..
			] => {
				statements.push(Lang::Mul(*x, *y));
				i += 6;
			},
			[
				Token::Do,
				Token::BracketOpen,
				Token::BracketClose,
				..
			] => {
				statements.push(Lang::Do);
				i += 3;
			},
			[
				Token::Dont,
				Token::BracketOpen,
				Token::BracketClose,
				..
			] => {
				statements.push(Lang::Dont);
				i += 3;
			},
			_ => i += 1
		}
	}
	statements
}

// =============================================================================
// vv tokenizer vv

fn tokenize (input : &str) -> Vec<Token> {
	let mut i : usize = 0;
	let mut tokens = Vec::new();
	let chars : Vec<char> = input.chars().collect();
	while i < chars.len() {
		let tok;
		match chars[i] {
			'0' ..= '9' => tok = Token::Num (tokenize_number(&chars, &mut i)),
			'('         => { i += 1; tok = Token::BracketOpen },
			','         => { i += 1; tok = Token::Comma },
			')'         => { i += 1; tok = Token::BracketClose },
			_           => tok = tokenize_iden(&chars, &mut i),
		}
		tokens.push(tok);
	}
	tokens
}

fn tokenize_number(chars : &Vec<char>, index : &mut usize) -> i32 {
	let mut len = 0;
	let mut str = String::new();
	while (*index < chars.len()) && (len < 3) && (chars[*index].is_numeric()) {
		str.push(chars[*index]);
		len += 1;
		*index += 1;
	}
	str.parse().unwrap()
}

fn tokenize_iden(chars : &Vec<char>, index : &mut usize) -> Token {
	match &chars[*index..] {
		['m', 'u', 'l', ..] => {
			*index += 3;
			Token::Mul
		},
		['d', 'o', 'n', '\'', 't', ..] => {
			*index += 5;
			Token::Dont
		},
		['d', 'o', ..] => {
			*index += 2;
			Token::Do
		},
		_ => {
			*index += 1;
			Token::Corrupted
		}
	}
}

// =============================================================================
// vv tests vv

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn input_exists() {
		shared::read_input(DAY);
    }


    #[test]
	fn test_numb () {
		let tokens = tokenize(&"24");
		assert_eq!(tokens, [Token::Num(24)].to_vec());
	}

    #[test]
	fn test_tokenize_expr () {
		let tokens = tokenize(&"mul(24,154)");
		assert_eq!(tokens, [Token::Mul, Token::BracketOpen, Token::Num(24), Token::Comma, Token::Num(154), Token::BracketClose].to_vec());
	}
}