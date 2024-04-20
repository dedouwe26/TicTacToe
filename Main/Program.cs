using OxDEDTerm;
using TTT;
using TTT.Modules;

namespace Main;

public static class Program {
    private static (int x, int y) StartOffset;
    private static (int x, int y) FieldOffset;
    private static (int x, int y) Name1Offset;
    private static (int x, int y) Name2Offset;
    private static (int x, int y) EndOffset;
    private static string CreateField(ReadOnlyGrid<TileState> grid) {
        const string Field = " 0 \u2502 1 \u2502 2 \n\u2500\u2500\u2500\u253C\u2500\u2500\u2500\u253C\u2500\u2500\u2500\n 3 \u2502 4 \u2502 5 \n\u2500\u2500\u2500\u253C\u2500\u2500\u2500\u253C\u2500\u2500\u2500\n 6 \u2502 7 \u2502 8 ";
        return Field.Replace('0', TicTacToe.GetOwnerID(grid[0])).Replace('1', TicTacToe.GetOwnerID(grid[1])).Replace('2', TicTacToe.GetOwnerID(grid[2]))
                    .Replace('3', TicTacToe.GetOwnerID(grid[3])).Replace('4', TicTacToe.GetOwnerID(grid[4])).Replace('5', TicTacToe.GetOwnerID(grid[5]))
                    .Replace('6', TicTacToe.GetOwnerID(grid[6])).Replace('7', TicTacToe.GetOwnerID(grid[7])).Replace('8', TicTacToe.GetOwnerID(grid[8]));
        
    } 
    public static Module CreateModule(string option) {
        return option switch {
            "0" => new PlayerModule(),
            "1" => new RandomBotModule(),
            "2" => new SmartBotModule(),
            _ => throw new ArgumentException("Option not available", nameof(option))
        };
    }
    public static void Main(string[] args) {
        Terminal.WriteLine("Tic Tac Toe", new Style(){Bold = true, foregroundColor = Color.Orange});
        Terminal.WriteLine("\nModules:\n  0) Human\n  1) Random bot\n  2) Smart bot");
        Module module1;
        while(true) {
            Terminal.Write("Module 1 = ");
            try {
                module1 = CreateModule(Terminal.ReadLine()!);
            } catch (ArgumentException) {
                Terminal.WriteLine("Invalid option");
                continue;
            }
            break;
        }
        Module module2;
        while(true) {
            Terminal.Write("Module 2 = ");
            try {
                module2 = CreateModule(Terminal.ReadLine()!);
            } catch (ArgumentException) {
                Terminal.WriteLine("Invalid option");
                continue;
            }
            break;
        }
        if (module1 is PlayerModule) {
            (module1 as PlayerModule)!.OnChoose+=OnPlayer1Choose;
        }
        if (module2 is PlayerModule) {
            (module2 as PlayerModule)!.OnChoose+=OnPlayer2Choose;
        }

        TicTacToe game = new(module1, module2);
        
        Terminal.Clear();

        game.OnEvent+=(string ev, Dictionary<string, object> args) => {
            switch (ev) {
                case "versus":
                    Terminal.Write("\n");
                    StartOffset = Terminal.GetCursorPosition();
                    Terminal.Write(new string('\n', 8));
                    EndOffset = Terminal.GetCursorPosition();
                    Name1Offset = Name2Offset = StartOffset;
                    Name2Offset = (Name2Offset.x+((string)args["name1"]).Length+13, Name2Offset.y);
                    FieldOffset = (StartOffset.x, StartOffset.y+2);
                    Terminal.Set(args["name1"]+" (o) vs (x) "+args["name2"], Name1Offset);
                    break;
                case "start":
                    Terminal.Set(CreateField(game.GetGrid()), FieldOffset);
                    break;
                case "turn":
                    Terminal.Set((string)args["name"], (char)args["id"]=='o' ? Name1Offset : Name2Offset, new Style(){Bold=true});
                    break;
                case "tilechange":
                    Terminal.Set(CreateField(game.GetGrid()), FieldOffset);
                    break;
                case "win":
                    Terminal.Set((string)args["name"], (char)args["id"]=='o' ? Name1Offset : Name2Offset, new Style{Bold=true, foregroundColor = Color.Green});
                    break;
                case "lose":
                    Terminal.Set((string)args["name"], (char)args["id"]=='o' ? Name1Offset : Name2Offset, new Style{Bold=true, foregroundColor = Color.Red});
                    break;
                case "draw":
                    Terminal.Set(module1.Name, Name1Offset, new Style{Bold=true, foregroundColor = Color.Orange});
                    Terminal.Set(module2.Name, Name2Offset, new Style{Bold=true, foregroundColor = Color.Orange});
                    break;
                case "stop":
                    
                    Terminal.Goto(EndOffset);
                    break;
            }
        };

        game.Start();
    }
    private static byte OnPlayer1Choose(PlayerModule module) {
        Terminal.Set("Player 1 (0-8 l-r t-b): ", EndOffset);
        byte option = byte.Parse(Terminal.ReadLine()!);
        Terminal.Set("                         ", EndOffset);
        return option;
    }
    private static byte OnPlayer2Choose(PlayerModule module) {
        Terminal.Set("Player 2 (0-8 l-r t-b): ", EndOffset);
        byte option = byte.Parse(Terminal.ReadLine()!);
        Terminal.Set("                         ", EndOffset);
        return option;
    }
}