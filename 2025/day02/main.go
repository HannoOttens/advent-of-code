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

func parseInt(s string) int64 {
	rslt, err := strconv.ParseInt(s, 0, 64)
	check(err)
	return rslt
}

func pow(x int64, e int64) int64 {
	var res int64 = 1
	for range e {
		res *= x
	}
	return res
}

func digits(x int64) int64 {
	i := int64(10)
	digits := int64(1)
	for i < x {
		digits++
		i *= 10
	}
	return digits
}

func drop_last_n(n int64, x int64) int64 {
	return x / pow(10, n)
}

func part_a(numbers []string) {
	total_doubles := int64(0)
	for _, numb := range numbers {
		rnge := strings.Split(numb, "-")
		low := parseInt(rnge[0])
		high := parseInt(rnge[1])
		n_low := digits(low)
		n_high := digits(high)

		if n_low != n_high {
			// feit uit input: max 1 digit verschil
			// gewoon even pakken (enige met resultaten)
			if n_low%2 == 0 {
				n_high = n_low
			} else {
				n_low = n_high
			}
		}

		if n_low%2 == 0 {
			range_low := drop_last_n(n_low/2, low)
			range_high := drop_last_n(n_low/2, high)

			for i := range_low; i <= range_high; i++ {
				dble := i*pow(10, n_low/2) + i
				if (dble >= low) && (dble <= high) && (digits(dble)%2 == 0) {
					total_doubles += dble
				}
			}
		}
	}

	fmt.Println("Part A:", total_doubles)
}

func process(low, high, n_digits int64) int64 {
	total_doubles := int64(0)
	for i := range 1 + (n_digits / 2) {
		if i > 0 && n_digits%i == 0 {
			range_low := drop_last_n(n_digits-i, low)
			range_high := drop_last_n(n_digits-i, high)

			for curr := range_low; curr <= range_high; curr++ {
				if process(curr, curr, i) == 0 {
					dble := curr
					for range n_digits/i - 1 {
						dble = curr + dble*pow(10, i)
					}
					if (dble >= low) && (dble <= high) {
						total_doubles += dble
					}
				}
			}
		}
	}
	return total_doubles
}

func part_b(numbers []string) {
	total_doubles := int64(0)
	for _, numb := range numbers {
		rnge := strings.Split(numb, "-")
		low := parseInt(rnge[0])
		high := parseInt(rnge[1])
		n_low := digits(low)
		n_high := digits(high)

		if n_low != n_high {
			total_doubles += process(pow(10, n_high-1), high, n_high)
			total_doubles += process(low, pow(10, n_low)-1, n_low)
		} else {
			total_doubles += process(low, high, n_high)
		}
	}

	fmt.Println("Part B:", total_doubles)
}

func main() {
	path := "../inputs/02.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	numbers := strings.Split(str, ",")
	part_a(numbers)
	part_b(numbers)
}
