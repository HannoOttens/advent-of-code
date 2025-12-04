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

func pick(grid [][]byte, x, y int) byte {
	if y < 0 || x < 0 {
		return '.'
	}
	if y >= len(grid) || x >= len(grid[y]) {
		return '.'
	}
	return grid[y][x]
}

func count_adjecent(grid [][]byte, x, y int) int {
	rolls_of_paper := 0
	for dy := -1; dy <= 1; dy++ {
		for dx := -1; dx <= 1; dx++ {
			if (dx == 0) && (dy == 0) {
				continue
			}

			if pick(grid, x+dx, y+dy) == '@' {
				rolls_of_paper++
			}
		}
	}
	return rolls_of_paper
}

func part_a(grid [][]byte) {
	forkable_rolls_of_paper := 0
	for y := range grid {
		for x := range grid[y] {
			if (pick(grid, x, y) == '@') && (count_adjecent(grid, x, y) < 4) {
				forkable_rolls_of_paper++
			}
		}
	}

	fmt.Println("Part A:", forkable_rolls_of_paper)
}

//------------------------------------------------------------------------------
// vv part b vv

type Posn struct {
	x int
	y int
}

func part_b(grid [][]byte) {
	forked_rolls := 0

	fork_any := true
	for fork_any {
		forkable := map[Posn]bool{}

		for y := range grid {
			for x := range grid[y] {
				if (pick(grid, x, y) == '@') && (count_adjecent(grid, x, y) < 4) {
					forkable[Posn{x: x, y: y}] = true
				}
			}
		}

		for posn := range forkable {
			forked_rolls++
			grid[posn.y][posn.x] = '.'
		}

		fork_any = len(forkable) > 0
	}

	fmt.Println("Part B:", forked_rolls)
}

//------------------------------------------------------------------------------

func main() {
	start := time.Now()

	path := "../inputs/04.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	rows := strings.Split(str, "\r\n")

	grid := make([][]byte, len(rows))
	for i := range rows {
		grid[i] = []byte(rows[i])
	}

	part_a(grid)
	part_b(grid)

	elapsed := time.Since(start)
	fmt.Printf("Runtime %s", elapsed)
}
