using System.Collections.Generic;
using Entitas;
using UnityEngine;
using Utility;

public sealed class DisplayActiveCharacterSystem : ReactiveSystem<GameEntity> {

    readonly GameContext _context;
    private IGroup<GameEntity> _cellEntitiesGroup;

    public DisplayActiveCharacterSystem(Contexts contexts) : base(contexts.game) {
        _context = contexts.game;
        _cellEntitiesGroup = contexts.game.GetGroup(GameMatcher.CellPosition);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
        return context.CreateCollector(GameMatcher.ActiveCharacter.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity) {
        return true;
    }

    protected override void Execute(List<GameEntity> entities) {

        foreach (GameEntity cellEntity in _cellEntitiesGroup) {
            if (cellEntity.isActiveCharacterCell) {
                cellEntity.isActiveCharacterCell = false;
            }
        }

        foreach (GameEntity entity in entities)
        {
            GameObject characterGo = entity.view.gameObject;

            Transform characterCollider = characterGo.transform.Find("CharacterCylinder");

            if (CommonUtility.RaycastWorldPositionToCell(characterCollider.position, out GameEntity activeCellEntity)) {
                if (activeCellEntity.hasCellPosition) {
                    activeCellEntity.isActiveCharacterCell = entity.isActiveCharacter;
                }
            }
        }
    }
}
