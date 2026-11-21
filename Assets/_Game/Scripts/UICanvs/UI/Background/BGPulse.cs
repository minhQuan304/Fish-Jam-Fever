using UnityEngine;
using UnityEngine.UI;

namespace FishJam.UI
{
    /// <summary>Sang toi dan (tia nang, quang sang) cho SpriteRenderer hoac Image (Canvas), tuy chon xoay va troi ngang.
    /// driftX tinh theo don vi cua cha (the gioi voi sprite, pixel voi Canvas).</summary>
    public class BGPulse : MonoBehaviour
    {
        public float minAlpha = .15f, maxAlpha = .5f;
        public float period = 8f, phase;
        public float rotateAmount = 4f;   // do
        public float driftX = .15f;       // don vi the gioi

        SpriteRenderer sr; Graphic g; Quaternion baseRot; Vector3 basePos;

        void Awake() { sr = GetComponent<SpriteRenderer>(); g = GetComponent<Graphic>(); baseRot = transform.localRotation; basePos = transform.localPosition; }

        void Update()
        {
            float s = Mathf.Sin((Time.time + phase) * Mathf.PI * 2f / period) * .5f + .5f;
            float a = Mathf.Lerp(minAlpha, maxAlpha, s);
            if (sr) { var c = sr.color; c.a = a; sr.color = c; }
            if (g) { var c = g.color; c.a = a; g.color = c; }
            transform.localRotation = baseRot * Quaternion.Euler(0, 0, (s - .5f) * rotateAmount);
            transform.localPosition = basePos + new Vector3((s - .5f) * 2f * driftX, 0, 0);
        }
    }
}
