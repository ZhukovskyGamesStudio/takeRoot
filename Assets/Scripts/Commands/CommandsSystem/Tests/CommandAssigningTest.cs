using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;

public class CommandAssigningTest : MonoBehaviour, IUpdateService {
    public Transform movePos;

    private IWorkerAssigner workerAssigner;
    private CommandService _commandService;

    public void Start() {
        _commandService = new CommandService();
        workerAssigner = new WorkerAssigner(this, _commandService);
        ServiceLocator.Container.RegisterSingle<IPathfindService>(new MockPathfindService());

        GameObject obj = new();
        SpriteRenderer sprite = obj.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSquareSprite();
        obj.AddComponent<Mover>();
        Worker worker = obj.AddComponent<Worker>();

        workerAssigner.Workers.Add(worker);

        DestroyCommand command = new(1, null, _commandService, this);
    }

    private Sprite CreateSquareSprite() {
        // Создаем простую текстуру 16x16 пикселей
        Texture2D texture = new(16, 16);
        Color[] pixels = new Color[16 * 16];

        // Заполняем белым цветом
        for (int i = 0; i < pixels.Length; i++) {
            pixels[i] = Color.white;
        }

        texture.SetPixels(pixels);
        texture.Apply();

        // Создаем спрайт из текстуры
        return Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16f);
    }

    private readonly List<IUpdatable> _updatables = new();

    public void Register(IUpdatable updatable) {
        if (!_updatables.Contains(updatable)) {
            _updatables.Add(updatable);
        }
    }

    public void Unregister(IUpdatable updatable) {
        _updatables.Remove(updatable);
    }

    public void Update() {
        foreach (IUpdatable updatable in _updatables.ToArray()) {
            updatable.Update();
        }
    }
}