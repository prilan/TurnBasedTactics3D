using Entitas;

public class SingleEntitiesInitSystem : IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly GameContext _context;

    public SingleEntitiesInitSystem(Contexts contexts)
    {
        _contexts = contexts;
        _context = contexts.game;
    }

    public void Initialize()
    {
        GameEntity characterPathEntity = _context.CreateEntity();
        characterPathEntity.isCharacterPath = true;
    }
}
