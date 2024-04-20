using OxDEDTerm;

namespace TTT;

public abstract class Module {
    private TicTacToe? game;
    public TicTacToe Game {
        protected get {
            return game!;
        } set {
            if (game != null) {
                game = value;
            }
        }
    }
    public char ID { get { return Game.GetID(this); } }
    protected TileState ClaimedState { get { return TicTacToe.GetClaimedStateFor(ID); } }
    protected TileState EnemyClaimedState { get { return TicTacToe.GetClaimedStateFor(TicTacToe.Opponent(ID)); } }
    protected ReadOnlyGrid<TileState> Grid { get {
        return Game.GetGrid();
    } }
    public abstract string Name { get; }

    public abstract byte Choose();
    public virtual void End(bool hasWon, bool draw, bool forceStop) { }
}