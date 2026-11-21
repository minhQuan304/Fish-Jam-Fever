using UnityEngine;

namespace FishJam.UI
{
    /// <summary>Troi cham qua lai (suong, may nuoc): dich ngang/doc theo hinh sin, tuy chon sang toi.</summary>
    public class BGDrift : MonoBehaviour
    {
        public Vector2 amount = new Vector2(.6f, .08f);   // don vi the gioi
        public float period = 24f, phase;
        public float minAlpha = 1f, maxAlpha = 1f;

        Vector3 basePos; SpriteRenderer sr;

        void Awake() { basePos = transform.localPosition; sr = GetComponent<SpriteRenderer>(); }

        void Update()
        {
            float k = (Time.time + phase) * Mathf.PI * 2f / period;
            transform.localPosition = basePos + new Vector3(Mathf.Sin(k) * amount.x, Mathf.Sin(k * 1.7f) * amount.y, 0);
            if (sr && minAlpha != maxAlpha) { var c = sr.color; c.a = Mathf.Lerp(minAlpha, maxAlpha, Mathf.Sin(k * 1.3f) * .5f + .5f); sr.color = c; }
        }
    }
}
