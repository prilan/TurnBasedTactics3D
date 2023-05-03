public partial class GameEntity {

    static readonly MoveHintComponent moveHintComponent = new MoveHintComponent();

    public bool isMoveHint
    {
        get { return HasComponent(GameComponentsLookup.MoveHint); }
        set {
            if (value != isMoveHint) {
                var index = GameComponentsLookup.MoveHint;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : hoveredComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMoveHintComponent;

    public static Entitas.IMatcher<GameEntity> MoveHint
    {
        get {
            if (_matcherMoveHintComponent == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MoveHint);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMoveHintComponent = matcher;
            }

            return _matcherMoveHintComponent;
        }
    }
}
