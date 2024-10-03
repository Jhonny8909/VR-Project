using UnityEngine;

public class TriggerKnife : MonoBehaviour
{
    public GameObject knife;
    [HideInInspector]
    public GameObject heldKnife;
    public Transform Parent;
    public float totalThrows;
    public bool instan;

    private void Awake()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        print("Trigger");
        if (other.CompareTag("KnifeSpace"))
        {
            if(heldKnife != null)
            {
                Debug.Log("Knife ya spawneado");
            }
            else
            {
                heldKnife = Instantiate(knife, Parent);
                heldKnife.GetComponent<Rigidbody>().isKinematic = true;
                heldKnife.transform.localPosition = Vector3.zero;
                heldKnife.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                instan = true;
                Debug.Log("Knife Spawneado");
            }
        }
    }
}
