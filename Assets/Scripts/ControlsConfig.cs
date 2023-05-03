using UnityEngine;
using Utility;

public class ControlsConfig : MonoSingleton<ControlsConfig>
{
	public float cameraMinHeight;
	
	public float cameraMaxHeight;

	[Range(0, 100)]
	public float cameraMovementSpeed;
	
	[Range(0, 100)]
	public float cameraRotationSpeed;
	
	[Range(0, 100)]
	public float cameraZoomSpeed;
}
