using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reticula : MonoBehaviour
{
    public TriggerKnife Tk;
    public TextMeshProUGUI NumeroDeCuchillos;
    public TextMeshProUGUI NumeroDeShots;
    public Image InvisibilitySlider;
    VRInvisibility vrIn;
    [HideInInspector]
    public int ContadorKnifes;
    // Start is called before the first frame update
    void Start()
    {
        //Tk = FindObjectOfType(TriggerKnife);
        ActualizarNumero();
        vrIn = FindAnyObjectByType<VRInvisibility>();
    }

    // Update is called once per frame
    void Update()
    {
        ContadorKnifes = Tk.KnifesInBag;
        ActualizarNumero();
        UpdateInvisibilityCooldown();  
    }

    void ActualizarNumero()
    {
        NumeroDeCuchillos.text = "" + ContadorKnifes.ToString();
    }
    
    void UpdateInvisibilityCooldown()
    {
        if (InvisibilitySlider != null)
        {
            InvisibilitySlider.fillAmount = vrIn.elapsedTimeInvisibility;
        }
    }
}
