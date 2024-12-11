package days

import (
	"fmt"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay11() []int {
	lines := ReadLines("../inputs/11a.txt")
	data := []int{}

	for _, line := range lines {
		parts := strings.Fields(line)
		for i := range parts {
			p, _ := strconv.Atoi(parts[i])
			data = append(data, p)
		}
	}

	return data
}

func blink(data map[int]int) map[int]int {
	newData := make(map[int]int)

	for k, v := range data {
		if k == 0 {
			old := newData[1]
			newData[1] = v + old
		} else if len(fmt.Sprintf("%v", k))%2 == 0 {
			str := fmt.Sprintf("%v", k)
			left := str[:(len(str) / 2)]
			right := str[(len(str) / 2):]
			l, _ := strconv.Atoi(left)
			r, _ := strconv.Atoi(right)

			oldl := newData[l]
			newData[l] = oldl + v

			oldr := newData[r]
			newData[r] = oldr + v
		} else {
			newData[k*2024] = newData[k*2024] + v
		}
	}

	return newData
}

func sumData(data map[int]int) int {
	sum := 0
	for _, v := range data {
		sum += v
	}

	return sum
}

func Test_Day_11(t *testing.T) {
	data := getDataDay11()

	idx := make(map[int]int)
	for _, d := range data {
		a, ok := idx[d]
		if ok {
			a++
			idx[d] = a
		} else {
			idx[d] = 1
		}
	}

	sum25 := 0
	for i := 0; i < 75; i++ {
		if i == 25 {
			sum25 = sumData(idx)
		}
		idx = blink(idx)
	}

	assert.EqualValues(t, 228668, sum25)
	assert.EqualValues(t, 270673834779359, sumData(idx))

}
