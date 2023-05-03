using System.Collections.Generic;
using System.Linq;
using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace Sources.Systems.ExternalInput
{
    public sealed class HoverOnCellsSystem : ReactiveSystem<InputEntity> {

        readonly InputContext _context;
        readonly GameContext _gameContext;
        readonly MetaContext _metaContext;
        private IGroup<GameEntity> _characterEntitiesGroup;
        private IGroup<GameEntity> _cellEntitiesGroup;
        private IGroup<GameEntity> _hoverCellEntitiesGroup;
        private IGroup<GameEntity> _obstacleEntitiesGroup;
        private FindCharacterPathService _findPathService;
        private ClearMoveHintNoWayService _clearMoveHintNoWayService;

        public HoverOnCellsSystem(Contexts contexts) : base(contexts.input) {
            _context = contexts.input;
            _gameContext = contexts.game;
            _metaContext = contexts.meta;
            _cellEntitiesGroup = contexts.game.GetGroup(GameMatcher.CellPosition);
            _characterEntitiesGroup = contexts.game.GetGroup(GameMatcher.Character);
            _hoverCellEntitiesGroup = contexts.game.GetGroup(GameMatcher.HoverCell);
            _obstacleEntitiesGroup = contexts.game.GetGroup(GameMatcher.Obstacle);
            _findPathService = new FindCharacterPathService(contexts);
            _clearMoveHintNoWayService = new ClearMoveHintNoWayService(contexts);
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context) {
            return context.CreateCollector(InputMatcher.AllOf(InputMatcher.ScreenPoint, InputMatcher.MouseEvent));
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasScreenPoint;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            if (_metaContext.isGameStateInEdit)
                return;

            InputEntity entity = entities.First();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(entity.screenPoint.value), out hit, Mathf.Infinity)) {

                GameObject hoverTargetGo = null;

                if (hit.collider != null) {
                    hoverTargetGo = hit.collider.gameObject;
                } else if (hit.rigidbody != null) {
                    hoverTargetGo = hit.rigidbody.gameObject;
                }

                if (hoverTargetGo == null) {
                    return;
                }

                if (hoverTargetGo.transform.name == "LevelSurface") {
                    _clearMoveHintNoWayService.ClearMoveHint();
                }

                EntityLink clickedEntityLink = hoverTargetGo.GetComponentInParent<EntityLink>();

                if (clickedEntityLink == null) {
                    return;
                }

                GameEntity characterEntity = _characterEntitiesGroup.GetEntities().Where(character => character.isActiveCharacter).First();
                CharacterComponent characterComponent = (CharacterComponent)characterEntity.GetComponent(GameComponentsLookup.Character);
                Int2 startCharacterPosition = characterComponent.cellPosition;

                foreach (GameEntity cellEntity in _cellEntitiesGroup) {
                    cellEntity.isHovered = false;
                }

                GameEntity hoveredEntity = (GameEntity)clickedEntityLink.entity;

                if (hoveredEntity.hasCellPosition) {
                    hoveredEntity.isHovered = true;
                }

                if (!hoveredEntity.HasComponent(GameComponentsLookup.CellPosition)) {
                    _clearMoveHintNoWayService.ClearMoveHint();
                    return;
                }

                CellPositionComponent cellPositionComponent = (CellPositionComponent)hoveredEntity.GetComponent(GameComponentsLookup.CellPosition);

                Int2 cellPosition = cellPositionComponent.value;

                GameEntity[] hoverCellEntities = _hoverCellEntitiesGroup.GetEntities();
                if (hoverCellEntities.Any()) {
                    GameEntity hoverCellEntity = hoverCellEntities.Single();
                    HoverCellComponent hoverCellComponent = (HoverCellComponent)hoverCellEntity.GetComponent(GameComponentsLookup.HoverCell);
                    if (hoverCellComponent.hoverCellPosition.Equals(cellPosition)) {
                        // Повторно вычисления не производим
                        return;
                    } else {
                        hoverCellEntity.ReplaceHoverCellPosition(cellPosition);
                    }
                } else {
                    GameEntity hoverCellEntity = _gameContext.CreateEntity();
                    hoverCellEntity.AddHoverCellPosition(cellPosition);
                }

                HashSet<Int2> handledPoints = new HashSet<Int2>();

                if (_findPathService.FindPath(startCharacterPosition, cellPosition, ref handledPoints, out List<Int2> pathPositions)) {
                    if (pathPositions.Any()) {
                        foreach (GameEntity cellEntity in _cellEntitiesGroup.GetEntities()) {
                            if (pathPositions.Contains(cellEntity.cellPosition.value)) {
                                cellEntity.isMoveHint = true;
                            } else {
                                cellEntity.isMoveHint = false;
                            }
                        }

                        _clearMoveHintNoWayService.ClearNoWay();

                        _gameContext.characterPathEntity.SetCharacterPath(pathPositions);
                    }
                } else {
                    // Не найден путь
                    _gameContext.characterPathEntity.ClearCharacterPath();

                    GameEntity characterHereEntity = _characterEntitiesGroup.GetEntities().Where(character => character.character.cellPosition.Equals(cellPosition)).FirstOrDefault();
                    GameEntity obstacleHereEntities = _obstacleEntitiesGroup.GetEntities().Where(obst => obst.obstacle.cellPosition.Equals(cellPosition)).FirstOrDefault();
                    if (characterHereEntity != null || obstacleHereEntities != null) {
                        return;
                    }

                    // Установка метки, что пути нет
                    SetNoWay(cellPosition);
                }
            }
        }

        private void SetNoWay(Int2 cellPosition, bool needResetAllCells = false)
        {
            foreach (GameEntity cellEntity in _cellEntitiesGroup.GetEntities()) {
                if (!needResetAllCells && cellEntity.cellPosition.value.Equals(cellPosition)) {
                    cellEntity.isNoWay = true;
                } else {
                    cellEntity.isNoWay = false;
                }
            }
        }
    }
}
