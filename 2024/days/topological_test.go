package days

import (
	"fmt"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getData() (map[[2]int]Rule, [][]int) {

	lines := ReadLines("../inputs/5.txt")
	pages := make([][]int, len(lines))
	rules := make(map[[2]int]Rule)
	parseR := true
	lineIdx := 0

	for _, line := range lines {
		if len(line) == 0 {
			parseR = false
			continue
		}

		if parseR {
			ruleStr := strings.Split(line, "|")
			l, err := strconv.Atoi(ruleStr[0])
			if err != nil {
				fmt.Println("Invalid numbers. Please enter valid integers.")
				continue
			}
			r, err := strconv.Atoi(ruleStr[1])
			if err != nil {
				fmt.Println("Invalid numbers. Please enter valid integers.")
				continue
			}

			rules[[2]int{l, r}] = Rule{Second: r, First: l}

		} else {
			pageStr := strings.Split(line, ",")
			var numbers []int
			for _, p := range pageStr {
				l, err := strconv.Atoi(p)
				if err != nil {
					fmt.Println("Invalid numbers. Please enter valid integers.")
					continue
				}
				numbers = append(numbers, l)
			}
			pages[lineIdx] = numbers
			lineIdx++
		}
	}

	return rules, pages[:lineIdx]
}

func topologicalSort2(rules []Rule, numbers []int) []int {
	result := make([]int, 0)
	graph := make(map[int][]int)
	inDegree := make(map[int]int)

	for _, node := range numbers {
		graph[node] = []int{}
		inDegree[node] = 0
	}

	for _, r := range rules {
		inDegree[r.Second]++
		graph[r.First] = append(graph[r.First], r.Second)
	}

	queue := []int{}
	for node, degree := range inDegree {
		if degree == 0 {
			queue = append(queue, node)
		}
	}
	for len(queue) > 0 {
		current := queue[0]
		queue = queue[1:]
		result = append(result, current)

		for _, neighbor := range graph[current] {
			inDegree[neighbor]--
			if inDegree[neighbor] == 0 {
				queue = append(queue, neighbor)
			}
		}
	}

	return result
}

func Test_Topological_Sort(t *testing.T) {

	rules, pages := getData()

	for i, page := range pages {
		efRules := getRules(page, rules)
		newPage := topologicalSort2(efRules, page)
		if i == 0 {
			assert.EqualValues(t, []int{97, 75, 47, 61, 53}, newPage)
		}
		if i == 1 {
			assert.EqualValues(t, []int{61, 29, 13}, newPage)
		}
		if i == 2 {
			assert.EqualValues(t, []int{97, 75, 47, 29, 13}, newPage)
		}
	}
}
