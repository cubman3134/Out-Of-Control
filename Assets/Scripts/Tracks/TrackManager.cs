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
    GameObject scriptHandler;
    ScoreHandler scoreHandler;
    GameHandler gameHandler;

    GameObject walls;
    GameObject leftWall;
    GameObject rightWall;
    GameObject ground;

    GameObject obstacles;
    GameObject leftObstacle;
    GameObject middleObstacle;
    GameObject rightObstacle;

    List<GameObject> activeObstacles;

    GameObject bounds;
    GameObject scoreBounds;

    private void Start()
    {
        activeObstacles = new List<GameObject>() { leftObstacle, middleObstacle, rightObstacle };
        scoreCounted = false;
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        scriptHandler = GameObject.FindGameObjectWithTag("ScriptHandler");
        scoreHandler = scriptHandler.GetComponent<ScoreHandler>();
        gameHandler = scriptHandler.GetComponent<GameHandler>();
        int currentLevel = gameHandler.HardnessLevel;
        int numLevels = gameHandler.IncreaseHardnessAtNumObjects.Count;
        
        int maxPossibleNumObstacles = 2;
        int maxNumObstacles = Mathf.CeilToInt(maxPossibleNumObstacles * (currentLevel / numLevels));
        int minNumObstacles = Mathf.FloorToInt(maxPossibleNumObstacles * (currentLevel / numLevels));
        int numberOfObstacles = Random.Range(minNumObstacles, maxNumObstacles + 1);
        int amountToTakeAway = activeObstacles.Count - numberOfObstacles;
        HashSet<int> usedIndexes = new HashSet<int>();
        while (usedIndexes.Count != amountToTakeAway)
        {
            int randInt = Random.Range(0, activeObstacles.Count);
            if (usedIndexes.Contains(randInt)) continue;
            activeObstacles[randInt].SetActive(false);
        }
        for (int i = 0; i < activeObstacles.Count; i++)
        {
            if (!activeObstacles[i].activeSelf)
            {
                activeObstacles.RemoveAt(i);
                i = i - 1;
            }
        }

        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            if(gameObject.transform.GetChild(i).gameObject.name == "Bounds")
            {
                bounds = gameObject.transform.GetChild(i).gameObject;
                continue;
            }
            if(gameObject.transform.GetChild(i).gameObject.name == "Walls")
            {
                walls = gameObject.transform.GetChild(i).gameObject;
                continue;
            }
            if (gameObject.transform.GetChild(i).gameObject.name == "Obstacle")
            {
                obstacles = gameObject.transform.GetChild(i).gameObject;
                continue;
            }
            if (gameObject.transform.GetChild(i).gameObject.name == "ScoreBounds")
            {
                scoreBounds = gameObject.transform.GetChild(i).gameObject;
                continue;
            }
        }
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
    float maxDistance;

    private void Update()
    {
        //if distance between player and this area is too great, delete this game object
        //and only if the scoreCounted is true, that way we know that the player has already been here
        if(scoreCounted && Vector3.Distance(player.gameObject.transform.position, gameObject.transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
        //only check intersections if player is currently within the bounds of this game object
        if (bounds.GetComponent<BoxCollider>().bounds.Intersects(player.GetComponent<BoxCollider>().bounds))
        {
            if (scoreBounds.GetComponent<BoxCollider>().bounds.Intersects(player.GetComponent<BoxCollider>().bounds))
            {
                scoreCounted = true;

            }
            foreach (var curActiveObstacle in activeObstacles)
            {
                if (curActiveObstacle.GetComponent<BoxCollider>().bounds.Intersects(player.GetComponent<BoxCollider>().bounds))
                {
                    gameHandler.LoseLife();
                    break;
                }
            }
        }

        
    }
}
