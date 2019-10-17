using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LunerGameManager : MonoBehaviour
{
    private static LunerGameManager m_Instance;
    public static LunerGameManager Instance { get { return m_Instance; } }

    private float m_Score = 0;
    public float Score { get { return m_Score; } }

    private bool m_HasGameStarted = false;
    public bool HasGameStarted { get { return m_HasGameStarted;  } }

    private bool m_IsTransitioning = false;
    public bool IsTransitioning { get { return m_IsTransitioning; } }

    private bool m_HasGameEnded = false;
    public bool HasGameEnded { get { return m_HasGameEnded; } }

    private LunerLander lander;
    private float landerStartFuel;

    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI rotationText;
    private string m_TransitionMessage = "";

    void Start()
    {
        m_Instance = this;
        lander = FindObjectOfType<LunerLander>();
        landerStartFuel = lander.fuel;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_HasGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                m_IsTransitioning = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartGame();
            }
        }
        if(m_HasGameEnded)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ResetGame();
            }
        }
        

        fuelText.text = "Fuel Left: " + lander.fuel.ToString("F1");
        if(lander.fuel < 20.0f)
        {
            fuelText.color = Color.yellow;
        }
        else
        {
            fuelText.color = Color.white;
        }
        scoreText.text = "Score: " + Score.ToString("F1");
        velocityText.text = "Velocity: " + lander.velocity.magnitude.ToString("F2");
        if(lander.velocity.magnitude > lander.maxVelocityMagnitudeOnLanding)
        {
            velocityText.color = Color.red;
        }
        else if (lander.velocity.magnitude > lander.maxVelocityMagnitudeOnSafeLanding)
        {
            velocityText.color = Color.yellow;
        }
        else
        {
            velocityText.color = Color.white;
        }
        rotationText.text = "Rotation: " + lander.rotation.ToString("F2");
        if (lander.rotation > lander.maxRotationOffsetOnLanding || lander.rotation < -lander.maxRotationOffsetOnLanding)
        {
            rotationText.color = Color.red;
        }
        else
        {
            rotationText.color = Color.white;
        }
    }

    public void StartGame()
    {
        m_HasGameStarted = true;
    }

    public void OnGUI()
    {
        if(!m_HasGameStarted)
        {
            GUIStyle tStyle = GUI.skin.label;
            tStyle.fontSize = 40;
            tStyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(Screen.safeArea, "Press P to Play",tStyle);
        }
        if(m_IsTransitioning)
        {
            GUIStyle tStyle = GUI.skin.label;
            tStyle.fontSize = 40;
            tStyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(Screen.safeArea, m_TransitionMessage, tStyle);
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.6f), "Press P To Resume", tStyle);
        }
        
    }

    public void ShowTransitionMessage(string message)
    {
        m_IsTransitioning = true;
        m_TransitionMessage = message;
    }

    public void EndGame()
    {
        m_HasGameEnded = true;
    }

    public void addToScore(float toAdd)
    {
        m_Score += toAdd;
    }

    public void ResetGame()
    {
        m_Score = 0.0f;
        lander.fuel = landerStartFuel;
        m_HasGameEnded = false;
        m_IsTransitioning = false;
        lander.ResetGame();
    }
}
