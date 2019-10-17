using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LunerPlatform : AABBHull2D
{
    public float score = 1.5f;
    public float fuelGained = 10.0f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GameObject fuelText = new GameObject();
        fuelText.name = "Fuel Text";
        fuelText.transform.parent = gameObject.transform;
        fuelText.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f) * (halfLength.y + 1.0f);
        fuelText.AddComponent(typeof(TextMeshPro));
        TextMeshPro fuelTextComp = fuelText.GetComponent<TextMeshPro>();
        fuelTextComp.fontSize = 10.0f;
        fuelTextComp.alignment = TextAlignmentOptions.Center;
        fuelTextComp.text = fuelGained + " fuel";

        GameObject scoreText = new GameObject();
        scoreText.name = "Score Text";
        scoreText.transform.parent = gameObject.transform;
        scoreText.transform.localPosition = new Vector3(0.0f,1.0f,0.0f) * (halfLength.y + 2.0f);
        scoreText.AddComponent(typeof(TextMeshPro));
        TextMeshPro scoreTextComp = scoreText.GetComponent<TextMeshPro>();
        scoreTextComp.fontSize = 14.0f;
        scoreTextComp.alignment = TextAlignmentOptions.Center;
        scoreTextComp.text = score.ToString("F1") + "xp";
    }
}
