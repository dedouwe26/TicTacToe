using OxDEDTerm;

namespace TTT.Modules;

public class PlayerModule : Module {
    public override string Name => "Human";

    public event Func<PlayerModule, byte>? OnChoose;

    public override byte Choose() {
        return OnChoose!.Invoke(this);
    }
}