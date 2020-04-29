using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _pressRkeyText;
    private GameManager _gameManager;
    [SerializeField]
    private Scrollbar _thrusters;
    private Scrollbar _thrusterValue;
   
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _pressRkeyText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _thrusterValue = _thrusters.GetComponent<Scrollbar>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

   public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void GameOverSequence()
    {
        StartCoroutine(GameOverFlickerRoutine());
        _pressRkeyText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    public void ThrusterBarUp()
    {

        _thrusterValue.value += 1f;

        if (_thrusterValue.value >= 1f)
        {
            _thrusterValue.value = 1f;             
        }
       
    }
     public void ThrusterBarDown()
    {
      
        StartCoroutine(ThusterCooldownRoutine());

    } 

    IEnumerator ThusterCooldownRoutine()
    {    
            yield return new WaitForSeconds(2.0f);           
            _thrusterValue.value = 0;           
    }

}
