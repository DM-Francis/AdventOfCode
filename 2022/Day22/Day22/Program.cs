using Day22;
using Day22.MapTypes;

var input = File.ReadAllText("input.txt");
var exampleInput = File.ReadAllText("example-input.txt");

var inputSplit = input.Split("\n\n");
var mapString = inputSplit[0];
var directionsString = inputSplit[1].TrimEnd('\n');

var map = MapFunctions.CreateMapFromInputString(mapString);
var startingPosition = MapFunctions.GetStartingSquare(map);
var commands = MapFunctions.SplitIntoCommands(directionsString);

var lastLocation = MapFunctions.FollowCommandsOnMap(map, new Location(startingPosition, Facing.Right), commands);
var finalPassword = (lastLocation.Position.Y + 1) * 1000 + (lastLocation.Position.X + 1) * 4 + (int)lastLocation.Facing;

Console.WriteLine($"Final password: {finalPassword}");
// Gets the wrong answer for the actual input - must be a subtle bug somewhere