using UnityEngine;

public class VisualHints : MonoBehaviour
{
    public Material HueMaterial;

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
    }
}
