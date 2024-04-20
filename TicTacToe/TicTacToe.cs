namespace TTT;

public sealed class TicTacToe {
    public static readonly Line[] AllLines = [
        new(0,3,6),
        new(1,4,7),
        new(2,5,8),
        new(0,1,2),
        new(3,4,5),
        new(6,7,8),
        new(0,4,8),
        new(2,4,6)
    ];
    public event EventCallback? OnEvent;

    public readonly Module module1;
    public readonly Module module2;
    private readonly Grid<TileState> grid = new (TileState.Empty);
    public bool IsOver { get; private set; }
    private char previousModule = 'x';
    public TicTacToe(Module module1, Module module2) {
        this.module1 = module1;
        this.module2 = module2;
        
    }
    public void Start() {
        OnEvent?.Invoke("versus", new (){
            ["name1"] = module1.Name, 
            ["name2"] = module2.Name
        });

        OnEvent?.Invoke("start", []);

        

        while (!IsOver) {
            Module current = WhoIsNext();
            OnEvent?.Invoke("turn", new(){
                ["name"] = current.Name,
                ["id"] = GetID(current)
            });
            byte chosenTile = current.Choose(this);
            if (!IsMoveValid(chosenTile)) {
                continue;
            }

            grid[chosenTile] = GetClaimedStateFor(GetID(current));
            OnEvent?.Invoke("tilechange", new (){
                ["name"] = current.Name,
                ["id"] = GetID(current),
                ["chosenTile"] = chosenTile,
                ["newState"] = GetClaimedStateFor(GetID(current))
            });

            CheckForVictories();

            previousModule = GetID(current);
        }
    }
    public ReadOnlyGrid<TileState> GetGrid() {
        return new(grid);
    }
    public Module GetModule(char id) {
        return id == 'o' ? module1 : module2;
    }
    public char GetID(Module module) {
        return module==module1 ? 'o' : 'x';
    }
    public static TileState GetClaimedStateFor(char ID) {
        return ID == 'o' ? TileState.o : TileState.x;
    }
    public static char GetOwnerID(TileState tile) {
        return tile == TileState.o ? 'o' : tile == TileState.x ? 'x' : ' ';
    }
    public void Stop(char winnerID = '\0') {
        module1.End(winnerID == 'o', winnerID == 'd', winnerID == '\0');
        module2.End(winnerID == 'x', winnerID == 'd', winnerID == '\0');
        OnEvent?.Invoke("stop", new (){
            ["forceStop"] = winnerID == '\0',
            ["draw"] = winnerID == 'd',
            ["winner"] = winnerID=='o' || winnerID=='x' ? winnerID : '\0'
        });
        IsOver = true;
    }
    public void CheckForVictories() {
        foreach (Line line in AllLines) {
            char? ID = CheckForVictory(line);
            if (ID!=null) {
                OnEvent?.Invoke("win", new (){
                    ["name"] = GetModule(ID.Value).Name,
                    ["id"] = ID.Value,
                    ["line"] = line
                });
                OnEvent?.Invoke("lose", new (){
                    ["name"] = GetModule(Opponent(ID.Value)).Name,
                    ["id"] = Opponent(ID.Value),
                    ["line"] = line
                });
                Stop(ID!.Value);
                return;
            }
        }
        bool foundEmptyTile = false;
        foreach (TileState tile in grid) {
            if (tile == TileState.Empty) {
                foundEmptyTile = true;
                break;
            }
        }
        if (!foundEmptyTile) {
            OnEvent?.Invoke("draw", []);
            Stop('d');
        }
    }
    public static char Opponent(char ID) {
        return ID == 'o' ? 'x' : 'o';
    }
    public char? CheckForVictory(Line line) {
        return ((grid[line.start] == grid[line.middle]) && (grid[line.middle] == grid[line.end]) && (grid[line.end] == grid[line.start])) ? GetOwnerID(grid[line.start]) : null;
    }
    public bool IsMoveValid(byte tile) {
        return grid[tile]==TileState.Empty;
    }
    public Module WhoIsNext() {
        return previousModule == 'x' ? module1 : module2;
    }
    public void ListenFor(string eventName, ListenCallback callback) {
        OnEvent+=(ev, args) => {
            if (ev == eventName) {
                callback(args);
            }
        };
    }
}