package days

import (
	"math"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay20() [][]rune {
	maze := make([][]rune, max)

    lines := ReadLines("../inputs/20a.txt")
	for _, line := range lines {
		maze = append(maze, []rune(line))
	}

	return maze
}

func findMinPath20(maze [][]rune, start Point) (map[Point]int, map[Point]Point) {
	defaultPoint := Point{X: -1, Y: -1}
	pg := parseInput18(maze)

	dists := make(map[Point]int)
	paths := make(map[Point]Point)
	var q []Point
	for p := range pg {
		dists[p] = math.MaxInt
		paths[p] = defaultPoint
		q = append(q, p)
	}
	dists[start] = 0

	removePos := func(e Point) {
		var tmp []Point
		for _, p := range q {
			if p == e {
				continue
			}
			tmp = append(tmp, p)
		}
		q = tmp
	}

	findMin := func() Point {
		cost := math.MaxInt
		e := defaultPoint
		for _, v := range q {
			if dists[v] < cost {
				cost = dists[v]
				e = v
			}
		}

		return e
	}

	for len(q) > 0 {
		curr := findMin()
		if curr == defaultPoint {
			break
		}
		removePos(curr)
		for _, n := range pg[curr].Edges {
			if dists[n] > dists[curr]+1 {
				dists[n] = dists[curr] + 1
				paths[n] = curr
			}
		}
	}

	return dists, paths
}

func solve20(maze [][]rune) (int, int) {
	var start, end Point

	for y := range maze {
		for x := range maze {
			if maze[y][x] == 'S' {
				start = Point{X: x, Y: y}
				maze[y][x] = '.'
			}
			if maze[y][x] == 'E' {
				end = Point{X: x, Y: y}
				maze[y][x] = '.'
			}
		}
	}

	distStart, paths := findMinPath20(maze, start)
	distEnd, _ := findMinPath20(maze, end)

	//get starting path
	tmp := end
	startingPath := make(map[Point]bool)
	startingPath[start] = true

	for tmp != start {
		startingPath[tmp] = true
		tmp = paths[tmp]
	}

	shortcuts2 := 0
	opt := distStart[end]
	for ds := range distStart {
		for de := range distEnd {
			diff := int(math.Abs(float64(de.X-ds.X)) + math.Abs(float64(de.Y-ds.Y)))
			if diff <= 2 {
				if distStart[ds]+diff+distEnd[de] <= opt-100 {
					shortcuts2++
				}
			}
		}
	}

	shortcuts20 := 0
	for ds := range distStart {
		for de := range distEnd {
			diff := int(math.Abs(float64(de.X-ds.X)) + math.Abs(float64(de.Y-ds.Y)))
			if diff <= 20 {
				if distStart[ds]+diff+distEnd[de] <= opt-100 {
					shortcuts20++
				}
			}
		}
	}


	return shortcuts2, shortcuts20
}

func Day_20(t *testing.T) {
	maze := getDataDay20()
	//printMaze(maze)
	result1, result2 := solve20(maze)

	assert.EqualValues(t, 1321, result1)
	assert.EqualValues(t, 971737, result2)
}
