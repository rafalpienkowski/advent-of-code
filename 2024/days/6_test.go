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
	Visited  int
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
		v.Visited = 0
		copy[k] = v
	}

	return copy
}

func (p *Point) Add(other Point) Point {
	return Point{X: p.X + other.X, Y: p.Y + other.Y}
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

	return Point{X: -1, Y: -1}, -1
}

func walk(lab Graph, pos Point, direction int) Graph {
	var next Point
	for direction >= 0 && lab[pos].Visited <= 4 {
		n := lab[pos]
		n.Visited++
		lab[pos] = n
		next, direction = step(n, direction, lab)
		pos = next
	}

	return lab
}

func solve(lab Graph, pos Point) (int, int) {
	start := pos
	init := lab
	t := walk(lab, pos, 0)
	moves := 0

	//print(t)

	obstacles := 0
	for n := range maps.Values(t) {
		if n.Visited > 0 {
			moves++
			if n.Point == start {
				fmt.Println("Starting position")
				continue
			}
			//fmt.Printf("Testing %v\n", n.Point)
			tmp := copyGraph(init)
			tmpp := tmp[n.Point]
			tmpp.Obstacle = true
			tmp[n.Point] = tmpp

			tmp2 := walk(tmp, start, 0)
			if hasLoop(tmp2) {
				//fmt.Printf("Obstacle: y: %v, x: %v\n", n.Point.Y, n.Point.X)
				//fmt.Println("Loop")
				//print(tmp2)
				obstacles++
			}
		}
	}

	return moves, obstacles
}

func hasLoop(lab Graph) bool {
	for n := range maps.Values(lab) {
		if n.Visited > 4 {
			return true
		}
	}
	return false
}

func print(lab Graph) {
	for y := range max {
		for x := range max {
			n, ok := lab[Point{X: x, Y: y}]
			if !ok {
				fmt.Print("T")
			} else if n.Obstacle {
				fmt.Print("#")
			} else if n.Visited > 0 {
				fmt.Print(n.Visited)
			} else {
				fmt.Print(".")
			}
		}
		fmt.Println()
	}
}

func Test_Day_6(t *testing.T) {

	data, start := getDataDay6()
	lab := createGraph(data)

	m, o := solve(lab, start)

	assert.EqualValues(t, 4454, m)
	////assert.EqualValues(t, 41, m)
	assert.EqualValues(t, 3, o)
	//1230 - too low
	//4434 - too high
}
