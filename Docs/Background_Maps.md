# Nền gameplay động: 5 map

**Ngày:** 21/11/2026. **Người làm:** minhquan (phần UI).

**Mục tiêu:** thay 16 ảnh nền tĩnh bằng 5 nền chọn lọc, mỗi nền có hiệu ứng riêng cho đúng chủ đề.

## 1. Cách hoạt động chung

### Chọn nền

`GameplayWorld.RandomizeBackground()` được gọi mỗi lần vào màn:
- Thư mục `Resources/backgroundUI/` có prefab `BackgroundTheme` → chọn ngẫu nhiên 1 nền, không lặp lại nền vừa dùng. Prefab được tạo làm con của object `Background`, còn ảnh tĩnh cũ bị ẩn.
- Thư mục rỗng → quay về cách cũ (ảnh `Map_x` ngẫu nhiên).

### Vùng nhìn thấy

- Ảnh nền 2048×2048 px (12×12 đơn vị). Camera ortho cỡ 4.8, màn 9:16, nên chỉ thấy khoảng **x −2.7…2.7, y −4.8…4.8**: dải giữa, khoảng 45% bề ngang.
- Kệ bể che phần trên, bong bóng cá che nửa dưới. Vì vậy hiệu ứng được đặt ở **hai mép và khoảng giữa kệ bể với bong bóng**.
- Màn rộng hơn (tablet) sẽ thấy thêm hai bên.

### Thứ tự hiển thị

Toàn bộ nền nằm sau gameplay. Cá trong bong bóng có order 10.

| Order | Lớp |
|---|---|
| −100 | `Base`: ảnh nền |
| −99 | Tia nắng, cá xa, sương xa |
| −98 | Ánh sáng tổ ong trên cát |
| −97 | Sương gần, đèn và pha lê phát sáng |
| −96 | Lớp cây tách riêng (chỉ Map_3) |
| −95 | Particle: bong bóng, bụi, tuyết biển |

### Thành phần dùng chung

| Thành phần | Loại | Làm gì | Thông số chính |
|---|---|---|---|
| `FishJam/BG Sway` | Shader trên `Base` | Làm gợn lắc vùng cây theo mặt nạ `sway_mask.png`: gốc đứng yên, ngọn lắc. Không cần cắt cây ra lớp riêng. | `_Amp` (độ lắc), `_Speed`, `_Freq` |
| `FishJam/BG Caustics` | Shader trên `SandCaustics` | 2 lớp ánh sáng tổ ong (`honey_a.png`) trượt ngược chiều và gợn sóng, chỉ hiện trong `sand_mask.png`, hoà kiểu sáng | `_Intensity`, `_Color`, `_Tile1/2`, `_Speed1/2` |
| `FishJam/Sprite Screen` | Shader | Hoà kiểu sáng (screen) cho tia nắng, đèn, bụi | `_Color` |
| `BGPulse` | Script | Tia nắng sáng tối dần, xoay ±, trôi ngang | `minAlpha`, `maxAlpha`, `period`, `rotateAmount`, `driftX` |
| `BGSwimmer` | Script | Cá xa bơi ngang, ra khỏi màn thì quay lại ở độ cao ngẫu nhiên | `speed` (âm = sang trái), `yRange` |
| `BGSway` | Script | Lắc cả một lớp quanh pivot | `angle`, `period`, `bend` |
| `BGDrift` | Script | Trôi chậm qua lại, sáng tối dần (sương) | `amount`, `period`, `minAlpha/maxAlpha` |
| `BGFlicker` | Script | Sáng tối không đều theo nhiễu Perlin (đèn, pha lê) | `minAlpha/maxAlpha`, `speed`, `scaleAmount` |
| `BackgroundTheme` | Script ở gốc prefab | Đánh dấu prefab nền, ghi tên map và màu ánh sáng chủ đạo | `mapName`, `lightTint` |

**Đường dẫn:**
- Script: `Scripts/UICanvs/UI/Background/`
- Shader: `Shaders/`
- Ảnh và mặt nạ: `Art/Background/Map_x/`
- Material: `Art/Background/Materials/`

