using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region Enums
public enum BlockType
{
    Clear = 0,
    Blocked = 10
}

public enum NodeType
{

}
#endregion

#region Base Classes
public class EntityBase
{
    public Guid Guid;
    private bool isDisabled;

    public EntityBase()
    {
        this.Guid = Guid.NewGuid();
    }

    public bool IsDisabled()
    {
        return this.isDisabled;
    }

    public void Disable()
    {
        this.isDisabled = true;
    }
}

public class NamedEntity : EntityBase
{
    public string Name;

    public NamedEntity()
        : base()
    {

    }
}

public class RenderableEntity : NamedEntity
{
    private GameObject _RenderObj;

    public RenderableEntity()
        : base()
    {

    }

    public void CreateRenderer(GameObject obj)
    {
        this._RenderObj = obj;
    }

    public GameObject GetRenderer()
    {
        return this._RenderObj;
    }
}

public class BlockingBase : RenderableEntity
{
    private BlockType _blockType = BlockType.Clear;

    public BlockingBase()
        : base()
    {

    }

    public void SetBlockType(BlockType blockType)
    {
        this._blockType = blockType;
    }

    public BlockType GetBlockType()
    {
        return this._blockType;
    }
}
#endregion

#region Mapping
public class BlockableEntity : BlockingBase
{
    public BlockableEntity()
        : base()
    {

    }

    public bool IsBlocked()
    {
        if (this.GetBlockType() == BlockType.Blocked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns block type as a resistance value between 0 and 1 as a float
    /// </summary>
    public float GetResistance()
    {
        return (int)this.GetBlockType() / 10;
    }
}

public class MapNode : BlockableEntity
{
    private Vector3 _location;
    private List<EntityBase> _contents;

    public MapNode()
        : base()
    {
        this._contents = new List<EntityBase>();
    }

    public List<EntityBase> GetContents()
    {
        return this._contents;
    }

    public void AddContent(EntityBase content)
    {
        this._contents.Add(content);
    }

    public EntityBase RemoveContent(Guid guid)
    {
        if (guid == Guid.Empty)
            return null;

        var toReturn = this._contents.Where(c => c.Guid == guid).SingleOrDefault();

        if (toReturn != null)
        {
            this._contents.Remove(toReturn);
        }

        return toReturn;
    }

    public EntityBase RemoveContent(EntityBase content)
    {
        var toReturn = this._contents.Where(c => c.Guid == content.Guid).SingleOrDefault();

        return toReturn;
    }
}

public class BlockingEntity : BlockingBase
{
    public BlockingEntity()
        : base()
    {

    }
}

public class MappableEntity : BlockingEntity
{
    public List<MapNode> Containers;

    private bool _masterAwoken;
    private MapMaster _mapMaster;

    public MappableEntity()
        : base()
    {

    }

    private void AwakeMapMaster()
    {
        if (this._masterAwoken == false)
        {
            this._mapMaster = GameObject.FindGameObjectWithTag("MapMaster").GetComponent<MapMaster>();
            this._masterAwoken = true;
        }
    }

    public void PlaceAt(int x, int y, int xSize = 1, int ySize = 1, bool destroyPrevious = false)
    {
        this.AwakeMapMaster();

        var nodes = this._mapMaster.PlaceAt(this, x, y, xSize, ySize);

        if (destroyPrevious)
        {
            this.Containers = nodes;
        }
        else
        {
            this.Containers.AddRange(nodes);
        }
    }
}

public class Terrain : MappableEntity
{
    public Terrain(List<Vector2> terrainLocations)
        : base()
    {
        terrainLocations.ForEach(l => this.PlaceAt((int)l.x, (int)l.y));
    }
}
#endregion