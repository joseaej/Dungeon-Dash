using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    private float point;
    private TextMeshProUGUI textMeshPro;
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMeshPro.text = point.ToString("0");
    }
    public void GetPoint(float puntos){
        point +=puntos;
    }
}

