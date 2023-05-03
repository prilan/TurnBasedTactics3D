using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;
using Utility;

public sealed class MoveCharacterSystem : ReactiveSystem<InputEntity>, IInitializeSystem {

    readonly GameContext _context;
    readonly MetaContext _metaContext;
    readonly GameContext _gameContext;
    private IGroup<GameEntity> _movingNavAgentsGroup;
    private ClearMoveHintNoWayService _clearMoveHintNoWayService;

    public MoveCharacterSystem(Contexts contexts) : base(contexts.input) {
        _context = contexts.game;
        _metaContext = contexts.meta;
        _gameContext = contexts.game;
        _clearMoveHintNoWayService = new ClearMoveHintNoWayService(contexts);
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) {
        return context.CreateCollector(InputMatcher.Tick);
    }

    protected override bool Filter(InputEntity entity) {
        return true;
    }

    public void Initialize()
    {
        _movingNavAgentsGroup = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Character, GameMatcher.MoveCharacter));
    }

    protected override void Execute(List<InputEntity> entities) {
        float frameCountForOneCellMove = 60; // 10

        foreach (GameEntity entity in _movingNavAgentsGroup.GetEntities()) {
            GameObject entityGo = entity.view.gameObject;

            if (entity.hasCharacter && entity.hasMoveCharacter) {
                CharacterComponent character = entity.character;
                MoveCharacterComponent moveCharacter = entity.moveCharacter;

                List<Int2> pathCellPositions = moveCharacter.pathCellPositions;

                if (moveCharacter.moveCharacterData.moveStepIndex == -1 || moveCharacter.moveCharacterData.frameInStepIndex == -1) {
                    moveCharacter.moveCharacterData.moveStepIndex = 0;
                    moveCharacter.moveCharacterData.frameInStepIndex = 0;
                } else {
                    moveCharacter.moveCharacterData.frameInStepIndex++;
                }

                Int2 currentCharacterStartCellPosition = character.cellPosition;

                if (moveCharacter.moveCharacterData.frameInStepIndex == frameCountForOneCellMove) {
                    moveCharacter.moveCharacterData.moveStepIndex++;
                    moveCharacter.moveCharacterData.frameInStepIndex = 0;
                }

                if (moveCharacter.moveCharacterData.moveStepIndex > 0) {
                    currentCharacterStartCellPosition = pathCellPositions[moveCharacter.moveCharacterData.moveStepIndex - 1];
                }

                if (moveCharacter.moveCharacterData.moveStepIndex == pathCellPositions.Count) {
                    // Завершаем движение
                    Int2 targetCell = pathCellPositions.Last();
                    Vector3 targetTrasformPosition = CommonUtility.CalculateTransformPositionByCellposition(targetCell);

                    entityGo.gameObject.transform.position = targetTrasformPosition;
                    character.cellPosition = targetCell;

                    entity.RemoveMoveCharacter();

                    entity.isActiveCharacter = false;
                    entity.isActiveCharacter = true;

                    _metaContext.isGameStateAnimating = false;

                    _clearMoveHintNoWayService.ClearMoveHint();

                    return;
                }

                Int2 currentTargetCell = pathCellPositions[moveCharacter.moveCharacterData.moveStepIndex];

                Int2 stepCellVector = currentTargetCell - currentCharacterStartCellPosition;
                Vector2 stepTransformVector = stepCellVector * CommonConsts.CellSize;

                Vector2 frameStepVector = stepTransformVector / frameCountForOneCellMove;
                Vector3 frameStepTransformVector = new Vector3(frameStepVector.x, 0, frameStepVector.y);

                entityGo.gameObject.transform.position += frameStepTransformVector;
            }
        }
    }
}
