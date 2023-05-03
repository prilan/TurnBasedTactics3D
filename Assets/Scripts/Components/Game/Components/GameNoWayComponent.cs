public partial class GameEntity {

    static readonly NoWayComponent noWayComponent = new NoWayComponent();

    public bool isNoWay
    {
        get { return HasComponent(GameComponentsLookup.NoWay); }
        set {
            if (value != isNoWay) {
                var index = GameComponentsLookup.NoWay;
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

    static Entitas.IMatcher<GameEntity> _matcherNoWayComponent;

    public static Entitas.IMatcher<GameEntity> NoWay
    {
        get {
            if (_matcherNoWayComponent == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.NoWay);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherNoWayComponent = matcher;
            }

            return _matcherNoWayComponent;
        }
    }
}
