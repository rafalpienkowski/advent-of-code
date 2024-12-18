package days

import (
	"bufio"
	"container/heap"
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

func adj(pd PointD, maze [][]rune) []MazeStep {
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

type PointD struct {
	P Point
	D int
}

type Item struct {
	node     PointD
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
	paths := make(map[Point][][]Point)

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

	pq := &PriorityQueue{}
	heap.Init(pq)

	paths[start] = [][]Point{{start}}
	heap.Push(pq, &Item{node: PointD{P: start, D: 0}, distance: 0})

	for pq.Len() > 0 {
		current := heap.Pop(pq).(*Item)
		currentNode := current.node
		currentDist := current.distance

		if currentDist > dist[currentNode.P] {
			continue
		}

		for _, a := range adj(currentNode, maze) {
			newDist := currentDist + a.Dist
			if dist[a.To] > newDist {
				dist[a.To] = newDist
				paths[a.To] = [][]Point{}
				for _, path := range paths[currentNode.P] {
					newPath := append([]Point{}, path...)
					paths[a.To] = append(paths[a.To], append(newPath, a.To))
				}
				heap.Push(pq, &Item{node: PointD{P: a.To, D: a.Direction}, distance: newDist})
			} else if newDist == dist[a.To] {
				for _, path := range paths[currentNode.P] {
					newPath := append([]Point{}, path...)
					paths[a.To] = append(paths[a.To], append(newPath, a.To))
				}
			}
		}
	}

	fmt.Printf("Paths: %v\n", len(paths[end][0]))

	res := make(map[Point]bool)
	return dist[end], len(res)
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
	result1, result2 := findMinPath(maze)

	assert.EqualValues(t, 7036, result1)
	assert.EqualValues(t, 45, result2)
	//assert.EqualValues(t, 130536, result1)
}
