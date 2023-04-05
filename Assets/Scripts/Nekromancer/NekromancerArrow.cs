using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum VizualizationType
{
    MoveDir,
    LookDir,
    Both,
    None
}
public class NekromancerArrow : MonoBehaviour
{
    [SerializeField] protected bool active = true;
    [SerializeField] protected Nekromancer nekromancer = null;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected VizualizationType vizualizationType = VizualizationType.None;

    private void Update()
    {
        if (!active) return;

        Vector2 moveDir = nekromancer.InputController.InputData.MoveDir;
        Vector2 lookDir = nekromancer.InputController.InputData.LookDir;

        switch (vizualizationType)
        {
            case VizualizationType.MoveDir:
                transform.right = moveDir;
                break;

            case VizualizationType.LookDir:
                transform.right = lookDir;
                break;

            case VizualizationType.Both:
                if (lookDir != Vector2.zero)
                    transform.right = lookDir;
                else if (moveDir != Vector2.zero)
                    transform.right = moveDir;
                break;

            case VizualizationType.None:
                transform.localScale = Vector3.zero;
                break;
        }
    }
}
