using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunerGameManager : MonoBehaviour
{
    private static LunerGameManager m_Instance;
    public static LunerGameManager Instance { get { return m_Instance; } }

    private bool m_HasGameStarted = false;
    public bool HasGameStarted { get { return m_HasGameStarted;  } }

    void Start()
    {
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
        }
    }

    public void StartGame()
    {

    }

    public void EndGame()
    {

    }
}
