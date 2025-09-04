using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomDecorObject : MonoBehaviour {
    [Serializable]
    public class PerlinGroup {
        public float key;

        [Range(0f, 1f)]
        public float density = 1f;

        public Sprite[] sprites;
    }

    [SerializeField]
    private List<PerlinGroup> _groups;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public void Init(float seed) {
        int combinedSeed = Mathf.FloorToInt(seed * 10000) + transform.GetInstanceID();
        System.Random rng = new(combinedSeed);
        

        var pos = transform.position;
        float p = Mathf.PerlinNoise((pos.x + seed) / 10f, (pos.y + seed) / 10f);

        var g = _groups.OrderBy(x => Mathf.Abs(x.key - p)).FirstOrDefault();
        if (g.sprites == null || g.sprites.Length == 0 || rng.NextDouble() > g.density)
        {
            Destroy(gameObject);
            return;
        }

        _spriteRenderer.sprite = g.sprites[rng.Next(g.sprites.Length)];
    }
}