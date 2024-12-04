using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed;    // カメラの左右・前後移動スピード
    public float elevationSpeed;  // 上下移動スピードを増やす
    public float maxY;        // カメラのY軸の最大高さ
    public float minY;         // カメラのY軸の最小高さ（ズーム制限）
    public float minX;        // X軸の最小制限
    public float maxX;         // X軸の最大制限
    public float maxZ;         // Z軸の最大制限
    public float minZ;        // Z軸の最小制限 (-89に設定)

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleMovement();
        HandleElevation();
        ApplyPositionLimits(); // カメラの位置制限を適用
    }

    // WASDで前後左右に移動
    void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // W/SキーでZ軸の前後移動
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward; // Z軸正方向に前進
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= Vector3.forward; // Z軸負方向に後退
        }

        // A/DキーでX軸の左右移動
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= Vector3.right; // X軸負方向に移動（左）
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right; // X軸正方向に移動（右）
        }

        // カメラを移動
        cam.transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    // Qで下に移動、Eで上に移動
    void HandleElevation()
    {
        Vector3 elevationDirection = Vector3.zero;

        // Eキーで上に移動
        if (Input.GetKey(KeyCode.E))
        {
            elevationDirection += Vector3.up; // 上昇
        }

        // Qキーで下に移動
        if (Input.GetKey(KeyCode.Q))
        {
            elevationDirection -= Vector3.up; // 下降
        }

        // カメラの上下移動
        cam.transform.position += elevationDirection * elevationSpeed * Time.deltaTime;
    }

    // カメラの位置制限
    void ApplyPositionLimits()
    {
        // カメラの現在位置を取得
        Vector3 position = cam.transform.position;

        // Y軸の制限（990以上、10以下にならない）
        if (position.y > maxY)
        {
            position.y = maxY;
        }
        else if (position.y < minY)
        {
            position.y = minY;  // 下にズームしすぎないように最低値を10に制限
        }

        // X軸の制限（-77以下にならない、87以上にならない）
        if (position.x < minX)
        {
            position.x = minX;
        }
        else if (position.x > maxX)
        {
            position.x = maxX;
        }

        // Z軸の制限（-89以下にならない、40以上にならない）
        if (position.z < minZ)
        {
            position.z = minZ;
        }
        else if (position.z > maxZ)
        {
            position.z = maxZ;
        }

        // カメラの位置を更新
        cam.transform.position = position;
    }
}
