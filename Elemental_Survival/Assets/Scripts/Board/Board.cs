using UnityEngine;

public enum AreaType
{
    Start,
    Area1,
    Area2,
}

public enum BoardType
{
    Type41,
    Type49,
}

public class Board : MonoBehaviour
{
    public BoardType boardType;
    [SerializeField] private GameObject board41, board49;
    [Space]
    [SerializeField] private Ground ground;
    [SerializeField] private float height;
    private const int MAX = 32;
    private const int HALF = MAX / 2;

    private static readonly Ground[,] grounds = new Ground[MAX, MAX];

    private static readonly (int x, int z)[][] area =
    {
        new (int, int)[]    // start
        {
            ( 0,  0),

            (-1,  1),
            ( 1,  1),
            ( 1, -1),
            (-1, -1),

            (-2,  2),
            ( 0,  2),
            ( 2,  2),
            ( 2,  0),
            ( 2, -2),
            ( 0, -2),
            (-2, -2),
            (-2,  0),
        },
        new (int, int)[]    // area 1
        {
            (-1,  1),
            ( 1,  1),
            ( 1, -1),
            (-1, -1),
        },
        new (int, int)[]    // area 2
        {
            (-2,  2),
            ( 0,  2),
            ( 2,  2),
            ( 2,  0),
            ( 2, -2),
            ( 0, -2),
            (-2, -2),
            (-2,  0),
        },
    };

    private void Awake()
    {
        switch (boardType)
        {
        case BoardType.Type41:
            board41.SetActive(true);
            for (int x = -4; x <= 4; x++)
            {
                for (int z = System.Math.Abs(x) % 2 - 4; z <= 4; z += 2)
                {
                    grounds[x + HALF, z + HALF] = Instantiate(ground, new Vector3(x, height, z), Quaternion.identity, transform);
                }
            }
            break;

        case BoardType.Type49:
            board49.SetActive(true);
            for (int x = -6; x <= 6; x++)
            {
                int to = 6 - System.Math.Abs(x);
                for (int z = -to; z <= to; z += 2)
                {
                    grounds[x + HALF, z + HALF] = Instantiate(ground, new Vector3(x, height, z), Quaternion.identity, transform);
                }
            }
            break;
        
        default:
            break;
        }
    }

    public static int GetGrounds(AreaType areaType, int x, int z, Ground[] arr)
    {
        int len = arr.Length;
        for (int i = 0; i < len; i++)
        {
            arr[i] = null;
        }

        int areaID = (int)areaType;
        int n = area[areaID].Length;
        for (int i = 0; i < n; i++)
        {
            arr[i] = grounds[x + HALF + area[areaID][i].x, z + HALF + area[areaID][i].z];
        }
        return n;
    }
}