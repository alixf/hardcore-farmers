using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    private Coroutine pointsCoroutine = null;
    public Text pointsText;
    public int totalPoints = 0;
    public int currentPoints = 0;

    Tile.ObjectType comboType = Tile.ObjectType.NONE;
    int comboCount = 0;

    public RectTransform comboBonusPrefab;
    public RectTransform terrainBonusPrefab;

    public void AddPoints(Tile.ObjectType harvestType, Tile.ObjectType bonusType, int pointsToAdd)
    {
        if(bonusType == harvestType)
        {

            pointsToAdd *= 3;
            SpawnTerrainBonus();
        }
        if(harvestType == comboType)
            comboCount++;
        else
            comboCount = 0;

        if(comboCount > 0)
        {
            SpawnComboBonus(comboCount);
        }

        comboType = harvestType;
        pointsToAdd *= (1 + Mathf.CeilToInt(comboCount * 0.5f));

        totalPoints += pointsToAdd;
        if(pointsCoroutine != null)
            StopCoroutine(pointsCoroutine);
        pointsCoroutine = StartCoroutine(IncrementPoints());
    }

    IEnumerator IncrementPoints()
    {
        float duration = 0.5f;
        int start = currentPoints;
        for(float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            currentPoints = Mathf.FloorToInt(Mathf.Lerp((float) currentPoints, (float) totalPoints, clock / 0.5f));
            RefreshPoints();
            yield return null;
        }
        currentPoints = totalPoints;
        RefreshPoints();
    }

    public void RefreshPoints()
    {
        string pointsStr = "" + currentPoints;
        string prefix = "<color=#999999>";
        for (int i = 0; i < 8 - pointsStr.Length; i++)
            prefix += "0";
        prefix += "</color>";
        pointsText.text = prefix + pointsStr;
    }

    void SpawnTerrainBonus()
    {
        RectTransform bonus = Instantiate(terrainBonusPrefab);
        bonus.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        bonus.anchoredPosition = terrainBonusPrefab.anchoredPosition;
        bonus.localRotation = terrainBonusPrefab.localRotation;
        bonus.localScale = terrainBonusPrefab.localScale;

        StartCoroutine(Coroutines.Move2D(bonus, new Vector2(bonus.anchoredPosition.x, 100), 1f, Easings.CubicEaseOut));
        StartCoroutine(Coroutines.Fade(bonus.GetComponent<CanvasGroup>(), 0f, 1f, 0.2f));
        StartCoroutine(Coroutines.Fade(bonus.GetComponent<CanvasGroup>(), 1f, 0f, 0.2f, Easings.Linear, 0.75f));
        StartCoroutine(Coroutines.Destroy(bonus.gameObject, 1f));
    }
    void SpawnComboBonus(int combo)
    {
        RectTransform bonus = Instantiate(comboBonusPrefab);
        bonus.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        bonus.anchoredPosition = comboBonusPrefab.anchoredPosition;
        bonus.localRotation = comboBonusPrefab.localRotation;
        bonus.localScale = comboBonusPrefab.localScale;
        bonus.GetComponent<Text>().text = "COMBO X" + string.Format("{0:0.0}", 1f + combo * 0.5f);

        StartCoroutine(Coroutines.Move2D(bonus, new Vector2(bonus.anchoredPosition.x, 100), 1f, Easings.CubicEaseOut));
        StartCoroutine(Coroutines.Fade(bonus.GetComponent<CanvasGroup>(), 0f, 1f, 0.2f));
        StartCoroutine(Coroutines.Fade(bonus.GetComponent<CanvasGroup>(), 1f, 0f, 0.2f, Easings.Linear, 0.75f));
        StartCoroutine(Coroutines.Destroy(bonus.gameObject, 1f));
    }
}
