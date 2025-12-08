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

func read_in() [][]byte {
	path := "../inputs/07.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	rows := strings.Split(str, "\r\n")

	grid := make([][]byte, len(rows))
	for i := range rows {
		grid[i] = []byte(rows[i])
	}
	return grid
}

//------------------------------------------------------------------------------
// vv part a vv

func part_a(grid [][]byte) {
	state := make([]bool, len(grid[0]))

	for x := range grid[0] {
		if grid[0][x] == 'S' {
			state[x] = true
			break
		}
	}

	splits := 0
	for y := range grid {
		for x := range grid[y] {
			if state[x] && grid[y][x] == '^' {
				state[x-1] = true
				state[x+1] = true
				state[x] = false
				splits++
			}
		}
	}

	fmt.Println("Part A:", splits)
}

//------------------------------------------------------------------------------
// vv part b vv

func part_b(grid [][]byte) {
	state := make([]uint64, len(grid[0]))

	for x := range grid[0] {
		if grid[0][x] == 'S' {
			state[x] = 1
			break
		}
	}

	for y := range grid {
		for x := range grid[y] {
			if state[x] > 0 && grid[y][x] == '^' {
				state[x-1] = state[x-1] + state[x]
				state[x+1] = state[x+1] + state[x]
				state[x] = 0
			}
		}
	}

	total_timelines := uint64(0)
	for _, val := range state {
		total_timelines += val
	}

	fmt.Println("Part B:", total_timelines)
}

//------------------------------------------------------------------------------

func main() {
	start := time.Now()

	grid := read_in()
	part_a(grid)
	part_b(grid)

	elapsed := time.Since(start)
	fmt.Printf("Runtime %s", elapsed)
}
