using UnityEngine;

namespace Settlers.Test {
    public class InitCommandTargets : MonoBehaviour {
        private void Start() {
            int id = 0;
            foreach (CommandTarget target in FindObjectsByType<CommandTarget>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
                target.Data.Id = ++id;
            }
        }
    }
}