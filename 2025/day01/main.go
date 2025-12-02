package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func check(e error) {
	if e != nil {
		panic(e)
	}
}

func abs(x int) int {
	if x >= 0 {
		return x
	}
	return -x
}

func main() {
	path := "../inputs/01.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	lines := strings.Split(str, "\r\n")

	pos := 50
	on_zero := 0
	pass_zero := 0
	for _, text := range lines {
		c := text[0]
		num, err := strconv.ParseInt(text[1:], 0, 32)
		inum := int(num)
		check(err)

		pass_zero += inum / 100
		inum %= 100

		switch c {
		case 'L':
			pos -= inum
		case 'R':
			pos += inum
		}

		if pos == 0 {
			on_zero++
			pass_zero++
		} else if pos < 0 {
			if abs(pos) != inum {
				pass_zero++
			}
			pos += 100
		} else if pos >= 100 {
			pass_zero++
			pos -= 100
		}
	}

	fmt.Println("The dial was on zero this many times: ", on_zero)
	fmt.Println("The dial passed zero this many times: ", pass_zero)
}
