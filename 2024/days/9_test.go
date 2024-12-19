package days

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"testing"

	"github.com/stretchr/testify/assert"
)

func getDataDay9() []rune {
	var data []rune

	readFile, err := os.Open("../inputs/9a.txt")
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

type Block struct {
	Pos, Size int
}

func checkSum3(memory []rune) int {
	files := make(map[int]Block)
	var blanks []Block
	fid := 0
	pos := 0

	for i := 0; i < len(memory); i++ {
		size, _ := strconv.Atoi(string(memory[i]))
		if i%2 == 0 {
			files[fid] = Block{Pos: pos, Size: size}
			fid++
		} else {
			if size > 0 {
				blanks = append(blanks, Block{Pos: pos, Size: size})
			}
		}

		pos += size
	}

    for fid > 0 {
        fid--
        file := files[fid]
        for i, b := range blanks {
            if b.Pos > file.Pos {
                blanks = blanks[:i]
                break
            }
            if file.Size <= b.Size {
                file.Pos = b.Pos
                files[fid] = file
                if file.Size == b.Size {
                    blanks = append(blanks[:i], blanks[i+1:]...)
                } else {
                    b.Size -= file.Size
                    b.Pos += file.Size
                    blanks[i] = b
                }
                break
            }
        }
    }

	sum := 0

    for fid, f := range files {
        x := f.Pos
        e := x + f.Size
        for x < e {
            sum += fid * x
            x++
        }
    }

	return sum
}

func Day_9(t *testing.T) {
	mem := getDataDay9()
	result := checkSum(mem)
	result2 := checkSum3(mem)

	assert.EqualValues(t, 6337367222422, result)
	assert.EqualValues(t, 6361380647183, result2)
}
