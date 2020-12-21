using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : Menu
{
    public Transform IconGroup;
    public Toggle IconPrefab;

    private Queue<Toggle> icons = new Queue<Toggle>();

    public void SetIconAmount(int amount)
    {
        var diff = Mathf.Abs(amount - icons.Count);
        if (amount < icons.Count)
        {
            for (int i = 0; i < diff; i++)
            {
                var del = icons.Dequeue();
                Destroy(del.gameObject);
            }
        }
        else if (amount > icons.Count)
        {
            for (int i = 0; i < diff; i++)
            {
                var icon = Instantiate(IconPrefab, parent: IconGroup);
                icons.Enqueue(icon);
            }
        }

        ResetIconCount();
    }

    public void UpdateIconCount(int count)
    {
        var queueAr = icons.ToArray();
        for (int i = 0; i < count; i++)
        {
            queueAr[i].isOn = true;
        }
    }

    public void ResetIconCount()
    {
        foreach (var item in icons)
        {
            item.isOn = false;
        }
    }
}
