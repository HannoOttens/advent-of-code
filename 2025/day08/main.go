package main

import (
	"cmp"
	"fmt"
	"math"
	"os"
	"slices"
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
	x int
	y int
	z int
}

type Dist struct {
	p1   int
	p2   int
	dist float64
}

//------------------------------------------------------------------------------
// vv parsing vv

func read_in() []Posn {
	path := "../inputs/08.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	rows := strings.Split(str, "\r\n")

	positions := make([]Posn, len(rows))
	for i := range rows {
		ints := strings.Split(rows[i], ",")
		x, _ := strconv.ParseInt(ints[0], 0, 32)
		y, _ := strconv.ParseInt(ints[1], 0, 32)
		z, _ := strconv.ParseInt(ints[2], 0, 32)
		positions[i] = Posn{
			x: int(x),
			y: int(y),
			z: int(z),
		}
	}
	return positions
}

//------------------------------------------------------------------------------
// vv part a vv

func distance(a Posn, b Posn) float64 {
	dx := float64(a.x - b.x)
	dy := float64(a.y - b.y)
	dz := float64(a.z - b.z)
	return math.Sqrt(dx*dx + dy*dy + dz*dz)
}

func calc_dinstances(positions []Posn) []Dist {
	pairs := len(positions) * (len(positions) - 1) / 2
	distances := make([]Dist, 0, pairs)
	for i1, p1 := range positions {
		for i2, p2 := range positions {
			if i2 <= i1 {
				continue
			}

			dist := distance(p1, p2)
			distances = append(distances, Dist{
				p1:   i1,
				p2:   i2,
				dist: dist,
			})
		}
	}
	slices.SortFunc(distances, func(a, b Dist) int {
		return cmp.Compare(a.dist, b.dist)
	})
	return distances
}

func part_a(positions []Posn) {
	// Find distances and sort by
	distances := calc_dinstances(positions)

	// Find circuits by joining the first 1000 closest pairs
	circuits := make([]int, len(positions))
	new_ciruit := 1
	for _, dist := range distances[0:1000] {
		if circuits[dist.p1] == 0 && circuits[dist.p2] == 0 {
			circuits[dist.p1] = new_ciruit
			circuits[dist.p2] = new_ciruit
			new_ciruit++
		} else if circuits[dist.p2] == 0 {
			circuits[dist.p2] = circuits[dist.p1]
		} else if circuits[dist.p1] == 0 {
			circuits[dist.p1] = circuits[dist.p2]
		} else if circuits[dist.p1] != circuits[dist.p2] {
			replace := circuits[dist.p2]
			for i := range circuits {
				if circuits[i] == replace {
					circuits[i] = circuits[dist.p1]
				}
			}
		}
	}

	// Count circuits size
	circuit_sizes := make([]int, 0)
	for current := range new_ciruit {
		circuit_size := 0
		for _, circuit := range circuits {
			if circuit == current+1 {
				circuit_size++
			}
		}

		if circuit_size > 0 {
			circuit_sizes = append(circuit_sizes, circuit_size)
		}
	}
	slices.Sort(circuit_sizes)
	slices.Reverse(circuit_sizes)

	// Multiply largest 3
	answer := 1
	for _, size := range circuit_sizes[:3] {
		answer *= size
	}

	fmt.Println("Part A:", answer)
}

//------------------------------------------------------------------------------
// vv part b vv

func all_equal(circuits []int) bool {
	frst := circuits[0]
	for _, x := range circuits[1:] {
		if x != frst {
			return false
		}
	}
	return true
}

func part_b(positions []Posn) {
	// Find distances and sort by
	distances := calc_dinstances(positions)

	// Find circuits until were done
	circuits := make([]int, len(positions))
	new_ciruit := 1

	last_dist := Dist{}
	for _, dist := range distances {
		if circuits[dist.p1] == 0 && circuits[dist.p2] == 0 {
			circuits[dist.p1] = new_ciruit
			circuits[dist.p2] = new_ciruit
			new_ciruit++
		} else if circuits[dist.p2] == 0 {
			circuits[dist.p2] = circuits[dist.p1]
		} else if circuits[dist.p1] == 0 {
			circuits[dist.p1] = circuits[dist.p2]
		} else if circuits[dist.p1] != circuits[dist.p2] {
			replace := circuits[dist.p2]
			for i := range circuits {
				if circuits[i] == replace {
					circuits[i] = circuits[dist.p1]
				}
			}
		}

		if all_equal(circuits) {
			last_dist = dist
			break
		}
	}

	fmt.Println("Part B:", positions[last_dist.p1].x*positions[last_dist.p2].x)
}

//------------------------------------------------------------------------------

func main() {

	positions := read_in()
	start := time.Now()
	part_a(positions)
	elapsed := time.Since(start)
	fmt.Printf("Runtime - Part A: %s\n", elapsed)

	start = time.Now()
	part_b(positions)
	elapsed = time.Since(start)
	fmt.Printf("Runtime - Part B: %s\n", elapsed)
}
