package days

import (
	"fmt"
	"sort"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func abs(x int) int {
	if x < 0 {
		return -x
	}
	return x
}

func getDataDay1() ([]int, []int) {
	lines := ReadLines("../inputs/1a.txt")

    var left []int
	var right []int

	for _, line := range lines {
		parts := strings.Fields(line)
		l, err1 := strconv.Atoi(parts[0])
		r, err2 := strconv.Atoi(parts[1])
		if err1 != nil || err2 != nil {
			fmt.Println("Invalid numbers. Please enter valid integers.")
			continue
		}

		left = append(left, l)
		right = append(right, r)
	}

    return left, right
}

func Test_Day_1_A(t *testing.T) {
    diff := 0

    left, right := getDataDay1()
    sort.Ints(left)
    sort.Ints(right)

    for idx := range left {
        diff += abs(left[idx] - right[idx])
    }

	assert.EqualValues(t, 2166959, diff)
}


func Test_Day_1_B(t *testing.T) {
    similarity := 0

    left, right := getDataDay1()
    counts := make(map[int]int)

    for _, num := range right {
        counts[num]++
    }

    for idx := range left {
        similarity += left[idx] * counts[left[idx]]
    }

	assert.EqualValues(t, 23741109, similarity)
}
