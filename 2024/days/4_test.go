package days

import (
	"bufio"
	"fmt"
	"os"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay4() [][]rune {

	readFile, err := os.Open("../inputs/4a.txt")

	if err != nil {
		fmt.Println(err)
	}
	fileScanner := bufio.NewScanner(readFile)
	var data [][]rune

	for fileScanner.Scan() {
		line := fileScanner.Text()
		data = append(data, []rune(line))
	}

	readFile.Close()

	return data
}

var xmas = "XMAS"
var samx = "SAMX"

func gotId(str string) bool {
	if str == xmas || str == samx {
		return true
	}
	return false
}

func getSubstring(data [][]rune, x int, y int, dx int, dy int) string {
	var str []rune

	for i := 0; i < 4; i++ {
		str = append(str, data[y+dy*i][x+dx*i])
	}

	return string(str)
}

func addResult(results map[string]bool, co Coordinates) {
	_, ok := results[co.print()]
	if !ok {
		_, ok2 := results[co.printReverse()]
		if !ok2 {
			results[co.print()] = true
		}
	}
}

type Coordinate struct {
	X, Y int
}

type Coordinates [4]Coordinate

func (c *Coordinates) print() string {
	var builder strings.Builder
	for i := range c {
		builder.WriteString(fmt.Sprintf("(%v,%v)", c[i].X, c[i].Y))
	}

	return builder.String()
}

func (c *Coordinates) printReverse() string {
	var builder strings.Builder
	for i := range c {
		builder.WriteString(fmt.Sprintf("(%v,%v)", c[3-i].X, c[3-i].Y))
	}

	return builder.String()
}

func createCoordinates(x int, y int, dx int, dy int) Coordinates {
	var co []Coordinate
	for i := 0; i < 4; i++ {
		co = append(co, Coordinate{X: x + dx*i, Y: y + dy*i})
	}

	return Coordinates(co)
}

func Test_Day_4_A(t *testing.T) {
	data := getDataDay4()
	limit := len(data)
	results := make(map[string]bool)

	for y, row := range data {
		for x := range row {
			if x+4 <= limit {
				if gotId(getSubstring(data, x, y, 1, 0)) {
					addResult(results, createCoordinates(x, y, 1, 0))
				}
			}
			if x-4 >= 0 {
				if gotId(getSubstring(data, x, y, -1, 0)) {
					addResult(results, createCoordinates(x, y, -1, 0))
				}
			}
			if y+4 <= limit {
				if gotId(getSubstring(data, x, y, 0, 1)) {
					addResult(results, createCoordinates(x, y, 0, 1))
				}
			}
			if y-4 >= 0 {
				if gotId(getSubstring(data, x, y, 0, -1)) {
					addResult(results, createCoordinates(x, y, 0, -1))
				}
			}
			if x+4 <= limit && y+4 <= limit {
				if gotId(getSubstring(data, x, y, 1, 1)) {
					addResult(results, createCoordinates(x, y, 1, 1))
				}
			}
			if x+4 <= limit && y-4 >= 0 {
				if gotId(getSubstring(data, x, y, 1, -1)) {
					addResult(results, createCoordinates(x, y, 1, -1))
				}
			}
			if x-4 >= 0 && y+4 <= limit {
				if gotId(getSubstring(data, x, y, -1, 1)) {
					addResult(results, createCoordinates(x, y, -1, 1))
				}
			}
			if x-4 >= 0 && y-4 >= 0 {
				if gotId(getSubstring(data, x, y, -1, -1)) {
					addResult(results, createCoordinates(x, y, -1, -1))
				}
			}
		}
	}

	assert.EqualValues(t, 2571, len(results))
}

func Test_Day_4_B(t *testing.T) {
	data := getDataDay4()
	limit := len(data)
	results := 0

	for y, row := range data {
		for x := range row {
			if x+2 < limit && y+2 < limit && 
                data[y][x] == 'M' && 
				data[y+2][x+2] == 'S' &&
                data[y+1][x+1] == 'A' &&
				data[y+2][x] == 'M' &&
				data[y][x+2] == 'S' {
				results++
			}
			if x+2 < limit && y+2 < limit && 
                data[y][x] == 'S' && 
				data[y+2][x+2] == 'M' &&
                data[y+1][x+1] == 'A' &&
				data[y+2][x] == 'S' &&
				data[y][x+2] == 'M' {
				results++
			}
			if x+2 < limit && y+2 < limit && 
                data[y][x] == 'S' && 
				data[y+2][x+2] == 'M' &&
                data[y+1][x+1] == 'A' &&
				data[y+2][x] == 'M' &&
				data[y][x+2] == 'S' {
				results++
			}
			if x+2 < limit && y+2 < limit && 
                data[y][x] == 'M' && 
				data[y+2][x+2] == 'S' &&
                data[y+1][x+1] == 'A' &&
				data[y+2][x] == 'S' &&
				data[y][x+2] == 'M' {
				results++
			}
		}
	}

	assert.EqualValues(t, 1992, results)
}
