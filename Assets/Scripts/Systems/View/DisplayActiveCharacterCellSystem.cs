using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class DisplayActiveCharacterCellSystem : ReactiveSystem<GameEntity> {

    readonly GameContext _context;

    public DisplayActiveCharacterCellSystem(Contexts contexts) : base(contexts.game) {
        _context = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
        return context.CreateCollector(GameMatcher.ActiveCharacterCell.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity) {
        return true;
    }

    protected override void Execute(List<GameEntity> entities) {

        foreach (GameEntity entity in entities)
        {
            GameObject entityGo = entity.view.gameObject;

            Transform activeCharacterTransform = entityGo.transform.Find("ActiveCharacter");

            if (activeCharacterTransform != null) {
                activeCharacterTransform.gameObject.SetActive(entity.isActiveCharacterCell);
            }
        }
    }
}
