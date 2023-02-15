using SPStudios.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
public class GameOverManager : MonoBehaviour
{
    [SerializeField] Button m_runNewGame;
    [SerializeField] Button m_mainMenu;
    [SerializeField] public GamePlayManager m_gameManager;
    [SerializeField] TotalGameOver m_totalGameOver;
    [SerializeField] TMP_InputField m_namePlayer;
    [SerializeField] AudioClip gameOverTheme;
    private void OnEnable()
    {
        m_totalGameOver.Init(this);
        if (Singletons.Get<MusicPlayer>().GetStem(0) != gameOverTheme)
        {
            Singletons.Get<MusicPlayer>().SetStem(0, gameOverTheme);
            StartCoroutine(Singletons.Get<MusicPlayer>().RestartAllStems());
        }
        m_runNewGame.onClick.AddListener(delegate { RunGame(); });
        m_mainMenu.onClick.AddListener(delegate { GotoMainMenu(); });
    }
    private void OnDisable()
    {
        m_runNewGame.onClick.RemoveListener(delegate { RunGame(); });
        m_mainMenu.onClick.RemoveListener(delegate { GotoMainMenu(); });
    }
    void SaveNewScore()
    {
        int playerScore = m_gameManager.trackManager.characterController.characterCollider.deathData.score;
        int highestScore = 0;
        if (m_gameManager.PlayerData.highscores.Count > 0) 
        {
            highestScore = m_gameManager.PlayerData.highscores[0].score;
        }
        if (playerScore > highestScore)
        {
            string name = m_namePlayer.text;
            if (name.Equals("")) name = "Trash Cat";
            m_gameManager.PlayerData.AddHighScore(name, playerScore);
        }
        Singletons.Get<SaveManager>().Save();
    }
    void RunGame()
    {
        SaveNewScore();
        Singletons.Get<GameController>().LoadScence("RunGame");
    }
    void GotoMainMenu()
    {
        SaveNewScore();
        Singletons.Get<SaveManager>().Save();
        Singletons.Get<GameController>().LoadScence("Main");
    }
}
