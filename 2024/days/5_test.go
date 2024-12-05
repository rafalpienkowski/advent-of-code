package days

import (
	"fmt"
	"maps"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Rule struct {
	Second, First int
}

func getDataDay5() (map[[2]int]Rule, [][]int) {
	lines := ReadLines("../inputs/5a.txt")
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

func getRules(page []int, rules map[[2]int]Rule) []Rule {
	filtered := make(map[[2]int]Rule, len(rules))

	for r := range maps.Values(rules) {
		for _, p := range page {
			if (r.Second == p && contains(page, r.First)) ||
				(r.First == p && contains(page, r.Second)) {
				filtered[[2]int{r.Second, r.First}] = r
			}
		}
	}

	var final []Rule
	for r := range maps.Values(filtered) {
		final = append(final, r)
	}

	return final
}

func contains(slice []int, value int) bool {
	for _, v := range slice {
		if v == value {
			return true
		}
	}
	return false
}

func checkOrder(rules []Rule, sequence []int) bool {
	indexMap := make(map[int]int)
	for i, num := range sequence {
		indexMap[num] = i
	}

	for _, rule := range rules {
		first := rule.First
		second := rule.Second

		if indexMap[first] >= indexMap[second] {
			return false
		}
	}

	return true
}

func topologicalSort(rules []Rule, sequence []int) []int {
	graph := make(map[int][]int)
	inDegree := make(map[int]int)

	for _, node := range sequence {
		graph[node] = []int{}
		inDegree[node] = 0
	}

	for _, rule := range rules {
		from, to := rule.First, rule.Second
		graph[from] = append(graph[from], to)
		inDegree[to]++
	}

	queue := []int{}
	for node, degree := range inDegree {
		if degree == 0 {
			queue = append(queue, node)
		}
	}

	var sorted []int
	for len(queue) > 0 {
		current := queue[0]
		queue = queue[1:]
		sorted = append(sorted, current)

		for _, neighbor := range graph[current] {
			inDegree[neighbor]--
			if inDegree[neighbor] == 0 {
				queue = append(queue, neighbor)
			}
		}
	}

    return sorted
}

func Test_Day_5_A(t *testing.T) {

	rules, pages := getDataDay5()
	sum := 0
	sum2 := 0

	for _, page := range pages {
		efRules := getRules(page, rules)
		result := checkOrder(efRules, page)
		if result {
			sum += page[len(page)/2]
		} else {
			newPage := topologicalSort(efRules, page)
			sum2 += newPage[len(newPage)/2]
		}
	}

	assert.EqualValues(t, 5651, sum)
	assert.EqualValues(t, 4743, sum2)
}
