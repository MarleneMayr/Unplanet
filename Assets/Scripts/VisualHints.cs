using UnityEngine;

public class VisualHints : MonoBehaviour
{
    public Material HueMaterial; // blue-orange
    public Material HueMaterial_Hint;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        HueMaterial.SetFloat("_Height", GameState.progress);
        HueMaterial_Hint.SetFloat("_Height", GameState.progress);
    }
}
