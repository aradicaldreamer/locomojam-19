using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField]
    private bool[] directions = new bool[4];//0=north, 1=west, 2=south,  3=east

    // Start is called before the first frame update
    void Start()
    {
        if (transform.eulerAngles.z != 0)
        {
            int ONE_ROTATION = 90;
            int rotations = ((int)transform.eulerAngles.z / ONE_ROTATION) % 4;
            bool[] temp_directions = new bool[4];

            //switch directions
            for (int i = 0; i < directions.Length; i++)
            {
                //calculate the index the value will shift to
                int temp_index = (i + rotations) >= directions.Length ? i + rotations - 4 : i + rotations;

                temp_directions[temp_index] = directions[i];
            }

            for (int i = 0; i < directions.Length; i++)
            {
                directions[i] = temp_directions[i];
            }
        }
    }

    //0=north,1=west,2=south,3=east
    public bool HasDirection(int direction)
    {
        return directions[direction];
    }
}
