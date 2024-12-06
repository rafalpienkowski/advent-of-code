package bonus

import (
	"fmt"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Node struct {
	Visited bool
	Edges   []int
}

type Graph map[int]Node

func fromEdges(edges [][2]int) Graph {
	nodes := make(map[int]Node)

	for _, e := range edges {
		node := nodes[e[0]]
		node.Edges = append(node.Edges, e[1])
		nodes[e[0]] = node
		node2 := nodes[e[1]]
		node2.Edges = append(node2.Edges, e[0])
		nodes[e[1]] = node2
	}

	return nodes
}

func print(graph Graph) {
	for n := range graph {
		fmt.Printf("Node: %v, visited: %v, edges: %v\n", n, graph[n].Visited, graph[n].Edges)
	}
}

func dsf_rec(graph Graph, i int) {
	//fmt.Printf("Visiting %v\n", i)
	node := graph[i]
	node.Visited = true
	graph[i] = node
	for _, n := range node.Edges {
		next := graph[n]
		if !next.Visited {
			dsf_rec(graph, n)
		}
	}
}

func Test_Dfs_Rec(t *testing.T) {
	edges := [][2]int{
		{1, 2},
		{2, 3},
		{3, 4},
		{3, 5},
		{1, 5},
		{4, 8},
		{8, 5},
		{8, 9},
		{6, 5},
		{5, 7},
	}

	graph := fromEdges(edges)
	//fmt.Println("---------")
	//print(graph)
	//fmt.Println("----DSF-rec-----")
	dsf_rec(graph, 1)
	//fmt.Println("---------")

	assert.EqualValues(t, 0, 0)
}

type Point struct {
	X, Y int
}

type PointNode struct {
	Visited bool
	Edges   []Point
}

type PointGraph map[Point]PointNode

func parseInput(input string) PointGraph {
	nodes := make(map[Point]PointNode)

	lines := strings.Split(input, "\n")
	var runes [][]rune
	for _, line := range lines {
		runes = append(runes, []rune(line))
	}

	add := func(p1 Point, p2 Point) {
		node := nodes[p1]
		node.Edges = append(node.Edges, p2)
		nodes[p1] = node
		node2 := nodes[p2]
		node2.Edges = append(node2.Edges, p1)
		nodes[p2] = node2
	}

	for x := range len(lines) {
		for y := range len(lines[0]) {
			if runes[x][y] != 'O' {
				continue
			}
			if x+1 < len(lines) && runes[x+1][y] == 'O' {
				add(Point{x, y}, Point{x + 1, y})
			}
			if y+1 < len(lines) && runes[x][y+1] == 'O' {
				add(Point{x, y}, Point{x, y + 1})
			}
			if x-1 > 0 && runes[x-1][y] == 'O' {
				add(Point{x, y}, Point{x - 1, y})
			}
			if y-1 > 0 && runes[x][y-1] == 'O' {
				add(Point{x, y}, Point{x, y - 1})
			}
		}
	}

	return nodes
}

func printPointGraph(graph PointGraph) {
	for n := range graph {
		fmt.Printf("Node: %v, visited: %v, edges: %v\n", n, graph[n].Visited, graph[n].Edges)
	}
}

var moves int = 0
func dsf(graph PointGraph, p Point) {
	//fmt.Printf("Visiting %v\n", p)
    moves++
	node := graph[p]
	node.Visited = true
	graph[p] = node
	for _, n := range node.Edges {
		next := graph[n]
		if !next.Visited {
			dsf(graph, n)
		}
	}
}

func Test_Labirynth(t *testing.T) {
	input :=
		`OOOXXOOXXX
XOXXXXXOOO
XOOOOOOOXO
XXXXXXXXXO
XXXOOOOOOO
XXXOXXXXOX
XOOOOOOOOX
XXXOXOXXXX
OOOOXOOOOX
XXXXXXXXOO`

	graph := parseInput(input)
	//printPointGraph(graph)

	dsf(graph, Point{0, 0})

	fmt.Println("---------")
	escape := graph[Point{X: 9, Y: 9}]
	fmt.Printf("Visited: %v in %v moves\n", escape.Visited, moves)

	assert.EqualValues(t, 0, 0)
}
