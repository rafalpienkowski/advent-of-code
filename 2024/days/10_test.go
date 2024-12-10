package days

import (
	"strconv"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay10() ([][]int, []Point) {

	lines := ReadLines("../inputs/10a.txt")
	mmap := make([][]int, len(lines))
	var starting []Point
	for y := range len(lines) {
		mmap[y] = make([]int, len(lines[y]))
	}

	for y := range len(lines) {
		for x := range len(lines[0]) {
			p := Point{X: x, Y: y}
			num, _ := strconv.Atoi(string(lines[y][x]))
			if num == 0 {
				starting = append(starting, p)
			}
			mmap[y][x] = num
		}
	}

	return mmap, starting
}

func paths(mmap [][]int, start Point) int {
	hills := make(map[Point]int)
	results := make(map[Point]int)
	hills[start] = 0

	for len(hills) > 0 {
		var p Point
		var h int
		taken := false
		for k := range hills {
			if taken {
				continue
			}
			p = k
			h = hills[p]
			taken = true
		}
		delete(hills, p)

		if h == 9 {
			results[p] = h
			continue
		}

		x := p.X
		y := p.Y
		if x+1 < len(mmap[0]) && mmap[y][x+1] == h+1 {
			hills[Point{X: x + 1, Y: y}] = h + 1
		}
		if x-1 >= 0 && mmap[y][x-1] == h+1 {
			hills[Point{X: x - 1, Y: y}] = h + 1
		}
		if y+1 < len(mmap) && mmap[y+1][x] == h+1 {
			hills[Point{X: x, Y: y + 1}] = h + 1
		}
		if y-1 >= 0 && mmap[y-1][x] == h+1 {
			hills[Point{X: x, Y: y - 1}] = h + 1
		}
	}

	return len(results)
}

type Hill struct {
	P Point
	H int
}

func trails(mmap [][]int, start Point) int {

	hills := []Hill{{H: 0, P: start}}
	result := 0

	for len(hills) > 0 {
		h := hills[0]
		hills = hills[1:]
		if h.H == 9 {
			result++
			continue
		}

		x := h.P.X
		y := h.P.Y
		if x+1 < len(mmap[0]) && mmap[y][x+1] == h.H+1 {
			hills = append(hills, Hill{P: Point{X: x + 1, Y: y}, H: h.H + 1})
		}
		if x-1 >= 0 && mmap[y][x-1] == h.H+1 {
			hills = append(hills, Hill{P: Point{X: x - 1, Y: y}, H: h.H + 1})
		}
		if y+1 < len(mmap) && mmap[y+1][x] == h.H+1 {
			hills = append(hills, Hill{P: Point{X: x, Y: y + 1}, H: h.H + 1})
		}
		if y-1 >= 0 && mmap[y-1][x] == h.H+1 {
			hills = append(hills, Hill{P: Point{X: x, Y: y - 1}, H: h.H + 1})
		}
	}
	return result
}

func Test_Day_10(t *testing.T) {
	mmap, starting := getDataDay10()
	sum1 := 0
	sum2 := 0

	for s := range starting {
		sum1 += paths(mmap, starting[s])
		sum2 += trails(mmap, starting[s])
	}

	assert.EqualValues(t, 652, sum1)
	assert.EqualValues(t, 1432, sum2)

}
