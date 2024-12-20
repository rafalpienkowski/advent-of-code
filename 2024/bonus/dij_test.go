package bonus

import (
	"math"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Edge struct {
	A, B, W int
}

func Dij(edges []Edge, start int, end int) int {
	d := make(map[int]int)
	d[0] = math.MaxInt
	d[1] = math.MaxInt
	d[2] = math.MaxInt
	d[3] = math.MaxInt
	d[4] = math.MaxInt
	d[5] = math.MaxInt
	d[start] = 0

	p := make(map[int]int)
	p[0] = -1
	p[1] = -1
	p[2] = -1
	p[3] = -1
	p[4] = -1
	p[5] = -1

	neighbours := func(from int) []Edge {
		var n []Edge

		for _, e := range edges {
			if e.A == from {
				n = append(n, e)
			}
		}

		return n
	}
	Q := []int{0, 1, 2, 3, 4, 5}

	removePos := func(e int) {
		var tmp []int
		for _, q := range Q {
			if q == e {
				continue
			}
			tmp = append(tmp, q)
		}
		Q = tmp
	}

	for len(Q) > 0 {
		cost := math.MaxInt
		e := -1
		for _, v := range Q {
			if d[v] < cost {
				cost = d[v]
				e = v
			}
		}
		if e < 0 {
			break
		}

		removePos(e)

		for _, n := range neighbours(e) {
			if d[n.B] > d[n.A]+n.W {
				d[n.B] = d[n.A] + n.W
				p[n.B] = n.A
			}
		}
    }

	return d[end]
}

func Test_Alg(t *testing.T) {
	edges := []Edge{
		{A: 0, B: 1, W: 3},
		{A: 0, B: 4, W: 3},
		{A: 1, B: 2, W: 1},
		{A: 2, B: 3, W: 3},
		{A: 2, B: 5, W: 1},
		{A: 3, B: 1, W: 3},
		{A: 4, B: 5, W: 2},
		{A: 5, B: 0, W: 6},
		{A: 5, B: 3, W: 1},
	}

	result := Dij(edges, 0, 3)

	assert.EqualValues(t, 6, result)
}
