using UnityEngine;

public class TriggerKnife : MonoBehaviour
{
    public GameObject knife; // Prefab del cuchillo
    [HideInInspector]
    public GameObject heldKnife; // Cuchillo actualmente sostenido
    public Transform parent; // Padre al que se instanciar� el cuchillo
    public int maxKnives = 5; // N�mero m�ximo de cuchillos que pueden ser instanciados
    private int currentKnives = 0; // Contador de cuchillos instanciados

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KnifeSpace"))
        {
            if (currentKnives >= maxKnives)
            {
                Debug.Log("Ya se han instanciado el n�mero m�ximo de cuchillos.");
                return; // No hacer nada si se alcanz� el l�mite
            }

            if (heldKnife != null)
            {
                Debug.Log("Knife ya spawneado");
            }
            else
            {
                InstantiateKnife();
            }
        }
    }

    private void InstantiateKnife()
    {
        heldKnife = Instantiate(knife, parent);
        Rigidbody rb = heldKnife.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Hace que el cuchillo no se mueva al instante
        }

        heldKnife.transform.localPosition = Vector3.zero; // Ajusta la posici�n local
        heldKnife.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f); // Ajusta la escala
        currentKnives++; // Incrementa el contador de cuchillos
        Debug.Log("Cuchillo instanciado en el espacio adecuado. Total de cuchillos: " + currentKnives);
    }

    public void KnifeThrown()
    {
        if (heldKnife != null)
        {
            currentKnives--; // Decrementa el contador al lanzar el cuchillo
            heldKnife = null; // Resetea la referencia al cuchillo
        }
    }
}