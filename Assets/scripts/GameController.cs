using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject elementPrefab;

    public Color blockColor;
    public Color yellowColor;
    public Color orangeColor;
    public Color redColor;

    public UnityEvent onWin;


    Transform[,] kletks = new Transform[5, 5];

    private void Awake()
    {
        for(int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                kletks[x, y] = transform.GetChild(x + y * 5);
            }
        }

        GenerateField();
    }

    public void CheckWin()
    {
        if (isFieldWin())
        {
            onWin.Invoke();
        }
    }

    public bool CanMoveToXY(int x, int y)
    {
        if (x < 0 || x > 4 || y < 0 || y > 4)
            return false;

        return kletks[x, y].childCount == 0;
    }
    public void SetParentKletka(Transform element, int x, int y)
    {
        if (x < 0 || x > 4 || y < 0 || y > 4 || kletks[x,y].childCount > 0)
            return;

        element.SetParent(kletks[x, y]);
    }

    void SetBlock(int x, int y, ElementType t) 
    {
        Instantiate(elementPrefab, kletks[x, y]).GetComponent<Element>().initElement(x, y, t, this);
    }

    void GenerateField() 
    {
        SetBlock(1, 0, ElementType.BLOCK);
        SetBlock(3, 0, ElementType.BLOCK);
        SetBlock(1, 2, ElementType.BLOCK);
        SetBlock(3, 2, ElementType.BLOCK);
        SetBlock(1, 4, ElementType.BLOCK);
        SetBlock(3, 4, ElementType.BLOCK);

        for(ElementType t = ElementType.YELLOW; t <= ElementType.RED; t++)
        {
            for(int i = 0; i < 5; i++)
            {
                int x = Random.Range(0, 5);
                int y = Random.Range(0, 5);
                if (kletks[x,y].childCount == 0)
                {
                    SetBlock(x, y, t);
                }
                else
                {
                    i--;
                }
            }
        }

        if (isFieldWin())
        {
            ReGenerateField();
        }
    }

    bool isFieldWin() 
    {
        for(int i = 0; i < 5; i++)
        {
            if (kletks[0, i].childCount > 0 && kletks[0, i].GetComponentInChildren<Element>().type
                != ElementType.YELLOW) return false;
            if (kletks[2, i].childCount > 0 && kletks[2, i].GetComponentInChildren<Element>().type
                != ElementType.ORANGE) return false;
            if (kletks[4, i].childCount > 0 && kletks[4, i].GetComponentInChildren<Element>().type
                != ElementType.RED) return false;
        }
        return true;
    }
    public void ReGenerateField() 
    {
        ClearField();
        StartCoroutine(Gen());
    }

    public void Win()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator Gen()
    {
        yield return null;
        GenerateField();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    void ClearField()
    {
        for(int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++) 
            {
                if (kletks[x,y].childCount > 0)
                {
                    Destroy(kletks[x, y].GetChild(0).gameObject);
                }
            }
        }
    }

    public Color GetColorByType(ElementType t)
    {
        switch (t)
        {
            case ElementType.BLOCK: return blockColor;
            case ElementType.YELLOW: return yellowColor;
            case ElementType.ORANGE: return orangeColor;
            case ElementType.RED: return redColor;
        }
        return Color.white;
    }
}
