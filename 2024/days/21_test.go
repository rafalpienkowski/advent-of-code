package days

import (
	"container/list"
	"math"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Move struct {
	Key, Move string
}

type Key struct {
	Visited    bool
	Neighboars []Move
}

func getDataDay21() []string {
	lines := ReadLines("../inputs/21a.txt")

	return lines
}

var dirPad = map[string][]Move{

	"A": {{Key: "^", Move: "<"}, {Key: ">", Move: "v"}},
	"<": {{Key: "v", Move: ">"}},
	">": {{Key: "v", Move: "<"}, {Key: "A", Move: "^"}},
	"v": {{Key: "<", Move: "<"}, {Key: ">", Move: ">"}, {Key: "^", Move: "^"}},
	"^": {{Key: "v", Move: "v"}, {Key: "A", Move: ">"}},
}

var keypad = map[string][]Move{
	"A": {{Key: "0", Move: "<"}, {Key: "3", Move: "^"}},
	"0": {{Key: "2", Move: "^"}, {Key: "A", Move: ">"}},
	"1": {{Key: "2", Move: ">"}, {Key: "4", Move: "^"}},
	"2": {
		{Key: "0", Move: "v"},
		{Key: "1", Move: "<"},
		{Key: "5", Move: "^"},
		{Key: "3", Move: ">"},
	},
	"3": {{Key: "2", Move: "<"}, {Key: "A", Move: "v"}, {Key: "6", Move: "^"}},
	"4": {{Key: "1", Move: "v"}, {Key: "5", Move: ">"}, {Key: "7", Move: "^"}},
	"5": {
		{Key: "2", Move: "v"},
		{Key: "4", Move: "<"},
		{Key: "6", Move: ">"},
		{Key: "8", Move: "^"},
	},
	"6": {{Key: "3", Move: "v"}, {Key: "5", Move: "<"}, {Key: "9", Move: "^"}},
	"7": {{Key: "4", Move: "v"}, {Key: "8", Move: ">"}},
	"8": {{Key: "7", Move: "<"}, {Key: "5", Move: "v"}, {Key: "9", Move: ">"}},
	"9": {{Key: "8", Move: "<"}, {Key: "6", Move: "v"}},
}

type Path struct {
	Node string
	Path []string
}

func findShortestPaths(start, target string, pad map[string][]Move) []string {
	queue := list.New()
	queue.PushBack(Path{Node: start, Path: []string{}})
	visited := make(map[string]bool)
	shortestPaths := [][]string{}
	shortestLength := -1

	for queue.Len() > 0 {
		current := queue.Remove(queue.Front()).(Path)

		if shortestLength != -1 && len(current.Path) > shortestLength {
			break
		}

		if current.Node == target {
			if shortestLength == -1 || len(current.Path) == shortestLength {
				shortestLength = len(current.Path)
				shortestPaths = append(shortestPaths, current.Path)
			}
			continue
		}

		visited[current.Node] = true

		for _, neighbor := range pad[current.Node] {
			if !visited[neighbor.Key] {
				newPath := append([]string{}, current.Path...)
				newPath = append(newPath, neighbor.Move)
				queue.PushBack(Path{Node: neighbor.Key, Path: newPath})
			}
		}
	}

	var paths []string

	for i := range shortestPaths {
		paths = append(paths, strings.Join(shortestPaths[i], "")+"A")
	}

	return paths
}

func minLen(test []string) int {
	min := math.MaxInt
	for _, t := range test {
		if len(t) < min {
			min = len(t)
		}
	}
	return min
}

func solve21_2(numbers []string) int {
	result := 0

	for _, number := range numbers {
		minMin := math.MaxInt
		start := "A"
		var pp []string
		for _, d := range number {
			digit := string(d)
			paths := findShortestPaths(start, digit, keypad)

			var result []string
			if len(pp) > 0 {
				for _, str1 := range pp {
					for _, str2 := range paths {
						result = append(result, str1+str2)
					}
				}
				pp = result
			}
			if len(pp) == 0 {
				pp = append(pp, paths...)
			}

			start = digit
		}

		min1 := minLen(pp)

		for _, path1 := range pp {
			if len(path1) > min1 {
				continue
			}
			start1 := "A"
			var pp1 []string
			for _, d := range path1 {
				pos1 := string(d)
				paths1 := findShortestPaths(start1, pos1, dirPad)

				var result []string
				if len(pp1) > 0 {
					for _, str1 := range pp1 {
						for _, str2 := range paths1 {
							result = append(result, str1+str2)
						}
					}
					pp1 = result
				}
				if len(pp1) == 0 {
					pp1 = append(pp1, paths1...)
				}

				start1 = pos1
			}

			min2 := minLen(pp1)

			for _, path2 := range pp1 {
				if len(path2) > min2 {
					continue
				}
				start2 := "A"
				var pp2 []string
				for _, d2 := range path2 {
					pos2 := string(d2)
					paths2 := findShortestPaths(start2, pos2, dirPad)

					var result []string
					if len(pp2) > 0 {
						for _, str1 := range pp2 {
							for _, str2 := range paths2 {
								result = append(result, str1+str2)
							}
						}
						pp2 = result
					}
					if len(pp2) == 0 {
						pp2 = append(pp2, paths2...)
					}

					start2 = pos2
				}

				min3 := minLen(pp2)
				if min3 < minMin {
					minMin = min3
				}
			}
		}

		dInt, _ := strconv.Atoi(number[:3])

		result += dInt * minMin
	}

	return result
}

func Day_21(t *testing.T) {

	data := getDataDay21()
	result1 := solve21_2(data)

	assert.EqualValues(t, 126384, result1)
}
