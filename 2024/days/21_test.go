package days

import (
	"math"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Key struct {
	Value      rune
	Visited    bool
	Neighboars []rune
}

func newKeyPad() []Key {
	return []Key{
		{Value: 'A', Neighboars: []rune{'0', '3'}},
		{Value: '0', Neighboars: []rune{'2', 'A'}},
		{Value: '1', Neighboars: []rune{'2', '4'}},
		{Value: '2', Neighboars: []rune{'0', '1', '5', '3'}},
		{Value: '3', Neighboars: []rune{'2', 'A', '6'}},
		{Value: '4', Neighboars: []rune{'1', '5', '7'}},
		{Value: '5', Neighboars: []rune{'2', '4', '6', '8'}},
		{Value: '6', Neighboars: []rune{'3', '5', '9'}},
		{Value: '7', Neighboars: []rune{'4', '8'}},
		{Value: '8', Neighboars: []rune{'7', '5', '9'}},
		{Value: '9', Neighboars: []rune{'8', '6'}},
	}
}

func getKeys() []rune {
	return []rune{'A', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'}
}

func findMinPaths() map[rune]map[rune]rune {

	maps := make(map[rune]map[rune]rune)

	for _, k := range getKeys() {

		dists := make(map[rune]int)
		paths := make(map[rune]rune)
        pg := newKeyPad()

		var q []rune
		for _, p := range getKeys() {
			dists[p] = math.MaxInt
			paths[p] = ' '
			q = append(q, p)
		}
		dists[k] = 0

		removePos := func(e rune) {
			var tmp []rune
			for _, p := range q {
				if p == e {
					continue
				}
				tmp = append(tmp, p)
			}
			q = tmp
		}

		findMin := func() rune {
			cost := math.MaxInt
			e := ' '
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
			if curr == ' ' {
				break
			}
			removePos(curr)
			for _, n := range pg[curr].Neighboars {
				if dists[n] > dists[curr]+1 {
					dists[n] = dists[curr] + 1
					paths[n] = curr
				}
			}
		}
	}

	return maps
}

func solve21() int {
	return 0
}

func Day_21(t *testing.T) {
	result1 := solve21()

	assert.EqualValues(t, 1321, result1)
}
