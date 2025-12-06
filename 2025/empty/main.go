package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
	"time"
)

func check(e error) {
	if e != nil {
		panic(e)
	}
}

type Sum struct {
	op   byte
	vals []int64
}

//------------------------------------------------------------------------------
// vv parsing vv

func read_column(rows []string, column int) int64 {
	str := string(rows[0][column])
	str += string(rows[1][column])
	str += string(rows[2][column])
	str += string(rows[3][column])
	val, err := strconv.ParseInt(strings.TrimSpace(str), 0, 32)
	check(err)
	return val
}

func parse_sum(rows []string, p, i int, part_b bool) Sum {
	if part_b {
		vals := make([]int64, 0)
		for n := p; n < i; n++ {
			vals = append(vals, read_column(rows, n))
		}
		return Sum{
			op:   rows[4][p],
			vals: vals,
		}
	} else {
		x, _ := strconv.ParseInt(strings.TrimSpace(rows[0][p:i]), 0, 32)
		y, _ := strconv.ParseInt(strings.TrimSpace(rows[1][p:i]), 0, 32)
		z, _ := strconv.ParseInt(strings.TrimSpace(rows[2][p:i]), 0, 32)
		w, _ := strconv.ParseInt(strings.TrimSpace(rows[3][p:i]), 0, 32)
		return Sum{
			op:   rows[4][p],
			vals: []int64{x, y, z, w},
		}
	}
}

func read_in(part_b bool) []Sum {
	path := "../inputs/06.txt"
	dat, err := os.ReadFile(path)
	str := string(dat)
	rows := strings.Split(str, "\r\n")
	check(err)

	sums := make([]Sum, 0)
	prev := 0
	for i := range rows[4][1:] {
		if rows[4][i+1] == ' ' {
			continue
		}

		sums = append(sums, parse_sum(rows, prev, i, part_b))
		prev = i + 1
	}
	sums = append(sums, parse_sum(rows, prev, len(rows[0]), part_b))

	return sums
}

//------------------------------------------------------------------------------

func calculate(sums []Sum) int64 {
	total := int64(0)
	for _, sum := range sums {
		switch sum.op {
		case '+':
			for _, val := range sum.vals {
				total += val
			}
		case '*':
			new := int64(1)
			for _, val := range sum.vals {
				new *= val
			}
			total += new
		}
	}
	return total
}

func main() {
	start := time.Now()

	sums := read_in(false)
	fmt.Println("Part B:", calculate(sums))
	sums_b := read_in(true)
	fmt.Println("Part B:", calculate(sums_b))

	elapsed := time.Since(start)
	fmt.Printf("Runtime %s", elapsed)
}
