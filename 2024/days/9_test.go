package days

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
	"testing"
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

func checkSum2(memory []rune) int {
	disk := strings.Builder{}
	var input []DiskLoc
	var final []DiskLoc
	d := make(map[int][]DiskLoc)

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

	s := len(input) - 1
	for i := s; i >= 0; i-- {
		curr := input[i]
		if curr.Free {
			continue
		}
        d[curr.Size] = append(d[curr.Size], curr)
	}

    fmt.Printf("D: %v\n", d)

	for i := s; i >= 0; i-- {
		j := s - i
		begin := input[j]
		if !begin.Free {
			final = append(final, begin)
			continue
		}

		fi := findNonFreeIdx(input, begin.Size)
		if fi > 0 {
			tmp := input[fi]
			final = append(final, tmp)

			tmp.Free = true
			tmp.Value = 0
			input[fi] = tmp
			begin.Size = begin.Size - tmp.Size
		}

		if begin.Size > 0 {
			final = append(final, begin)
		}

	}

	disk = strings.Builder{}
	fmt.Printf("Final:\n")
	for _, f := range final {
		for s := 0; s < f.Size; s++ {
			if f.Free {
				disk.WriteString(".")
			} else {
				disk.WriteString(fmt.Sprintf("%v", f.Value))
			}
		}
	}
	fmt.Println(disk.String())
	//0099.111777244.3332...5555.6666.333.8888..
	//00992111777.44.333....5555.6666.....8888..

	sum := 0
	return sum
}

func Test_Day_9(t *testing.T) {
	//getDataDay9()
	//result := checkSum(mem)
	//checkSum2(mem)

	//assert.EqualValues(t, 6337367222422, result)
	//assert.EqualValues(t, 2858, result2)
}
