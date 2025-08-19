using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using WorldObjects;
using Random = UnityEngine.Random;

public class Zombie : ECSEntity {
    public const float CellSize = 1f;
    private float _changeAttackTargetCooldown;
    private Vector2Int? _currentAttackTarget;

    private HashSet<Vector2Int> _currentMovementTarget;

    private bool _isMoving;

    private float _movingCooldown;

    private object _performingCoroutine;

    private SpriteRenderer _spriteRenderer;
    
    public Gridable Gridable;
    public ZombieData ZombieData { get; private set; }
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    protected override void Awake() {
        base.Awake();
        ZombieData = GetEcsComponent<ZombieData>();
        Gridable = GetEcsComponent<Gridable>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        GetEcsComponent<Listener>().HasHeard += HasHeard;
        GetEcsComponent<Damagable>().OnDiedAction += OnDied;
    }

    private void Update() {
        _movingCooldown -= Time.deltaTime;
        _changeAttackTargetCooldown -= Time.deltaTime;
        if (_performingCoroutine != null)
            return;
        switch (ZombieData.State) {
            case EnemyState.Passive:
                CheckForSettlerNear();
                if (_currentAttackTarget != null)
                    break;
                PassiveMove();
                break;
            case EnemyState.Rage:
                CheckForSettlerNear();
                if (_currentAttackTarget == null)
                    PassiveMove();
                else
                    ForceMove();
                break;
            case EnemyState.Idle:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangeState(EnemyState state) {
        ZombieData.State = state;
    }

    private void AddRagePoints(int value) {
        ZombieData.CurrentRagePoints += value;
        ZombieData.CurrentRagePoints = Mathf.Clamp(ZombieData.CurrentRagePoints, 0, ZombieData.PointsToRageState);

        if (ZombieData.CurrentRagePoints == ZombieData.PointsToRageState) {
            ChangeState(EnemyState.Rage);
        }
    }

    private void SetRagePoints(int value) {
        ZombieData.CurrentRagePoints = Mathf.Clamp(value, 0, ZombieData.PointsToRageState);
        if (ZombieData.CurrentRagePoints == ZombieData.PointsToRageState) {
            ChangeState(EnemyState.Rage);
        }
    }

    private void AddAttackTarget(Vector2Int targetCell) {
        _currentAttackTarget = targetCell;
        _changeAttackTargetCooldown = Core.ConfigManager.ZombieConfig.ChangeTargetCooldown;
    }

    private void CheckForSettlerNear() {
        if (_changeAttackTargetCooldown > 0)
            return;
        foreach (Settler settler in Core.SettlersManager.Settlers) {
            if (Gridable.InteractableCells.Contains(settler.GetCellOnGrid) && settler.SettlerData._mood != Mood.Neutral) {
                SetRagePoints(ZombieData.PointsToRageState);
                AddAttackTarget(settler.GetCellOnGrid);
            }
        }
    }

    private void PassiveMove() {
        if (_isMoving)
            return;
        if (_movingCooldown > 0)
            return;

        _currentMovementTarget ??= FindTarget();
        if (_currentMovementTarget == null)
            return;

        Vector2Int? step = TryPassiveMoveToCell();
        if (step == null) return;

        _performingCoroutine = StartCoroutine(MoveToCell(step.Value));
    }

    private HashSet<Vector2Int> FindTarget() {
        var currentPosition = ZombieData.GetCellOnGrid;

        var newPosY = Random.Range(-5 + currentPosition.y, 5 + currentPosition.y);
        var newPosX = Random.Range(-5 + currentPosition.x, 5 + currentPosition.x);
        var target = new HashSet<Vector2Int> { new(newPosX, newPosY) };

        var path = Core.AStarPathfinding.FindPathForZombies(currentPosition, target, 1);
        if (path != null) {
            return target;
        }

        return null;
    }

    private Vector2Int? TryPassiveMoveToCell() {
        Vector2Int target;
        var pathStep = ExactInteractionChecker.NextStepOnPathForZombies(ZombieData.GetCellOnGrid, _currentMovementTarget, 1);
        if (pathStep != null) {
            target = pathStep.Value;
        } else {
            _currentMovementTarget = null;
            return null;
        }

        if (_currentMovementTarget.Contains(target)) {
            _currentMovementTarget = null;
            _movingCooldown = Core.ConfigManager.ZombieConfig.MoveCooldown;
            return null;
        }

        return target;
    }

    private void ForceMove() {
        if (CanAttackTarget()) {
            _performingCoroutine = StartCoroutine(StartAttack());
            return;
        }

        Vector2Int? step = TryForceMoveToCell();
        if (step == null) return;
        if (!AStarPathfinding.IsWalkable(step.Value) || !AStarPathfinding.IsWalkable(new Vector2Int(step.Value.x + 1, step.Value.y))) {
            var stepX = step.Value.x;
            var stepY = step.Value.y;
            var damagable = FindObjectsByType<Destructable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(g =>
                g.GetComponent<Gridable>().GetOccupiedPositions().Contains(step.Value) ||
                g.GetComponent<Gridable>().GetOccupiedPositions().Contains(new Vector2Int(stepX + 1, stepY)));
            if (damagable != null) {
                _performingCoroutine = StartCoroutine(TryDestroy(damagable));
                return;
            }
        }

        _performingCoroutine = StartCoroutine(MoveToCell(step.Value));
    }

    private bool CanAttackTarget() {
        return _currentAttackTarget != null && Gridable.InteractableCells.Contains(_currentAttackTarget.Value);
    }

    private IEnumerator StartAttack() {
        yield return new WaitForSeconds(Core.ConfigManager.ZombieConfig.AttackTime);
        var settler = Core.SettlersManager.GetSettlerAt(_currentAttackTarget.Value);
        if (settler != null) {
            settler.GetEcsComponent<Damagable>().OnAttacked(1);
        }

        _currentAttackTarget = null;
        _performingCoroutine = null;
    }

    private Vector2Int? TryForceMoveToCell() {
        Vector2Int target;
        var pathStep = ExactInteractionChecker.NextStepForZombieOnPathWithWallsAsObstacle(ZombieData.GetCellOnGrid,
            new HashSet<Vector2Int> { _currentAttackTarget.Value }, 1);
        if (pathStep != null) {
            target = pathStep.Value;
        } else {
            return null;
        }

        return target;
    }

    private IEnumerator TryDestroy(Destructable target) {
        yield return new WaitForSeconds(Core.ConfigManager.ZombieConfig.DestroyTime);
        if (target != null)
            target.OnAttacked(Core.ConfigManager.ZombieConfig.DestroyDamage);
        _performingCoroutine = null;
    }

    private IEnumerator MoveToCell(Vector2Int target) {
        Vector3 target3 = new Vector3(target.x, target.y);
        Vector3 diff = target3 - transform.position;
        if (diff.x > 0) {
            _spriteRenderer.flipX = true;
        }

        if (diff.x < 0) {
            _spriteRenderer.flipX = false;
        }

        yield return new WaitForSeconds(Core.ConfigManager.ZombieConfig.MovePause);
        yield return StartCoroutine(LerpFromTo(transform.position, target3 * CellSize, Core.ConfigManager.ZombieConfig.MoveTime));
        Gridable.PositionChanged();
        _performingCoroutine = null;
    }

    private IEnumerator LerpFromTo(Vector3 from, Vector3 to, float time) {
        float elapsedTime = 0f;
        _isMoving = true;

        while (elapsedTime < time) {
            float t = elapsedTime / time;
            transform.position = Vector3.Lerp(from, to, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = to;
        _isMoving = false;
    }

    private void HasHeard(Vector2Int noisePosition) {
        AddRagePoints(10);
        if (_changeAttackTargetCooldown > 0)
            return;
        if (ZombieData.State == EnemyState.Rage)
            AddAttackTarget(noisePosition);
    }

    private void OnDied()
    {
        
    }
}

public enum EnemyState {
    Idle,
    Passive,
    Rage
}