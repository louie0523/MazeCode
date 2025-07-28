using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class MapLocation
{
    public int x;
    public int z;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_z"></param>
    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public Vector2 Tovector()
    {
        return new Vector2(x, z);
    }

    public static MapLocation operator +(MapLocation a, MapLocation b)
        => new MapLocation(a.x + b.x, a.z + b.z);

    public override bool Equals(object obj)//object == 특정 모양이 없는 상태
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return x == ((MapLocation)obj).x && z == ((MapLocation)obj).z;//특정 모양이 없는 object를 MapLocation으로 형변환
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}



public class Maze : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>()
    {
        new MapLocation(1, 0),
        new MapLocation(0,1),
        new MapLocation(-1,0),
        new MapLocation(0,-1),
    };
    public int width = 30;
    public int depth = 30;
    public byte[,] map;
    public int scale = 6;


    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        Generate();
        DrawMap();
    }

    void InitialiseMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1; //1은 벽, 0은 통로
            }
        }
    }

    public virtual void Generate()
    {
        /*
        for (int z =0; z < depth; z++)
        {
            for(int x = 0; x < width; x++)
            {

            }
        }
        */

    }

    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }
            }
        }
    }

    /// <summary>
    ///4방위 복도를 검색한다. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

}
