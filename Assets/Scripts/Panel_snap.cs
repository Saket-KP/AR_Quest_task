using UnityEngine;
using UnityEngine.Video;

public class Panel_snap : MonoBehaviour
{
    public GameObject refer;
    private Transform pos;
    void Start()
    {
        pos = refer.GetComponent<Transform>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Panel")
        {
            collision.gameObject.transform.position = pos.position;
            collision.gameObject.transform.rotation = pos.rotation;
            collision.gameObject.transform.localScale = pos.localScale;

        }
    }
}