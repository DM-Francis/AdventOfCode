var input = File.ReadAllLines("input.txt");

var steps = new Dictionary<string, Step>();

foreach (var line in input)
{
    var lineSplit = line.Split(" ");
    string preStepName = lineSplit[1];
    string postStepName = lineSplit[7];

    AddStepIfNotExists(steps, preStepName);
    AddStepIfNotExists(steps, postStepName);

    var preStep = steps[preStepName];
    var postStep = steps[postStepName];
    
    postStep.PrerequisiteSteps.Add(preStep);
    preStep.DependantSteps.Add(postStep);
}

// Part 1
string result = ProcessStepsInOrder(steps.Values);
Console.WriteLine($"Steps processed in order: {result}");

// Part 2
ResetSteps(steps.Values);
int timeElapsed = ProcessStepsWithParallelWorkers(steps.Values, 5);
Console.WriteLine($"Time elapsed to process all steps: {timeElapsed}");


static void AddStepIfNotExists(IDictionary<string, Step> dictionary, string name)
{
    if (!dictionary.ContainsKey(name))
        dictionary.Add(name, new Step{Name = name});
}


static string ProcessStepsInOrder(IReadOnlyCollection<Step> steps)
{
    var orderedSteps = new List<string>(steps.Count);

    while (steps.Any(s => !s.IsDone))
    {
        var stepToProcess = steps.Where(s => !s.IsDone && s.PrerequisiteSteps.Count(p => !p.IsDone) == 0).OrderBy(s => s.Name).First();
        orderedSteps.Add(stepToProcess.Name);
        stepToProcess.IsDone = true;
    }

    return string.Join("", orderedSteps);
}

static int ProcessStepsWithParallelWorkers(IReadOnlyCollection<Step> steps, int workers)
{
    var completedSteps = new List<Step>(steps.Count);

    int currentTime = -1;
    int availableWorkers = workers;
    var inProgressSteps = new List<Step>();
    while (steps.Any(s => !s.IsDone))
    {
        currentTime++;
        availableWorkers += UpdateState(inProgressSteps, currentTime, completedSteps);
        var stepsToStart = steps
            .Where(s => !s.IsDone && !s.IsProcessing && s.PrerequisiteSteps.Count(p => !p.IsDone) == 0)
            .OrderBy(s => s.Name)
            .Take(availableWorkers);

        foreach (var step in stepsToStart)
        {
            step.CompletionTime = currentTime + step.TimeToComplete;
            availableWorkers--;
            inProgressSteps.Add(step);
        }
    }

    return currentTime;
}

static int UpdateState(ICollection<Step> inProgressSteps, int currentTime, ICollection<Step> completedSteps)
{
    var completed = inProgressSteps.Where(s => s.CompletionTime <= currentTime).ToList();
    foreach (var step in completed)
    {
        step.IsDone = true;
        inProgressSteps.Remove(step);
        completedSteps.Add(step);
    }

    return completed.Count;
}

static void ResetSteps(IEnumerable<Step> steps)
{
    foreach (var step in steps)
        step.IsDone = false;
}


public class Step
{
    public required string Name { get; init; }
    public bool IsDone { get; set; }
    public int TimeToComplete => Name[0] - 4;
    public int? CompletionTime { get; set; }
    public bool IsProcessing => !IsDone && CompletionTime.HasValue;

    public List<Step> PrerequisiteSteps { get; } = new();
    public List<Step> DependantSteps { get; } = new();
}