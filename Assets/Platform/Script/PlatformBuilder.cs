using UnityEngine;

public class PlatformBuilder : MonoBehaviour
{
    public int width = 3;
    public int depth = 3;

    public GameObject cornerTile;
    public GameObject sideTile;
    public GameObject centerTile;

    public float tileSize = 2f; // IMPORTANT

    public void Build()
    {
        #if UNITY_EDITOR
                if (Application.isPlaying)
                    return;
        #endif

        Clear();

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

        // ALL tiles need X = -90
        Vector3 rot = new Vector3(-90f, 0f, 0f);

        // ---- CORNERS ----
        if (left && top) rot.y = -90f;
        else if (right && top) rot.y = 0f;
        else if (left && bot) rot.y = 180f;
        else if (right && bot) rot.y = 90f;

        // ---- SIDES ----
        else if (top) rot.y = 0f;
        else if (bot) rot.y = 180f;
        else if (left) rot.y = -90f;
        else if (right) rot.y = 90f;

        // ---- CENTER ----
        // rot already correct (-90, 0, 0)

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
            width * tileSize,
            tileSize,
            depth * tileSize
        );
    }


}
