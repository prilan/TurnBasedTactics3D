public partial class GameEntity {

    public ObstacleComponent obstacle { get { return (ObstacleComponent)GetComponent(GameComponentsLookup.Obstacle); } }
    public bool hasObstacle { get { return HasComponent(GameComponentsLookup.Character); } }

    public void AddObstacle(Int2 newValue) {
        var index = GameComponentsLookup.Obstacle;
        var component = CreateComponent<ObstacleComponent>(index);
        component.cellPosition = newValue;
        AddComponent(index, component);
    }

    public void ReplaceObstacleCellPosition(Int2 newValue)
    {
        obstacle.cellPosition = newValue;
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherObstacle;

    public static Entitas.IMatcher<GameEntity> Obstacle
    {
        get {
            if (_matcherObstacle == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Obstacle);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherObstacle = matcher;
            }

            return _matcherObstacle;
        }
    }
}
