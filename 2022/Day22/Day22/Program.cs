using Day22;

var input = File.ReadAllText("input.txt");
var exampleInput = File.ReadAllText("example-input.txt");

var inputSplit = input.Split("\n\n");
var mapString = inputSplit[0];
var directionsString = inputSplit[1].TrimEnd('\n');

var map = MapFunctions.CreateMapFromInputString(mapString);
var startingPosition = MapFunctions.GetStartingSquare(map);
var commands = MapFunctions.SplitIntoCommands(directionsString);

int overallToRight = commands.Count(x => x is 'R') - commands.Count(x => x is 'L');

var lastLocation = MapFunctions.FollowCommandsOnMap(map, new Location(startingPosition, Facing.Right), commands);
var finalPassword = (lastLocation.Position.Y + 1) * 1000 + (lastLocation.Position.X + 1) * 4 + (int)lastLocation.Facing;

Console.WriteLine($"Overall to right: {overallToRight}");
Console.WriteLine($"Last location: {lastLocation}");
Console.WriteLine($"Final password: {finalPassword}");