using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation I, float g, float h,  float f, GameObject marker, PathMarker p)
    {
        location = I;
        G = g;
        H = h;
        F = f;
        this.marker = marker;
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || this.GetType().Equals(obj.GetType()))
        {  
            return false;
        } else
        {
            return location.Equals(((PathMarker)obj).location);
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}
public class FindPathAStar : MonoBehaviour
{
    public Maze maze;
    public Material closeMateral;
    public Material openMateral;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    PathMarker goalNode;
    PathMarker startNode;

    PathMarker lastPos;
    bool done = false;

    GameObject Unit;
    bool isMove = false;
    List<Vector3> movePath = new List<Vector3>();

    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach(GameObject m in markers)
        {
            Destroy(m);
        }
    }

    void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();
        for(int z = 1; z < maze.depth - 1; z++)
        {
            for(int x = 1; x < maze.width - 1; x++)
            {
                locations.Add(new MapLocation(x,z));
            }
        }

        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0, Instantiate(start, startLocation, Quaternion.identity), null);

        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0, Instantiate(end, goalLocation, Quaternion.identity), null);

        open.Clear();
        closed.Clear();

        open.Add(startNode);
        lastPos = startNode;

    }

    void Search(PathMarker thisNode)
    {
        if (thisNode.Equals(goalNode))
        {
            done = true;
            return;
        }

        foreach(MapLocation dir in maze.directions)
        {
            MapLocation neighbor = dir + thisNode.location;

            if(maze.map[neighbor.x, neighbor.z] == 1)
            {
                continue;
            }

            if(neighbor.x < 1 || neighbor.x >= maze.width || neighbor.z < 1 || neighbor.z >= maze.depth)
            {
                continue;
            }

            if(IsClosed(neighbor))
            {
                continue;
            }

            float G = Vector2.Distance(thisNode.location.Tovector(), neighbor.Tovector()) + thisNode.G;
            float H = Vector2.Distance(neighbor.Tovector(), goalNode.location.Tovector());
            float F = G + H;
            GameObject pathBlock = Instantiate(pathP, new Vector3(neighbor.x * maze.scale, 0, neighbor.z * maze.scale), Quaternion.identity);

            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = "G : " + G.ToString("0.00");
            values[1].text = "H : " + H.ToString("0.00");
            values[2].text = "F : " + F.ToString("0.00");

            if(UpdateMarker(neighbor, G,H,F, thisNode))
            {
                open.Add(new PathMarker(neighbor, G, H, F, pathBlock, thisNode));
            }

        }

        open = open.OrderBy(p => p.F).ToList<PathMarker>();

        PathMarker pm = (PathMarker)open.ElementAt(0);

        closed.Add(pm);

        open.RemoveAt(0);

        pm.marker.GetComponent<Renderer>().material = closeMateral;

        lastPos = pm;
    }

    bool IsClosed(MapLocation mapLocation)
    {
        return false;
    }
    
    bool UpdateMarker(MapLocation mapLocation, float G, float H, float F, PathMarker thisNode)
    {
        return false;
    }

}
