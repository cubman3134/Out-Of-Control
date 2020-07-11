using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameObject = UnityEngine.GameObject;

/// <summary>
/// The TrackManager handles creating track segments, moving them and handling the whole pace of the game.
/// 
/// The cycle is as follows:
/// - Begin is called when the game starts.
///     - if it's a first run, init the controller, collider etc. and start the movement of the track.
///     - if it's a rerun (after watching ads on GameOver) just restart the movement of the track.
/// - Update moves the character and - if the character reaches a certain distance from origin (given by floatingOriginThreshold) -
/// moves everything back by that threshold to "reset" the player to the origin. This allow to avoid floating point error on long run.
/// It also handles creating the tracks segements when needed.
/// 
/// If the player has no more lives, it pushes the GameOver state on top of the GameState without removing it. That way we can just go back to where
/// we left off if the player watches an ad and gets a second chance. If the player quits, then:
/// 
/// - End is called and everything is cleared and destroyed, and we go back to the Loadout State.
/// </summary>
public class TrackManager : MonoBehaviour
{
    GameObject player;

    GameObject walls;
    GameObject leftWall;
    GameObject rightWall;
    GameObject ground;

    GameObject obstacles;
    GameObject leftObstacle;
    GameObject middleObstacle;
    GameObject rightObstacle;

    Vector3 m_CameraOriginalPos = Vector3.zero;
    protected int m_Score;
    protected float m_ScoreAccum;
    protected bool m_Rerun;     // This lets us know if we are entering a game over (ads) state or starting a new game (see GameState)

    private void Start()
    {
        scoreCounted = false;
        if (gameObject.transform.GetChild(0).gameObject.name == "Walls")
        {
            walls = gameObject.transform.GetChild(0).gameObject;
            obstacles = gameObject.transform.GetChild(1).gameObject;
        } else
        {
            walls = gameObject.transform.GetChild(1).gameObject;
            obstacles = gameObject.transform.GetChild(0).gameObject;
        }
        for(int i = 0; i < walls.transform.childCount; i++)
        {
            if(walls.transform.GetChild(i).gameObject.name == "Right")
            {
                rightWall = walls.transform.GetChild(i).gameObject;
                continue;
            }
            if (walls.transform.GetChild(i).gameObject.name == "Ground")
            {
                ground = walls.transform.GetChild(i).gameObject;
                continue;
            }
            if (walls.transform.GetChild(i).gameObject.name == "Left")
            {
                leftWall = walls.transform.GetChild(i).gameObject;
                continue;
            }
        }
        for(int i = 0; i < obstacles.transform.childCount; i++)
        {
            if (obstacles.transform.GetChild(i).gameObject.name == "Left")
            {
                leftObstacle = obstacles.transform.GetChild(i).gameObject;
                continue;
            }
            if (obstacles.transform.GetChild(i).gameObject.name == "Mid")
            {
                middleObstacle = obstacles.transform.GetChild(i).gameObject;
                continue;
            }
            if (obstacles.transform.GetChild(i).gameObject.name == "Right")
            {
                rightObstacle = obstacles.transform.GetChild(i).gameObject;
                continue;
            }
        }

    }

    bool scoreCounted;

    private void Update()
    {
        //if distance between player and this area is too great, delete this game object

        
    }
}
