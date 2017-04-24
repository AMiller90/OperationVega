
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsText : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        this.canrotate = true;
    }

    private bool canrotate;

    private void Start()
    {
        
    }
    private void Update()
    {
        if(this.canrotate)
            this.transform.Rotate(Vector3.down, Time.deltaTime * 50);
    }
}
