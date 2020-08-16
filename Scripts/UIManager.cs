using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image livesImage;
    public Image livesImage2;
    public Sprite[] lives;
    public Text ScoreText;
    public Text levelUp;
    public Image inicio;
    public int Score = 0;
    public GameObject TitleScreen;
    public Text Help;
    public Text bestScore;
    public GameManager _gameManager;
    private int qntEnemyInGame = 0;
    private int limitOfEnemysInGame = 10;
    [SerializeField]
    private int _bestScore = 0;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager._isCoOpMode == true)
            livesImage2.enabled = false;
    }

    public void notifyBornAndDeathEnemy(string ocasion)
    {
        if (ocasion == "death")
            qntEnemyInGame--;
        else if (ocasion == "born")
            qntEnemyInGame++;
    }

    public bool canBornEnemy()
    {
        return (qntEnemyInGame <= limitOfEnemysInGame);
    }

    public void ShowScreen()
    { 
        TitleScreen.SetActive(true);
        Help.enabled = true;
        ScoreText.enabled = false;
        levelUp.enabled = false;
        bestScore.text = "Melhor Score: " + _bestScore; 
        bestScore.enabled = true;
    }
    public void NotShowScreen ()
    {
        TitleScreen.SetActive(false);
        ScoreText.enabled = true;
        levelUp.enabled = true;
        Help.enabled = false;
        bestScore.enabled = false;
        Score = 0;
        UpdateNivel("Nivel: 0");
        UpdateScore(0);
    }
    
    
    public void UpdateNivel(string n)
    {
        levelUp.text = n;
    }
    public void UpdateLives(int l, byte who)
    {
        if (who == 1)
        {
            livesImage.sprite = lives[l];
        }
        else if (who == 2)
        {
            livesImage2.sprite = lives[l];
        }
        if (l <= 0)
        {
            if (_gameManager._isCoOpMode)
            {
                int nextPlayer = (int)Math.Pow(((who + 1) % 3), Math.Abs(who - 2));
                GameObject find = GameObject.Find("Player " + nextPlayer + "(Clone)");
                if (!find)
                {
                    if (Score > _bestScore)
                    {
                        _bestScore = Score;
                    }
                    ShowScreen();
                    _gameManager.SetStateOfGame(false);
                }
            }
            else
            {
                if (Score > _bestScore)
                {
                    _bestScore = Score;
                }
                ShowScreen();
                _gameManager.SetStateOfGame(false);
                
            }
        }
    }


    public void UpdateScore(int pontuacao)
    {
        Score += pontuacao;
        ScoreText.text = "Score: " + Score;
    }
}


