using UnityEngine;
using UnityEngine.Video;

public class mfiwananbedonewiththis : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<VideoPlayer>().loopPointReached += (VideoPlayer vp) => Application.Quit();
        Debug.Log("QUit applciatejigne");
    }

    // Update is called once per frame
    void Update()
    {
        /*if(!GetComponent<VideoPlayer>().isPlaying)
        {

        }*/
    }
}
