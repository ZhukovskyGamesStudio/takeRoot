using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldObjects;

public class TacticalDamagable : ECSComponent {
    [field: SerializeField]
    public int Health { get; private set; }

    [SerializeField]
    protected List<ResourceData> _dropOnDestroyed;

    private float _blinkTime;

    private Animatable _animatable;
    private Gridable _gridable;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Material flashMaterial;
    private Coroutine flashRoutine;

    [SerializeField]
    private float _flashDuration = 0.6f;

    [SerializeField]
    private AnimationCurve
        flashCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0)); // Кривая для плавности

    private TacticalInteractable _interactable;
    public Action OnDiedAction;

    public override int GetDependancyPriority() {
        return 2;
    }

    public override void Init(ECSEntity entity) {
        if (entity is ITacticalInteractable) {
            entity.GetEcsComponent<TacticalInteractable>().AddToPossibleCommands(TacticalCommand.TacticalAttack);
            _interactable = entity.GetEcsComponent<TacticalInteractable>();
        }

        flashMaterial = spriteRenderer.material;
        _animatable = entity.GetEcsComponent<Animatable>();
        _gridable = entity.GetEcsComponent<Gridable>();
    }

    private void Update() {
        if (_blinkTime > 0) {
            _blinkTime -= Time.deltaTime;
            if (_blinkTime <= 0) {
                // Вернуть в обычное состояние
                flashMaterial.SetColor("_Color", Color.white);
            }
        }
    }

    public void OnAttacked(int damageAmount) {
        //_interactable.CancelCommand();
        Health -= damageAmount;
        TriggerFlash();
        if (Health > 0) {
            _animatable?.TriggerDamaged();
        } else {
            OnDied();
            _interactable.CancelCommand();
            _animatable?.TriggerDied();
        }
    }

    private void OnDied() {
        Vector2Int pos = _gridable.GetBottomLeftOnGrid;
        ResourceManager.SpawnResourcesAround(_dropOnDestroyed, pos);
        _interactable.OnDestroyed();
        OnDiedAction?.Invoke();
        if (this != null) {
            Destroy(gameObject);
        }
    }

    public void TriggerFlash() {
        if (flashRoutine != null) {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect() {
        float elapsedTime = 0f;

        while (elapsedTime < _flashDuration) {
            // Используем кривую для плавного изменения _FlashAmount
            float flashStrength = flashCurve.Evaluate(elapsedTime / _flashDuration);
            flashMaterial.SetFloat("_FlashAmount", flashStrength);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Гарантируем, что в конце эффект полностью выключится
        flashMaterial.SetFloat("_FlashAmount", 0f);
        flashRoutine = null;
    }
}