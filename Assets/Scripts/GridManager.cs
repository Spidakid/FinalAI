using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private Node[,] grid;
    private static int numofinstance;
    public int maxRow = 10;
    public int maxColumn = 10;
    /// <summary>
    /// GridManager Constructor
    /// </summary>
    public GridManager()
    {
        grid = new Node[maxColumn, maxRow];
    }
    private void Awake()
    {
        //Get reference to a single GridManager script
        Instance = (GridManager)FindObjectOfType(typeof(GridManager));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
