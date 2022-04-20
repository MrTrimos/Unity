using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameControler : MonoBehaviour
{

    public static GameControler Instance;

    public static int Points { get; private set; } // оцки игры

    public static bool GameStarted { get; private set; } // старт игры

    [SerializeField]
    private TextMeshProUGUI gameResult;
    [SerializeField]
    private TextMeshProUGUI pointText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }



    // Start is called before the first frame update
    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        gameResult.text = "";

        SetPoints(0); // Обнуление очков
        GameStarted = true;

        Field.Instance.GenerateField();
    }


    public void Win()
    {
        GameStarted = false;
        gameResult.text = "You Win";
    }


    public void Lose()
    {
        GameStarted = false;
        gameResult.text = "You Lose";
    }

    public void AddPoints(int points) // добавление очков
    {
        SetPoints(Points + points);
    }

    private void SetPoints(int points) // устанавливает количиство очков и выводит на экран
    {
        Points = points;
        pointText.text = Points.ToString();
    }

}