---

## 2. Map_3: Rạn san hô (`BG_Map_3`)

**Cảm giác:** rạn san hô ấm áp, đông vui, có cá bơi ngang.

| Lớp | Chi tiết |
|---|---|
| `Base` | Ảnh `Art/Background/Map_3/base.png`: Map_3 đã **xoá rặng san hô** (tách bằng GrabCut) rồi vá và làm mờ. Đây là map duy nhất có nền riêng. |
| `Reef` | Rặng san hô cam và ống tím tách thành ảnh `reef.png`, pivot ở gốc. `BGSway` lắc 1,6°, chu kỳ 4,6 giây. Dịch phải 0,5 đơn vị để mép trái màn chơi thấy nhiều san hô hơn. |
| `Rays` (3 tia) | Màu kem ấm, sáng tối 0,12–0,45, chu kỳ 9 / 10,7 / 12,4 giây |
| `FarFish` (3 con) | `fish_14`, `fish_201`, `fish_17` tô xanh mờ, tốc độ 0,32 / −0,22 / 0,27. **Điểm riêng của map.** |
| `SandCaustics` | Material `BG_Caustics_Map3`, độ sáng 0,55 |
| `Bubbles` | 1,6 bong bóng/giây từ đáy |
| `Dust` | 4 hạt/giây, khắp màn |

## 3. Map_4: Đường cát có cỏ (`BG_Map_4`)

**Cảm giác:** sáng sủa, thoáng, gần phong cách Main Menu. Map "dễ chịu" nhất.

| Lớp | Chi tiết |
|---|---|
| `Base` | Ảnh gốc Map_4, material `BG_Sway_Map_4`. **Rong biển xanh** ở hai bên đường lắc theo mặt nạ (độ lắc 0,007, tốc độ 1,3). |
| `Rays` (3 tia) | Màu xanh nhạt, sáng tối 0,10–0,40, đè lên các tia nắng đã vẽ sẵn ở đỉnh ảnh |
| `FarFish` (2 con) | Độ mờ 0,55 và 0,43 |
| `SandCaustics` | Ánh sáng tổ ong **chạy dọc con đường cát**, độ sáng 0,55, màu kem |
| `Bubbles`, `Dust` | 1,4 bong bóng/giây; 3,5 hạt/giây |

## 4. Map_5: Hang đá (`BG_Map_5`)

**Cảm giác:** vào sâu trong hang, ánh sáng rọi từ cửa hang, yên tĩnh.

| Lớp | Chi tiết |
|---|---|
| `Base` | Ảnh gốc Map_5, material `BG_Sway_Map_5`. **Rặng san hô xa** (trên phải) và cây nhỏ cạnh bọt biển lắc rất nhẹ (độ lắc 0,005). |
| `Rays` (3 tia) | **Luồng sáng ấm rọi xuống từ cửa hang** ở giữa trên (x 0,1…1,3, y 4,6), xoè ra −9° / 1° / 9°, sáng tối 0,12–0,50. **Điểm riêng của map.** |
| `Dust` | **Bụi dày** (7 hạt/giây) tập trung trong luồng sáng (khung 3,2 × 7,5 quanh x 0,6) |
| `FarFish` (1 con) | Độ mờ 0,45 |
| `SandCaustics` | Độ sáng 0,42 (đã giảm vì cát Map_5 rất sáng) |
| `Bubbles` | 0,9 bong bóng/giây, ít vì là trong hang |

## 5. Map_2: Đền đổ tím (`BG_Map_2`)

**Cảm giác:** huyền bí, ma mị, tông tím lạnh.

