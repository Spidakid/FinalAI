using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidOrigin : MonoBehaviour
{
    public GameObject leader;
    [Tooltip("Flock origin's distance from the Leader")]
    public float distanceFromLeader = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = leader.transform.position + (leader.transform.forward * -1) * distanceFromLeader;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Debug
    private void OnValidate()
    {
        this.transform.position = leader.transform.position + (leader.transform.forward * -1) * distanceFromLeader;
    }
    #endregion
}
