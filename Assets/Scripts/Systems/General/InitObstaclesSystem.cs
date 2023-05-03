using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;
using Utility;
using Object = UnityEngine.Object;

public class InitObstaclesSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public InitObstaclesSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        List<ViewableEntityInitializer> initializableEntities = Object.FindObjectsOfType<ViewableEntityInitializer>().ToList();

        for (int index = 0; index < initializableEntities.Count; index++) {
            ViewableEntityInitializer initializableEntity = initializableEntities[index];
            List<ObstacleComponentMonoBehaviour> obstacleMonoComponents = initializableEntity.GetComponents<ObstacleComponentMonoBehaviour>().ToList();

            if (!obstacleMonoComponents.Any())
                continue;

            ObstacleComponentMonoBehaviour obstacleMonoComponent = obstacleMonoComponents[0];

            ObstacleComponent obstacleComponent = (ObstacleComponent)obstacleMonoComponent.Component;

            GameObject characterGo = initializableEntity.gameObject;

            if (CommonUtility.RaycastWorldPositionToCell(characterGo.transform.position, out GameEntity cellEntity)) {

                CellPositionComponent cellPositionComponent = (CellPositionComponent)cellEntity.GetComponent(GameComponentsLookup.CellPosition);
                Int2 cellPosition = cellPositionComponent.value;

                obstacleComponent.cellPosition = cellPosition;
            }
        }
    }
}
