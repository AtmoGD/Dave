using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] protected Animator animator = null;
    [SerializeField] protected GameObject contentObject = null;
    [SerializeField] protected int startingIndex = 0;
    protected List<UIMenuItem> contentItems = new List<UIMenuItem>();
    protected int currentIndex = 0;
    protected UIMenuItem currentItem = null;

    public void Awake()
    {
        currentIndex = startingIndex;

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

    public void Start()
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
