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
// vv parsing vv

func read_in() {

}

//------------------------------------------------------------------------------
// vv part a vv

func part_a(rows []string) {
	fmt.Println("Part A:", 0)
}

//------------------------------------------------------------------------------
// vv part b vv

func part_b(rows []string) {
	fmt.Println("Part B:", 0)
}

//------------------------------------------------------------------------------

func main() {
	start := time.Now()

	path := "../inputs/05.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	rows := strings.Split(str, "\r\n")

	part_a(rows)
	part_b(rows)

	elapsed := time.Since(start)
	fmt.Printf("Runtime %s", elapsed)
}
