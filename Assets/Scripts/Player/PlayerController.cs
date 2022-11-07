using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] public Nekromancer nekromancer = null;
    [SerializeField] private Nekromancer nekromancerPrefab = null;
    [SerializeField] private Crystal crystal = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private CursorController cursor = null;

    public InputController InputController { get; private set; }
    public Crystal Crystal { get => crystal; }

    public void Init(InputController _inputController)
    {
        InputController = _inputController;

        if (!nekromancer)
            nekromancer = Instantiate(nekromancerPrefab, spawnPoint.position, Quaternion.identity);

        nekromancer.Init(this);
        cursor.Init(this);

        crystal.OnCrystalDestroyed += CrystalDestroyed;
    }

    public void CrystalDestroyed()
    {
        print("Crystal destroyed");
    }
}
