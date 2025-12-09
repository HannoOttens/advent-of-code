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

type Posn struct {
	x int64
	y int64
}

//------------------------------------------------------------------------------
// vv parsing vv

func read_in() []Posn {
	path := "../inputs/09.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	rows := strings.Split(str, "\r\n")

	positions := make([]Posn, len(rows))
	for i := range rows {
		ints := strings.Split(rows[i], ",")
		x, _ := strconv.ParseInt(ints[0], 0, 32)
		y, _ := strconv.ParseInt(ints[1], 0, 32)
		positions[i] = Posn{
			x: x,
			y: y,
		}
	}
	return positions
}

//------------------------------------------------------------------------------
// vv part a vv

func abs(val int64) int64 {
	if val < 0 {
		return -val
	}
	return val
}

func size(a Posn, b Posn) int64 {
	dx := abs(a.x-b.x) + 1
	dy := abs(a.y-b.y) + 1
	size := dx * dy
	if size < 0 {
		size = -size
	}
	return size
}

func part_a(positions []Posn) {
	max_size := int64(0)
	for i_a, pos_a := range positions {
		for _, pos_b := range positions[i_a:] {
			size := size(pos_a, pos_b)
			if size > max_size {
				max_size = size
			}
		}
	}

	fmt.Println("Part A:", max_size)
}

//------------------------------------------------------------------------------
// vv part b vv

func next(positions []Posn, index int) Posn {
	if index+1 == len(positions) {
		return positions[0]
	}
	return positions[index+1]
}

func min(a, b int64) int64 {
	if a < b {
		return a
	}
	return b
}

func max(a, b int64) int64 {
	if a > b {
		return a
	}
	return b
}

func intersect(box_uppr, box_lowl, vec_a, vec_b Posn) bool {
	// Zorgt dat we rechterbovenhoek en linker onder hoek van box hebben
	if box_uppr.x > box_lowl.x {
		box_lowl.x, box_uppr.x = box_uppr.x, box_lowl.x
	}
	if box_uppr.y > box_lowl.y {
		box_lowl.y, box_uppr.y = box_uppr.y, box_lowl.y
	}

	// Kijk of er een intersection plaatsvind
	if vec_a.x == vec_b.x {
		x_range := vec_a.x > box_uppr.x && vec_a.x < box_lowl.x
		y_range := !(max(vec_a.y, vec_b.y) <= box_uppr.y || min(vec_a.y, vec_b.y) >= box_lowl.y)
		return x_range && y_range
	} else {
		y_range := vec_a.y > box_uppr.y && vec_a.y < box_lowl.y
		x_range := !(max(vec_a.x, vec_b.x) <= box_uppr.x || min(vec_a.x, vec_b.x) >= box_lowl.x)
		return x_range && y_range
	}
}

func part_b(positions []Posn) {
	max_size := int64(0)
	for i_a, pos_a := range positions {
		for _, pos_b := range positions[i_a+1:] {
			valid := true
			for i_c, pos_c := range positions {
				if pos_a != pos_c && pos_b != pos_c {
					next := next(positions, i_c)
					valid = valid && !intersect(pos_a, pos_b, pos_c, next)
				}
			}

			size := size(pos_a, pos_b)
			if valid && (size > max_size) {
				max_size = size
			}
		}
	}

	fmt.Println("Part B:", max_size)
}

//------------------------------------------------------------------------------

func main() {
	start := time.Now()
	positions := read_in()
	elapsed := time.Since(start)
	fmt.Printf("Runtime - Parsing: %s\n", elapsed)

	start = time.Now()
	part_a(positions)
	elapsed = time.Since(start)
	fmt.Printf("Runtime - Part A: %s\n", elapsed)

	start = time.Now()
	part_b(positions)
	elapsed = time.Since(start)
	fmt.Printf("Runtime - Part B: %s\n", elapsed)
}
