package days

import (
	"fmt"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay12() map[Point]Plot {

	lines := ReadLines("../inputs/12.txt")
	data := make(map[Point]Plot)

	for y := range len(lines) {
		for x := range len(lines[0]) {
			p := Point{X: x, Y: y}
			data[p] = Plot{Point: p, Value: string(lines[y][x]), Neighbors: make(map[Point]bool)}
		}
	}

	return data
}

type Plot struct {
	Value     string
	Point     Point
	Neighbors map[Point]bool
}

func getNeighbors(input map[Point]Plot, plot Plot) []Point {
	var neighboars []Point
	for _, d := range directions {
		np := plot.Point.Add(d)
		n, ok := input[np]
		if ok && plot.Value == n.Value {
			neighboars = append(neighboars, np)
		}
	}

	return neighboars
}

func calculate(input map[Point]Plot) (int, int) {
	var grouped []Plot

	for c := range input {
		current := input[c]
		delete(input, current.Point)
		neighbors := getNeighbors(input, current)
		for len(neighbors) > 0 {
			n := neighbors[0]
			neighbors = neighbors[1:]
			current.Neighbors[n] = true
			neighbors = append(neighbors, getNeighbors(input, input[n])...)
			delete(input, n)
		}
		grouped = append(grouped, current)
	}

	/*
		for _, v := range grouped {
			if v.Value == "R" {
				fmt.Printf("Plot: %v %v\n", v.Value, v.Point)
				for k := range v.Neighbors {
					fmt.Printf("%v\n", k)
				}
			}
		}
	*/

	result := 0
	discount := 0
	for _, p := range grouped {
		check := p.Neighbors
		check[p.Point] = true
		perimeter := calcPerimeter(check)
		result += len(check) * perimeter

		if p.Value == "R" {
			slides := calcDiscount(check)
			discount += len(check) * slides
		}
	}

	return result, discount
}

var directions2 = [2]Point{
	{X: 1, Y: 0}, //east
	{X: 0, Y: 1}, //south
}

type PairPoint struct {
	A, B Point
}

func calcDiscount(points map[Point]bool) int {
	fmt.Printf("input %v\n", points)

	tmp := make(map[PairPoint]bool)
	kept := make(map[PairPoint]bool)

	for k := range points {
		for _, d := range directions {
			p := k.Add(d)
			_, ok := points[p]
			if !ok {
				tmp[PairPoint{A: k, B: p}] = true
			}
		}
	}
	fmt.Printf("PairPoint %v\n", tmp)
	for k := range tmp {
		keep := true
		for _, d := range directions2 {
			pp := PairPoint{A: k.A.Add(d), B: k.B.Add(d)}
			_, ok := tmp[pp]
			if ok {
				keep = false
			}
		}
		if keep {
			kept[k] = true
		}
	}
	fmt.Printf("len %v\n", len(kept))
	fmt.Printf("kept %v\n", kept)

	return len(kept)
}

func calcPerimeter(points map[Point]bool) int {
	var peris []Point
	for p := range points {
		for _, d := range directions {
			next := p.Add(d)
			_, ok := points[next]
			if !ok {
				peris = append(peris, next)
			}
		}
	}

	return len(peris)
}

func Day_12(t *testing.T) {
	plots := getDataDay12()
	result1, result2 := calculate(plots)

	assert.EqualValues(t, 1930, result1)
	//assert.EqualValues(t, 1450816, result1)
	assert.EqualValues(t, 1206, result2)
	//assert.EqualValues(t, 865662, result2)

}
