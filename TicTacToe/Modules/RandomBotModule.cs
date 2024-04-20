namespace TTT.Modules;

public class RandomBotModule : Module {
    public override string Name => "Random bot";
    private readonly Random random = new();

    public override byte Choose() {
        List<byte> possibleOptions = [];
        for (byte b = 0; b < Grid.Size; b++) {
            if (Grid[b] == TileState.Empty) {
                possibleOptions.Add(b);
            }
        }
        return possibleOptions[random.Next(possibleOptions.Count)];
    }
}