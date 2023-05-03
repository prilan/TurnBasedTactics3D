using Entitas;

public class InitModeSystem : IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly MetaContext _context;

    public InitModeSystem(Contexts contexts)
    {
        _contexts = contexts;
        _context = contexts.meta;
    }

    public void Initialize()
    {
        _context.isGameStateInEdit = true;
        _context.isGameStateInGame = false;
    }
}
