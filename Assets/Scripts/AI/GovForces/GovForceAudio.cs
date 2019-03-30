using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovForceAudio : MonoBehaviour
{
    [Tooltip("Sets the maximum sound radius. Also sets the audioSource min distance to this number")]
    public float maxSoundRadius = 1f;
    private float distToRadius = 0;//Distance to reach from 0 to 1(max sound radius)
    public bool showDebug = false;

    private AStarNavigation astar;
    private AudioSource audio;
    private GameObject soundObj;

    // Start is called before the first frame update
    void Start()
    {
        AddSoundObject();

        astar = this.GetComponent<AStarNavigation>();
        audio = this.GetComponent<AudioSource>();
        this.GetComponent<AudioSource>().minDistance = maxSoundRadius;
    }

    // Update is called once per frame
    void Update()
    {
        AStarSoundEmission();
    }
    /// <summary>
    /// Creates an empty object to detect other colliders
    /// </summary>
    private void AddSoundObject()
    {
        //Creates an empty object
        soundObj = new GameObject("Sound");
        //set empty object as this object's child
        soundObj.transform.SetParent(this.transform, false);
       
        soundObj.AddComponent<SphereCollider>(); //Adds a sphere collider to the empty object
        soundObj.GetComponent<SphereCollider>().isTrigger = true;//Set empty object sphere collider to be a trigger

        //set hearing aspect to GovForce
        soundObj.AddComponent<HearAspect>();
        soundObj.GetComponent<HearAspect>().aspect = Aspect.AspectName.GovForce;
    }
    /// <summary>
    /// Plays the sound depending on if astar goal is reached
    /// </summary>
    private void AStarSoundEmission()
    {
        //Check if the object reaches the their target
        if (!astar.reachedGoal)
        {
            PlaySound(true);
        }
        else
        {
            PlaySound(false);
        }
    }
    /// <summary>
    /// Turns the audio sound on/off & updates the current sound radius
    /// </summary>
    /// <param name="_makesound"></param>
    private void PlaySound(bool _makesound)
    {
        if (_makesound && !audio.isPlaying)
        {
            audio.Play();
        }
        else if(!_makesound && audio.isPlaying)
        {
            audio.Stop();
        }
        UpdateSoundRadius();
    }
    /// <summary>
    /// Updates the current sound radius 
    /// </summary>
    private void UpdateSoundRadius()
    {
        if (audio.isPlaying)
        {
            distToRadius += Time.deltaTime;
            soundObj.GetComponent<SphereCollider>().radius = Mathf.Lerp(0, audio.minDistance, distToRadius);
            if (distToRadius >= 1)
            {
                distToRadius = 0;
            }
        }
        else
        {
            soundObj.GetComponent<SphereCollider>().radius = 0;
        }
    }
}
