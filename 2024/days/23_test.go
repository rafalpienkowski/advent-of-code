package days

import (
	"fmt"
	"sort"
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

func containsString(slice []string, value string) bool {
	for _, v := range slice {
		if v == value {
			return true
		}
	}
	return false
}

func solve23(nodes map[string][]string) (int, string) {
	result := 0
    seen := make(map[string]bool)

    for k, v := range nodes {
        fmt.Printf("Node %v: %v\n", k, v)
    }

	for ka, a := range nodes {
		astarts := strings.HasPrefix(ka, "t")
		for _, b := range a {
			if !containsString(nodes[b], ka) {
				continue
			}
			bstarts := strings.HasPrefix(b, "t")
			for _, kc := range nodes[b] {
				cstarts := strings.HasPrefix(kc, "t")

                if !containsString(nodes[kc], b) || !containsString(nodes[kc], ka) {
                    continue
                }

                pair := [] string {ka, b, kc}
                sort.Strings(pair)
                normalized := strings.Join(pair, ",")
                _, s := seen[normalized]
				if (astarts || bstarts || cstarts) && !s {
                    //fmt.Printf("Found %v, %v, %v\n", ka, b, kc)
                    seen[normalized] = true
					result += 1
				}
			}
		}
	}

    /*
    for k, v := range nodes {

        for _, n := range v {

        }
    }
    */

	return result, ""
}

func Day_23(t *testing.T) {
	lanNodes := getDataDay23()
	result1, result2 := solve23(lanNodes)

	assert.EqualValues(t, 7, result1)
	assert.EqualValues(t, "co,de,ka,ta", result2)
	//assert.EqualValues(t, 1154, result1)

}
