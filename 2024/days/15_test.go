package days

import (
	"fmt"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay15() (map[Point]string, []Point, int, int) {
	warehouse := make(map[Point]string)
	moves := make([]Point, 0)
	maxx := 0
	maxy := 0

	lines := ReadLines("../inputs/15b.txt")
	for y, line := range lines {

		if len(line) == 0 {
			continue
		}

		if line[0] == '#' {
			for x := range line {
				warehouse[Point{X: x, Y: y}] = string(line[x])
			}
			maxy = y + 1
			maxx = len(line)
		}

		if line[0] == '<' ||
			line[0] == '>' ||
			line[0] == 'v' ||
			line[0] == '^' {

			for i := range len(line) {
				if line[i] == '<' {
					moves = append(moves, Point{X: -1, Y: 0})
				}
				if line[i] == '>' {
					moves = append(moves, Point{X: 1, Y: 0})
				}
				if line[i] == 'v' {
					moves = append(moves, Point{X: 0, Y: 1})
				}
				if line[i] == '^' {
					moves = append(moves, Point{X: 0, Y: -1})
				}
			}
		}
	}

	return warehouse, moves, maxx, maxy
}

func printWarehouse(warehouse map[Point]string, maxx int, maxy int) {

	for y := range maxy {
		for x := range maxx {
			fmt.Printf("%v", warehouse[Point{X: x, Y: y}])
		}
		fmt.Println()
	}
	fmt.Println()
}

func calcMoves(warehouse map[Point]string, moves []Point, maxx int, maxy int) int {
	var pos Point

	for y := range maxy {
		for x := range maxx {
			p := Point{X: x, Y: y}
			if warehouse[p] == "@" {
				pos = p
				break
			}
		}
	}

	for _, m := range moves {
		next := pos.Add(m)

		if warehouse[next] == "." {
			warehouse[pos] = "."
			warehouse[next] = "@"
			pos = next
		}

		if warehouse[next] == "O" {
			tmp := next
			queue := make([]Point, 0)
			cont := true
			skip := false
			for cont {
				switch warehouse[tmp] {
				case "#":
					cont = false
					skip = true
				case ".":
					cont = false
					queue = append(queue, tmp)
				default:
					queue = append(queue, tmp)
					tmp = tmp.Add(m)
				}
			}
			if !skip {
				for _, q := range queue {
					warehouse[q] = "O"
				}
				warehouse[next] = "@"
				warehouse[pos] = "."
				pos = next
			}
		}

		//fmt.Println()
		//printWarehouse(warehouse, maxx, maxy)
	}
	//fmt.Println()
	//printWarehouse(warehouse, maxx, maxy)

	return calc(warehouse, maxx, maxy)
}

func calc(warehouse map[Point]string, maxx int, maxy int) int {
	result := 0
	for y := range maxy {
		for x := range maxx {
			if warehouse[Point{X: x, Y: y}] == "O" {
				result += y*100 + x
			}
		}
	}

	return result
}

func calcMoves2(warehouse map[Point]string, moves []Point, maxx int, maxy int) int {

	var pos Point
	extended := make(map[Point]string)

	for y := range maxy {
		for x := range maxx {
			p := Point{X: x, Y: y}
			newP := Point{X: 2 * x, Y: y}
			newP2 := Point{X: 2*x + 1, Y: y}
			if warehouse[p] == "@" {
				pos = newP
				extended[newP] = "@"
				extended[newP2] = "."
			}
			if warehouse[p] == "." {
				extended[newP] = "."
				extended[newP2] = "."
			}
			if warehouse[p] == "O" {
				extended[newP] = "["
				extended[newP2] = "]"
			}
			if warehouse[p] == "#" {
				extended[newP] = "#"
				extended[newP2] = "#"
			}
		}
	}

	printWarehouse(extended, 2*maxx, maxy)
	idx := 0

	for _, m := range moves {
		if idx > 0 {
			continue
		}
		idx++

		fmt.Printf("Moving %v\n", m)
		next := pos.Add(m)

		if extended[next] == "." {
			extended[pos] = "."
			extended[next] = "@"
			pos = next
		}

		p2m := make(map[Point]bool)
        i := 0
		if extended[next] == "[" || extended[next] == "]" {
			tmp := next
			for i < len(p2m) {
                p2m[tmp] = true
				if extended[tmp] == "[" {
					p2m[tmp.Add(Point{X: tmp.X + 1, Y: tmp.Y})] = true
				}
				if extended[tmp] == "]" {
					p2m[tmp.Add(Point{X: tmp.X - 1, Y: tmp.Y})] = true
				}
				tmp = tmp.Add(m)
                i++
			}
		}

		fmt.Println()
		printWarehouse(extended, 2*maxx, maxy)
	}
	//fmt.Println()
	//printWarehouse(warehouse, maxx, maxy)

	return calc(warehouse, maxx, maxy)
}

func Day_15(t *testing.T) {
	warehouse, moves, maxx, maxy := getDataDay15()
	//result1 := calcMoves(warehouse, moves, maxx, maxy)
	result2 := calcMoves2(warehouse, moves, maxx, maxy)

	//assert.EqualValues(t, 1552463, result1)
	//assert.EqualValues(t, 10092, result1)
	assert.EqualValues(t, 9021, result2)
}
