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

	lines := getLines(file)
	games := make([]Game, 0)

	idSum := 0
	powerSum := 0
	for _, line := range lines {
		if line == "" {
			continue
		}

		game, err := parseGame(line)
		if err != nil {
			panic(err)
		}

		games = append(games, game)
		fmt.Println(game)
		if possibleWithMax(game, 12, 14, 13) {
			idSum += game.id
		}

		powerSum += getPowerOfMinimumCubes(game)
	}

	fmt.Printf("Sum of ids of possible games: %v\n", idSum)
	fmt.Printf("Sum of powers of minimum sets: %v\n", powerSum)
}

func getLines(fileData []byte) []string {
	return strings.Split(string(fileData), "\n")
}

type Game struct {
	id      int
	reveals []Reveal
}

type Reveal struct {
	red   int
	blue  int
	green int
}

func possibleWithMax(game Game, red, blue, green int) bool {
	for _, reveal := range game.reveals {
		if reveal.red > red || reveal.blue > blue || reveal.green > green {
			return false
		}
	}

	return true
}

func getPowerOfMinimumCubes(game Game) int {
	maxRed := 0
	maxBlue := 0
	maxGreen := 0

	for _, reveal := range game.reveals {
		if reveal.red > maxRed {
			maxRed = reveal.red
		}
		if reveal.blue > maxBlue {
			maxBlue = reveal.blue
		}
		if reveal.green > maxGreen {
			maxGreen = reveal.green
		}
	}

	return maxRed * maxBlue * maxGreen
}

func parseGame(rawGame string) (Game, error) {
	split := strings.Split(rawGame, ": ")
	header := split[0]
	data := split[1]
	id, err := strconv.Atoi(strings.Fields(header)[1])
	if err != nil {
		return Game{}, err
	}

	reveals := make([]Reveal, 0)
	rawReveals := strings.Split(data, "; ")
	for _, rawReveal := range rawReveals {
		reveal, err := parseReveal(rawReveal)
		if err != nil {
			return Game{}, err
		}
		reveals = append(reveals, reveal)
	}

	return Game{id, reveals}, nil
}

func parseReveal(rawReveal string) (Reveal, error) {
	red := 0
	blue := 0
	green := 0
	parts := strings.Split(rawReveal, ", ")
	for _, part := range parts {
		temp := strings.Fields(part)
		num, err := strconv.Atoi(temp[0])
		if err != nil {
			return Reveal{}, err
		}
		switch color := temp[1]; color {
		case "red":
			red = num
		case "blue":
			blue = num
		case "green":
			green = num
		default:
			return Reveal{}, fmt.Errorf("Unrecognised color: %v", color)
		}
	}

	return Reveal{red, blue, green}, nil
}
