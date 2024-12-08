package days

import (
	"bufio"
	"fmt"
	"maps"
	"os"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay8() [][]rune {
	var data [][]rune

	readFile, err := os.Open("../inputs/8a.txt")
	if err != nil {
		fmt.Println(err)
	}

	fileScanner := bufio.NewScanner(readFile)

	for fileScanner.Scan() {
		line := fileScanner.Text()
		max = len(line)
		data = append(data, []rune(line))
	}
	readFile.Close()

	return data
}

func diff(p1 Point, p2 Point) [2]Point {
	return [2]Point{
		{X: p1.X - (p2.X - p1.X), Y: p1.Y - (p2.Y - p1.Y)},
		{X: p2.X + (p2.X - p1.X), Y: p2.Y + (p2.Y - p1.Y)},
	}
}

func diffInGrid(p1 Point, p2 Point, maxx int, maxy int) []Point {
	var candidates []Point
	diff := Point{X: p2.X - p1.X, Y: p2.Y - p1.Y}
	c := p1.Sub(diff)
	for c.X >= 0 && c.Y >= 0 && c.X < maxx && c.Y < maxy {
		candidates = append(candidates, c)
		c = c.Sub(diff)
	}

	d := p2.Add(diff)
	for d.X >= 0 && d.Y >= 0 && d.X < maxx && d.Y < maxy {
		candidates = append(candidates, d)
		d = d.Add(diff)
	}

	return candidates
}

func (p *Point) InGrid(maxx int, maxy int) bool {
	return p.X >= 0 && p.Y >= 0 && p.X < maxx && p.Y < maxy
}

func Test_Day_8(t *testing.T) {
	city := getDataDay8()
	maxy := len(city)
	maxx := len(city[0])
	nodes := make(map[Point]bool)
	stations := make(map[rune][]Point)
	for y := range city {
		for x := range city[y] {
			if city[y][x] != '.' {
				p := Point{X: x, Y: y}
				stations[city[y][x]] = append(stations[city[y][x]], p)
                nodes[p] = true
			}
		}
	}
	/* Part 1
		for s := range maps.Keys(stations) {
			for i, p := range stations[s] {
				for j := i+1; j < len(stations[s]); j++ {
	                candidates := diff(p, stations[s][j])
	                if candidates[0].InGrid(maxx, maxy) {
	                    nodes[candidates[0]] = true
	                }
	                if candidates[1].InGrid(maxx, maxy) {
	                    nodes[candidates[1]] = true
	                }
				}
			}
	    }
	    Part 1 */

	for s := range maps.Keys(stations) {
		for i, p := range stations[s] {
			for j := i + 1; j < len(stations[s]); j++ {
				candidates := diffInGrid(p, stations[s][j], maxx, maxy)
				for _, c := range candidates {
					nodes[c] = true
				}
			}
		}
	}

    /*
	for y := range city {
		for x := range city[y] {
			_, ok := nodes[Point{X: x, Y: y}]
			if ok {
				fmt.Print(string('#'))
			} else {
				fmt.Printf("%v", string(city[y][x]))
			}
		}
		fmt.Println()
	}
    */

	assert.EqualValues(t, 34, len(nodes))
}
