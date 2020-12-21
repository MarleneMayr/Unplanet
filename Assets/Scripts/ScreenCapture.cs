using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

public class ScreenCapture : MonoBehaviour
{
#if ENABLE_INPUT_SYSTEM
InputAction screenshot;

void Start()
{
    screenshot = new InputAction("CaptureScreenshot", binding: "<Keyboard>/c");
    screenshot.Enable();
}
#endif

    // Update is called once per frame
    void Update()
    {
        //if (Mathf.Approximately(screenshot.ReadValue<float>(), 1))
        //{ // capture screen
        //    UnityEngine.ScreenCapture.CaptureScreenshot("screenCapture", 2);
        //}
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            UnityEngine.ScreenCapture.CaptureScreenshot("screenCapture.png", 2);
    }
}
