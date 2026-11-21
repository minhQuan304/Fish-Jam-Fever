using UnityEngine;

namespace FishJam.UI
{
    /// <summary>Lac qua lai quanh pivot (dat pivot sprite o goc cay). Dung cho san ho, rong tren nen gameplay.</summary>
    public class BGSway : MonoBehaviour
    {
        public float angle = 2f;          // bien do (do)
        public float period = 4f;         // giay / chu ky
        public float phase;               // lech pha (giay)
        [Tooltip("Gia lap uon: gian ngang theo nhip lac")]
        public float bend = 0.015f;

        Quaternion baseRot; Vector3 baseScale;

        void Awake() { baseRot = transform.localRotation; baseScale = transform.localScale; }

        void Update()
        {
            float s = Mathf.Sin((Time.time + phase) * Mathf.PI * 2f / period);
            transform.localRotation = baseRot * Quaternion.Euler(0, 0, s * angle);
            if (bend != 0f) transform.localScale = new Vector3(baseScale.x * (1f + s * bend), baseScale.y * (1f - Mathf.Abs(s) * bend * .5f), baseScale.z);
        }
    }
}
