using UnityEngine;

namespace FishJam.UI
{
    /// <summary>Ca nho boi ngang phia xa (bong mo), het man thi quay lai tu mep kia o do cao ngau nhien.
    /// Dung duoc voi SpriteRenderer (don vi the gioi) hoac Image trong Canvas (don vi pixel): speed, xRange, yRange, bobAmount theo don vi cua cha.</summary>
    public class BGSwimmer : MonoBehaviour
    {
        public float speed = .35f;                       // don vi / giay (am = boi sang trai)
        public Vector2 xRange = new Vector2(-3.6f, 3.6f);
        public Vector2 yRange = new Vector2(.2f, 1.4f);
        public float bobAmount = .05f, bobSpeed = 2.2f;
        [Tooltip("Sprite goc quay mat sang trai")]
        public bool spriteFacesLeft = true;

        SpriteRenderer sr; float y, seed, baseScaleX;

        void Awake() { sr = GetComponent<SpriteRenderer>(); baseScaleX = Mathf.Abs(transform.localScale.x); seed = Random.value * 10f; y = transform.localPosition.y; Face(); }

        void Face()
        {
            bool flip = spriteFacesLeft ? speed > 0 : speed < 0;
            if (sr) { sr.flipX = flip; return; }
            var s = transform.localScale; s.x = baseScaleX * (flip ? -1f : 1f); transform.localScale = s;   // Image khong co flipX
        }

        void Update()
        {
            var p = transform.localPosition;
            p.x += speed * Time.deltaTime;
            if (speed > 0 && p.x > xRange.y) { p.x = xRange.x; y = Random.Range(yRange.x, yRange.y); }
            if (speed < 0 && p.x < xRange.x) { p.x = xRange.y; y = Random.Range(yRange.x, yRange.y); }
            p.y = y + Mathf.Sin((Time.time + seed) * bobSpeed) * bobAmount;
            transform.localPosition = p;
        }
    }
}
