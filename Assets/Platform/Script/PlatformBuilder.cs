using UnityEngine;

public class PlatformBuilder : MonoBehaviour
{
    public int width = 3;
    public int depth = 3;

    [Header("3D Tiles")]
    public GameObject cornerTile;
    public GameObject sideTile;
    public GameObject centerTile;

    [Header("2D Tiles (For Thin Platforms)")]
    public GameObject centerTile2D;
    public GameObject endTile2D;

    public float tileSize = 2f; // IMPORTANT

    public void Build()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            return;
#endif

        Clear();

        //  Thin platform check
        if (width == 1 || depth == 1)
        {
            BuildThinPlatform();
            UpdateCollider();
            return;
        }

        //  Normal platform
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 position = new Vector3(
                    (x - (width / 2)) * tileSize,
                    0f,
                    -z * tileSize
                );

                GameObject prefab = GetTile(x, z);
                Quaternion rotation = GetRotation(x, z);

                Instantiate(prefab, position, rotation, transform);
            }
        }

        UpdateCollider();
    }

    //  NEW — Thin Platform Builder
    void BuildThinPlatform()
    {
        int length = Mathf.Max(width, depth);

        for (int i = 0; i < length; i++)
        {
            Vector3 position;

            if (width == 1) // Vertical strip
            {
                position = new Vector3(
                    0f,
                    0f,
                    -i * tileSize
                );
            }
            else // Horizontal strip
            {
                position = new Vector3(
                    (i - (length / 2)) * tileSize,
                    0f,
                    0f
                );
            }

            GameObject prefab =
                (i == 0 || i == length - 1)
                ? endTile2D
                : centerTile2D;

            Quaternion rotation = GetThinRotation();

            Instantiate(prefab, position, rotation, transform);
        }
    }

    Quaternion GetThinRotation()
    {
        Vector3 rot = new Vector3(-90f, 0f, 0f);

        if (width == 1)
            rot.y = 0f;      // Vertical platform
        else
            rot.y = 90f;     // Horizontal platform

        return Quaternion.Euler(rot);
    }

    GameObject GetTile(int x, int z)
    {
        bool left = x == 0;
        bool right = x == width - 1;
        bool top = z == 0;
        bool bot = z == depth - 1;

        if ((left && top) || (left && bot) || (right && top) || (right && bot))
            return cornerTile;

        if (left || right || top || bot)
            return sideTile;

        return centerTile;
    }

    Quaternion GetRotation(int x, int z)
    {
        bool left = x == 0;
        bool right = x == width - 1;
        bool top = z == 0;
        bool bot = z == depth - 1;

        Vector3 rot = new Vector3(-90f, 0f, 0f);

        if (left && top) rot.y = -90f;
        else if (right && top) rot.y = 0f;
        else if (left && bot) rot.y = 180f;
        else if (right && bot) rot.y = 90f;

        else if (top) rot.y = 0f;
        else if (bot) rot.y = 180f;
        else if (left) rot.y = -90f;
        else if (right) rot.y = 90f;

        return Quaternion.Euler(rot);
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
#else
            Destroy(transform.GetChild(i).gameObject);
#endif
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = transform.position + new Vector3(
                    (x - (width / 2)) * tileSize,
                    0f,
                    -z * tileSize
                );

                Gizmos.DrawWireCube(pos, Vector3.one * tileSize);
            }
        }
    }

    public void UpdateCollider()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (!col)
            col = gameObject.AddComponent<BoxCollider>();

        col.center = new Vector3(
            0f,
            0f,
            -(depth - 1) * tileSize * 0.5f
        );

        col.size = new Vector3(
            Mathf.Max(1, width) * tileSize,
            tileSize,
            Mathf.Max(1, depth) * tileSize
        );
    }
}
