using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static GridManager instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first GridManager object in the scene.
                instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if (instance == null)
                    Debug.Log("Could not locate an GridManager object. \n You have to have exactly one GridManager in the scene.");
            }
            return instance;
        }
    }

    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnApplicationQuit()
    {
        instance = null;
    }

    public int maxRows;
    public int maxColumns;
    public float gridTileSize;
    [Tooltip("The group to identify as an obstacle")]
    public string obstacleTag = "Obstacle";
    public bool showGrid = true;
    public bool showObstacleBlocks = true;
    public Color gridColor = Color.blue;

    private Vector3 startPosition;//Origin of the grid manager
    private GameObject[] obstacleArray;
    public Node[,] grid { get; set; }

    

    //Initialise the grid manager
    void Awake()
    {
        startPosition = this.transform.position;
        //Get the list of obstacles objects tagged as "Obstacle"
        obstacleArray = GameObject.FindGameObjectsWithTag(obstacleTag);
        CalculateObstacles();
    }

    /// <summary>
    /// Calculate which tiles in the grids are mark as obstacles
    /// </summary>
    void CalculateObstacles()
    {
        //Initialise the nodes
        grid = new Node[maxColumns, maxRows];

        int index = 0;
        //Setup Grid
        for (int i = 0; i < maxColumns; i++)
        {
            for (int j = 0; j < maxRows; j++)
            {
                Vector3 cellPos = GetGridTileCenter(index);
                //set node position to the center of a grid tile
                Node node = new Node(cellPos);
                grid[i, j] = node;

                index++;
            }
        }

        //For each obstacle found on the world, record it in our list
        //Sets all obstacles to Grid
        if (obstacleArray != null && obstacleArray.Length > 0)
        {
            foreach (GameObject data in obstacleArray)
            {
                
                int obstSizeZ = (int)(Mathf.Ceil(data.transform.lossyScale.x)/gridTileSize);//X Axis on grid from origin
                int obstSizeX = (int)(Mathf.Ceil(data.transform.lossyScale.z)/gridTileSize);//Y Axis on grid from origin
                //Amount of tiles in front of the center of the obstacle
                int forwardXLength = 0;
                int forwardZLength = 0;
                //Amount of tiles in the back of the center of the obstacle
                int backwardXLength = 0;
                int backwardZLength = 0;
                if (obstSizeX % 2 != 0)
                {
                    //Odd
                    //if the obstacle occupy an odd # of tiles along X-Axis
                    forwardXLength += (obstSizeX - 1) / 2;
                    backwardXLength += (obstSizeX - 1) / 2;
                }
                else
                {
                    //Even
                    //if the obstacle occupy an even # of tiles along X-Axis
                    forwardXLength += (obstSizeX / 2);
                    backwardXLength += obstSizeX - forwardXLength;
                }
                if (obstSizeZ % 2 != 0)
                {
                    //Odd
                    //if the obstacle occupy an odd # of tiles along Z-Axis
                    forwardZLength += (obstSizeZ - 1) / 2;
                    backwardZLength += (obstSizeZ - 1) / 2;
                }
                else
                {
                    //Even
                    //if the obstacle occupy an even # of tiles along Z-Axis
                    forwardZLength += (obstSizeZ / 2);
                    backwardZLength += obstSizeZ - forwardZLength;
                }
                AssignObstaclesToGrid(data,forwardXLength,backwardXLength,forwardZLength,backwardZLength);

            }
        }
    }
    #region Helper Functions
    /// <summary>
    /// Assign Obstacles to the grid based on the amount of tiles current obstacle occupies
    /// </summary>
    /// <param name="_curobstacle">current obstacle in question</param>
    /// <param name="_xforlength">amount of tiles in front of current position X</param>
    /// <param name="_xbacklength">amount of tiles behind current position X</param>
    /// <param name="_zforlength">amount of tiles in front of current position Z</param>
    /// <param name="_zbacklength">amount of tiles behind current position Z</param>
    private void AssignObstaclesToGrid(GameObject _curobstacle,int _xforlength,int _xbacklength, int _zforlength, int _zbacklength)
    {
        //ForwardX
        for (int i = 0; i < _xforlength + 1; i++)
        {
            int indexCell = GetGridIndex(_curobstacle.transform.position);
            int row = GetColumn(indexCell);//row
            int col = GetRow(indexCell);//column

            //Also make the node a blocked status
            grid[col + i, row].isObstacle = true;
        }
        //BackwardX
        for (int i = 0; i < _xbacklength + 1; i++)
        {
            int indexCell = GetGridIndex(_curobstacle.transform.position);
            int row = GetColumn(indexCell);//row
            int col = GetRow(indexCell);//column

            //Also make the node a blocked status
            grid[col - i, row].isObstacle = true;
        }
        //ForwardZ
        for (int i = 0; i < _zforlength + 1; i++)
        {
            int indexCell = GetGridIndex(_curobstacle.transform.position);
            int row = GetColumn(indexCell);//row
            int col = GetRow(indexCell);//column

            //Also make the node a blocked status
            grid[col, row + i].isObstacle = true;
        }
        //BackwardZ
        for (int i = 0; i < _zbacklength + 1; i++)
        {
            int indexCell = GetGridIndex(_curobstacle.transform.position);
            int row = GetColumn(indexCell);//row
            int col = GetRow(indexCell);//column

            //Also make the node a blocked status
            grid[col, row - i].isObstacle = true;
        }
    }
    /// <summary>
    /// Returns position of the grid tile in world coordinates
    /// </summary>
    public Vector3 GetGridTileCenter(int _index)
    {
        //Start Position of tile
        Vector3 tilePos = GetGridTilePosition(_index);
        //Set position to the center of tile
        tilePos.x += (gridTileSize / 2.0f);
        tilePos.z += (gridTileSize / 2.0f);

        return tilePos;
    }

    /// <summary>
    /// Returns position of the grid tile in a given index
    /// </summary>
    public Vector3 GetGridTilePosition(int _index)
    {
        //Retrieve Column and Row index position of Tile
        int row = GetRow(_index);
        int col = GetColumn(_index);
        //Sets the position of the tile on the grid
            //X and Z coordinates of the tile on the grid
        float xPosInGrid = col * gridTileSize;
        float zPosInGrid = row * gridTileSize;

        return startPosition + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }

    /// <summary>
    /// Gets the index data of a grid tile(not real index. data based off grid not node)
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns>returns the index data of a grid tile</returns>
    public int GetGridIndex(Vector3 _pos)
    {
        //Check if position is within grid boundaries
        if (!IsInBounds(_pos))
        {
            return -1;
        }

        _pos -= startPosition;

        int col = (int)(_pos.x / gridTileSize);
        int row = (int)(_pos.z / gridTileSize);
        //Get the index of tile on the grid(Ex: if [row = 1] and [col = 0] and [maxColumn = 4] then result will be 4)
        //*In order to get real index put this data into the GetRow/GetColumn functions
        return (row * maxColumns + col);
    }

    /// <summary>
    /// Get the row number of the grid cell in a given index
    /// </summary>
    public int GetRow(int index)
    {
        int row = index / maxColumns;
        return row;
    }

    /// <summary>
    /// Get the column number of the grid cell in a given index
    /// </summary>
    public int GetColumn(int index)
    {
        int col = index % maxColumns;
        return col;
    }

    /// <summary>
    /// Check whether the current position is inside the grid or not
    /// </summary>
    public bool IsInBounds(Vector3 pos)
    {
        //Get the max width and height of the grid
        float width = maxColumns * gridTileSize;
        float height = maxRows * gridTileSize;
        //checks if out of bounds
        return (pos.x >= startPosition.x && pos.x <= startPosition.x + width && pos.z <= startPosition.z + height && pos.z >= startPosition.z);
    }


    /// <summary>
    /// Get the neighbor nodes in 4 different directions
    /// </summary>
    public void GetNeighbors(Node _node, ArrayList _neighbors)
    {
        Vector3 neighborPos = _node.position;
        int neighborIndex = GetGridIndex(neighborPos);

        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);

        //Top
        int NodeRow = row - 1;
        int NodeColumn = column;
        AssignNeighbour(NodeRow, NodeColumn, _neighbors);

        //Bottom
        NodeRow = row + 1;
        NodeColumn = column;
        AssignNeighbour(NodeRow, NodeColumn, _neighbors);

        //Right
        NodeRow = row;
        NodeColumn = column + 1;
        AssignNeighbour(NodeRow, NodeColumn, _neighbors);

        //Left
        NodeRow = row;
        NodeColumn = column - 1;
        AssignNeighbour(NodeRow, NodeColumn, _neighbors);
    }

    /// <summary>
    /// Check the neighbour. If it's not an obstacle, assign the neighbor.
    /// </summary>
    /// <param name='row'>
    /// Row.
    /// </param>
    /// <param name='column'>
    /// Column.
    /// </param>
    /// <param name='neighbors'>
    /// Neighbors.
    /// </param>
    void AssignNeighbour(int row, int column, ArrayList neighbors)
    {
        //Check if neighbor is not out of bounds
        if (row != -1 && column != -1 && row < maxRows && column < maxColumns)
        {
            Node nodeToAdd = grid[row, column];
            //Check if node is a neighbor
            if (!nodeToAdd.isObstacle)
            {
                //adds node to list of neighbors
                neighbors.Add(nodeToAdd);
            }
        }
    }
