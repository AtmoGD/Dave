using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] GameObject contentObject = null;
    List<UIMenuItem> contentItems = new List<UIMenuItem>();
    int currentIndex = 0;
    UIMenuItem currentItem = null;

    private void Start()
    {
        if (contentObject)
        {
            contentItems.Clear();

            foreach (Transform child in contentObject.transform)
            {
                UIMenuItem item = child.GetComponent<UIMenuItem>();
                if (item)
                    contentItems.Add(item);
            }
        }

        UpdateSelection(-1);
    }

    public void UpdateSelection(int _dir)
    {
        if (contentItems.Count == 0) return;

        currentIndex += _dir;

        currentIndex = Mathf.Clamp(currentIndex, 0, contentItems.Count - 1);

        if (currentItem != contentItems[currentIndex])
        {
            currentItem?.Deselect();

            currentItem = contentItems[currentIndex];

            currentItem?.Select();
        }

    }

    public void InteractWithSelection()
    {
        currentItem?.Interact();
    }
}
