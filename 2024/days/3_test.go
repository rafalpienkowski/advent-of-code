package days

import (
	"fmt"
	"regexp"
	"strconv"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay3() []string {
	lines := ReadLines("../inputs/3a.txt")
	return lines
}

func Test_Day_3_A(t *testing.T) {
	data := getDataDay3()
	result := 0

	re := regexp.MustCompile(`mul\(\d{1,3},\d{1,3}\)`)
	re2 := regexp.MustCompile(`\d{1,3}`)
	for _, line := range data {

		matches := re.FindAllString(line, -1)
		fmt.Println(matches)

		for _, match := range matches {
			matches2 := re2.FindAllString(match, -1)
			l, _ := strconv.Atoi(matches2[0])
			p, _ := strconv.Atoi(matches2[1])
			result += (l * p)
		}
	}

	assert.EqualValues(t, 161, result)
}

func Test_Day_3_B(t *testing.T) {
	data := getDataDay3()
	result := 0

	re := regexp.MustCompile(`do(?:n't)?\(\)|mul\(\d{1,3},\d{1,3}\)`)
	re2 := regexp.MustCompile(`\d{1,3}`)
	skip := false

	for _, line := range data {

		matches := re.FindAllString(line, -1)

		for _, match := range matches {
			if skip && match != "do()" {
				continue
			}
			if match == "don't()" {
				skip = true
				continue
			} else if match == "do()" {
				skip = false
				continue
			}

			matches2 := re2.FindAllString(match, -1)
			l, _ := strconv.Atoi(matches2[0])
			p, _ := strconv.Atoi(matches2[1])
			result += (l * p)
		}
	}

	assert.EqualValues(t, 48, result)
}