#endregion
    #region Debug
    /// <summary>
    /// Show Debug Grids and obstacles inside the editor
    /// </summary>
    void OnDrawGizmos()
    {
        //Draw Grid
        if (showGrid)
        {
            DebugDrawGrid(this.transform.position, maxRows, maxColumns, gridTileSize, gridColor);
        }

        //Grid Start Position
        Gizmos.DrawSphere(transform.position, 0.5f);

        //Draw Obstacle obstruction
        if (showObstacleBlocks)
        {
           

            if (obstacleArray != null && obstacleArray.Length > 0)
            {
                foreach (GameObject data in obstacleArray)
                {

                    int obstSizeX = Mathf.CeilToInt(data.transform.lossyScale.x);
                    int obstSizeZ = Mathf.CeilToInt(data.transform.lossyScale.z);
                    Vector3 obstacleTileSize = new Vector3(obstSizeX, 1.0f, obstSizeZ);
                    Gizmos.DrawCube(GetGridTileCenter(GetGridIndex(data.transform.position)), obstacleTileSize);
                }
            }
        }
    }

    /// <summary>
    /// Draw the debug grid lines in the rows and columns order
    /// </summary>
    public void DebugDrawGrid(Vector3 _origin, int _numRows, int _numCols, float _cellSize, Color _color)
    {
        float width = (_numCols * _cellSize);
        float height = (_numRows * _cellSize);

        // Draw the horizontal grid lines
        for (int i = 0; i < _numRows + 1; i++)
        {
            Vector3 startPos = _origin + i * _cellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, _color);
        }

        // Draw the vertial grid lines
        for (int i = 0; i < _numCols + 1; i++)
        {
            Vector3 startPos = _origin + i * _cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPos, endPos, _color);
        }
    }
    #endregion
}
