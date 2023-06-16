using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCreditsController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public bool IsCredits { get; private set; } = false;

    public void PlayerCanceled()
    {
        if (IsCredits)
            ShowStartButtons();
    }

    public void ShowStartButtons()
    {
        anim.SetTrigger("ShowStartButtons");
        IsCredits = false;
    }

    public void ShowCredits()
    {
        anim.SetTrigger("ShowCreditsMenu");
        IsCredits = true;
    }

    public void HideAll()
    {
        anim.SetTrigger("HideAll");
    }
}
