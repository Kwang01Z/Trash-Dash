using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SPStudios.Tools;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_tutorialValidatedObstacles;
    [SerializeField] GameObject m_sideSlideTuto;
    [SerializeField] GameObject m_upSlideTuto;
    [SerializeField] GameObject m_downSlideTuto;
    [SerializeField] GameObject m_finishTuto;
    [SerializeField] Button m_goToLoadout;

    protected int m_TutorialClearedObstacle = 0;
    protected int k_ObstacleToClear = 3;
    protected bool m_IsTutorial;
    protected bool m_DisplayTutorial;
    protected bool m_CountObstacles = true;
    protected int m_CurrentSegmentObstacleIndex = 0;
    protected bool m_WasMoving;

    TrackSegment m_NextValidSegment = null;
    TrackManager m_trackManager;

    PlayerData m_playerData;
    public void SetUpTutorial(GamePlayManager manager)
    {
        m_playerData = manager.PlayerData;
        m_IsTutorial = !m_playerData.isTutorialDone;
        m_trackManager = manager.trackManager;
        m_trackManager.isTutorial = m_IsTutorial;
        if (m_IsTutorial)
        {
            m_tutorialValidatedObstacles.text = $"0/{k_ObstacleToClear}";
            m_DisplayTutorial = true;
            // Setup NextValidSegment from trackManager when action run
            m_trackManager.newSegmentCreated = (segment) =>
            {
                if (m_trackManager.currentZone != 0 && !m_CountObstacles && m_NextValidSegment == null)
                {
                    m_NextValidSegment = segment;
                }
            };
            // Setup currentSegment from trackManager when action run
            m_trackManager.currentSegementChanged = (segment) =>
            {
                m_CurrentSegmentObstacleIndex = 0;
                if (!m_CountObstacles && m_trackManager.currentSegment == m_NextValidSegment)
                {
                    m_trackManager.characterController.currentTutorialLevel += 1;
                    m_CountObstacles = true;
                    m_NextValidSegment = null;
                    m_DisplayTutorial = true;

                    m_tutorialValidatedObstacles.text = $"{m_TutorialClearedObstacle}/{k_ObstacleToClear}";
                }
            };
        }
        m_goToLoadout.onClick.AddListener(delegate { FinishTutorial(); });
    }
    public void TutorialCheckObstacleClear()
    {
        if (m_trackManager.segments.Count == 0 || !m_trackManager.isTutorial)
        {
            return;
        }   
        if (m_trackManager.isMoving)
        {
            m_tutorialValidatedObstacles.gameObject.SetActive(true);
        }
        if (AudioListener.pause && !m_trackManager.characterController.tutorialWaitingForValidation)
        {
            m_DisplayTutorial = false;
            DisplayTutorial(false);
        }
        float ratio = m_trackManager.currentSegmentDistance / m_trackManager.currentSegment.worldLength;
        float nextObstaclePosition = m_CurrentSegmentObstacleIndex < m_trackManager.currentSegment.obstaclePositions.Length ? m_trackManager.currentSegment.obstaclePositions[m_CurrentSegmentObstacleIndex] : float.MaxValue;

        if (m_CountObstacles && ratio > nextObstaclePosition + 0.05f)
        {
            m_CurrentSegmentObstacleIndex += 1;

            if (!m_trackManager.characterController.characterCollider.tutorialHitObstacle)
            {
                m_TutorialClearedObstacle += 1;
                m_tutorialValidatedObstacles.text = $"{m_TutorialClearedObstacle}/{k_ObstacleToClear}";
            }

            m_trackManager.characterController.characterCollider.tutorialHitObstacle = false;

            if (m_TutorialClearedObstacle == k_ObstacleToClear)
            {
                m_TutorialClearedObstacle = 0;
                m_CountObstacles = false;
                m_NextValidSegment = null;
                m_trackManager.ChangeZone();

                m_tutorialValidatedObstacles.text = "Passed!";

                if (m_trackManager.currentZone == 0)
                {//we looped, mean we finished the tutorial.
                    m_trackManager.characterController.currentTutorialLevel = 3;
                    DisplayTutorial(true);
                }
            }
        }
        else if (m_DisplayTutorial && ratio > nextObstaclePosition - 0.1f)
        { 
            DisplayTutorial(true);
        }      
    }

    void DisplayTutorial(bool value)
    {
        if (value)
            Pause(false);
        else
        {
            Resume();
        }

        switch (m_trackManager.characterController.currentTutorialLevel)
        {
            case 0:
                m_sideSlideTuto.SetActive(value);
                m_trackManager.characterController.tutorialWaitingForValidation = value;
                break;
            case 1:
                m_upSlideTuto.SetActive(value);
                m_trackManager.characterController.tutorialWaitingForValidation = value;
                break;
            case 2:
                m_downSlideTuto.SetActive(value);
                m_trackManager.characterController.tutorialWaitingForValidation = value;
                break;
            case 3:
                m_finishTuto.SetActive(value);
                m_trackManager.characterController.StopSliding();
                m_trackManager.characterController.tutorialWaitingForValidation = value;
                break;
            default:
                break;
        }
    }


    public void FinishTutorial()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        m_playerData.isTutorialDone = true;
        Singletons.Get<SaveManager>().Save();
        Singletons.Get<GameController>().LoadScence("Main");
    }
    public void Pause(bool displayMenu = true)
    {
        AudioListener.pause = true;
        Time.timeScale = 0;

        m_WasMoving = m_trackManager.isMoving;
        m_trackManager.StopMove();
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        m_trackManager.StartMove(false);

        AudioListener.pause = false;
    }
}
