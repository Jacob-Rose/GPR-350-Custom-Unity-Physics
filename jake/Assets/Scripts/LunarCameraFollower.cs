using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarCameraFollower : MonoBehaviour
{

    public GameObject m_Target;
    public bool lockPitch = false;
    public bool lockYaw = false;
    public bool lockRoll = false;

    private Vector3 startOffset;
    // Start is called before the first frame update
    void Start()
    {
        startOffset = transform.position - m_Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRot = m_Target.transform.rotation;
        Matrix4x4 mat = Matrix4x4.TRS(startOffset, targetRot, Vector3.one);
        transform.position = mat.MultiplyPoint(Vector3.zero);
    }
}
