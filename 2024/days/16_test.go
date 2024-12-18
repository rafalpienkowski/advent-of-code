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
	From, To        Point
	Direction, Dist int
}

func adj(pd PointDir, maze [][]rune) []MazeStep {
	var adjs []MazeStep

	var dirs = [4]Point{
		{X: 1, Y: 0},
		{X: 0, Y: 1},
		{X: -1, Y: 0},
		{X: 0, Y: -1},
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
		nextStep := MazeStep{From: pd.P, To: next, Direction: (pd.D + 1) % 4, Dist: 1001}
		adjs = append(adjs, nextStep)
	}

	dir = dirs[(pd.D+3)%4]
	next = pd.P.Add(dir)
	if maze[next.Y][next.X] != '#' {
		nextStep := MazeStep{From: pd.P, To: next, Direction: (pd.D + 3) % 4, Dist: 1001}
		adjs = append(adjs, nextStep)
	}

	return adjs
}

type PointDir struct {
	P    Point
	D    int
	Dist int
}

type PointD struct {
	P Point
	D int
}

type Item struct {
	node     Point
	distance int
	index    int
}

type PriorityQueue []*Item

func (pq PriorityQueue) Len() int           { return len(pq) }
func (pq PriorityQueue) Less(i, j int) bool { return pq[i].distance < pq[j].distance }

func (pq PriorityQueue) Swap(
	i, j int,
) {
	pq[i], pq[j] = pq[j], pq[i]
	pq[i].index = i
	pq[j].index = j
}
func (pq *PriorityQueue) Push(x interface{}) { *pq = append(*pq, x.(*Item)) }
func (pq *PriorityQueue) Pop() interface{} {
	old := *pq
	n := len(old)
	item := old[n-1]
	old[n-1] = nil
	*pq = old[:n-1]
	return item
}

func findMinPath(maze [][]rune) (int, int) {
	var start, end Point
	dist := make(map[Point]int)
	path := make(map[Point][]Point)
	var queue []PointDir
	defPoint := Point{X: -1, Y: -1}
	for y := range maze {
		for x := range maze[y] {
			if maze[y][x] == '#' {
				continue
			}

			p := Point{X: x, Y: y}
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

	paths := make(map[Point][][]Point)
	paths[start] = [][]Point{{start}}

	queue = append(queue, PointDir{P: start, D: 0, Dist: 0})

	findMin := func() PointDir {
		tmp := PointDir{P: defPoint, D: 0, Dist: math.MaxInt}
		for _, pd := range queue {
			if pd.Dist < tmp.Dist {
				tmp = pd
			}
		}

		return tmp
	}

	remove := func(pd PointDir) []PointDir {
		var tmp []PointDir

		for _, p := range queue {
			if p == pd {
				continue
			}

			tmp = append(tmp, p)
		}

		return tmp
	}

	for len(queue) > 0 {
		curr := findMin()
		queue = remove(curr)

		for _, a := range adj(curr, maze) {

			if dist[a.From]+a.Dist < dist[a.To] {
				dist[a.To] = dist[a.From] + a.Dist
				queue = append(queue, PointDir{P: a.To, D: a.Direction, Dist: dist[a.To]})

				paths[a.To] = [][]Point{}
				for _, pat := range paths[curr.P] {
					newPath := append([]Point{}, pat...)
					paths[a.To] = append(paths[a.To], append(newPath, a.To))
				}

				path[a.To] = append(path[a.To], a.From)
			} else if dist[a.From]+a.Dist == dist[a.To] {
				path[a.To] = append(path[a.To], a.From)

				for _, pat := range paths[curr.P] {
					newPath := append([]Point{}, pat...)
					paths[a.To] = append(paths[a.To], append(newPath, a.To))
				}
			}
		}
	}

	fmt.Printf("Paths: %v\n", paths[end])

	res := make(map[Point]bool)

	return dist[end], len(res)
}

func printMaze(maze [][]rune) {
	for y := range maze {
		fmt.Printf("%v\n", string(maze[y]))
	}
	fmt.Println()
}

func Test_Day_16(t *testing.T) {
	maze := getDataDay16()
	printMaze(maze)
	result1, result2 := findMinPath(maze)

	assert.EqualValues(t, 7036, result1)
	assert.EqualValues(t, 45, result2)
	//assert.EqualValues(t, 130536, result1)
}
