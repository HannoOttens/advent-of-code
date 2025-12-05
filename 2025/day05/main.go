package main

import (
	"cmp"
	"fmt"
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

type Range struct {
	Min int64
	Max int64
}

//------------------------------------------------------------------------------
// vv parsing vv

func parse_range(rnge string) Range {
	parts := strings.Split(rnge, "-")
	min, err := strconv.ParseInt(parts[0], 0, 64)
	check(err)
	max, err := strconv.ParseInt(parts[1], 0, 64)
	check(err)
	return Range{
		Min: min,
		Max: max,
	}
}

func read_in() ([]Range, []int64) {
	path := "../inputs/05.txt"
	dat, err := os.ReadFile(path)
	check(err)

	str := string(dat)
	data := strings.Split(str, "\r\n\r\n")

	// Ranges
	ranges_str := strings.Split(data[0], "\r\n")
	ranges := make([]Range, len(ranges_str))
	for i := range ranges {
		ranges[i] = parse_range(ranges_str[i])
	}
	slices.SortFunc(ranges,
		func(a, b Range) int {
			return cmp.Compare(a.Min, b.Min)
		})

	// Ids
	ids_str := strings.Split(data[1], "\r\n")
	ids := make([]int64, len(ids_str))
	for i := range ids {
		val, err := strconv.ParseInt(ids_str[i], 0, 64)
		check(err)
		ids[i] = val
	}
	slices.Sort(ids)

	return ranges, ids
}

//------------------------------------------------------------------------------
// vv part a vv

func part_a(ranges []Range, ids []int64) {
	rnge_idx := 0

	fresh_ingredients := 0
	for _, id := range ids {
		for rnge_idx < len(ranges) && id > ranges[rnge_idx].Max {
			rnge_idx++
		}

		if rnge_idx == len(ranges) {
			break
		}

		if id >= ranges[rnge_idx].Min && id <= ranges[rnge_idx].Max {
			fresh_ingredients++
		}
	}

	fmt.Println("Part A:", fresh_ingredients)
}

//------------------------------------------------------------------------------
// vv part b vv

func part_b(ranges []Range) {
	combined_ranges := make([]Range, 1)
	combined_ranges[0] = ranges[0]

	for _, rnge := range ranges[1:] {
		end := len(combined_ranges) - 1
		if rnge.Min <= combined_ranges[end].Max {
			combined_ranges[end].Max = max(rnge.Max, combined_ranges[end].Max)
		} else {
			combined_ranges = append(combined_ranges, rnge)
		}
	}

	total_count := uint64(0)
	for _, rnge := range combined_ranges {
		total_count += uint64(rnge.Max - rnge.Min + 1)
	}
	fmt.Println("Part B:", total_count)
}

//------------------------------------------------------------------------------

func main() {
	start := time.Now()

	ranges, ids := read_in()

	part_a(ranges, ids)
	part_b(ranges)

	elapsed := time.Since(start)
	fmt.Printf("Runtime %s", elapsed)
}
