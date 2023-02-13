using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionBase : System.IComparable<MissionBase>
{
    // Mission type
    public enum MissionType
    {
        SINGLE_RUN,
        PICKUP,
        OBSTACLE_JUMP,
        SLIDING,
        MULTIPLIER
    }
    public MissionType missionType;
    public float progress;
    public float max;
    public int reward;
    public int level;
    public string desc;
   /* public bool isComplete { get { return (progress / max) >= 1.0f; } }*/

    public int CompareTo(MissionBase other)
    {
        return this.missionType.CompareTo(other.missionType);
    }
    public MissionBase GetMissionBaseFromType(MissionBase.MissionType a_missionBase, int levelMission)
    {
        switch (a_missionBase)
        {
            case MissionBase.MissionType.SINGLE_RUN:
                MissionBase missionSingleRun = new MissionBase();
                missionSingleRun.missionType = MissionBase.MissionType.SINGLE_RUN;
                missionSingleRun.progress = 0;
                missionSingleRun.max = 500 * levelMission;
                missionSingleRun.reward = 3 + 2 * levelMission;
                missionSingleRun.level = levelMission;
                missionSingleRun.desc = "Run " + ((int)missionSingleRun.max) + "m in a single run";
                return missionSingleRun;

            case MissionBase.MissionType.PICKUP:
                MissionBase missionPickUp = new MissionBase();
                missionPickUp.missionType = MissionBase.MissionType.PICKUP;
                missionPickUp.progress = 0;
                missionPickUp.max = 1000 * levelMission;
                missionPickUp.reward = 3 + 2 * levelMission;
                missionPickUp.level = levelMission;
                missionPickUp.desc = "Pickup " + missionPickUp.max + " fishbones";
                return missionPickUp;
            case MissionBase.MissionType.OBSTACLE_JUMP:
                MissionBase missionObstacleJump = new MissionBase();
                missionObstacleJump.missionType = MissionBase.MissionType.OBSTACLE_JUMP;
                missionObstacleJump.progress = 0;
                missionObstacleJump.max = 20 * levelMission;
                missionObstacleJump.reward = 3 + 2 * levelMission;
                missionObstacleJump.level = levelMission;
                missionObstacleJump.desc = "Jump over " + ((int)missionObstacleJump.max) + " barriers";
                return missionObstacleJump;
            case MissionBase.MissionType.SLIDING:
                MissionBase missionSliding = new MissionBase();
                missionSliding.missionType = MissionBase.MissionType.SLIDING;
                missionSliding.progress = 0;
                missionSliding.max = 20 * levelMission;
                missionSliding.reward = 3 + 2 * levelMission;
                missionSliding.level = levelMission;
                missionSliding.desc = "Slide for " + ((int)missionSliding.max) + "m";
                return missionSliding;
            case MissionBase.MissionType.MULTIPLIER:
                MissionBase missionMultiplayer = new MissionBase();
                missionMultiplayer.missionType = MissionBase.MissionType.MULTIPLIER;
                missionMultiplayer.progress = 0;
                missionMultiplayer.max = 3 * levelMission;
                missionMultiplayer.reward = 3 + 2 * levelMission;
                missionMultiplayer.level = levelMission;
                missionMultiplayer.desc = "Reach a x" + ((int)missionMultiplayer.max) + " multiplier";
                return missionMultiplayer;

        }
        return new MissionBase();
    }
    public void RunStart(TrackManager manager)
    {
        switch (missionType)
        {
            case MissionType.SINGLE_RUN:
                progress = 0;
                break;
            case MissionType.PICKUP:
                manager.previousCoinAmount = 0;
                break;
            case MissionType.OBSTACLE_JUMP:
                manager.m_Previous = null;
                manager.m_Hits = new Collider[manager.k_HitColliderCount];
                break;
            case MissionType.SLIDING:
                manager.m_PreviousWorldDist = manager.worldDistance;
                break;
            case MissionType.MULTIPLIER:
                progress = 0;
                break;
        }
    }
    public void UpdateMission(TrackManager manager)
    {
        switch (missionType)
        {
            case MissionType.SINGLE_RUN:
                progress = manager.worldDistance;
                break;
            case MissionType.PICKUP:
                int coins = manager.characterController.coins - manager.previousCoinAmount;
                progress += coins;

                manager.previousCoinAmount = manager.characterController.coins;
                break;
            case MissionType.OBSTACLE_JUMP:
                if (manager.characterController.isJumping)
                {
                    Vector3 boxSize = manager.characterController.characterCollider.collider.size + manager.k_CharacterColliderSizeOffset;
                    Vector3 boxCenter = manager.characterController.transform.position - Vector3.up * boxSize.y * 0.5f;

                    int count = Physics.OverlapBoxNonAlloc(boxCenter, boxSize * 0.5f, manager.m_Hits);

                    for (int i = 0; i < count; ++i)
                    {
                        Obstacle obs = manager.m_Hits[i].GetComponent<Obstacle>();

                        if (obs != null && obs is AllLaneObstacle)
                        {
                            if (obs != manager.m_Previous)
                            {
                                progress += 1;
                            }

                            manager.m_Previous = obs;
                        }
                    }
                }
                break;
            case MissionType.SLIDING:
                if (manager.characterController.isSliding)
                {
                    float dist = manager.worldDistance - manager.m_PreviousWorldDist;
                    progress += dist;
                }

                manager.m_PreviousWorldDist = manager.worldDistance;
                break;
            case MissionType.MULTIPLIER:
                if (manager.multiplier > progress)
                    progress = manager.multiplier;
                break;
        }
    }
}
