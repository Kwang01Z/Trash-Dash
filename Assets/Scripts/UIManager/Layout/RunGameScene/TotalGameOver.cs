using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TotalGameOver : MonoBehaviour
{
    [SerializeField] GameObject m_winLayout;
    [SerializeField] GameObject m_tryAgainLayout;
    [SerializeField] TextMeshProUGUI m_NameTop;
    [SerializeField] TextMeshProUGUI m_ScoreTop;
    [SerializeField] TextMeshProUGUI m_ScorePlayer;
    GamePlayManager m_manager;
    int playerScore;
    int highestScore;
    public void Init(GameOverManager gameOver)
    {
        m_manager = gameOver.m_gameManager;
        playerScore = m_manager.trackManager.characterController.characterCollider.deathData.score;
        if (m_manager.PlayerData.highscores.Count > 0)
        {
            highestScore = m_manager.PlayerData.highscores[0].score;
            m_NameTop.SetText(m_manager.PlayerData.highscores[0].name);
        } 
        if (playerScore > highestScore)
        {
            OpenWinLayout();
        }
        m_ScoreTop.SetText(highestScore.ToString());
        m_ScorePlayer.SetText(playerScore.ToString());

    }
    void OpenWinLayout(bool op = true)
    {
        m_winLayout.gameObject.SetActive(op);
        m_tryAgainLayout.gameObject.SetActive(!op);
    }
}
