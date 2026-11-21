using UnityEngine;

namespace FishSort
{
    // Root of everything that exists only while playing: background, tank shelf, waiting tray, bubble field and walls.
    // Lives in a prefab; LevelManager spawns it once and turns it on/off.
    public sealed class GameplayWorld : MonoBehaviour
    {
        // Sea backgrounds (Resources/data/images/sprites/Resources/background). One is picked at random for each level.
        //private const string BACKGROUND_FOLDER = "data/images/sprites/Resources/background/";


   
        [SerializeField] private BubbleField field;
        [SerializeField] private TankShelf shelf;
        [SerializeField] private WaitingTray tray;
        [SerializeField] private ComboCounter combo;
        [SerializeField] private FishingRod rod;
        [SerializeField] private CannonBooster cannon;
        [SerializeField] private FishJar jar;
        [SerializeField] private ScreenEdgeAnchor[] edgeAnchors;
        [SerializeField] private Transform backgroundRoot;

        public BubbleField Field => field;
        public TankShelf Shelf => shelf;
        public WaitingTray Tray => tray;
        public ComboCounter Combo => combo;
        public FishingRod Rod => rod;
        public CannonBooster Cannon => cannon;
        public FishJar Jar => jar;

        private int backgroundIndex = -1;

        // Nen dong (minhquan): prefab BackgroundTheme trong Resources/backgroundUI. Co prefab thi dung, khong thi dung anh tinh nhu cu.
      
        private const string THEME_FOLDER = "backgroundUI";
        private FishJam.UI.BackgroundTheme[] themes;
        private GameObject currentTheme;
        private int themeIndex = -1;


        // A new random background (never the same one twice in a row). It is loaded now, one at a time, and the
        // ones no longer used are released.
        public void RandomizeBackground()
        {
            // 1. Load danh sách background nếu chưa được load
            if (themes == null)
            {
                themes = Resources.LoadAll<FishJam.UI.BackgroundTheme>(THEME_FOLDER);
            }

            // 2. Kiểm tra danh sách background
            if (themes.Length == 0)
            {
                Debug.LogError("[GameplayWorld] Resources/backgroundUI chưa có nền");
                return;
            }

            // 3. Xác định phạm vi random
            int maxIndex = themes.Length;

            bool hasPreviousTheme = themeIndex >= 0;
            bool hasMultipleThemes = themes.Length > 1;

            if (hasPreviousTheme && hasMultipleThemes)
            {
                maxIndex = themes.Length - 1;
            }

            // 4. Random background mới
            int pick = Random.Range(0, maxIndex);

            // 5. Bỏ qua background đã chọn ở lần trước
            if (hasPreviousTheme && hasMultipleThemes)
            {
                if (pick >= themeIndex)
                {
                    pick++;
                }
            }

            // 6. Lưu lại index background mới
            themeIndex = pick;

            // 7. Xóa background cũ
            if (currentTheme != null)
            {
                Destroy(currentTheme);
            }

            // 8. Tạo background mới
            GameObject selectedBackground = themes[pick].gameObject;

            currentTheme = Instantiate(selectedBackground, backgroundRoot);
        }

        // A prefab cannot reference the scene camera, so the owner passes it in.
        public void Initialize(Camera sceneCamera)
        {
            for (int i = 0; i < edgeAnchors.Length; i++)
            {
                edgeAnchors[i].SetCamera(sceneCamera);
            }
        }
    }
}
