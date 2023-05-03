public partial class GameEntity {

    public CharacterComponent character { get { return (CharacterComponent)GetComponent(GameComponentsLookup.Character); } }
    public bool hasCharacter { get { return HasComponent(GameComponentsLookup.Character); } }

    public void AddCharacter(Int2 newValue) {
        var index = GameComponentsLookup.Character;
        var component = CreateComponent<CharacterComponent>(index);
        component.cellPosition = newValue;
        AddComponent(index, component);
    }

    public void ReplaceCharacterCellPosition(Int2 newValue)
    {
        character.cellPosition = newValue;
    }
}

public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherCharacter;

    public static Entitas.IMatcher<GameEntity> Character
    {
        get {
            if (_matcherCharacter == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Character);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCharacter = matcher;
            }

            return _matcherCharacter;
        }
    }
}
