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
        calibrationValueString := fmt.Sprint(first) + fmt.Sprint(last)
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

func getFirstDigit(str string) int {
    for i := 0; i < len(str); i++ {
        digit, found := checkForDigitAtIndex(str, i)
        if found {
            return digit
        }
    }

    panic("No digit found")
}

func getLastDigit(str string) int {
    for i := len(str) - 1; i >= 0; i-- {
        digit, found := checkForDigitAtIndex(str, i)
        if found {
            fmt.Printf("Found digit: %v\n", digit)
            return digit
        }
    }

    panic("No digit found")
}

func checkForDigitAtIndex(str string, i int) (digit int, found bool) {
    ch := str[i]
    switch {
    case ch >= '0' && ch <= '9':
        return int(ch - 48), true
    case alsoCheckWords == false:
        return 0, false
    case stringContainsWordBeforeIndex(str, i, "one"):
        return 1, true
    case stringContainsWordBeforeIndex(str, i, "two"):
        return 2, true
    case stringContainsWordBeforeIndex(str, i, "three"):
        return 3, true
    case stringContainsWordBeforeIndex(str, i, "four"):
        return 4, true
    case stringContainsWordBeforeIndex(str, i, "five"):
        return 5, true
    case stringContainsWordBeforeIndex(str, i, "six"):
        return 6, true
    case stringContainsWordBeforeIndex(str, i, "seven"):
        return 7, true
    case stringContainsWordBeforeIndex(str, i, "eight"):
        return 8, true
    case stringContainsWordBeforeIndex(str, i, "nine"):
        return 9, true
    default:
        return 0, false
    }
}

func stringContainsWordBeforeIndex(str string, i int, word string) bool {
    startIndex := i - len(word) + 1
    if startIndex < 0 {
        return false
    }
    
    subStr := str[startIndex:i+1]
    return subStr == word
}
