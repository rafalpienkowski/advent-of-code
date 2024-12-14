package days

import (
	"regexp"
	"strconv"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Robot struct {
	Start, Vector Point
}

func getDataDay14() []Robot {
	var robots []Robot
	lines := ReadLines("../inputs/14a.txt")
	re := regexp.MustCompile(`p=(-?\d+),(-?\d+)\s+v=(-?\d+),(-?\d+)`)

	for _, line := range lines {

		matches := re.FindStringSubmatch(line)

		pX, _ := strconv.Atoi(matches[1])
		pY, _ := strconv.Atoi(matches[2])
		vX, _ := strconv.Atoi(matches[3])
		vY, _ := strconv.Atoi(matches[4])

		pointP := Point{X: pX, Y: pY}
		pointV := Point{X: vX, Y: vY}

		robots = append(robots, Robot{Start: pointP, Vector: pointV})
	}

	return robots
}

func calcRobots(robots []Robot) int {
	time := 100
	result := make([]int, 4)

	wide := 101
	tall := 103
	halfw := 50
	halft := 51

	for _, r := range robots {
		r.Start.X = (r.Start.X + r.Vector.X*time) % wide
		if r.Start.X < 0 {
			r.Start.X += wide
		}
		r.Start.Y = (r.Start.Y + r.Vector.Y*time) % tall
		if r.Start.Y < 0 {
			r.Start.Y += tall
		}
		if r.Start.X == halfw || r.Start.Y == halft {
			continue
		}
		if r.Start.X < halfw && r.Start.Y < halft {
			result[0] += 1
		}
		if r.Start.X > halfw && r.Start.Y < halft {
			result[1] += 1
		}
		if r.Start.X < halfw && r.Start.Y > halft {
			result[2] += 1
		}
		if r.Start.X > halfw && r.Start.Y > halft {
			result[3] += 1
		}
	}

	sum := 1
	for _, r := range result {
		sum *= r
	}
	return sum
}

func stepsToTree(robots []Robot) int {
	wide := 101
	tall := 103

	for i := range 25000 {
		seen := make(map[Point]bool)
		for _, r := range robots {
			r.Start.X = (r.Start.X + r.Vector.X*i) % wide
			if r.Start.X < 0 {
				r.Start.X += wide
			}
			r.Start.Y = (r.Start.Y + r.Vector.Y*i) % tall
			if r.Start.Y < 0 {
				r.Start.Y += tall
			}
            seen[r.Start] = true
		}
        if len(seen) == len(robots) {
            return i
        }
	}

	return 0
}

func Day_14(t *testing.T) {
	robots := getDataDay14()
	result1 := calcRobots(robots)
	result2 := stepsToTree(robots)

	assert.EqualValues(t, 228410028, result1)
	assert.EqualValues(t, 8258, result2)

}
