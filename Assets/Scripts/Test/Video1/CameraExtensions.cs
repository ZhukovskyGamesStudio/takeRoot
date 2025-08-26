using System.Collections;
using Unity.Cinemachine;

public static class CameraExtensions {
    public static IEnumerator UnzoomCamera(this CinemachineCamera camera, float to, float speed) {
        while (camera.Lens.OrthographicSize < to) {
            camera.Lens.OrthographicSize += speed;
            yield return null;
        }
    }

    public static IEnumerator ZoomCamera(this CinemachineCamera camera, float to, float speed) {
        while (camera.Lens.OrthographicSize > to) {
            camera.Lens.OrthographicSize -= speed;
            yield return null;
        }
    }
}