namespace Day22.Rendering;

public interface IRenderer
{
    void Draw(MapSquare[,] map, IReadOnlyList<Location> path);
}