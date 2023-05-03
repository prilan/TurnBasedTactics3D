using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;
using Utility;
using Object = UnityEngine.Object;

public class InitBoardCellsSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public InitBoardCellsSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        List<ViewableEntityInitializer> initializableEntities = Object.FindObjectsOfType<ViewableEntityInitializer>().ToList();
        
        foreach (ViewableEntityInitializer initializableEntity in initializableEntities)
        {
            List<CellPositionComponentMonoBehaviour> cellPositionMonoComponents = initializableEntity.GetComponents<CellPositionComponentMonoBehaviour>().ToList();

            if (!cellPositionMonoComponents.Any())
                continue;

            CellPositionComponentMonoBehaviour cellPositionMonoComponent = cellPositionMonoComponents[0];
            Vector3 transformPosition = cellPositionMonoComponent.transform.position;

            CellPositionComponent cellPositionComponent = (CellPositionComponent)cellPositionMonoComponent.Component;

            Int2 boardCellPosition = CommonUtility.CalculateCellPosition(transformPosition);

            cellPositionComponent.value = boardCellPosition;
        }
    }
}
