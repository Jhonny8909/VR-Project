using TMPro;
using UnityEngine;

public class Reticula : MonoBehaviour
{
    public TriggerKnife Tk;
    public TextMeshProUGUI NumeroDeCuchillos;
    public TextMeshProUGUI NumeroDeShots;
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
    }

    void ActualizarNumero()
    {
        NumeroDeCuchillos.text = "" + ContadorKnifes.ToString();
    }
}
