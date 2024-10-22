using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Reticula : MonoBehaviour
{
    public TriggerKnife Tk;
    public TextMeshProUGUI NumeroDeCuchillos;
    public TextMeshProUGUI NumeroDeShots;
    [HideInInspector]
    public int ContadorKnifes;
    // Start is called before the first frame update
    void Start()
    {
        //Tk = FindObjectOfType(TriggerKnife);
        ActualizarNumero();
    }

    // Update is called once per frame
    void Update()
    {
        ContadorKnifes = Tk.KnifesInBag;
        ActualizarNumero();
    }

    void ActualizarNumero()
    {
        NumeroDeCuchillos.text = "" + ContadorKnifes.ToString();
    }
}