| Lớp | Chi tiết |
|---|---|
| `Base` | Ảnh gốc Map_2, material `BG_Sway_Map_2`. Cây ở chân cột và cầu thang lắc (độ lắc 0,006). |
| `Fog` (3 lớp) | Ảnh sương `fog.png` (tạo bằng nhiễu, 512×256), màu tím nhạt, độ mờ 0,30–0,55. Trôi chậm qua lại với chu kỳ 26 / 33 / 40 giây. 1 lớp xa (sau cát) và 2 lớp gần. **Điểm riêng của map.** |
| `MarineSnow` | **Tuyết biển** rơi lất phất từ trên xuống, 5 hạt/giây, sống 14–20 giây |
| `Rays` (2 tia) | Rất mờ (0,04–0,18), tím nhạt |
| `SandCaustics` | Màu oải hương, độ sáng 0,35, đè lên các vân sáng đã vẽ sẵn trên nền đá |
| `Bubbles` | 0,6 bong bóng/giây |

## 6. Map_24: Lâu đài dưới biển (`BG_Map_24`)

**Cảm giác:** lấp lánh, có "người ở", nắng mạnh.

| Lớp | Chi tiết |
|---|---|
| `Base` | Ảnh gốc Map_24, material `BG_Sway_Map_24`. Rong xanh và san hô hồng lắc (độ lắc 0,006). |
| `Glows/Crystal…` | Quầng sáng xanh lam trên pha lê. `BGFlicker` chậm (tốc độ 0,8), độ mờ 0,45–1. **Điểm riêng của map.** |
| `Glows/Light…` | Quầng sáng vàng trên đèn và cửa sổ lâu đài. `BGFlicker` nhanh (tốc độ 3), giống ánh nến. |
| `Rays` (3 tia) | Mạnh nhất trong 5 map (0,14–0,50), xanh nhạt |
| `BubbleStream` | Chuỗi bong bóng bay lên đều ở mép phải (x 2,0), 4 bong bóng/giây |
| `SandCaustics` | Độ sáng 0,5, xanh nhạt |
| `Bubbles`, `Dust` | 1,2 bong bóng/giây; 3,5 hạt/giây |

**Lưu ý vị trí:** phần lớn pha lê và đèn trong ảnh nằm ngoài dải nhìn thấy trên điện thoại.
- `CrystalView1`, `CrystalView2` và `LightView` là các điểm sáng **nằm trong vùng nhìn thấy**.
- `Crystal1–3` và `Light1–3` nằm ngoài, chỉ hiện trên màn rộng (tablet).

---

## 7. Chỉnh nhanh

| Muốn | Sửa ở đâu |
|---|---|
| Cây lắc mạnh hoặc yếu hơn | `_Amp` trong `Materials/BG_Sway_Map_x` (Map_3: `angle` của `BGSway` trên `Reef`) |
| Ánh sáng tổ ong sáng hoặc tối hơn | `_Intensity` trong `Materials/BG_Caustics_Map_x` |
| Tia nắng rõ hơn | `maxAlpha` của `BGPulse` trên `Rays/Ray*` |
| Nhiều hoặc ít bong bóng, bụi | `Emission > Rate over Time` của `Bubbles` / `Dust` / `MarineSnow` |
| Thêm vùng cây lắc | Vẽ trắng lên `Art/Background/Map_x/sway_mask.png` (đen = đứng yên, trắng = lắc nhiều; nên để gốc tối và ngọn sáng) |
| Thêm nền thứ 6 | Tạo prefab có `BackgroundTheme` ở gốc, `Base` dùng material `BG Sway`, đặt vào `Resources/backgroundUI/`. Game tự nhận. |
| Tắt nền động | Chuyển các prefab ra khỏi `Resources/backgroundUI/`. Game quay về ảnh tĩnh. |

## 8. Còn lại

- 11 ảnh `Map_x` không dùng vẫn nằm trong `Resources/data/images/sprites/Resources/background/`. Nên chuyển ra ngoài `Resources` cho nhẹ bản build, khi đã chốt bỏ hẳn cách cũ.
- Ảnh nháp nằm trong `Docs/bg_work/`, có thể xoá.
- Chuyển động thật (lắc, bơi, trôi) chưa được xem trên máy thật. Các ảnh chụp kiểm tra chỉ là khung tĩnh trong Editor.
