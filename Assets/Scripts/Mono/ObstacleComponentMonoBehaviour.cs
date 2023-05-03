using Entitas;

public class ObstacleComponentMonoBehaviour : BaseComponentMonoBehaviour
{
    public Int2 cellPosition;
    private ObstacleComponent component;

    public override IComponent Component
    {
        get
        {
            if (component == null) {
                return component = new ObstacleComponent {
                    cellPosition = cellPosition
                };
            }

            return component;
        }
    }
}
