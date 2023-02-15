using SPStudios.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button m_resumeButton;
    [SerializeField] Button m_mainmenuButton;
    private void OnEnable()
    {
        Time.timeScale = 0;
        m_resumeButton.onClick.AddListener(delegate { ResumeButtonPressed(); });
        m_mainmenuButton.onClick.AddListener(delegate { GotoMainMenu(); });
    }
    private void OnDisable()
    {
        m_resumeButton.onClick.RemoveListener(delegate { ResumeButtonPressed(); });
        m_mainmenuButton.onClick.RemoveListener(delegate { GotoMainMenu(); });
    }
    void ResumeButtonPressed()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    void GotoMainMenu()
    {
        Time.timeScale = 1;
        Singletons.Get<SaveManager>().Save();
        Singletons.Get<GameController>().LoadScence("Main");
    }
}
