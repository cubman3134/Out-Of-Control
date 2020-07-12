using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    GameObject scoreText;
    GameObject livesText;
    GameObject gameOverObject;
    ScoreHandler scoreHandler;
    GameHandler gameHandler;
    public Button gameOverButton;

    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameObject.FindGameObjectWithTag("ScriptHandler").GetComponent<GameHandler>();
        scoreHandler = GameObject.FindGameObjectWithTag("ScriptHandler").GetComponent<ScoreHandler>();
        gameOverButton.onClick.AddListener(OnGameOverButtonClick);
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            if(gameObject.transform.GetChild(i).name == "ScoreText")
            {
                scoreText = gameObject.transform.GetChild(i).gameObject;
                continue;
            }
            if (gameObject.transform.GetChild(i).name == "LivesText")
            {
                livesText = gameObject.transform.GetChild(i).gameObject;
                continue;
            }
            if (gameObject.transform.GetChild(i).name == "GameOver")
            {
                gameOverObject = gameObject.transform.GetChild(i).gameObject;
                continue;
            }
        } 
    }

    public void SetGameOverObjectActive()
    {
        gameOverObject.SetActive(true);
    }

    void OnGameOverButtonClick()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.GetComponent<UnityEngine.UI.Text>().text = $"Score: {scoreHandler.Score}";
        livesText.GetComponent<UnityEngine.UI.Text>().text = $"Lives: {gameHandler.LivesLeft}";

        
    }
}
