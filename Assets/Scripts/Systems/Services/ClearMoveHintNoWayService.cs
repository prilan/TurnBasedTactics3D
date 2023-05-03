using Entitas;

public class ClearMoveHintNoWayService
{
    readonly GameContext _gameContext;
    private IGroup<GameEntity> _cellEntitiesGroup;

    public ClearMoveHintNoWayService(Contexts contexts)
    {
        _gameContext = contexts.game;
        _cellEntitiesGroup = contexts.game.GetGroup(GameMatcher.CellPosition);
    }

    public void ClearMoveHint()
    {
        _gameContext.characterPathEntity.ClearCharacterPath();

        GameEntity[] cellEntities = _cellEntitiesGroup.GetEntities();
        foreach (GameEntity cellEntity in cellEntities) {
            cellEntity.isMoveHint = false;
        }
    }

    public void ClearNoWay()
    {
        foreach (GameEntity cellEntity in _cellEntitiesGroup.GetEntities()) {
            cellEntity.isNoWay = false;
        }
    }
}

