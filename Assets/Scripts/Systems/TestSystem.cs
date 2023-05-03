using Entitas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystem : ReactiveSystem<InputEntity>
{
    public TestSystem(IContext<InputEntity> context) : base(context)
    {
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
