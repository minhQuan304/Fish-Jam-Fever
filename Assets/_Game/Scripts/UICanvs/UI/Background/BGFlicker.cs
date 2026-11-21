using UnityEngine;
using UnityEngine.UI;

namespace FishJam.UI
{
    /// <summary>Den / pha le phat sang: sang toi khong deu (nhieu Perlin) va phong to nhe.</summary>
    public class BGFlicker : MonoBehaviour
    {
        public float minAlpha = .35f, maxAlpha = .9f;
        public float speed = 1.5f;
        public float scaleAmount = .08f;

        SpriteRenderer sr; Graphic g; Vector3 baseScale; float seed;

        void Awake() { sr = GetComponent<SpriteRenderer>(); g = GetComponent<Graphic>(); baseScale = transform.localScale; seed = Random.value * 100f; }

        void Update()
        {
            float n = Mathf.PerlinNoise(seed, Time.time * speed);
            float a = Mathf.Lerp(minAlpha, maxAlpha, n);
            if (sr) { var c = sr.color; c.a = a; sr.color = c; }
            if (g) { var c = g.color; c.a = a; g.color = c; }
            transform.localScale = baseScale * (1f + (n - .5f) * 2f * scaleAmount);
        }
    }
}
