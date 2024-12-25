package days

import (
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay23() map[string][]string {
	lanNodes := make(map[string][]string)
	lines := ReadLines("../inputs/23.txt")
	for _, line := range lines {
		parts := strings.Split(line, "-")
		node1 := lanNodes[parts[0]]
		node1 = append(node1, parts[1])
		lanNodes[parts[0]] = node1

		node2 := lanNodes[parts[1]]
		node2 = append(node2, parts[0])
		lanNodes[parts[1]] = node2
	}

	return lanNodes
}

func containsNode(nodes []string, element string) bool {
	for _, v := range nodes {
		if v == element {
			return true
		}
	}
	return false
}

func intersection(slice1, slice2 []string) []string {
	elementMap := make(map[string]bool)
	for _, v := range slice1 {
		elementMap[v] = true
	}

	var result []string
	for _, v := range slice2 {
		if elementMap[v] {
			result = append(result, v)
			delete(elementMap, v)
		}
	}

	return result
}

func solve23(nodes map[string][]string) int {
	result := 0

	for ka, a := range nodes {
		astarts := strings.HasPrefix(ka, "t")
		for kb, b := range nodes {

			if !containsNode(a, kb) {
				continue
			}
			if len(a) <= len(b) {
				continue
			}

			bstarts := strings.HasPrefix(kb, "t")

			cs := intersection(a, b)

			for _, c := range cs {
				if len(c) > len(a) && len(c) > len(b) {
					cstarts := strings.HasPrefix(c, "t")
					if astarts || bstarts || cstarts {
						result += 1
					}
				}
			}
		}
	}

	return result
}

func Day_23(t *testing.T) {
	lanNodes := getDataDay23()
	result1 := solve23(lanNodes)

	assert.EqualValues(t, 7, result1)
	//assert.EqualValues(t, 14869099597, result1)

}
