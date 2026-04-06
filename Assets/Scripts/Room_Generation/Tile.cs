using UnityEngine;

public enum TileType { Floor, Wall }

public class Tile : MonoBehaviour
{
    public TileType type;
    [HideInInspector] public Vector2Int coordinate;

    public GridEntity occupant;

    void Awake()
    {
        coordinate = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }
}