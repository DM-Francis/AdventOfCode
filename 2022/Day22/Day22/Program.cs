using Day22;
using Day22.Rendering;
using Raylib_cs;

var input = File.ReadAllText("input.txt");
// var exampleInput = File.ReadAllText("example-input.txt");



var inputSplit = input.Split("\n\n");
var mapString = inputSplit[0];
var directionsString = inputSplit[1].TrimEnd('\n');

var map = MapFunctions.CreateMapFromInputString(mapString);
var startingPosition = MapFunctions.GetStartingSquare(map);
var commands = MapFunctions.SplitIntoCommands(directionsString);

int overallToRight = commands.Count(x => x is 'R') - commands.Count(x => x is 'L');

Raylib.InitWindow(map.GetLength(0) * 3, map.GetLength(1) * 3, "Day 22");
Raylib.SetTargetFPS(60);

IReadOnlyList<Location> path = new List<Location>();
var runningTask = Task.Run(() => MapFunctions.FollowCommandsOnMap(
    map, new Location(startingPosition, Facing.Right), commands, true,
    TimeSpan.FromMilliseconds(10),
    x => path = x));

var renderer = new RaylibRenderer();

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.BLACK);

    renderer.Draw(map, path);

    Raylib.EndDrawing();
}

var lastLocation = runningTask.Result;
var finalPassword = (lastLocation.Position.Y + 1) * 1000 + (lastLocation.Position.X + 1) * 4 + (int)lastLocation.Facing;

Console.WriteLine($"Overall to right: {overallToRight}");
Console.WriteLine($"Last location: {lastLocation}");
Console.WriteLine($"Final password: {finalPassword}");

void UpdatePath(List<Location> updatedPath)
{
    path = updatedPath;
}