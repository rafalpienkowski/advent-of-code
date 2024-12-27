package days

import (
	"sort"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Operation struct {
	A, B, OP, R string
}

func getDataDay24() (map[string]bool, []Operation) {
	variables := make(map[string]bool)
	var operations []Operation

	lines := ReadLines("../inputs/24a.txt")

	isVariable := true
	for _, line := range lines {
		if len(line) == 0 {
			isVariable = false
			continue
		}

		if isVariable {
			parts := strings.Split(line, ":")
			name := strings.TrimSpace(parts[0])
			value, _ := strconv.ParseBool(strings.TrimSpace(parts[1]))

			variables[name] = value
		} else {
			parts := strings.Split(line, " -> ")
			leftParts := strings.Fields(parts[0])

			op := Operation{
				A:  leftParts[0],
				OP: leftParts[1],
				B:  leftParts[2],
				R:  parts[1],
			}

			operations = append(operations, op)
		}
	}

	return variables, operations
}

func solve24(variables map[string]bool, operations []Operation) int {

	for len(operations) > 0{
        op := operations[0]
        operations = operations[1:]

		_, oka := variables[op.A]
		_, okb := variables[op.B]
		if !oka {
			//fmt.Printf("Can't find variable A: %v\n", op.A)
            operations = append(operations, op)
            continue
		}
		if !okb {
			//fmt.Printf("Can't find variable B: %v\n", op.B)
            operations = append(operations, op)
            continue
		}
		switch op.OP {
		case "AND":
			variables[op.R] = variables[op.A] && variables[op.B]
		case "XOR":
			variables[op.R] = (!variables[op.A] && variables[op.B]) ||
				(variables[op.A] && !variables[op.B])
		case "OR":
			variables[op.R] = variables[op.A] || variables[op.B]
		}
	}
	var keys []string
	for key := range variables {
		if strings.HasPrefix(key, "z") {
			keys = append(keys, key)
		}
	}
	sort.Sort(sort.Reverse(sort.StringSlice(keys)))
    number := 0
    for _, key := range keys {
        bit := 0
        if variables[key] {
            bit = 1
        }
        number = (number << 1) | bit
    }

	return number
}

func Day_24(t *testing.T) {
	variables, operations := getDataDay24()
	result1 := solve24(variables, operations)

	assert.EqualValues(t, 2024, result1)
	assert.EqualValues(t, 46362252142374, result1)
}
