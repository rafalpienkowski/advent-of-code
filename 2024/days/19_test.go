package days

import (
	"sort"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay19() ([]string, []string) {
	lines := ReadLines("../inputs/19a.txt")
	var towels []string
	var patterns []string

	for i, line := range lines {
		if len(line) == 0 {
			continue
		}
		if i == 0 {
			for _, t := range strings.Split(line, ", ") {
				towels = append(towels, strings.Trim(t, " "))
			}
			continue
		}
		patterns = append(patterns, strings.Trim(line, " "))
	}

	sort.Slice(towels, func(i, j int) bool {
		return len(towels[i]) < len(towels[j])
	})

	return towels, patterns
}

var cache map[string]int

func patternMatch(towels []string, pattern string) int {
	_, ok := cache[pattern]
	if !ok {
		if len(pattern) == 0 {
			return 1
		}

		result := 0
		for _, t := range towels {
			if strings.HasPrefix(pattern, t) {
				result += patternMatch(towels, pattern[len(t):])
			}
			cache[pattern] = result
		}
	}

	return cache[pattern]
}

func findMatches(towels []string, patterns []string) (int, int) {
	result1 := 0
	result2 := 0
	cache = make(map[string]int)

	for _, p := range patterns {

		tmp := patternMatch(towels, p)
		result2 += tmp
		if tmp > 0 {
			result1++
		}
	}

	return result1, result2
}

func Day_19(t *testing.T) {
	towels, patterns := getDataDay19()

	result1, result2 := findMatches(towels, patterns)

	assert.EqualValues(t, 287, result1)
	assert.EqualValues(t, 571894474468161, result2)
}
