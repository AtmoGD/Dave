using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] GameObject contentObject = null;
    List<UIMenuItem> contentItems = new List<UIMenuItem>();
    int currentIndex = 0;
    UIMenuItem currentItem = null;

    private void Awake()
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
    }

    private void Start()
    {
        UpdateSelection(0);
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

    public void SetIsActive(bool _active)
    {
        animator.SetBool("Active", _active);
    }

    public void InteractWithSelection()
    {
        currentItem?.Interact();
    }
}
