package main

import (
	"fmt"
	"time"
)

func check(e error) {
	if e != nil {
		panic(e)
	}
}

//------------------------------------------------------------------------------
// vv parsing vv

func read_in(part_b bool) []TYPE {
}

//------------------------------------------------------------------------------
// vv part a vv

func part_a(rows []TYPE) {
	fmt.Println("Part A:", 0)
}

//------------------------------------------------------------------------------
// vv part b vv

func part_b(rows []TYPE) {
	fmt.Println("Part B:", 0)
}

//------------------------------------------------------------------------------

func main() {
	start := time.Now()

	rows := read_in(false)
	part_a(rows)
	part_b(rows)

	elapsed := time.Since(start)
	fmt.Printf("Runtime %s", elapsed)
}
