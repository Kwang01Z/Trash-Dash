using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPStudios.Tools;
public class GamePlayManager : MonoBehaviour
{
    [SerializeField] TrackManager m_trackManager;
    [SerializeField] public WholeLayout m_wholeLayout;
    [SerializeField] TutorialManager m_tutorialManager;
    [SerializeField] DeathPopup m_deathPopup;
    [SerializeField] GameOverManager m_gameOverManager;
    [SerializeField] AudioClip gameTheme;
    protected bool m_GameoverSelectionDone;
    protected float m_TimeSinceStart;
    protected bool m_Finished;
    public List<PowerupIcon> m_PowerupIcons = new List<PowerupIcon>();
    
    public TrackManager trackManager { get { return m_trackManager; } }

    PlayerData m_playerData;
    public PlayerData PlayerData { get { return m_playerData; } }
    void Start()
    {
        m_playerData = Singletons.Get<SaveManager>().GetData();
        if (Singletons.Get<MusicPlayer>().GetStem(0) != gameTheme)
        {
            Singletons.Get<MusicPlayer>().SetStem(0, gameTheme);
            StartCoroutine(Singletons.Get<MusicPlayer>().RestartAllStems());
        }
        ResetGame();
    }
    private void Update()
    {
        if (trackManager.isLoaded)
        {
            m_tutorialManager.TutorialCheckObstacleClear();
            m_wholeLayout.UpdateUI(this);
            trackManager.UpdateInventoryUsing(this);
        }
    }

    public void ResetGame()
    {
        m_GameoverSelectionDone = false;
        StartGame();
    }
    public void StartGame()
    {
        m_wholeLayout.PauseButton.gameObject.SetActive(!m_trackManager.isTutorial);
        if (!m_trackManager.isRerun)
        {
            m_TimeSinceStart = 0;
            m_trackManager.characterController.currentLife = m_trackManager.characterController.maxLife;
        }
        m_tutorialManager.SetUpTutorial(this);

        m_Finished = false;
        m_PowerupIcons.Clear();
        StartCoroutine(trackManager.Begin(this));
    }
    public void OpenDeathPopup()
    {
        m_deathPopup.gameObject.SetActive(true);
        m_deathPopup.Open(this);
        Singletons.Get<SaveManager>().Save();
    }
    public void OpenGameOverLayout()
    {
        m_gameOverManager.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    
}
