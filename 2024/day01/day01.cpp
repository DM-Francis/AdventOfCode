#include <algorithm>
#include <cstdio>
#include <iostream>
#include <map>
#include <string>
#include <vector>

using namespace std;

vector<string> split(string input, string delimiter) {
    vector<string> output;
    int start = 0;
    int del_pos = -1;
    while ((del_pos = input.find(delimiter, del_pos + 1)) >= 0) {
	/*cout << del_pos << endl;*/
	auto value = input.substr(start, del_pos);
	output.push_back(value);
	start = del_pos + delimiter.length();
    }

    auto value = input.substr(start, del_pos);
    output.push_back(value);

    return output;
}

int main() {
    string line;
    vector<int> left;
    vector<int> right;

    while (getline(cin, line)) {
	auto splitline = split(line, "   ");
	left.push_back(stoi(splitline[0]));
	right.push_back(stoi(splitline[1]));
    }

    sort(left.begin(), left.end());
    sort(right.begin(), right.end());

    int totalDistance = 0;
    cout << "Part One:\n";
    for (int i = 0; i < left.size(); i++) {
	int distance = abs(left[i] - right[i]);
	totalDistance += distance;
    }

    cout << to_string(totalDistance) + "\n\n";

    cout << "Part Two:\n";
    int score = 0;
    for (int i = 0; i < left.size(); i++) {
	int leftVal = left[i];
	int rightCount = count(right.begin(), right.end(), leftVal);
	score += leftVal * rightCount;
    }

    cout << "Score: " + to_string(score) + "\n";

    return 0;
}

