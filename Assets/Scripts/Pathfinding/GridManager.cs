using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private Node[,] grid;
    private Vector3 startOfGridPosition;
    [SerializeField,Tooltip("Number of rows on the grid")]
    private int maxRows = 10;
    [SerializeField, Tooltip("Number of columns on the grid")]
    private int maxColumns = 10;
    [SerializeField,Tooltip("Size of each grid tile")]
    private float gridTileSize;
    [SerializeField,Tooltip("The group to identify as an obstacle")]
    private string obstacleTag = "Obstacle";
    private GameObject[] obstaclesArray;
    [SerializeField]
    private bool showGrid = true;
    public Color gridColor = Color.blue;
    
    private void Awake()
    {
        //Get reference to a single GridManager script
        Instance = (GridManager)FindObjectOfType(typeof(GridManager));
    }
    private void Start()
    {
        grid = new Node[maxColumns, maxRows];
        startOfGridPosition = new Vector3();//set to Vector3.zero
        CalculateObstacles();
    }
    private void CalculateObstacles()
    {
        obstaclesArray = GameObject.FindGameObjectsWithTag("Obstacle");
        int index = 0;
        //Setup Grid 
        for (int i = 0; i < maxColumns; i++)
        {
            for (int j = 0; j < maxRows; j++)
            {
                Vector3 tilePos = GetGridTileCenter(index);
                //set node position to the center of a grid tile
                Node node = new Node(tilePos);
                grid[i, j] = node;
                index++;
            }
        }
        //For each obstacle found on the world, record it in our list
        //Sets all obstacles to Grid
        if (obstaclesArray.Length > 0 && obstaclesArray!= null)
        {
            for (int i = 0; i < obstaclesArray.Length; i++)
            {
                int indexTile = GetGridTileIndex(obstaclesArray[i].transform.position);
                int column = GetColumn(indexTile);
                int row = GetRow(indexTile);
                grid[row,column].isObstacle = true;
            }
        }
    }
    public void GetNeighbors(Node _node, ArrayList _neighbors)
    {
        Vector3 curNodePos = _node.position;
        int curNodeIndex = GetGridTileIndex(curNodePos);

        int row = GetRow(curNodeIndex);
        int column = GetColumn(curNodeIndex);

        //Top
        int NodeRow = row - 1;
        int NodeColumn = column;
        AssignNeighbor(NodeRow,NodeColumn,_neighbors);
        //Bottom
        NodeRow = row + 1;
        NodeColumn = column;
        AssignNeighbor(NodeRow, NodeColumn, _neighbors);
        //Left
        NodeRow = row;
        NodeColumn = column - 1;
        AssignNeighbor(NodeRow, NodeColumn, _neighbors);
        //Right
        NodeRow = row;
        NodeColumn = column + 1;
        AssignNeighbor(NodeRow, NodeColumn, _neighbors);
    }
    public void AssignNeighbor(int _row,int _column,ArrayList _neighbors)
    {
        //Check if neighbor is not out of bounds
        if (_row != -1 && _column != -1 &&
            _row < maxRows && _column < maxColumns)
        {
            Node neighborNode = grid[_row,_column];
            //Check if node is a neighbor
            if (!neighborNode.isObstacle)
            {
                //adds node to list of neighbors
                _neighbors.Add(neighborNode);
            }
        }
    }
    #region Helper Functions
    /// <summary>
    /// Gets the center position of a grid tile.
    /// </summary>
    /// <param name="_index">tile index</param>
    /// <returns>returns the center position of the grid tile</returns>
    private Vector3 GetGridTileCenter(int _index)
    {
        //Start Position of tile
        Vector3 tilePos = GetGridTilePosition(_index);
        float halfOfTile = gridTileSize / 2.0f;
        //Set position to the center of tile
        tilePos += new Vector3(halfOfTile,0,halfOfTile);
        return tilePos;
    }
    /// <summary>
    /// Gets the position of a grid tile
    /// </summary>
    /// <param name="_index"></param>
    /// <returns>returns the position of a grid tile</returns>
    private Vector3 GetGridTilePosition(int _index)
    {
        //Retrieve Column and Row index position of Tile
        int col = GetColumn(_index);
        int row = GetRow(_index);
        //Sets the position of the tile on the grid
            //X and Z coordinates of the tile on the grid
        float xPosInGrid = col * gridTileSize;
        float zPosInGrid = row * gridTileSize;
        
        return new Vector3(xPosInGrid,0,zPosInGrid);
    }
    /// <summary>
    /// Gets the index data of a grid tile(not real index. data based off grid not node)
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns>returns the index data of a grid tile</returns>
    private int GetGridTileIndex(Vector3 _pos)
    {
        //Check if position is within grid boundaries
        if (!IsInBounds(_pos))
        {
            return -1;
        }
        int col = (int)(_pos.x / gridTileSize);
        int row = (int)(_pos.x / gridTileSize);
        //Get the index of tile on the grid(Ex: if [row = 1] and [col = 0] and [maxColumn = 4] then result will be 4)
        //*In order to get real index put this data into the GetRow/GetColumn functions
        return (row * maxColumns + col);
    }
    /// <summary>
    /// Gets the row of the grid tile
    /// </summary>
    /// <param name="_index"></param>
    /// <returns>returns the row of the grid tile</returns>
    private int GetRow(int _index)
    {
        int row = _index / maxColumns;
        return row;
    }
    /// <summary>
    /// Gets the column of the grid tile
    /// </summary>
    /// <param name="_index"></param>
    /// <returns>returns the column of the grid tile</returns>
    private int GetColumn(int _index)
    {
        int column = _index % maxColumns;
        return column;
    }
    private bool IsInBounds(Vector3 _pos)
    {
        //Get the max width and height of the grid
        float maxGridWidth = maxColumns * gridTileSize;
        float maxGridHeight = maxRows * gridTileSize;
        //checks if out of bounds
        return (_pos.x >= startOfGridPosition.x && _pos.x <= maxGridWidth &&
            _pos.z >= startOfGridPosition.z && _pos.z <= maxGridHeight);
    }
    #endregion
    #region Debug
    private void OnDrawGizmos()
    {
        if (showGrid)
        {
            DebugDrawGrid(this.transform.position,maxRows,maxColumns,gridTileSize,gridColor);
        }
    }
    private void DebugDrawGrid(Vector3 _origin,int _maxRows,int _maxColumns,float _cellSize, Color _color)
    {
        float width = _maxColumns * _cellSize;
        float height = _maxRows * _cellSize;

        //Draw horizontal grid lines
        for (int i = 0; i < _maxRows + 1; i++)
        {
            Vector3 startPos = _origin + i * _cellSize * new Vector3(0f,0f,1f);
            Vector3 endPos = startPos + width * new Vector3(1f, 0f, 0f);
            Debug.DrawLine(startPos, endPos, _color);
        }
        //Draw vertical grid lines
        for (int i = 0; i < _maxColumns + 1; i++)
        {
            Vector3 startPos = _origin + i * _cellSize * new Vector3(1f, 0f, 0f);
            Vector3 endPos = startPos + height * new Vector3(0f, 0f, 1f);
            Debug.DrawLine(startPos, endPos, _color);
        }
    }
    #endregion
}
