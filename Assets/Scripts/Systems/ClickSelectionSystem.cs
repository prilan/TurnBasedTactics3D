using Entitas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class ClickSelectionSystem : ReactiveSystem<InputEntity>
    {
        readonly InputContext _context;
        private IGroup<GameEntity> _selectedEntitiesGroup;
        private IGroup<InputEntity> _keyEventGroup;

        public ClickSelectionSystem(Contexts contexts) : base(contexts.input)
        {
            _context = contexts.input;
            _selectedEntitiesGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Selected));
            _keyEventGroup = contexts.input.GetGroup(InputMatcher.AllOf(InputMatcher.KeyEvent, InputMatcher.KeyHeld));
        }

        protected override void Execute(List<InputEntity> entities)
        {
            throw new System.NotImplementedException();
        }

        protected override bool Filter(InputEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            throw new System.NotImplementedException();
        }
    }
}