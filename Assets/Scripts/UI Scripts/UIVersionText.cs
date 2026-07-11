using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIVersionText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        TMP_Text versionText = GetComponent<TMP_Text>();
        versionText.text = $"v.{Application.version}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
