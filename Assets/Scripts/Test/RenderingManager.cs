using UnityEngine;
using UnityEngine.Rendering;

public class RenderingManager : MonoBehaviour
{
    void Awake() {
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0, 1, 0); // Изометрия: сверху вниз
    }
}
