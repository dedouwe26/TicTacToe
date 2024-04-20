using System.Collections;

namespace TTT;

public class Grid<T> : IEnumerable<T> {
    public readonly T[] grid = new T[9];
    public Grid(T defaultValue) {
        Array.Fill(grid, defaultValue);
    }
    public Grid(T p0, T p1, T p2, T p3, T p4, T p5, T p6, T p7, T p8) {
        grid = [p0, p1, p2, p3, p4, p5, p6, p7, p8];
    }
    public readonly int Size = 9;
    public T this[byte index] {
        get {
            return grid[index];
        }
        set {
            grid[index] = value;
        }
    }

    public IEnumerator<T> GetEnumerator() {
        return ((IEnumerable<T>)grid).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}

public class ReadOnlyGrid<T> : IEnumerable<T> {
    private readonly T[] grid = new T[9];
    public ReadOnlyGrid(Grid<T> grid) {
        this.grid = grid.grid;
    }
    public readonly int Size = 9;
    public T this[byte index] {
        get {
            return grid[index];
        }
    }

    public IEnumerator<T> GetEnumerator() {
        return ((IEnumerable<T>)grid).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}