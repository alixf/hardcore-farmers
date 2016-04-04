using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTerrain : MonoBehaviour
{
    public static Vector3 camStartingPos;

    public Transform hexTile;
    public Transform focusedTile;
    public Vector2 focusedPosition;
    public int size;
    public Dictionary<int, Transform> tiles = new Dictionary<int, Transform>();
    public Vector3 cameraOffset;
    public Transform highlight;
    public Texture2D[] levels;
    public int levelIndex = 0;

    public Transform bonusPrefab;

    public Sprite carrotSprite, lettuceSprite, potatoesSprite;

    void Start()
    {
        camStartingPos = Camera.main.transform.position;
    }

    public void Build()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float yy = y;
                float xx = x;
                if (y % 2 == 0)
                    xx += 0.5f;
                float factor = Mathf.Cos(Mathf.Deg2Rad * 30);
                var tile = Instantiate(hexTile, new Vector3(xx * factor, Random.Range(0f, 0.1f), yy * 0.75f), Quaternion.AngleAxis(90, Vector3.left)) as Transform;
                tile.SetParent(transform);
                tiles.Add(Vec2(x, y), tile);
                tile.localScale = Vector3.one;
                float colorFactor = Random.Range(0.95f, 1f);
                tile.GetComponent<MeshRenderer>().material.color = new Color(colorFactor, colorFactor, colorFactor);

                Texture2D level = levels[levelIndex];
                Color c = level.GetPixel(size-1-x, size-1-y);
                if (c == Color.red || c == Color.green || c == Color.blue)
                {
                    var bonus = Instantiate(bonusPrefab);
                    bonus.SetParent(tile);
                    bonus.localPosition = bonusPrefab.localPosition;
                    bonus.localRotation = bonusPrefab.localRotation;
                    bonus.localScale = bonusPrefab.localScale;

                    if (c == Color.red) // Carrot
                    {
                        bonus.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
                        bonus.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f, 0.50f);
                        bonus.GetChild(0).GetComponent<SpriteRenderer>().sprite = carrotSprite;
                        tile.GetComponent<Tile>().bonusType = Tile.ObjectType.CARROT;
                    }
                    if (c == Color.green) // Lettuce
                    {
                        bonus.GetComponent<SpriteRenderer>().color = new Color(0.25f, 1f, 0f);
                        bonus.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.25f, 1f, 0f, 0.15f);
                        bonus.GetChild(0).GetComponent<SpriteRenderer>().sprite = lettuceSprite;
                        tile.GetComponent<Tile>().bonusType = Tile.ObjectType.LETTUCE;
                    }
                    if (c == Color.blue) // Potatoes
                    {
                        bonus.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f);
                        bonus.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 0.20f);
                        bonus.GetChild(0).GetComponent<SpriteRenderer>().sprite = potatoesSprite;
                        tile.GetComponent<Tile>().bonusType = Tile.ObjectType.POTATOES;
                    }
                }
            }
        }

        cameraOffset = Camera.main.transform.position;
        Focus(new Vector2(Mathf.Floor(size / 2), Mathf.Floor(size / 2)));
    }

    int Vec2(int x, int y)
    {
        return x * size + y;
    }

    int Vec2(Vector2 pos)
    {
        return Mathf.FloorToInt(pos.x) * size + Mathf.FloorToInt(pos.y);
    }

    void Update()
    {
        if (!GameManager.instance.allowAction)
            return;

        if (GameManager.instance.inputType == GameManager.InputType.MOVING)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && focusedPosition.x < size-1)
            {
                UnFocus(focusedPosition);
                focusedPosition.Set(focusedPosition.x + 1, focusedPosition.y);
                Focus(focusedPosition);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && focusedPosition.x > 0)
            {
                UnFocus(focusedPosition);
                focusedPosition.Set(focusedPosition.x - 1, focusedPosition.y);
                Focus(focusedPosition);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && focusedPosition.y > 0)
            {
                UnFocus(focusedPosition);
                focusedPosition.Set(focusedPosition.x, focusedPosition.y - 1);
                Focus(focusedPosition);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && focusedPosition.y < size-1)
            {
                UnFocus(focusedPosition);
                focusedPosition.Set(focusedPosition.x, focusedPosition.y + 1);
                Focus(focusedPosition);
            }
        }
    }

    void UnFocus(Vector2 position)
    {
        Transform prevTile;
        if (tiles.TryGetValue(Vec2(position), out prevTile)) {
        }
    }
    void Focus(Vector2 position)
    {
        if(tiles.TryGetValue(Vec2(position), out focusedTile)) {
            focusedPosition = position;
            highlight.SetParent(focusedTile, false);
            Camera.main.GetComponent<MovingCamera>().MoveTo(focusedTile.position + cameraOffset);

        }
    }

    public void Cleanup()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        tiles.Clear();
        Camera.main.transform.position = camStartingPos;
    }

    public void NextLevel()
    {
        levelIndex = (levelIndex + 1) % levels.Length;
    }
}
