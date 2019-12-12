using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LunarLanderGameManager : MonoBehaviour
{
    public static LunarLanderGameManager m_Instance { get; private set; }
    public TextMeshProUGUI m_ScoreUIText;
    public TextMeshProUGUI m_LivesUIText;
    public int m_Lives = 3;
    public OBBHull3D m_Player;

    private Quaternion m_PlayerStartRot;
    private Vector3 m_PlayerStartPos;



    public int m_Score = 0;
    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
        m_PlayerStartPos = m_Player.transform.position;
        m_PlayerStartRot = m_Player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        m_ScoreUIText.text = m_Score.ToString();
        m_LivesUIText.text = m_Lives.ToString();
    }

    public void ResetPlayer()
    {
        m_Lives--;
        m_Player.m_Position = m_PlayerStartPos;
        m_Player.m_Rotation = m_PlayerStartRot;
        m_Player.m_AngularVelocity = Vector3.zero;
        m_Player.m_AngularAcceleration = Vector3.zero;
        m_Player.m_Velocity = Vector3.zero;
        m_Player.m_Acceleration = Vector3.zero;
    }
}
