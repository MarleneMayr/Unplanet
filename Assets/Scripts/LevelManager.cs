using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] levels;
    private static int levelIndex = 0;

    public Level GetNext()
    {
        foreach (var l in levels)
        {
            l.gameObject.SetActive(false);
        }
        Level next = levels[levelIndex];
        next.gameObject.SetActive(true);
        next.Init();

        levelIndex++;
        if (levelIndex >= levels.Length) levelIndex = 0;
        return next;
    }
}
