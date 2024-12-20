package days

import (
	"fmt"
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

func solve20(maze [][]rune) int {
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

	dist, paths := findMinPath20(maze, start)

	//get starting path
	tmp := end
	startingPath := make(map[Point]bool)
	startingPath[start] = true

	for tmp != start {
		startingPath[tmp] = true
		tmp = paths[tmp]
	}

	//find potential shortcuts
	shortcuts := 0
	seen := make(map[Point]bool)
	for p := range startingPath {
		for _, d := range directions {
			next := p.Add(d)
			//next is wall
			if maze[next.Y][next.X] == '#' {
				//after wall is paths
				nextNext := next.Add(d)
				if nextNext.Y < 0 || nextNext.X < 0 || nextNext.X >= len(maze[0]) ||
					nextNext.Y >= len(maze) {
					continue
				}
				if maze[nextNext.Y][nextNext.X] == '.' {
					_, ok := startingPath[nextNext]
					if ok {
						_, was := seen[next]
						if !was {
							seen[next] = true
							dist1 := dist[p]
							dist2 := dist[nextNext]
							save := math.Abs(float64(dist2-dist1)) - 2
							if save >= 100 {
								shortcuts++
							}
						}
					}
				}
			}
		}
	}

	//6781
	fmt.Printf("Shortcuts: %v\n", shortcuts)
	//examine shortcuts

	return shortcuts
}

func Test_Day_18(t *testing.T) {
	maze := getDataDay20()
	//printMaze(maze)
	result1 := solve20(maze)

	assert.EqualValues(t, 1321, result1)
	//assert.EqualValues(t, 22, result1)

    //1289 - to low, some else
}
