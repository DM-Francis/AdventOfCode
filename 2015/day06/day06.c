#include <stdio.h>
#include <string.h>

void string_split(char* str, char *strings[]) {
	char* token;
	int i = 0;
	while ((token = strtok_r(str, " ", &str))) {
		strings[i++] = token;
	}
}

int main() {
	FILE *fptr;
	fptr = fopen("input.txt", "r");
	char line[100];
	char strings[100][100];

	while (fgets(line, 100, fptr)) {
		string_split(line, strings);
	}

	fclose(fptr);
}
