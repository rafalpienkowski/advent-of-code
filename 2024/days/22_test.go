package days

import (
	"strconv"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay22() []int {
	var secrets []int

	lines := ReadLines("../inputs/22a.txt")
	for _, line := range lines {
		l, _ := strconv.Atoi(line)
		secrets = append(secrets, l)
	}

	return secrets
}

func solve22(secrets []int) int {
	result := 0
    var mod = 16777216 - 1

	cache := make(map[int]int)

	for _, s := range secrets {
        tmp := s
		for i := 0; i < 2000; i++ {

            start := tmp
			val, ok := cache[start]
			if ok {
                tmp = val
                continue
			}

            res1 := tmp << 6 // step1: multiply by 64
            // mix and prune
            tmp = (tmp ^ res1) & mod

            res2 := tmp >> 5 // step2: dividing by 32
            tmp = (tmp ^ res2) & mod

            res3 := tmp << 11 // step3: multiply by 2024

            // mix and prune
            tmp = (tmp ^ res3) & mod

            cache[start] = tmp
		}

		result += tmp
	}

	return result
}

func Day_22(t *testing.T) {
	secrets := getDataDay22()
	result1 := solve22(secrets)

	assert.EqualValues(t, 37327623, result1)
	//assert.EqualValues(t, 14869099597, result1)

}
