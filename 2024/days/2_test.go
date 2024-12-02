package days

import (
	"fmt"
	"slices"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay2() [][]int {
	lines := ReadLines("../inputs/2a.txt")

	data := make([][]int, len(lines))

	for i, line := range lines {
		levels := strings.Fields(line)
		for _, level := range levels {
			l, err := strconv.Atoi(level)
			if err != nil {
				fmt.Println("Invalid numbers. Please enter valid integers.")
				continue
			}
			data[i] = append(data[i], l)
		}
	}

	return data
}

func Test_Day_2_A(t *testing.T) {
	data := getDataDay2()
	good := 0

	for _, level := range data {
		save := false
		direction := level[1]-level[0] > 0

		for j, value := range level {
			if j+1 == len(level) {
				break
			}
			diff := level[j+1] - value
			if diff > 0 != direction {
				save = false
				break
			}

			if abs(diff) == 0 || abs(diff) > 3 {
				save = false
				break
			}

			save = true
		}

		if save == true {
			good++
		}
	}

	assert.EqualValues(t, 321, good)
}

func testLevel(level []int) (bool, int) {
	direction := level[1]-level[0] > 0

	for j, value := range level {
		if j+1 == len(level) {
			break
		}

		diff := level[j+1] - value
		if diff > 0 != direction {
			return false, j + 1
		}

		if abs(diff) == 0 || abs(diff) > 3 {
			return false, j + 1
		}
	}

	return true, -1
}

func remove(slice []int, s int) []int {
	return append(slice[:s], slice[s+1:]...)
}

func test(level []int, t *testing.T) bool {
	save, err := testLevel(level)
	if !save {
		level = remove(level, err)
		save1, _ := testLevel(level)
		save = save1
	}

	return save
}

func Test_Day_2_B(t *testing.T) {
	data := getDataDay2()
	good := 0

	for _, level := range data {
		orig := make([]int, len(level))
        copy(orig, level)

		save := test(level, t)
		if !save {
			slices.Reverse(orig)
			save = test(orig, t)
		}

		if save == true {
			good++
		}
	}

	assert.EqualValues(t, 386, good)
}
