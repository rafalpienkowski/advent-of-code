package days

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay25() ([][5]int, [][5]int) {
	var locks [][5]int
	var keys [][5]int
	keyIndicator := "....."
	lockIndicator := "#####"
	isKey := false
	isLock := false
	var input [5]int

	lines := ReadLines("../inputs/25a.txt")
	for li, line := range lines {
		if li%8 == 0 && line == keyIndicator {
			isKey = true
			continue
		}
		if li%8 == 0 && line == lockIndicator {
			isLock = true
			continue
		}

		if len(line) == 0 || li == len(lines)-1 {

			if isKey {
				if li != len(lines)-1 {
					for h := range 5 {
						input[h] -= 1
					}
				}
				keys = append(keys, input)
			}

			if isLock {
				locks = append(locks, input)
			}

			isKey = false
			isLock = false
			input = [5]int{0, 0, 0, 0, 0}
			continue
		}

		for i, c := range line {
			if c == '#' {
				input[i] += 1
			}
		}

	}

	return keys, locks
}

func solve25(keys [][5]int, locks [][5]int) int {
	result := 0

	for _, lock := range locks {
		for _, key := range keys {
			match := true
			for i := range 5 {
				if lock[i]+key[i] > 5 {
					match = false
					break
				}
			}
			if match {
				result++
			}
		}
	}

	return result
}

func Test_Day_25(t *testing.T) {
	keys, locks := getDataDay25()
	result1 := solve25(keys, locks)

	//assert.EqualValues(t, 3, result1)
	assert.EqualValues(t, 2835, result1)
}
