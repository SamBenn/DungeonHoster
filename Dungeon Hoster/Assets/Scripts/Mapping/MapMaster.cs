using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapMaster : MonoBehaviour
{
    public Texture2D Map;
    public GameObject MapMasterObj;

    public GameObject Test;

    private MapNode[,] _mapNodes;

	void Start () {
        this._mapNodes = new MapNode[50, 50];
        ParseMap(Map);
	}
	
	void Update () {
	
	}

    private void ParseMap(Texture2D map)
    {
        var mapWidth = map.width;
        var mapHeight = map.height;

        this._mapNodes = new MapNode[mapWidth, mapHeight];

        for(int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                var pixel = map.GetPixel(x, y);
                var node = new MapNode();
                GameObject obj = GameObject.Instantiate(Test);

                obj.transform.SetParent(MapMasterObj.transform);
                obj.transform.position = new Vector3(x, y, 0);

                obj.GetComponent<SpriteRenderer>().color = pixel;
                obj.name = string.Format("({0}, {1}) Tile", x, y);
                node.CreateRenderer(obj);
            }
        }
    }

    private MapNode PlaceAtSingle(MappableEntity content, int x, int y)
    {
        var node = _mapNodes[x, y];
        node.AddContent(content);
        return node;
    }

    public List<MapNode> PlaceAt(MappableEntity content, int xPlace, int yPlace, int xSize = 1, int ySize = 1)
    {
        List<MapNode> nodes = new List<MapNode>();

        try
        {
            if(xSize > 1 || ySize > 1)
            {
                var xMin = xPlace;
                var xMax = xPlace + xSize;
                var yMin = yPlace;
                var yMax = yPlace + ySize;

                for(int x = xMin; x < xMax; x++)
                {
                    for(int y = yMin; y < yMax; y++)
                    {
                        var node = this.PlaceAtSingle(content, x, y);
                        nodes.Add(node);
                    }
                }
            }
            else
            {
                var node = this.PlaceAtSingle(content, xPlace, yPlace);
                nodes.Add(node);
            }
        }
        catch
        {
            //TODO: Make this a part of the Entity Out of Bounds framework
            throw;
        }

        return nodes;
    }
}
