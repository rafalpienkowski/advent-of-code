package days

import (
	"bufio"
	"fmt"
	"os"
	"slices"
	"strconv"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay9() []rune {
	var data []rune

	readFile, err := os.Open("../inputs/9.txt")
	if err != nil {
		fmt.Println(err)
	}

	fileScanner := bufio.NewScanner(readFile)

	for fileScanner.Scan() {
		line := fileScanner.Text()
		data = []rune(line)
	}
	readFile.Close()

	return data
}

func checkSum(memory []rune) int {
	idx := 0
	sum := 0
	last := len(memory) - 1
	a := newAllock(memory, last, last/2)

	for i := 0; i < len(memory); i++ {

		if i > a.Idx {
			continue
		}
		if i == last {
			for j := 0; j < a.Available; j++ {
				sum += idx * a.Value
				idx++
			}
			continue
		}

		curr, _ := strconv.Atoi(string(memory[i]))
		for j := 0; j < curr; j++ {

			if i%2 == 0 {
				sum += idx * (i / 2)
			} else {
				if a.Available == 0 {
					last -= 2
					a = newAllock(memory, last, last/2)
				}
				if i > a.Idx {
					continue
				}
				sum += idx * a.Value
				a.Available--
			}
			idx++
		}
	}
	fmt.Println()

	return sum
}

func findData(data []Allock, size int) (Allock, []Allock, bool) {
	for i := (len(data) - 1); i >= 0; i-- {
		if data[i].Idx <= size {
			return data[i], append(data[:i], data[i+1:]...), true
		}
	}
	return data[len(data)-1], data, false
}

type Allock struct {
	Value     int
	Available int
	Idx       int
}

func newAllock(memory []rune, idx int, value int) Allock {
	v, _ := strconv.Atoi(string(memory[idx]))
	return Allock{Value: value, Available: v, Idx: idx}
}

type DiskLoc struct {
	Free  bool
	Value int
	Size  int
}

func findNonFreeIdx(loc []DiskLoc, size int) int {
	for i := len(loc) - 1; i > 0; i-- {
		if !loc[i].Free && loc[i].Size <= size {
			return i
		}
	}
	return -1
}

func printLoc(input []DiskLoc) {
	disk := strings.Builder{}
	for _, i := range input {
		if i.Free {
			for j := 0; j < i.Size; j++ {
				disk.WriteString(".")
			}
		} else {
			for j := 0; j < i.Size; j++ {
				disk.WriteString(fmt.Sprintf("%v", i.Value))
			}
		}
	}
	fmt.Printf("%v\n", disk.String())
}

func checkSum2(memory []rune) int {
	var input []DiskLoc

	idx := 0
	for i := 0; i < len(memory); i++ {
		curr, _ := strconv.Atoi(string(memory[i]))
		if curr == 0 {
			continue
		}
		free := i%2 == 1
		value := 0
		if !free {
			value = idx
			idx++
		}
		l := DiskLoc{Free: free, Value: value, Size: curr}
		input = append(input, l)
	}

	printLoc(input)

	for i := len(input) - 1; i > 0; i-- {
		if input[i].Free {
			continue
		}
		for j := 0; j < i; j++ {
			if !input[j].Free {
				continue
			}

			if input[i].Size <= input[j].Size {


				right := input[i]
				left := input[j]
				//fmt.Printf("Swapping: %v with %v\n", right, left)
				//fmt.Printf("Swapping: %v with %v\n", i, j)

				gap := left.Size - right.Size
				input[j] = right
				left.Size = right.Size
				input[i] = left

				//gap
				if gap > 0 {
					g := DiskLoc{Free: true, Value: 0, Size: gap}
					input = slices.Insert(input, j+1, g)
				}

				printLoc(input)
				break
			}
		}
	}

	fmt.Println()
	printLoc(input)

	sum := 0

	idx = 0
	for i := range input {
		for j := 0; j < input[i].Size; j++ {
			sum += idx * input[i].Value
			idx++
		}
	}

	return sum
}

func Day_9(t *testing.T) {
	mem := getDataDay9()
	//result := checkSum(mem)
	result2 := checkSum2(mem)

	//assert.EqualValues(t, 6337367222422, result)
	assert.EqualValues(t, 2857, result2)

	//6421320153831 -- to high
}
