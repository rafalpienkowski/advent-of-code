package days

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay16() [][]rune {
	readFile, err := os.Open("../inputs/16.txt")

	if err != nil {
		fmt.Println(err)
	}
	fileScanner := bufio.NewScanner(readFile)
	var maze [][]rune

	for fileScanner.Scan() {
		line := fileScanner.Text()
		maze = append(maze, []rune(line))
	}

	readFile.Close()

	return maze
}

type MazeStep struct {
	From, To          Point
	Direction, Dist int
}

func adj(pd PointDir, maze [][]rune) []MazeStep {
	var adjs []MazeStep

	var dirs = [4]Point{
		{X: 1, Y: 0},
		{X: 0, Y: -1},
		{X: -1, Y: 0},
		{X: 0, Y: 1},
	}

	dir := dirs[pd.D]
	next := pd.P.Add(dir)
	if maze[next.Y][next.X] != '#' {
		nextStep := MazeStep{From: pd.P, To: next, Direction: pd.D, Dist: 1}
		adjs = append(adjs, nextStep)
	}

	dir = dirs[(pd.D+1)%4]
	next = pd.P.Add(dir)
	if maze[next.Y][next.X] != '#' {
		nextStep := MazeStep{From: pd.P, To: next, Direction: (pd.D + 1) % 4, Dist: 1000}
		adjs = append(adjs, nextStep)
	}

	dir = dirs[(pd.D+3)%4]
	next = pd.P.Add(dir)
	if maze[next.Y][next.X] != '#' {
		nextStep := MazeStep{From: pd.P, To: next, Direction: (pd.D + 3) % 4, Dist: 1000}
		adjs = append(adjs, nextStep)
	}

	return adjs
}

type PointDir struct {
	P    Point
	D    int
	Dist int
}

func findMinPath(maze [][]rune) int {
	var start, end Point
	dist := make(map[Point]int)
	path := make(map[Point]Point)
	var queue []PointDir
	defPoint := Point{X: -1, Y: -1}

	for y := range maze {
		for x := range maze[y] {
			if maze[y][x] == '#' {
				continue
			}

			p := Point{X: x, Y: y}
			path[p] = defPoint
			dist[p] = math.MaxInt

			if maze[y][x] == 'S' {
				dist[p] = 0
				start = p
				maze[y][x] = '.'
			}
			if maze[y][x] == 'E' {
				end = p
				maze[y][x] = '.'
			}
		}
	}

    queue = append(queue, PointDir{P: start, D: 0, Dist: 0})

	idx := 0

	//for len(queue) > 0 {
	for idx < 15 {
        if len(queue) == 0 {
            fmt.Printf("End \n")
            break
        }
		fmt.Println("-----------------------")
		idx++
		for _, v := range queue {
			fmt.Printf("%v\n", v)
		}

		curr := queue[0]
		queue = queue[1:]
		if curr.P == end {
			fmt.Printf("Found!!!")
            fmt.Printf("Steps: %v", idx)
			return curr.Dist
		}

		for _, a := range adj(curr, maze) {
			if curr.Dist + a.Dist < dist[a.To] {
                dist[a.To] = curr.Dist + a.Dist
                queue = append(queue, PointDir{P: a.To, D: a.Direction, Dist: dist[a.To]})
			}
		}
	}

	return dist[end]
}

func printMaze(maze [][]rune) {
	for y := range maze {
		fmt.Printf("%v\n", string(maze[y]))
	}
	fmt.Println()
}

func Day_16(t *testing.T) {
	maze := getDataDay16()
	printMaze(maze)
	result1 := findMinPath(maze)

	assert.EqualValues(t, 7036, result1)
}
