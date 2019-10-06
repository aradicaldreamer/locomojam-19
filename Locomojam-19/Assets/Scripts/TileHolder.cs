using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{

    [SerializeField]
    private TileEnum tileType = TileEnum.Empty;
    private GameObject tileObject;
    private Tile tile;

    // Start is called before the first frame update
    void Awake()
    {
        tileObject = GameManager.Instance.SetTile(tileType);
        tileObject = Instantiate(tileObject, transform);
        tileObject.transform.localPosition = new Vector3(0, 0, 0);
        tile = tileObject.GetComponentInChildren<Tile>();
    }

    public TileEnum getTileType()
    {
        return tileType;
    }
    
    //0=north,1=west,2=south,3=east
    public bool HasDirection(int direction)
    {
        return tile.HasDirection(direction);
    }
}
