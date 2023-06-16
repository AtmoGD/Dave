using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] protected Animator animator = null;
    [SerializeField] protected bool selectOnStart = true;
    [SerializeField] protected UIMenuItem startingItem = null;
    [SerializeField] protected List<UIMenuItem> contentItems = new List<UIMenuItem>();
    [field: SerializeField] public bool CanBeCanceled { get; private set; } = true;
    [SerializeField] protected UnityEvent onTryCancelEvents = new UnityEvent();
    protected UIMenuItem currentItem = null;

    public virtual void Start()
    {
        if (selectOnStart && startingItem)
        {
            currentItem = startingItem;
            currentItem.Select();
        }
    }

    public virtual void UpdateSelection(Vector2 _dir)
    {
        if (contentItems.Count == 0) return;

        Direction dir = Utils.ClampInFourDirections(_dir);
        UIMenuItem nextItem = null;
        switch (dir)
        {
            case Direction.Up:
                nextItem = currentItem.up;
                break;
            case Direction.Down:
                nextItem = currentItem.down;
                break;
            case Direction.Left:
                nextItem = currentItem.left;
                break;
            case Direction.Right:
                nextItem = currentItem.right;
                break;
        }

        if (!nextItem) return;

        if (currentItem != nextItem)
        {
            currentItem?.Deselect();
            currentItem = nextItem;
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

    public virtual void TryToCancel()
    {
        onTryCancelEvents?.Invoke();
    }
}
