using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;


[Hotfix]

public class ChangeSelf : MonoBehaviour
{
    public Vector3 m_CurrentScale;

    public Quaternion m_CurrentRotation;

    public Vector3 m_LaterScale;

    public Quaternion m_LaterRotation;
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentScale = this.transform.localScale;
        m_CurrentRotation = this.transform.localRotation;

    }

    public void ChangeScale(Vector3 scale)
    {
        m_LaterScale = scale * 2;
    }

    public void ChangeRotation(Quaternion rotation)
    {
        m_LaterRotation = Quaternion.Euler(rotation.eulerAngles.x + 30, rotation.eulerAngles.y + 30, rotation.eulerAngles.z + 30);
    }
}
