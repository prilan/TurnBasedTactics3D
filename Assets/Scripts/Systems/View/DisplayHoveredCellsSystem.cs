using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class DisplayHoveredCellsSystem : ReactiveSystem<GameEntity> {

    readonly GameContext _context;

    public DisplayHoveredCellsSystem(Contexts contexts) : base(contexts.game) {
        _context = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
        return context.CreateCollector(GameMatcher.Hovered.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity) {
        return true;
    }

    protected override void Execute(List<GameEntity> entities) {
        
        foreach (GameEntity entity in entities)
        {
            GameObject entityGo = entity.view.gameObject;

            Transform hoverTransform = entityGo.transform.Find("Hover");

            if (hoverTransform != null)
            {
                hoverTransform.gameObject.SetActive(entity.isHovered);
            }
        }
    }
}
