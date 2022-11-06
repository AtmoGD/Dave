using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Nekromancer/Attack")]
public class Attack : ScriptableObject
{
    public new string name = "Attack";
    public GameObject prefab = null;
    public bool spawnAsChild = false;
    public int damage = 10;
    public Cooldown cooldown = new Cooldown("Attack", 1f);

    private void Start()
    {
        cooldown.name = name;
    }

    public bool CanBeUsed(Nekromancer _nekromancer)
    {
        return !_nekromancer.HasCooldown(name);
    }

    public void Use(Nekromancer _nekromancer)
    {
        if (!prefab) return;

        GameObject attack;

        if (spawnAsChild)
        {
            attack = Instantiate(prefab, _nekromancer.transform);
        }
        else
        {
            attack = Instantiate(prefab, _nekromancer.transform.position, _nekromancer.transform.rotation);
            attack.transform.right = _nekromancer.transform.right;
        }

        cooldown.name = name;
        _nekromancer.AddCooldown(cooldown.GetCopy(name));
    }
}
