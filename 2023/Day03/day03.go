package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func main() {
	file, err := os.ReadFile("input")
	if err != nil {
		panic(err)
	}

	numbers := getNumbersFromSchematic(file)
	sum := 0
	for _, num := range numbers {
		if num.isPartNumber {
			sum += num.value
		}
	}

	fmt.Printf("Sum of part numbers: %v\n", sum)
}

type Coord struct {
	x	int
	y	int
}

type Number struct {
	value		int
	startPos	Coord
	length		int
	isPartNumber	bool
}

func getNumbersFromSchematic(schematic []byte) []Number {
	lines := strings.Split(string(schematic), "\n")
	xLen := len(lines[0])
	yLen := len(lines) - 1

	numbers := make([]Number, 0)

	inNumber := false
	currentNumberStartX := 0
	currentNumberChars := make([]byte, 0)
	for y := 0; y < yLen; y++ {
		for x := 0; x < xLen; x++ {
			value := lines[y][x]

			switch {
			case value >= '0' && value <= '9' && !inNumber: // Hit first character in a number
				inNumber = true
				currentNumberChars = append(currentNumberChars, value)
				currentNumberStartX = x
			case value >= '0' && value <= '9' && inNumber:  // Hit the 2nd or later character in a number
				currentNumberChars = append(currentNumberChars, value)
			case inNumber: // Hit the first non-numberic character after a string of numbers
				num := createNumber(currentNumberChars, currentNumberStartX, y, lines)
				numbers = append(numbers, num)
				inNumber = false
				currentNumberChars = make([]byte, 0)
			default:
				inNumber = false
			}
		}

		if inNumber {
			num := createNumber(currentNumberChars, currentNumberStartX, y, lines)
			numbers = append(numbers, num)
			inNumber = false
			currentNumberChars = make([]byte, 0)
		}
	}

	return numbers
}

func createNumber(numberChars []byte, x int, y int, schematic []string) Number {
	numberString := string(numberChars)
	numberValue, err := strconv.Atoi(numberString)
	if err != nil {
		panic(err)
	}

	startPos := Coord{x,y}
	length := len(numberChars)
	fmt.Printf("startPos: %v, len: %v\n", startPos, length)
	isPartNumber := testIfNumIsPartNumber(Coord{x,y}, len(numberChars), schematic)

	number := Number{numberValue, startPos, length, isPartNumber}
	fmt.Printf("Parsed number: %v\n", number)
	return number
}

func testIfNumIsPartNumber(startPos Coord, length int, schematic []string) bool {
	// Test above number
	for i := -1; i < length + 1; i++ {
		x := startPos.x + i
		y := startPos.y - 1

		if coordIsSymbol(x, y, schematic) {
			return true
		}
	}

	// Test in front and behind number
	if coordIsSymbol(startPos.x - 1, startPos.y, schematic) {
		return true
	}
	if coordIsSymbol(startPos.x + length, startPos.y, schematic) {
		return true
	}

	// Test below number
	for i := -1; i < length + 1; i++ {
		x := startPos.x + i
		y := startPos.y + 1

		if coordIsSymbol(x, y, schematic) {
			return true
		}
	}

	return false
}

func coordIsSymbol(x int, y int, schematic []string) bool {
	if x < 0 || y < 0 || x >= len(schematic[0]) - 1 || y >= len(schematic) - 1 {
		return false;
	}

	value := schematic[y][x]
	if value != '.' && !(value >= '0' && value <= '9') {
		return true
	}

	return false
}

