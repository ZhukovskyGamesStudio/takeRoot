using UnityEngine;
using CodeBase.Services;

public class SimpleCommandTest : MonoBehaviour {
	private Worker _testWorker;
	private Worker _testWorker2;
	private CommandTarget _testTarget;
	
	private void Start() {
		CreateTestWorker();
		CreateTestTarget();
	}
	
	private void CreateTestWorker() {
		// Создаем Worker
		var workerGO = new GameObject("TestWorker");
		workerGO.transform.position = new Vector3(0, 0, 0);
		
		_testWorker = workerGO.AddComponent<Worker>();
		
		// Добавляем компоненты для движения и разрушения
		var mover = workerGO.AddComponent<Mover>();
		var destroyer = workerGO.AddComponent<Destroyer>();
		
		// Настраиваем параметры
		mover.moveSpeed = 3f;
		destroyer.damage = 25f;
		destroyer.hitCooldown = 0.5f;
		
		// Добавляем визуал (синий квадрат)
		var spriteRenderer = workerGO.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = CreateSquareSprite();
		spriteRenderer.color = Color.blue;
		
		// Добавляем коллайдер для взаимодействия
		var collider = workerGO.AddComponent<BoxCollider2D>();
		collider.size = new Vector2(1f, 1f);
		
		// Добавляем Selectable компонент
		var selectable = workerGO.AddComponent<Selectable>();
		
		var workerGO2 = new GameObject("TestWorker2");
		workerGO2.transform.position = new Vector3(-5, 0, 0);
		
		_testWorker2 = workerGO2.AddComponent<Worker>();
		
		// Добавляем компоненты для движения и разрушения
		var mover2 = workerGO2.AddComponent<Mover>();
		var destroyer2 = workerGO2.AddComponent<Destroyer>();
		
		// Настраиваем параметры
		mover2.moveSpeed = 3f;
		destroyer2.damage = 25f;
		destroyer2.hitCooldown = 0.5f;
		
		// Добавляем визуал (синий квадрат)
		var spriteRenderer2 = workerGO2.AddComponent<SpriteRenderer>();
		spriteRenderer2.sprite = CreateSquareSprite();
		spriteRenderer2.color = Color.blue;
		
		// Добавляем коллайдер для взаимодействия
		var collider2 = workerGO2.AddComponent<BoxCollider2D>();
		collider2.size = new Vector2(1f, 1f);
		
		// Добавляем Selectable компонент
		var selectable2 = workerGO2.AddComponent<Selectable>();
		
		Debug.Log("Test Worker created with Mover and Destroyer components");
	}
	
	private void CreateTestTarget() {
		// Создаем Target подальше от Worker
		var targetGO = new GameObject("TestTarget");
		targetGO.transform.position = new Vector3(8, 0, 0);
		
		_testTarget = targetGO.AddComponent<CommandTarget>();
		
		// Добавляем Health и ResourceDrop
		//var health = targetGO.AddComponent<Health>();
		var resourceDrop = targetGO.AddComponent<ResourceDrop>();
		
		// Настраиваем параметры
		//health.maxHealth = 100f;
		//health.currentHealth = 100f;
		resourceDrop.resourceType = "Wood";
		resourceDrop.dropAmount = 3;
		
		// Добавляем визуал (красный квадрат)
		var spriteRenderer = targetGO.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = CreateSquareSprite();
		spriteRenderer.color = Color.red;
		
		// Добавляем коллайдер для взаимодействия
		var collider = targetGO.AddComponent<BoxCollider2D>();
		collider.size = new Vector2(1f, 1f);
		
		Debug.Log("Test Target created with Health and ResourceDrop components");
	}
	
	private Sprite CreateSquareSprite() {
		// Создаем простую текстуру 16x16 пикселей
		Texture2D texture = new Texture2D(16, 16);
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
	
	private void OnGUI() {
		// Отображаем информацию о тесте
		GUI.Label(new Rect(10, 10, 400, 120), 
			"Simple Command System Test\n" +
			"Worker: " + (_testWorker != null ? "Created at (0,0)" : "Missing") + "\n" +
			"Target: " + (_testTarget != null ? "Created at (8,0)" : "Missing") + "\n" +
			"Worker Components: " + (_testWorker?.Mover != null ? "Mover ✓" : "Mover ✗") + 
			", " + (_testWorker?.Destroyer != null ? "Destroyer ✓" : "Destroyer ✗") + "\n" +
			"Target Components: " + (_testTarget?.Health != null ? "Health ✓" : "Health ✗") + 
			", ResourceDrop ✓\n" +
			"Worker registered in WorkerService: " + 
			(ServiceLocator.Container.Single<IWorkerService>()?.GetAllWorkers().Contains(_testWorker) ?? false)
		);
	}
} 