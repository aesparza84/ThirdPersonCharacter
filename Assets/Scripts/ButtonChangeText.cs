using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeText : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI myText;
    [SerializeField] string textChange;
    [SerializeField] Image background;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeText()
    {
        myText.color = Random.ColorHSV();
        myText.text = textChange;
    }

    public void changeColor()
    {
        background.color = Random.ColorHSV();   
    }    
}
