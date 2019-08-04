using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsDisplayUi : MonoBehaviour
{
    [SerializeField] Text fpsText;
    

    // Update is called once per frame
    void Update()
    {
        fpsText.text = "FPS: " + (1.0f / Time.deltaTime);
    }
}
