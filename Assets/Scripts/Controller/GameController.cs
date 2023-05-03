using DG.Tweening;
using Entitas;
using Sources.Systems.ExternalInput;
using Sources.Systems.Logic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public Contexts contexts
	{
		get; private set;
	}

	private Systems _systems;
	
	private void OnEnable()
	{
		DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(1000, 10);
		//		Application.targetFrameRate = 60;
		// 		var contexts = new Contexts();  

		Contexts.sharedInstance = null;
        contexts = Contexts.sharedInstance;
		
		_systems = new Feature("Systems")
				.Add(new Feature("General")
					//Initializers
					.Add(new TickSystem(contexts))
					.Add(new InitSceneEntitiesSystem(contexts))
                    .Add(new InitBoardCellsSystem(contexts))
					.Add(new InitCharactersSystem(contexts))
					.Add(new InitObstaclesSystem(contexts))
					.Add(new InitModeSystem(contexts))

					.Add(new SingleEntitiesInitSystem(contexts))

					//Reactive
					.Add(new CameraControllSystem(contexts))
					.Add(new ActiveCharacterMonitoringSystem(contexts))
                    .Add(new EditInstrumentHandleSystem(contexts))
                    .Add(new EditClickHandleSystem(contexts))

                    //Cleaning up
                    .Add(new EntityDestroySystem(contexts))
				) 
				.Add(new Feature("Ui")
					//Initializers
					.Add(new InitUiSystem(contexts))
					.Add(new UiSwitchModeSystem(contexts))

				//Cleaning up
				)
				.Add(new Feature("Input")
					.Add(new UserInputProcessor(contexts))
					.Add(new DragSelectionSystem(contexts))
					.Add(new HoverOnCellsSystem(contexts))
					.Add(new ClickMoveCharacterSystem(contexts))

                ) 
				.Add(new Feature("Movement")
					.Add(new UpdateViewPositionAndRotationSystem(contexts))
					.Add(new MoveCharacterSystem(contexts))
				) 
				.Add(new Feature("View")
					.Add(new DisplaySelectedEntitesSystem(contexts))
					.Add(new DisplayHoveredCellsSystem(contexts))
					.Add(new DisplayActiveCharacterCellSystem(contexts))
					.Add(new DisplayActiveCharacterSystem(contexts))
					.Add(new DisplayMoveHintCellsSystem(contexts))
					.Add(new DisplayNoWayCellSystem(contexts))
				) 
		;
		
		_systems.Initialize(); 
	}
	
	// Update is called once per frame
	void Update () {
		_systems.Execute();
		_systems.Cleanup();
	}
}
