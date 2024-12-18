package days

import (
	"math"
	"regexp"
	"strconv"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay18() [][]rune {
	max := 71
	//max := 7
	data := make([][]rune, max)
	//maxLine := 12
	maxLine := 1024

	for y := range max {
		data[y] = make([]rune, max)
		for x := range max {
			data[y][x] = '.'
		}
	}

	re := regexp.MustCompile(`^(\d+),(\d+)$`)
	lines := ReadLines("../inputs/18a.txt")
	for idx, line := range lines {
		if idx == maxLine {
			break
		}
		matches := re.FindStringSubmatch(line)

		x, _ := strconv.Atoi(matches[1])
		y, _ := strconv.Atoi(matches[2])

		data[y][x] = '#'
	}

	return data
}

func readAfter(maxLine int) Point {
	re := regexp.MustCompile(`^(\d+),(\d+)$`)
	lines := ReadLines("../inputs/18a.txt")
	var next Point

	for idx, line := range lines {
		if idx < maxLine {
			continue
		}
		matches := re.FindStringSubmatch(line)

		x, _ := strconv.Atoi(matches[1])
		y, _ := strconv.Atoi(matches[2])

		next = Point{X: x, Y: y}
		break
	}

	return next
}

type PointNode struct {
	Edges []Point
}

type PointGraph map[Point]PointNode

func findMinPath18(maze [][]rune) (map[Point]int, map[Point]Point) {
	s := Point{X: 0, Y: 0}
	def := Point{X: -1, Y: -1}
	pg := parseInput18(maze)

	d := make(map[Point]int)
	p := make(map[Point]Point)
	var Q []Point
	for pn := range pg {
		d[pn] = math.MaxInt
		p[pn] = def
		Q = append(Q, pn)
	}
	d[s] = 0

	removePos := func(e Point) {
		var tmp []Point
		for _, q := range Q {
			if q == e {
				continue
			}
			tmp = append(tmp, q)
		}
		Q = tmp
	}

	findMin := func() Point {
		cost := math.MaxInt
		e := def
		for _, v := range Q {
			if d[v] < cost {
				cost = d[v]
				e = v
			}
		}

		return e
	}

	for len(Q) > 0 {
		curr := findMin()
		if curr == def {
			break
		}
		removePos(curr)

		for _, n := range pg[curr].Edges {
			if d[n] > d[curr]+1 {
				d[n] = d[curr] + 1
				p[n] = curr
			}
		}
	}

	return d, p
}

func solve18(maze [][]rune) (int, Point) {
	//max := 7
	max := 71
	end := Point{X: max - 1, Y: max - 1}

	d, _ := findMinPath18(maze)
	dd := d[end]
	idx := 1024
	//idx := 12

	for idx < 3000 {
		next := readAfter(idx)
		maze[next.Y][next.X] = '#'
		d, _ = findMinPath18(maze)
		if d[end] == math.MaxInt {
			break
		}
		idx++
	}

	next := readAfter(idx)

	return dd, next
}

func parseInput18(maze [][]rune) PointGraph {
	nodes := make(map[Point]PointNode)

	add := func(p1 Point, p2 Point) {
		node := nodes[p1]
		node.Edges = append(node.Edges, p2)
		nodes[p1] = node
		node2 := nodes[p2]
		node2.Edges = append(node2.Edges, p1)
		nodes[p2] = node2
	}

	for y := range len(maze) {
		for x := range len(maze[0]) {
			if maze[y][x] != '.' {
				continue
			}
			if y+1 < len(maze) && maze[y+1][x] == '.' {
				add(Point{x, y}, Point{X: x, Y: y + 1})
			}
			if x+1 < len(maze) && maze[y][x+1] == '.' {
				add(Point{x, y}, Point{X: x + 1, Y: y})
			}
			if y-1 > 0 && maze[y-1][x] == '.' {
				add(Point{x, y}, Point{X: x, Y: y - 1})
			}
			if x-1 > 0 && maze[y][x-1] == '.' {
				add(Point{x, y}, Point{X: x - 1, Y: y})
			}
		}
	}

	return nodes
}

func Day_18(t *testing.T) {
	maze := getDataDay18()
	//printMaze(maze)
	result1, result2 := solve18(maze)

	assert.EqualValues(t, 304, result1)
	//assert.EqualValues(t, 22, result1)
	//assert.EqualValues(t, Point{X: 6, Y: 1}, result2)
	assert.EqualValues(t, Point{X: 50, Y: 28}, result2)
}
