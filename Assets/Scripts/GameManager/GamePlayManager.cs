using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPStudios.Tools;
public class GamePlayManager : MonoBehaviour
{
    [SerializeField] TrackManager m_trackManager;
    [SerializeField] WholeLayout m_wholeLayout;
    [SerializeField] TutorialManager m_tutorialManager;
    
    protected bool m_GameoverSelectionDone;
    protected float m_TimeSinceStart;
    protected bool m_Finished;
    List<PowerupIcon> m_PowerupIcons = new List<PowerupIcon>();
    
    public Modifier currentModifier = new Modifier();
    public TrackManager trackManager { get { return m_trackManager; } }

    PlayerData m_playerData;
    public PlayerData PlayerData { get { return m_playerData; } }
    void Start()
    {
        m_playerData = Singletons.Get<SaveManager>().GetData();
        ResetGame();
    }

    public void ResetGame()
    {
        m_GameoverSelectionDone = false;
        StartGame();
    }
    void StartGame()
    {
        m_wholeLayout.PauseButton.gameObject.SetActive(!m_trackManager.isTutorial);
        if (!m_trackManager.isRerun)
        {
            m_TimeSinceStart = 0;
            m_trackManager.characterController.currentLife = m_trackManager.characterController.maxLife;
        }
        currentModifier.OnRunStart(this);
        m_tutorialManager.SetUpTutorial(this);

        m_Finished = false;
        m_PowerupIcons.Clear();
        StartCoroutine(trackManager.Begin(this));
    }
    
    public void QuitToLoadout()
    {
        // Used by the pause menu to return immediately to loadout, canceling everything.
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        trackManager.End();
        trackManager.isRerun = false;
        Singletons.Get<SaveManager>().Save();
        Singletons.Get<GameController>().LoadScence("Main");
    }
}
