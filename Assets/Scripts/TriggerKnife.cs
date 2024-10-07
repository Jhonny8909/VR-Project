using UnityEngine;

public class TriggerKnife : MonoBehaviour
{
    public GameObject knife; // Prefab del cuchillo
    public Transform parent; // Padre al que se instanciará el cuchillo
    public int maxKnives = 5; // Número máximo de cuchillos que pueden ser instanciados
    private int currentKnives = 0; // Contador de cuchillos instanciados

    public GameObject heldKnife; // Cuchillo actualmente sostenido

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KnifeSpace"))
        {
            // Contar los cuchillos existentes en la escena
            currentKnives = GameObject.FindGameObjectsWithTag("Knife").Length;

            if (currentKnives >= maxKnives)
            {
                Debug.Log("Ya se han instanciado el número máximo de cuchillos.");
                return; // No hacer nada si se alcanzó el límite
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
        heldKnife.transform.localPosition = Vector3.zero; // Ajusta la posición local
        Debug.Log("Cuchillo instanciado en el espacio adecuado. Total de cuchillos: " + (currentKnives + 1));
    }

    public void KnifeThrown()
    {
        if (heldKnife != null)
        {
            heldKnife = null; // Resetea la referencia al cuchillo
        }
    }
}