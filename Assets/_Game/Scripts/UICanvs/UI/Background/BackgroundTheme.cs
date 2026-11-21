using UnityEngine;

namespace FishJam.UI
{
    /// <summary>
    /// Goc cua mot nen gameplay dong (Resources/backgroundUI/BG_*.prefab): nen da xoa cay + cac lop song dong
    /// (cay lac, anh sang to ong, tia nang, ca xa, bong bong). Dat lam con cua GameplayWorld/Background.
    /// </summary>
    public class BackgroundTheme : MonoBehaviour
    {
        [Tooltip("Ten anh nen goc (Map_x) – de doi chieu")]
        public string mapName;
        [Tooltip("Mau chu dao cua anh sang tren nen nay")]
        public Color lightTint = new Color(1f, .96f, .82f);
    }
}
