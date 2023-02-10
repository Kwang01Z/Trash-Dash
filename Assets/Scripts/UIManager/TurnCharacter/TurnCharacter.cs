using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCharacter : MonoBehaviour
{
    [SerializeField] Character m_character;
    bool isTuring;
    void Update()
    {
        m_character = GetComponentInChildren<Character>();
        if (m_character != null && !isTuring)
        {
            StartCoroutine(Turn());
        }
    }
    IEnumerator Turn()
    {
        isTuring = true;
        Quaternion newRot = m_character.transform.rotation;
        Quaternion turnAngle = Quaternion.Euler(0, newRot.y - 10, 0);
        newRot = turnAngle;
        m_character.transform.rotation = newRot;
        yield return new WaitForSeconds(1);
        isTuring = false;
    }
}
