using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using Cinemachine;

public class Nekromancer : MonoBehaviour, IDamagable
{
    public Action<IInteractable> OnInteract;
    public Action<Vector2> OnMove;

    #region References
    [SerializeField] public Rigidbody2D rb = null;
    [SerializeField] public Collider2D col = null;
    [SerializeField] public Animator skinAnimator = null;
    [field: SerializeField] public Transform Model { get; private set; } = null;
    [SerializeField] public List<Transform> gunPoints = null;
    [SerializeField] public Transform interactPoint = null;
    #endregion

    #region Data
    [SerializeField] private NekromancerData stats = null;
    [SerializeField] private SkillData baseSkillData = null;
    [SerializeField] private SkillData firstSkillData = null;
    [SerializeField] private SkillData secondSkillData = null;
    #endregion

    #region Stats
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int CurrentMana { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float LookSpeed { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    #endregion

    #region Private Variables
    private PlayerController playerController = null;
    private InputData currentInput = null;
    private Skill baseSkill = null;
    private Skill baseChargeSkill = null;
    private Skill firstSkill = null;
    private Skill secondSkill = null;
    private Skill currentSkill = null;
    [SerializeField] private List<Cooldown> cooldowns = new List<Cooldown>();
    private bool blocked = false;
    private float blockInteractionTime = 0f;
    #endregion

    #region Properties
    private LevelManager levelManager { get; set; }
    public LevelManager LevelManager
    {
        get
        {
            if (levelManager == null)
            {
                levelManager = LevelManager.Instance;
                if (levelManager == null)
                {
                    Debug.LogError("LevelManager is null");
                    return null;
                }
            }

            return levelManager;
        }
    }
    public PlayerController PlayerController { get { return playerController; } }
    public InputController InputController { get; set; }
    public InputData CurrentInput { get => currentInput; }

    public Skill BaseSkill { get => baseSkill; }
    public SkillData BaseSkillData { get => baseSkillData; }

    public Skill FirstSkill { get => firstSkill; }
    public SkillData FirstSkillData { get => firstSkillData; }
    public bool CanUseFirstSkill { get => firstSkillData.CanBeUsed(this); }

    public Skill SecondSkill { get => secondSkill; }
    public SkillData SecondSkillData { get => secondSkillData; }
    public bool CanUseSecondSkill { get => secondSkillData.CanBeUsed(this); }

    public Skill CurrentSkill { get => currentSkill; set => currentSkill = value; }
    public List<Cooldown> Cooldowns { get { return cooldowns; } }

    public IInteractable CanInteractWith { get; private set; } = null;
    public IInteractable CurrentInteractable { get; private set; } = null;
    #endregion

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;

        Health = stats.health;
        MaxHealth = stats.health;
        CurrentMana = stats.mana;
        Damage = stats.damage;
        Speed = stats.moveSpeed;
        LookSpeed = stats.lookSpeed;
        AttackSpeed = stats.attackSpeed;
        AttackRange = stats.attackRange;

        baseSkill = baseSkillData.GetSkillInstance();

        firstSkillData = GameManager.Instance.DataList.GetSkill(playerController.PlayerData.equippedSkills[0]);
        secondSkillData = GameManager.Instance.DataList.GetSkill(playerController.PlayerData.equippedSkills[1]);
        firstSkill = firstSkillData.GetSkillInstance();
        secondSkill = secondSkillData.GetSkillInstance();

        ChangeSkill(baseSkill, baseSkillData);
    }

    private void Update()
    {
        UpdateCooldowns();

        if (!InputController || blocked) return;

        currentInput = InputController.InputData;

        if (LevelManager.CurrentCycleState != null && LevelManager.CurrentCycleState.Cycle == Cycle.Day)
            CheckInteraction();

        currentSkill?.FrameUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!InputController || currentInput == null || blocked) return;

        currentSkill?.PhysicsUpdate(Time.deltaTime);
    }

    public void TakeDamage(int _damage, GameObject _sender)
    {
        if (_sender == gameObject) return;

        Health -= _damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Nekromancer died");

        LevelManager.Instance.NekromancerDie();

        PlayerController.OpenGameLostMenu();
    }

    #region State Machine
    public void ChangeSkill(Skill _skill = null, SkillData _skillData = null)
    {
        if (currentSkill != null)
        {
            currentSkill.Exit();
            currentSkill = null;
        }

        if (_skill == null)
        {
            _skill = baseSkill;
            _skillData = baseSkillData;
        }

        currentSkill = _skill;
        currentSkill.Enter(this, _skillData);
    }
    #endregion

