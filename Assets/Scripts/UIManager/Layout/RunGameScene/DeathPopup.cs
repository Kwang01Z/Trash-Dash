using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DeathPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_premiumOwnCount;
    [SerializeField] Button m_premiumButton;
    [SerializeField] Button m_gameOverButton;
    PlayerData m_playerData;
    GamePlayManager m_gamePlay;
    public void Open(GamePlayManager manager)
    {
        m_gamePlay = manager;
        m_playerData = manager.PlayerData;
        m_premiumOwnCount.SetText(m_playerData.premium.ToString());
    }
    private void OnEnable()
    {
        m_premiumButton.onClick.AddListener(delegate { PremiumButtonPressed(); });
        m_gameOverButton.onClick.AddListener(delegate { GameOverButtonPressed(); });
    }
    private void OnDisable()
    {
        m_premiumButton.onClick.RemoveListener(delegate { PremiumButtonPressed(); });
        m_gameOverButton.onClick.RemoveListener(delegate { GameOverButtonPressed(); });
    }
    void PremiumButtonPressed()
    {
        if (m_playerData.premium >= 3)
        {
            m_playerData.premium -= 3;
            m_gamePlay.trackManager.isRerun = true;
            m_gamePlay.trackManager.characterController.currentLife = 1;
            m_gamePlay.StartGame();
            gameObject.SetActive(false);
        }
    }
    void GameOverButtonPressed()
    {
        m_gamePlay.OpenGameOverLayout();
        gameObject.SetActive(false);
    }
}
