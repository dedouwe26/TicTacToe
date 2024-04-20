namespace TTT.Modules;

public class SmartBotModule : Module {
    public override string Name => "Smart bot";

    protected override byte Choose() {
        Grid<byte> priorityGrid = GetPriority();
        (byte index, byte value) highestPriority = (9, byte.MaxValue);
        for (byte i = 0; i < priorityGrid.Size; i++) {
            if (priorityGrid[i] < highestPriority.value) {
                highestPriority = (i, priorityGrid[i]);
            }
        }
        return highestPriority.index;
    }

    private Grid<byte> GetPriority() {
        Grid<byte> priorityGrid = new(byte.MaxValue);

        Grid<byte> possibleWinsGrid = GetPossibleWins();
        byte highest = possibleWinsGrid.Max();
        for (byte i = 0; i < possibleWinsGrid.Size; i++) {
            if (Grid[i]==TileState.Empty) {
                priorityGrid[i] = (byte)(highest - possibleWinsGrid[i] + 1);
            }
        }

        for (byte i = 0; i < Grid.Size; i++) {
            foreach (Line line in GetLines(i)) {
                if (IsAlmostClaimed(line)&&Grid[i]==TileState.Empty) {
                    priorityGrid[i] = 0;
                    return priorityGrid;
                }
            }
        }

        for (byte i = 0; i < Grid.Size; i++) {
            foreach (Line line in GetLines(i)) {
                if (IsAlmostEnemyClaimed(line)&&Grid[i]==TileState.Empty) {
                    priorityGrid[i] = 0;
                }
            }
        }
        
        return priorityGrid;
    }
    private Grid<byte> GetPossibleWins() {
        Grid<byte> possibleWins = new(0);
        foreach (Line line in TicTacToe.AllLines) {
            if (IsWinnable(line)) {
                possibleWins[line.start]++;
                possibleWins[line.middle]++;
                possibleWins[line.end]++;
            }
        }
        return possibleWins;
    }
    private static List<Line> GetLines(byte tile) {
        List<Line> lines = [];
        foreach (Line line in TicTacToe.AllLines) {
            if (line.start==tile||line.middle==tile||line.end==tile) {
                lines.Add(line);
            }
        }
        return lines;
    }
    private bool IsAlmostClaimed(Line line) {
        return new TileState[]{Grid[line.start], Grid[line.middle], Grid[line.end]}.Count(b => b==ClaimedState)==2;
    }
    private bool IsAlmostEnemyClaimed(Line line) {
        return new TileState[]{Grid[line.start], Grid[line.middle], Grid[line.end]}.Count(b => b==EnemyClaimedState)==2;
    }
    private bool IsWinnable(Line line) {
        return (Grid[line.start] != EnemyClaimedState) &&
               (Grid[line.middle] != EnemyClaimedState) &&
               (Grid[line.end] != EnemyClaimedState);
    }
}