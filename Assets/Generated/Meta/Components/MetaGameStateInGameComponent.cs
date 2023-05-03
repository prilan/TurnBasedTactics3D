//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class MetaContext {

    public MetaEntity gameStateInGameEntity { get { return GetGroup(MetaMatcher.GameStateInGame).GetSingleEntity(); } }

    public bool isGameStateInGame {
        get { return gameStateInGameEntity != null; }
        set {
            var entity = gameStateInGameEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isGameStateInGame = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class MetaEntity {

    static readonly GameStateInGameComponent gameStateInGameComponent = new GameStateInGameComponent();

    public bool isGameStateInGame {
        get { return HasComponent(MetaComponentsLookup.GameStateInGame); }
        set {
            if (value != isGameStateInGame) {
                var index = MetaComponentsLookup.GameStateInGame;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : gameStateInGameComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class MetaMatcher {

    static Entitas.IMatcher<MetaEntity> _matcherGameStateInGame;

    public static Entitas.IMatcher<MetaEntity> GameStateInGame {
        get {
            if (_matcherGameStateInGame == null) {
                var matcher = (Entitas.Matcher<MetaEntity>)Entitas.Matcher<MetaEntity>.AllOf(MetaComponentsLookup.GameStateInGame);
                matcher.componentNames = MetaComponentsLookup.componentNames;
                _matcherGameStateInGame = matcher;
            }

            return _matcherGameStateInGame;
        }
    }
}
