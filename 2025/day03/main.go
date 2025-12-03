package main

import (
	"fmt"
	"os"
	"strings"
	"time"
)

func check(e error) {
	if e != nil {
		panic(e)
	}
}

//------------------------------------------------------------------------------
// vv part a vv

func part_a(banks []string) {
	joltage := 0
	for _, bank := range banks {

		i1 := len(bank) - 2
		i2 := len(bank) - 1

		for scan := len(bank) - 3; scan >= 0; scan-- {
			if bank[scan] >= bank[i1] {
				if bank[i2] < bank[i1] {
					i2 = i1
				}
				i1 = scan
			}
		}

		jolt := int(10*(bank[i1]-'0') + (bank[i2] - '0'))
		joltage += jolt
	}

	fmt.Println("Part A:", joltage)
}

//------------------------------------------------------------------------------
// vv part b vv

const max int = 12

func shift(bank string, on *[max]int, i int) {
	if i == len(on)-1 {
		return
	}

	if bank[on[i+1]] <= bank[on[i]] {
		shift(bank, on, i+1)
		on[i+1] = on[i]
	}
}

func part_b(banks []string) {
	joltage := 0
	for _, bank := range banks {

		on := [max]int{}
		for i := range on {
			on[i] = len(bank) - max + i
		}

		for scan := len(bank) - max - 1; scan >= 0; scan-- {
			if bank[scan] >= bank[on[0]] {
				shift(bank, &on, 0)
				on[0] = scan
			}
		}

		jolt := 0
		for i := range on {
			jolt = 10*jolt + int(bank[on[i]]-'0')
		}
		joltage += jolt
	}

	fmt.Println("Part B:", joltage)
}

//------------------------------------------------------------------------------

func main() {
	start := time.Now()

	path := "../inputs/03.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	banks := strings.Split(str, "\r\n")

	part_a(banks)
	part_b(banks)

	elapsed := time.Since(start)
	fmt.Printf("Runtime %s", elapsed)
}
