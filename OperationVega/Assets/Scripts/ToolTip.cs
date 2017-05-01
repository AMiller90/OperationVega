
namespace Assets.Scripts
{
    using UI;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The tool tip class.
    /// This will handle the functionality of the tool tip behavior for
    /// hovering over an object to display its information.
    /// </summary>
    public class ToolTip : MonoBehaviour
    {
        /// <summary>
        /// The is tool tip active reference.
        /// Determines whether to display the tool tip panel or not.
        /// </summary>
        public static bool Istooltipactive;

        /// <summary>
        /// The instance of the tool tip class.
        /// </summary>
        private static ToolTip instance;

        /// <summary>
        /// The x offset of the panel from the mouse.
        /// </summary>
        private float xoffset;

        /// <summary>
        /// The y offset of the panel from the mouse.
        /// </summary>
        private float yoffset;

        /// <summary>
        /// The text display reference.
        /// The text on the panel to give information to.
        /// </summary>
        private Text textdisplay;

        /// <summary>
        /// The position set.
        /// Reference to the position to set the tool tip to.
        /// </summary>
        private bool positionSet = false;

        /// <summary>
        /// Gets the self instance of the tool tip.
        /// </summary>
        public static ToolTip Self
        {
            get
            {
                // Set instance to the value of instance if instance is NOT null; otherwise,
                // if instance = null, set instance to new instance().
                instance = instance ?? FindObjectOfType(typeof(ToolTip)) as ToolTip;
                return instance;
            }
        }

        /// <summary>
        /// Sets the object description reference.
        /// This will set the text each time an object is hovered over to display
        /// the proper information.
        /// </summary>
        public string Objectdescription
        {
            set
            {
                this.textdisplay.text = value;
            }
        }

        /// <summary>
        /// The start function.
        /// </summary>
        private void Start()
        {
            instance = this;
            Istooltipactive = true;
            this.textdisplay = this.GetComponentInChildren<Text>();
            
            // Give x more offset than y so the panel isnt preventing the mouse from ray casting
            this.xoffset = this.transform.GetComponent<RectTransform>().sizeDelta.x / 2 + 25;
            this.yoffset = this.transform.GetComponent<RectTransform>().sizeDelta.y / 2;
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// The OnEnable function.
        /// </summary>
        private void OnEnable()
        {
			if (!this.positionSet)
			{
				// Set the position to the mouse with an offset of having the mouse in the bottom left corner.
				this.transform.position = new Vector3(
					Input.mousePosition.x + this.xoffset,
					Input.mousePosition.y + this.yoffset,
					Input.mousePosition.z);
			    this.positionSet = true;
			}
        }

        /// <summary>
        /// The on disable function.
        /// </summary>
        private void OnDisable()
	    {
		    this.positionSet = false;
	    }
    }
}