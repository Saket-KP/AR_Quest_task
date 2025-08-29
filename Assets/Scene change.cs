using UnityEngine;
using UnityEngine.SceneManagement;

public class XRSceneChanger : MonoBehaviour
{
    public float a = 20f;

    void Update()
    {
        // reduce "a" over time
        a -= Time.deltaTime;

        // when "a" runs out, change scene
        if (a <= 0)
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
