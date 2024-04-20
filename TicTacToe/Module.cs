namespace TTT;

public abstract class Module {
    private TicTacToe? game;
    protected TicTacToe Game {
        get {
            return game!;
        }
    }
    public char ID { get { return Game.GetID(this); } }
    protected TileState ClaimedState { get { return TicTacToe.GetClaimedStateFor(ID); } }
    protected TileState EnemyClaimedState { get { return TicTacToe.GetClaimedStateFor(TicTacToe.Opponent(ID)); } }
    protected ReadOnlyGrid<TileState> Grid { get {
        return Game.GetGrid();
    } }
    public abstract string Name { get; }

    public byte Choose(TicTacToe game) {
        this.game = game;
        return Choose();
    }

    protected abstract byte Choose();
    public virtual void End(bool hasWon, bool draw, bool forceStop) { }
}