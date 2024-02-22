package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

var alsoCheckWords bool

func main() {
    file, err := os.ReadFile("input")
    if err != nil {
        panic(err)
    }

    lines := getLines(file)
    var sum int
    alsoCheckWords = true

    for _, line := range lines {

        if line == "" {
            continue
        }

        first := getFirstDigit(line)
        last := getLastDigit(line)
        calibrationValueString := string(first) + string(last)
        calibrationValue, err := strconv.Atoi(calibrationValueString)
        if err != nil {
            panic(err)
        }

        sum += calibrationValue
    }

    fmt.Println(sum)
}

func getLines(fileData []byte) []string {
    return strings.Split(string(fileData), "\n")
}

func getFirstDigit(str string) rune {
    for i := 0; i < len(str); i++ {
        digit, found := checkForDigitAtIndex(str, i)
        if found {
            return digit
        }
    }

    panic("No digit found")
}

func getLastDigit(str string) rune {
    for i := len(str) - 1; i >= 0; i-- {
        digit, found := checkForDigitAtIndex(str, i)
        if found {
            return digit
        }
    }

    panic("No digit found")
}

func checkForDigitAtIndex(str string, i int) (digit rune, found bool) {
    ch := str[i]
    switch {
    case ch >= '0' && ch <= '9':
        return rune(ch), true
    case alsoCheckWords == false:
        return '0', false
    case containsWordAt(str, i, "one"):
        return '1', true
    case containsWordAt(str, i, "two"):
        return '2', true
    case containsWordAt(str, i, "three"):
        return '3', true
    case containsWordAt(str, i, "four"):
        return '4', true
    case containsWordAt(str, i, "five"):
        return '5', true
    case containsWordAt(str, i, "six"):
        return '6', true
    case containsWordAt(str, i, "seven"):
        return '7', true
    case containsWordAt(str, i, "eight"):
        return '8', true
    case containsWordAt(str, i, "nine"):
        return '9', true
    default:
        return 0, false
    }
}

func containsWordAt(str string, i int, word string) bool {
    startIndex := i - len(word) + 1
    if startIndex < 0 {
        return false
    }
    
    subStr := str[startIndex:i+1]
    return subStr == word
}
