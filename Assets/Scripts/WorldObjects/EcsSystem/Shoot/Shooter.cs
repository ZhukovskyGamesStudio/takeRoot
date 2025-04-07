using UnityEngine;
using WorldObjects;

public abstract class Shooter : ECSComponent
{
    private TacticalInteractable _tacticalInteractable;
    
    [SerializeField]private Animator _animator;
    [SerializeField]private Transform _projectileSpawnPoint;
    
    [Header("Shooter Settings")]
    public float detectionRange = 15f;
    
    [Header("Projectile Settings")]
    [SerializeField] protected ProjectileSelectorType projectileSelectorType;
    private IProjectileSelector _projectileSelector;
    
    public Zombie currentTarget;
    private float _ShootTimer;

    protected bool CanShoot = false;
    
    public override void Init(ECSEntity entity)
    {
        _tacticalInteractable = entity.GetEcsComponent<TacticalInteractable>();
        _tacticalInteractable.AddToPossibleCommands(TacticalCommand.AddShootTarget);
        _tacticalInteractable.OnCommandPerformed += OnCommandPerformed;
        InitializeProjectileSelector();
    }
    
    private void InitializeProjectileSelector()
    {
        switch (projectileSelectorType)
        {
            case ProjectileSelectorType.Random:
                _projectileSelector = GetComponent<RandomProjectileSelector>();
                if (_projectileSelector == null)
                {
                    Debug.LogError("RandomProjectileSelector component missing!", this);
                }
                break;
        }
    }
    
    private void Update()
    {
        UpdateTarget();
        HandleShooting();
    }

    protected virtual void UpdateTarget()
    {
        if (currentTarget != null) return;
        var zombies = FindObjectsByType<Zombie>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Vector2Int shooterPosition = VectorUtils.ToVector2Int(_tacticalInteractable.Gridable.GetCenterOnGrid);
        Zombie closestTarget = null;
        float closestDistance = detectionRange;
        foreach (Zombie zombie in zombies)
        {
            var distance = Vector2Int.Distance(shooterPosition, VectorUtils.ToVector2Int(zombie.Gridable.GetCenterOnGrid));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = zombie;
            }
        }
        if (closestTarget != null)
            currentTarget = closestTarget;
    }
    
    protected virtual void HandleShooting()
    {
        if (currentTarget == null) return;

        if (CanShoot)
        {
            _animator.SetBool("Shoot", true);
            
            
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") ||
                !(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)) return;

            _animator.SetBool("Shoot", false);
            FinishShoot(); 
        }
        else
        {
            _animator.SetBool("Reload", true);

            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Reload") ||
                (!(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))) return;
            
            _animator.SetBool("Reload", false);
            Reload();
        }
    }

    protected virtual void Reload()
    {
        CanShoot = true;
        Debug.Log("Reloaded");
    }

    protected virtual void Shoot()
    {
        GameObject projectileObj = CreateProjectile();
        if (projectileObj == null) return;
        
        ConfigureProjectile(projectileObj); 
    }

    protected virtual void FinishShoot()
    {
        CanShoot = false;
    }

    protected GameObject CreateProjectile(bool isActive = true)
    {
        GameObject projectilePrefab = _projectileSelector.SelectProjectile(this);
        if (projectilePrefab == null) return null;
        
        GameObject projectileObj = Instantiate(projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity);
        projectileObj.SetActive(isActive);
        
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Init(currentTarget);
        return projectileObj;
    }

    protected virtual void ConfigureProjectile(GameObject projectileObj)
    { }

    private void OnCommandPerformed(TacticalCommand obj)
    {
        if (obj == TacticalCommand.AddShootTarget)
        {
            if (_tacticalInteractable.CommandToExecute.TacticalInteractable.TryGetComponent(out Zombie zombie))
                currentTarget = zombie;
        }
    }
    
    public override int GetDependancyPriority()
    {
        return 2;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}