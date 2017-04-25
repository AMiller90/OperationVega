
namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    /// <summary>
    /// The credits text class.
    /// Handles the behavior of the text on the credits screen.
    /// </summary>
    public class CreditsText : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// The on pointer click function.
        /// Reference to when this object is clicked.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        public void OnPointerClick(PointerEventData eventData)
        {
            this.canrotate = !this.canrotate;
        }

        /// <summary>
        /// The can rotate reference.
        /// Reference to whether or not the text should rotate.
        /// </summary>
        private bool canrotate;

        /// <summary>
        /// The start function.
        /// </summary>
        private void Start()
        {
            if (this.GetComponent<Text>().text == "Credits")
                this.canrotate = true;
            else
                this.canrotate = false;
        }

        /// <summary>
        /// The update function.
        /// </summary>
        private void Update()
        {
            if(this.canrotate)
                this.transform.Rotate(Vector3.down, Time.deltaTime * 50);
        }
    }
}
