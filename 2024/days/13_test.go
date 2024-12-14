package days

import (
	"regexp"
	"strconv"
	"testing"

	"github.com/stretchr/testify/assert"
)

type Arcade struct {
	A, B  Point
	Prize Point
}

func getDataDay13() []Arcade {
	lines := ReadLines("../inputs/13.txt")
	var arcades []Arcade

	btnPattern := `X\+(\d+), Y\+(\d+)`
	btnRe := regexp.MustCompile(btnPattern)

	pricePattern := `X=(\d+), Y=(\d+)`
	priceRe := regexp.MustCompile(pricePattern)

	i := 0
	for i < len(lines) {
		if len(lines[i]) == 0 {
			i++
			continue
		}

		matches := btnRe.FindStringSubmatch(lines[i])
		x, _ := strconv.Atoi(matches[1])
		y, _ := strconv.Atoi(matches[2])
		a := Point{X: x, Y: y}
		i++

		matches = btnRe.FindStringSubmatch(lines[i])
		x, _ = strconv.Atoi(matches[1])
		y, _ = strconv.Atoi(matches[2])
		b := Point{X: x, Y: y}
		i++

		matches = priceRe.FindStringSubmatch(lines[i])
		x, _ = strconv.Atoi(matches[1])
		y, _ = strconv.Atoi(matches[2])
		p := Point{X: x + 10000000000000, Y: y + 10000000000000}
		i++

		arcades = append(arcades, Arcade{A: a, B: b, Prize: p})
	}
	return arcades
}

func calculateGames(arcades []Arcade) int {
	//A - 3
	//B - 1
	result := 0
	for _, a := range arcades {
		for i := range 100 {
			for j := range 100 {
				if a.A.X*i+a.B.X*j == a.Prize.X &&
					a.A.Y*i+a.B.Y*j == a.Prize.Y {
					result += 3*i + j
					break
				}
			}
		}
	}
	return result
}


func Day_13(t *testing.T) {
	arcades := getDataDay13()
	result1 := calculateGames(arcades)

	assert.EqualValues(t, 480, result1)
	//assert.EqualValues(t, 39996, result1)
	//assert.EqualValues(t, 1206, result2)
	//assert.EqualValues(t, 865662, result2)

}
