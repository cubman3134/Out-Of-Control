using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private MapHandler mapHandler;
    private int _hardnessLevel = 0;
    private int numObjectsPassed;
    private List<int> _increaseHardnessAtNumObjects = new List<int>() { 20, 100, 200, 500, 1000 };
    private int _livesLeft;
    private float _damageThreshold;
    private float _lastDamageTime;
    public int HardnessLevel { get { return _hardnessLevel; } }
    public List<int> IncreaseHardnessAtNumObjects { get { return _increaseHardnessAtNumObjects; } }

    // Start is called before the first frame update
    void Start()
    {
        _damageThreshold = 2.0f;
        mapHandler = GameObject.FindGameObjectWithTag("ScriptHandler").GetComponent<MapHandler>();
        numObjectsPassed = 0;
        _livesLeft = 5;
    }

    public void LoseLife()
    {
        if(Time.time > _lastDamageTime + _damageThreshold)
        {
            _lastDamageTime = Time.time;
            _livesLeft -= 1;
            if(_livesLeft == 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {

    }

    public void PassObject()
    {
        numObjectsPassed += 1;
        if (_increaseHardnessAtNumObjects.Contains(numObjectsPassed))
        {
            _hardnessLevel += 1;
            if (_hardnessLevel == IncreaseHardnessAtNumObjects.Count)
            {
                mapHandler.AddDestination();
            }
            else
            {
                mapHandler.AddGasStation();
            }
        }
        
    }
}
