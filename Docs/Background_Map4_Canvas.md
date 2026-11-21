# Map_4 chuyển sang Canvas (World Space)

**Ngày:** 21/11/2026. **Phạm vi:** chỉ `BG_Map_4`. 4 nền còn lại vẫn dùng sprite.
**Kết quả:** game chọn ngẫu nhiên giữa 5 nền như cũ; khi ra Map_4 thì nền được vẽ bằng Canvas. Đã thử 8 lần vào màn, Map_4 bản Canvas xuất hiện đúng, Console không lỗi.

## 1. Các bước đã làm

| # | Việc | Chi tiết |
|---|---|---|
| 1 | Giữ bản sprite cũ để so sánh | `Resources/backgroundUI/BG_Map_4.prefab` chuyển sang `Art/Background/_SpriteVersion/BG_Map_4_Sprite.prefab` (ra khỏi `Resources`, nên game không còn chọn nó) |
| 2 | Sửa 3 script hiệu ứng cho chạy được với `Image` | `BGPulse`, `BGFlicker`, `BGSwimmer` (mục 3) |
| 3 | Dựng prefab mới `Resources/backgroundUI/BG_Map_4.prefab` | Cấu trúc ở mục 2. Các thông số chép từ bản sprite và đổi đơn vị sang pixel |
| 4 | Tăng độ sáng tia nắng | `BGPulse` 0,10–0,40 → **0,18–0,60** (trên nền Map_4 sáng, tia cũ gần như không thấy) |

**Không sửa:** `GameplayWorld.cs` (vẫn tìm prefab có `BackgroundTheme`), shader, material, ảnh.

## 2. Cấu trúc prefab mới

```
BG_Map_4                 BackgroundTheme (mapName = Map_4)
 ├─ Canvas               Canvas: World Space, Sorting Layer Default, Order -100
 │                       RectTransform 2048 x 2048, Scale 0.005859 (= 12/2048)
 │                       KHÔNG có GraphicRaycaster
 │   ├─ Base             Image: Map_4, 2048x2048, material BG_Sway_Map_4 (rong lắc)
 │   ├─ Rays/Ray1..3     Image: light_ray 1748x437, material BG_SpriteScreen, BGPulse
 │   ├─ FarFish/Fish1..2 Image: fish_14 / fish_201, BGSwimmer
 │   └─ SandCaustics     Image: sand_mask 2048x2048, material BG_Caustics_Map_4
 ├─ Bubbles              ParticleSystem (ngoài Canvas, order -95)
 └─ Dust                 ParticleSystem (ngoài Canvas, order -95)
```

**Nguyên tắc dựng:**
- **1 đơn vị Canvas = 1 pixel ảnh gốc.** Toạ độ trong ảnh 2048 px: `anchoredPosition = (px − 1024, 1024 − py)`. Canvas có scale 12/2048 nên phủ đúng 12×12 đơn vị như ảnh sprite cũ.
- **Thứ tự vẽ theo Hierarchy** (dưới vẽ sau). Cả Canvas vẽ như một khối ở order −100, sau gameplay.
- **Không chặn chạm:** mọi `Image` tắt Raycast Target, Canvas không có GraphicRaycaster.
- **Particle để ngoài Canvas**, vì `ParticleSystem` không vẽ trong Canvas. Chúng vẽ đè lên cả khối nền (order −95).

## 3. Thay đổi trong script

| Script | Trước | Sau |
|---|---|---|
| `BGPulse` | `[RequireComponent(SpriteRenderer)]`, chỉ đổi `sr.color` | Bỏ RequireComponent. Đổi alpha cho `SpriteRenderer` **hoặc** `Graphic` (Image) |
| `BGFlicker` | Như trên | Như trên |
| `BGSwimmer` | Lật cá bằng `sr.flipX` | Có SpriteRenderer thì vẫn `flipX`; với Image thì lật bằng `localScale.x` âm (Image không có flipX) |

Cả 3 vẫn chạy với bản sprite của 4 map còn lại, không cần sửa gì thêm ở đó.

## 4. Đổi đơn vị (thế giới → pixel, × 170,67)

| Thông số | Bản sprite | Bản Canvas |
|---|---|---|
| Vị trí tia nắng (y) | 5,2 | 887 |
| `BGPulse.driftX` | 0,18 | 31 |
| `BGSwimmer.speed` | 0,30 / −0,22 | 51 / −38 |
| `BGSwimmer.xRange` | −3,6 … 3,6 | −614 … 614 |
| `BGSwimmer.yRange` | 0,3 … 1,7 | 51 … 290 |
| Particle (`Bubbles`, `Dust`) | đơn vị thế giới | **giữ nguyên** (ở ngoài Canvas) |

## 5. So sánh nhanh

| | Sprite | Canvas (Map_4) |
|---|---|---|
| Hiển thị | Giống nhau | Giống nhau |
| Chỉnh vị trí | Đơn vị thế giới | Pixel theo ảnh gốc, kéo bằng RectTransform |
| Particle | Xen kẽ giữa các lớp được | Chỉ nằm trên toàn khối nền |
| Hiệu năng | Tốt | Tia nắng đổi alpha mỗi khung → Canvas dựng lại lưới. Nếu sau này thêm nhiều lớp nhấp nháy, nên tách chúng vào Canvas con |

## 6. Muốn quay lại / làm tiếp

- **Quay về bản sprite:** chuyển `_SpriteVersion/BG_Map_4_Sprite.prefab` vào `Resources/backgroundUI/` (đổi tên lại `BG_Map_4`) và chuyển bản Canvas ra ngoài.
- **Chuyển map khác sang Canvas:** làm theo mục 2 và đổi đơn vị theo mục 4. Riêng Map_3 có lớp san hô tách riêng (`reef.png`, pivot ở gốc) → đặt `RectTransform.pivot = (0.5, 0)`. Map_2 có sương (`BGDrift.amount` cũng phải nhân 170,67). Map_24 có đèn (`BGFlicker` đã hỗ trợ Image).

## 7. Lưu ý khi kiểm tra

- Chuyển động (lắc, bơi, tia nắng sáng tối) chỉ chạy khi cửa sổ Unity đang được chọn. Mình chưa xem chuyển động thật; ảnh chụp kiểm tra là khung tĩnh.
- Muốn xem nhanh Map_4: mở `Resources/backgroundUI/BG_Map_4.prefab` trong Prefab Mode, hoặc tạm chuyển 4 prefab kia ra ngoài `Resources/backgroundUI` để game chỉ chọn Map_4.
