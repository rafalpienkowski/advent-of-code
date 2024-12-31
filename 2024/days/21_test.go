package days

import (
	"fmt"
	"math"
	"strconv"
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
	lines := ReadLines("../inputs/21.txt")

    return lines
}

func newKeyPad() map[string]Key {
	keyPad := make(map[string]Key)

	keyPad["A"] = Key{Neighboars: []Move{{Key: "0", Move: "<"}, {Key: "3", Move: "^"}}}
	keyPad["0"] = Key{Neighboars: []Move{{Key: "2", Move: "^"}, {Key: "A", Move: ">"}}}
	keyPad["1"] = Key{Neighboars: []Move{{Key: "2", Move: ">"}, {Key: "4", Move: "^"}}}
	keyPad["2"] = Key{
		Neighboars: []Move{
			{Key: "0", Move: "v"},
			{Key: "1", Move: "<"},
			{Key: "5", Move: "^"},
			{Key: "3", Move: ">"},
		},
	}
	keyPad["3"] = Key{
		Neighboars: []Move{{Key: "2", Move: "<"}, {Key: "A", Move: "v"}, {Key: "6", Move: "^"}},
	}
	keyPad["4"] = Key{
		Neighboars: []Move{{Key: "1", Move: "v"}, {Key: "5", Move: ">"}, {Key: "7", Move: "^"}},
	}
	keyPad["5"] = Key{Neighboars: []Move{
		{Key: "2", Move: "v"},
		{Key: "4", Move: "<"},
		{Key: "6", Move: ">"},
		{Key: "8", Move: "^"},
	}}
	keyPad["6"] = Key{
		Neighboars: []Move{{Key: "3", Move: "v"}, {Key: "5", Move: "<"}, {Key: "9", Move: "^"}},
	}
	keyPad["7"] = Key{Neighboars: []Move{{Key: "4", Move: "v"}, {Key: "8", Move: ">"}}}
	keyPad["8"] = Key{
		Neighboars: []Move{{Key: "7", Move: "<"}, {Key: "5", Move: "v"}, {Key: "9", Move: ">"}},
	}
	keyPad["9"] = Key{Neighboars: []Move{{Key: "8", Move: "<"}, {Key: "6", Move: "v"}}}

	return keyPad
}

func getKeys() []string {
	return []string{"A", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
}

func getDirs() []string {
	return []string{">", "<", "v", "^", "A"}
}

func newDirPad() map[string]Key {
	dirPad := make(map[string]Key)

	dirPad["A"] = Key{Neighboars: []Move{{Key: "^", Move: "<"}, {Key: ">", Move: "v"}}}
	dirPad["<"] = Key{Neighboars: []Move{{Key: "v", Move: ">"}}}
	dirPad[">"] = Key{Neighboars: []Move{{Key: "v", Move: "<"}, {Key: "A", Move: "^"}}}
	dirPad["v"] = Key{
		Neighboars: []Move{{Key: "<", Move: "<"}, {Key: ">", Move: ">"}, {Key: "^", Move: "^"}},
	}
	dirPad["^"] = Key{Neighboars: []Move{{Key: "v", Move: "v"}, {Key: "A", Move: ">"}}}

	return dirPad
}

func findMinDirPaths() map[string]map[string]Move {
	maps := make(map[string]map[string]Move)
	defaultMove := Move{Key: " ", Move: " "}

	for _, k := range getDirs() {

		dists := make(map[string]int)
		paths := make(map[string]Move)
		pg := newDirPad()

		var q []string
		for _, p := range getDirs() {
			dists[p] = math.MaxInt
			paths[p] = defaultMove
			q = append(q, p)
		}
		dists[k] = 0

		removePos := func(e string) {
			var tmp []string
			for _, p := range q {
				if p == e {
					continue
				}
				tmp = append(tmp, p)
			}
			q = tmp
		}

		findMin := func() string {
			cost := math.MaxInt
			e := " "
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
			if curr == " " {
				break
			}
			removePos(curr)
			for _, n := range pg[curr].Neighboars {
				if dists[n.Key] > dists[curr]+1 {
					dists[n.Key] = dists[curr] + 1
					paths[n.Key] = Move{Key: curr, Move: n.Move}
				}
			}
		}

		maps[k] = paths
	}

	return maps
}

func findMinPaths() map[string]map[string]Move {

	maps := make(map[string]map[string]Move)
	defaultMove := Move{Key: " ", Move: " "}

	for _, k := range getKeys() {

		dists := make(map[string]int)
		paths := make(map[string]Move)
		pg := newKeyPad()

		var q []string
		for _, p := range getKeys() {
			dists[p] = math.MaxInt
			paths[p] = defaultMove
			q = append(q, p)
		}
		dists[k] = 0

		removePos := func(e string) {
			var tmp []string
			for _, p := range q {
				if p == e {
					continue
				}
				tmp = append(tmp, p)
			}
			q = tmp
		}

		findMin := func() string {
			cost := math.MaxInt
			e := " "
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
			if curr == " " {
				break
			}
			removePos(curr)
			for _, n := range pg[curr].Neighboars {
				if dists[n.Key] > dists[curr]+1 {
					dists[n.Key] = dists[curr] + 1
					paths[n.Key] = Move{Key: curr, Move: n.Move}
				}
			}
		}

		maps[k] = paths
	}

	return maps
}

func solve21(data []string) int {

	minPaths := findMinPaths()
	minDirPaths := findMinDirPaths()
    result := 0

	for _, d := range data {

		start := "A"
		startM1 := "A"
		startM2 := "A"
		var moves []string
		var moves1 []string
		var moves2 []string

		for _, c := range d {
			end := string(c)
			var cMoves []string

			for end != start {
				m := minPaths[start][end]
				end = m.Key
				cMoves = append([]string{m.Move}, cMoves...)
			}

			start = string(c)
			cMoves = append(cMoves, "A")
			moves = append(moves, cMoves...)
		}

		for _, m1 := range moves {
			end := m1
			var c1Moves []string

			for end != startM1 {
				m := minDirPaths[startM1][end]
				end = m.Key
				c1Moves = append([]string{m.Move}, c1Moves...)
			}

			startM1 = m1
			c1Moves = append(c1Moves, "A")
			moves1 = append(moves1, c1Moves...)
		}

		for _, m2 := range moves1 {
			end := m2
			var c2Moves []string

			for end != startM2 {
				m := minDirPaths[startM2][end]
				end = m.Key
				c2Moves = append([]string{m.Move}, c2Moves...)
			}

			startM2 = m2
			c2Moves = append(c2Moves, "A")
			moves2 = append(moves2, c2Moves...)
		}

        dInt, _ := strconv.Atoi(d[:3])
        fmt.Printf("Len %v, Code %v\n", len(moves2), dInt) 

        result += dInt * len(moves2)
	}

	return result
}

func Test_Day_21(t *testing.T) {

	data := getDataDay21()
	result1 := solve21(data)

	assert.EqualValues(t, 126384, result1)
}
