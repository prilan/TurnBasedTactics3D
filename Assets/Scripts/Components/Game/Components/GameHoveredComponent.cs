public partial class GameEntity {

    static readonly HoveredComponent hoveredComponent = new HoveredComponent();

    public bool isHovered
    {
        get { return HasComponent(GameComponentsLookup.Hovered); }
        set {
            if (value != isHovered) {
                var index = GameComponentsLookup.Hovered;
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

    static Entitas.IMatcher<GameEntity> _matcherHovered;

    public static Entitas.IMatcher<GameEntity> Hovered {
        get {
            if (_matcherHovered == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Hovered);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherHovered = matcher;
            }

            return _matcherHovered;
        }
    }
}
