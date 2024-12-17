package days

import (
	"bufio"
	"fmt"
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

/*
func findMinPath(maze Maze, start Point) int {
	//direction := Point{X: 1, Y: 0}
	p := maze[start]
	maze[start] = p

	queue := []PointWeigh{{P: start, W: 0}}

	for len(queue) > 0 {
		c := queue[0]
		queue = queue[1:]
		p = maze[c.P]
		if p.End {
            return c.W
		}
		for _, n := range p.Available {
			queue = append(queue, PointWeigh{P: n, W: c.W + 1})
		}
	}

	return -1
}
*/

type MazeStep struct {
	P    Point
	D, L int
}

func adj(step MazeStep, maze [][]rune) []MazeStep {
	var adjs []MazeStep

    /*
	var dirs = [4]Point{
		{X: 1, Y: 0},
		{X: 0, Y: -1},
		{X: -1, Y: 0},
		{X: 0, Y: 1},
	}
    */

	return adjs
}

func findMinPath(maze [][]rune) int {
	var s, e Point
	queue := make([]MazeStep, 0)

	for y := range maze {
		for x := range maze[y] {
			if maze[y][x] == 'S' {
				s = Point{X: x, Y: y}
				maze[y][x] = '.'
			}
			if maze[y][x] == 'E' {
				e = Point{X: x, Y: y}
			}
		}
	}

	queue = append(queue, MazeStep{P: s, D: 0, L: 0})
	for len(queue) > 0 {
		curr := queue[0]
		queue = queue[1:]

		if curr.P == e {
			return curr.L
		}
		for _, a := range adj(curr, maze) {
			queue = append(queue, a)
		}
	}

	return -1
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

	//assert.EqualValues(t, 7036, result1)
	assert.EqualValues(t, 0, result1)
}
