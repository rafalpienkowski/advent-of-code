package days

import (
	"bufio"
	"fmt"
	"maps"
	"os"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Point struct {
	X, Y int
}

type Node struct {
	Visited  bool
	Obstacle bool
	Point    Point
}

type Graph map[Point]Node

var directions = [4]Point{
	{X: 0, Y: -1}, //north
	{X: 1, Y: 0},  //east
	{X: 0, Y: 1},  //south
	{X: -1, Y: 0}, //west
}
var max = 0

func getDataDay6() ([][]rune, Point) {

	readFile, err := os.Open("../inputs/6a.txt")
	var start Point
	if err != nil {
		fmt.Println(err)
	}
	fileScanner := bufio.NewScanner(readFile)
	var data [][]rune

	for fileScanner.Scan() {
		line := fileScanner.Text()
		max = len(line)
		index := strings.IndexRune(line, '^')
		if index > 0 {
			start = Point{X: index, Y: len(data)}
		}
		data = append(data, []rune(line))
	}
	readFile.Close()

	return data, start
}

func createGraph(data [][]rune) Graph {
	graph := make(map[Point]Node)

	for y := range len(data) {
		for x := range len(data) {
			pos := Point{X: x, Y: y}
			node := graph[pos]
			node.Point = pos
			node.Obstacle = data[y][x] == '#'
			graph[pos] = node
		}
	}

	return graph
}

func copyGraph(org Graph) Graph {
	copy := make(map[Point]Node)
	for k, v := range org {
		copy[k] = v
	}

	return copy
}

func (p *Point) Add(other Point) Point {
	return Point{X: p.X + other.X, Y: p.Y + other.Y}
}

func (p *Point) Sub(other Point) Point {
	return Point{X: p.X - other.X, Y: p.Y - other.Y}
}

func step(node Node, direction int, lab Graph) (Point, int) {
	next := node.Point.Add(directions[direction])
	if next.X < 0 || next.X >= max || next.Y < 0 || next.Y >= max {
		return Point{X: -1, Y: -1}, -1
	}

	if !lab[next].Obstacle {
		return next, direction
	}

	direction = (direction + 1) % 4
	next = node.Point.Add(directions[direction])
	if !lab[next].Obstacle {
		return next, direction
	}

	direction = (direction + 1) % 4
	next = node.Point.Add(directions[direction])
	if !lab[next].Obstacle {
		return next, direction
	}

	direction = (direction + 1) % 4
	next = node.Point.Add(directions[direction])
	if !lab[next].Obstacle {
		return next, direction
	}

	return Point{X: -1, Y: -1}, -1
}

type Step struct {
	D int
	P Point
}

func walk(lab Graph, pos Point, direction int) (Graph, bool) {
	var next Point
	seen := make(map[Step]bool)
	for direction >= 0 {
		stepp := Step{P: pos, D: direction}
		_, s := seen[stepp]
		if s {
			return lab, true
		} else {
            seen[stepp] = true
        }
		n := lab[pos]
		n.Visited = true
		lab[pos] = n
		next, direction = step(n, direction, lab)
		pos = next
	}

	return lab, false
}

func solve(lab Graph, start Point, maxy int, maxx int) (int, int) {
    init := copyGraph(lab)
    t, _ := walk(lab, start, 0)
    firstRun := copyGraph(t)
	moves := 0
	loops := 0

	for l := range maps.Values(t) {
		if l.Visited {
			moves++
		}
	}

    for y := range maxy {
        for x := range maxx {
            p := Point{ X: x, Y:y}

			if p == start {
				continue
			}
            if init[p].Obstacle {
                continue
            }
            if !firstRun[p].Visited {
                continue
            }

			tmp := copyGraph(init)
			t := tmp[p]
			t.Obstacle = true
			tmp[p] = t

			_, loop := walk(tmp, start, 0)
			if loop {
				loops++
			}
        }
    }

	return moves, loops
}

func Day_6(t *testing.T) {

	data, start := getDataDay6()
	lab := createGraph(data)

	m, o := solve(lab, start, len(data), len(data[0]))

	assert.EqualValues(t, 4454, m)
	assert.EqualValues(t, 1503, o)
}
