using UnityEngine;
using XLua;

[Hotfix]
public class SphereMove : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.down * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * Time.deltaTime);
        }
    }
}
