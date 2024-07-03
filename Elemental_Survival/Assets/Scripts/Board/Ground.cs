using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Material red, yellow, green, blue, black, white;

    private Renderer groundRender;
    private PlaceColor currentColor = PlaceColor.Black;

    public Vector3 PlacePosition { get; private set; }

    private void Awake()
    {
        PlacePosition = transform.GetChild(0).position;
        groundRender = GetComponent<Renderer>();
        groundRender.material = black;
    }

    public bool Walkable(PlaceColor color, out bool isDiedOrMyColor)
    {
        isDiedOrMyColor = false;
        if (currentColor == PlaceColor.White)
        {
            return false;
        }
        else if (currentColor == PlaceColor.Black)
        {
            return true;
        }
        else if (currentColor == color || GameManager.Players[currentColor].CurrentState == Player.State.Die)
        {
            isDiedOrMyColor = true;
            return true;
        }
        return false;
    }

    public bool Placable(PlaceColor color, out bool isDiedColor)
    {
        isDiedColor = false;
        if (currentColor == PlaceColor.White || currentColor == color)
        {
            return false;
        }
        else if (currentColor == PlaceColor.Black)
        {
            return true;
        }
        else if (GameManager.Players[currentColor].CurrentState == Player.State.Die)
        {
            isDiedColor = true;
            return true;
        }
        return false;
    }
    
    public void ColoringPlace(PlaceColor color)
    {
        currentColor = color;
        groundRender.material = color switch
        {
            PlaceColor.Red    => red,
            PlaceColor.Yellow => yellow,
            PlaceColor.Green  => green,
            PlaceColor.Blue   => blue,
            PlaceColor.Black  => black,
            PlaceColor.White  => white,
            _ => throw new UnityException("Unassigned Color")
        };
    }
}
