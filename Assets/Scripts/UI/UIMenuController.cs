using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] protected Animator animator = null;
    [SerializeField] protected GameObject contentObject = null;
    [SerializeField] protected bool selectOnStart = true;
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
        if (selectOnStart)
            UpdateSelection(Vector2.zero);
    }

    public virtual void UpdateSelection(Vector2 _dir)
    {
        if (contentItems.Count == 0) return;

        currentIndex += (int)_dir.x;

        currentIndex = Mathf.Clamp(currentIndex, 0, contentItems.Count - 1);

        if (currentItem != contentItems[currentIndex])
        {
            currentItem?.Deselect();

            currentItem = contentItems[currentIndex];

            currentItem?.Select();
        }

    }

    public virtual void SetIsActive(bool _active)
    {
        animator.SetBool("Active", _active);
    }

    public virtual void InteractWithSelection()
    {
        currentItem?.Interact();
    }
}