    #region Update Stats
    public void AddMaxHealth(int _amount) { stats.health += _amount; }
    public void AddMaxMana(int _amount) { stats.mana += _amount; }
    public void AddDamage(float _damage) { Damage += _damage; }
    public void AddSpeed(float _speed) { Speed += _speed; }
    public void AddLookSpeed(float _lookSpeed) { LookSpeed += _lookSpeed; }
    public void AddAttackSpeed(float _attackSpeed) { AttackSpeed += _attackSpeed; }
    public void AddAttackRange(float _attackRange) { AttackRange += _attackRange; }
    public void BlockNekromancer(bool _block) { blocked = _block; }
    #endregion

    #region Check Methods
    public bool WantsToUseSkills()
    {
        if (CurrentInput.FirstSkill && CanUseFirstSkill)
        {
            ChangeSkill(FirstSkill, FirstSkillData);
            return true;
        }

        if (CurrentInput.SecondSkill && CanUseSecondSkill)
        {
            ChangeSkill(SecondSkill, SecondSkillData);
            return true;
        }

        return false;
    }

    private void CheckInteraction()
    {
        if (HasCooldown("Interact"))
        {
            Debug.Log("Has Interact cooldown");
            return;
        }

        if (LevelManager && LevelManager.CurrentCycleState != null && LevelManager.CurrentCycleState.Cycle == Cycle.Day)
            UpdateCanInteractWith();

        if (currentInput.Interact && CanInteractWith != null)
            InteractWithSelection();
    }

    private void UpdateCanInteractWith()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(interactPoint.position, stats.interactRadius);
        foreach (Collider2D col in collider)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
                CanInteractWith = interactable;
        }
    }

    private void InteractWithSelection()
    {
        if (CanInteractWith != null && CanInteractWith != CurrentInteractable)
        {
            CanInteractWith.Interact(this);
            CurrentInteractable = CanInteractWith;
            OnInteract?.Invoke(CurrentInteractable);
            InputController.ResetInteract();
            return;
        }
    }

    public void ResetInteractable()
    {
        CanInteractWith = null;
        CurrentInteractable = null;
    }

    private void CheckItems()
    {
    }
    #endregion

    #region Movement
    public void Move()
    {
        Vector2 newVelocity = currentInput.MoveDir * stats.moveSpeed * LevelManager.TimeScale;
        newVelocity = Vector2.Lerp(rb.velocity, newVelocity, stats.accleleration * Time.deltaTime);
        rb.velocity = newVelocity;

        if (rb.velocity.magnitude > stats.velocityActionThreshold)
            OnMove?.Invoke(rb.velocity);

        if (skinAnimator)
        {
            float speed = currentInput.MoveDir.magnitude;
            speed = rb.velocity.x > 0f ? speed : rb.velocity.x < 0f ? -speed : 0f;
            skinAnimator.SetFloat("Speed", speed);
        }
    }

    public void Look()
    {
        Vector2 lookDirection = GetLookDirection();

        if (lookDirection.magnitude < stats.lookThreshold) return;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotation = Quaternion.Slerp(Model.rotation, rotation, stats.lookSpeed * Time.deltaTime);

        Model.rotation = rotation;
    }

    public Vector2 GetLookDirection()
    {
        Vector2 lookDirection;

        if (currentInput.ControllSheme == "Keyboard")
        {
            lookDirection = currentInput.CursorWorldPosition - Model.position;
        }
        else
        {
            if (currentInput.LookDir.magnitude > stats.lookThreshold)
                lookDirection = currentInput.LookDir.normalized;
            else if (currentInput.MoveDir.magnitude > stats.lookThreshold)
                lookDirection = currentInput.MoveDir.normalized;
            else
                lookDirection = Model.right;
        }

        return lookDirection;
    }
    #endregion

    #region Cooldowns
    public void AddCooldown(Cooldown _cooldown)
    {
        cooldowns.Add(_cooldown);
    }

    public void RemoveCooldown(Cooldown _cooldown)
    {
        cooldowns.Remove(_cooldown);
    }

    public void UpdateCooldowns()
    {
        if (cooldowns.Count <= 0) return;

        cooldowns.ForEach(cooldown => cooldown.Update(Time.deltaTime));

        cooldowns.RemoveAll(cooldown => cooldown.Finished);
    }

    public bool HasCooldown(string _name)
    {
        return cooldowns.Exists(cooldown => cooldown.name == _name);
    }

    public int CountCooldowns(string _name)
    {
        return cooldowns.FindAll(cooldown => cooldown.name == _name).Count;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactPoint.position, stats.interactRadius);
    }
    #endregion
}
