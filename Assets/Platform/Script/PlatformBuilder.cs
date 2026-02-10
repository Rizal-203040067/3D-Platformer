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

    void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
