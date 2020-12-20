using UnityEngine;

public class ScreenEffect : MonoBehaviour
{
    public Material material;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        material.SetFloat("_Fade", 1);
    }

    public void Update()
    {
        //print("updating now");
        material.SetFloat("_Fade", 1 - GameState.progress);
    }
}
