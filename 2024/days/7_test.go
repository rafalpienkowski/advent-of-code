package days

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay7() map[int][]int {
	data := make(map[int][]int)

	readFile, err := os.Open("../inputs/7a.txt")
	if err != nil {
		fmt.Println(err)
	}

	fileScanner := bufio.NewScanner(readFile)

	for fileScanner.Scan() {
		line := fileScanner.Text()
		parts := strings.Split(line, ":")

		main, err := strconv.Atoi(strings.TrimSpace(parts[0]))
		if err != nil {
			fmt.Println(err)
		}

		numStrs := strings.Fields(parts[1])
		nums := make([]int, len(numStrs))
		for i, numStr := range numStrs {
			num, err := strconv.Atoi(numStr)
			if err != nil {
				fmt.Println(err)
			}
			nums[i] = num
		}

		data[main] = nums
	}
	readFile.Close()

	return data
}

func generateCombinations(options []string, length int) [][]string {
	if length == 0 {
		return [][]string{{}}
	}

	var combinations [][]string

	subCombinations := generateCombinations(options, length-1)
	for _, option := range options {
		for _, sub := range subCombinations {
			combinations = append(combinations, append([]string{option}, sub...))
		}
	}

	return combinations
}

func Day_7(t *testing.T) {
	data := getDataDay7()
	//ans1 := 0
	ans2 := 0
	options := []string{"add", "mul", "con"}

	for expected := range data {
		numbers := data[expected]
		var good bool
		for _, ops := range generateCombinations(options, len(numbers)-1) {
			result := numbers[0]
			for n := 1; n < len(numbers); n++ {
				op := ops[n-1]
				switch op {
				case "add":
					result = result + numbers[n]
				case "mul":
					result = result * numbers[n]
				default:
					r := strconv.Itoa(result)
					m := strconv.Itoa(numbers[n])
					result, _ = strconv.Atoi(r + m)
				}
			}
			if result == expected {
				good = true
				break
			}
		}
		if good {
			ans2 += expected
		}
	}

	/*
    //Part 1
				for opmask := range 1<<len(numbers) - 1 {
					result := numbers[0]
					for n := 1; n < len(numbers); n++ {
						op := (opmask >> (n - 1) & 1)
						if op == 1 {
							result = result * numbers[n]
						} else {
							result = result + numbers[n]
						}
						if result == expected {
		                    good = true
							break
						}
					}
				}
		if good {
			ans2 += expected
		}
	*/

	//assert.EqualValues(t, 6231007345478, ans1)
	assert.EqualValues(t, 333027885676693, ans2)
}
