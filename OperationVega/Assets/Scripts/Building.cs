
namespace Assets.Scripts
{
    using UI;

    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// The building class.
    /// This class is used to check if the object the mouse interacts with
    /// is a building or not.
    /// </summary>
    public class Building : MonoBehaviour
    {
        /// <summary>
        /// On Mouse Enter function.
        /// Handles when the mouse just started hovering over an object.
        /// </summary>
        public void OnMouseEnter()
        {
            // If the tooltip is active and the mouse isnt over a UI element then display tooltip
            if (ToolTip.Istooltipactive && !EventSystem.current.IsPointerOverGameObject())
            {
                UIManager.Self.Tooltipobjectpanel.gameObject.SetActive(true);
                switch (this.gameObject.name)
                {
                    case "Silo":
                        ToolTip.Self.Objectdescription = "Silo.\n This structure provides " +
                    " storage for the resources collected.";
                        break;
                    case "Decontamination":
                        ToolTip.Self.Objectdescription = "Decontamination Center.\n This structure provides " +
                    " the functionality of cleaning tainted resources.";
                        break;
                    case "Barracks":
                        ToolTip.Self.Objectdescription = "Barracks.\n This structure is the " +
                    " place where units will spawn when they are purchased.";
                        break;
                    case "Workshop":
                        ToolTip.Self.Objectdescription = "Workshop.\n This structure is the " +
                    " place where the rocket is stored when built.";
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// On Mouse Exit function.
        /// Handles when the mouse stops being over an object.
        /// </summary>
        public void OnMouseExit()
        {
            ToolTip.Self.Objectdescription = " ";
            UIManager.Self.Tooltipobjectpanel.gameObject.SetActive(false);
        }
    }
}
