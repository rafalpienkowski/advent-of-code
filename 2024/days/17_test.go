package days

import (
	"fmt"
	"math"
	"regexp"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay17() (int, int, int, []int) {

	re := regexp.MustCompile(`Register [A-Z]:\s*(-?\d+)`)
	lines := ReadLines("../inputs/17.txt")
	var a, b, c int
	var op []int

	for i, line := range lines {
		matches := re.FindStringSubmatch(line)
		if i == 0 {
			x, _ := strconv.Atoi(matches[1])
			a = x
		}
		if i == 1 {
			x, _ := strconv.Atoi(matches[1])
			b = x
		}
		if i == 2 {
			x, _ := strconv.Atoi(matches[1])
			c = x
		}
		if i == 4 {
			re := regexp.MustCompile(`\d+`)
			matches := re.FindAllString(line, -1)
			for _, match := range matches {
				num, _ := strconv.Atoi(match)
				op = append(op, num)
			}
			break
		}
	}

	return a, b, c, op
}

func calcOp(a int, b int, c int, op []int) []int {
	//fmt.Printf("A: %v, B: %v, C: %v, op: %v\n", a, b, c, op)
	pointer := 0
	var out []int

	combo := func(op int) int {
		if op < 4 {
			return op
		}
		if op == 4 {
			return a
		}
		if op == 5 {
			return b
		}
		if op == 6 {
			return c
		}
		return -1
	}

	for pointer < len(op) {
		//fmt.Printf("Pointer: %v, a: %v, b: %v, c: %v\n", pointer, a, b, c)

		if op[pointer] == 0 {
			numerator := a
			denominator := math.Pow(2, float64(combo(op[pointer+1])))
			result := int(numerator / int(denominator))

			//fmt.Printf("adv with result %v\n", result)
			a = result
		}

		if op[pointer] == 1 {
			result := b ^ op[pointer+1]

			//fmt.Printf("bxl with result %v\n", result)
			b = result
		}

		if op[pointer] == 2 {
			result := (combo(op[pointer+1]) % 8) & 0b111

			//fmt.Printf("bst with result %v\n", int(result))
			b = result
		}

		if op[pointer] == 3 {
			if a == 0 {
				//fmt.Printf("jnz do nothing \n")
			} else {
				result := op[pointer+1]
				//fmt.Printf("jnz jumping to %v\n", result)
				pointer = result
				continue
			}
		}

		if op[pointer] == 4 {
			result := b ^ c

			//fmt.Printf("bxc with result %v\n", result)
			b = result
		}

		if op[pointer] == 5 {
			result := (combo(op[pointer+1]) % 8)

			//fmt.Printf("out with result %v\n", result)
			out = append(out, result)
		}

		if op[pointer] == 6 {
			numerator := a
			denominator := math.Pow(2, float64(combo(op[pointer+1])))
			result := int(numerator / int(denominator))

			fmt.Printf("bdv with result %v\n", result)
			b = result
		}

		if op[pointer] == 7 {
			numerator := a
			denominator := math.Pow(2, float64(combo(op[pointer+1])))
			result := int(numerator / int(denominator))

			//fmt.Printf("cdv with result %v\n", result)
			c = result
		}

		pointer += 2
	}

	return out
}

func printOp(o []int) string {
    var strs []string
	for _, num := range o {
        strs = append(strs,strconv.Itoa(num))
	}

	return strings.Join(strs, ",")
}

func Day_17(t *testing.T) {
	a, b, c, op := getDataDay17()
	result1 := calcOp(a, b, c, op)

	assert.EqualValues(t, "4,6,3,5,6,3,5,2,1,0", printOp(result1))
}
