using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class DisplayMoveHintCellsSystem : ReactiveSystem<GameEntity> {

    readonly GameContext _context;

    public DisplayMoveHintCellsSystem(Contexts contexts) : base(contexts.game) {
        _context = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
        return context.CreateCollector(GameMatcher.MoveHint.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity) {
        return true;
    }

    protected override void Execute(List<GameEntity> entities) {
        
        foreach (GameEntity entity in entities)
        {
            GameObject entityGo = entity.view.gameObject;

            Transform moveHintTransform = entityGo.transform.Find("MoveHint");

            if (moveHintTransform != null)
            {
                moveHintTransform.gameObject.SetActive(entity.isMoveHint);
            }
        }
    }
}
