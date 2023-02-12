using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SPStudios.Tools;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_tutorialValidatedObstacles;
    [SerializeField] GameObject m_sideSlideTuto;
    [SerializeField] GameObject m_upSlideTuto;
    [SerializeField] GameObject m_downSlideTuto;
    [SerializeField] GameObject m_finishTuto;

    protected int m_TutorialClearedObstacle = 0;
    protected int k_ObstacleToClear = 3;
    protected bool m_IsTutorial;
    protected bool m_DisplayTutorial;
    protected bool m_CountObstacles = true;
    protected int m_CurrentSegmentObstacleIndex = 0;

    TrackSegment m_NextValidSegment = null;
    TrackManager m_trackManager;

    public int obstacleToClear { get { return k_ObstacleToClear; } }
    public int tutorialClearedObstacle { get { return m_TutorialClearedObstacle; }set { m_TutorialClearedObstacle = value; } }
    public TextMeshProUGUI tutorialValidatedObstacles { get { return m_tutorialValidatedObstacles; } }

    PlayerData m_playerData;
    public void SetUpTutorial(GamePlayManager manager)
    {
        m_playerData = manager.PlayerData;
        m_IsTutorial = !m_playerData.isTutorialDone;
        m_trackManager = manager.trackManager;
        m_trackManager.isTutorial = m_IsTutorial;
        if (m_IsTutorial)
        {
            tutorialValidatedObstacles.gameObject.SetActive(true);
            tutorialValidatedObstacles.text = $"0/{obstacleToClear}";
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

                    tutorialValidatedObstacles.text = $"{tutorialClearedObstacle}/{obstacleToClear}";
                }
            };
        }
    }
}
