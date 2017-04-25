
namespace Assets.Scripts
{
    using UI;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// The button tooltip class.
    /// This displays the costs of the specific parts as a tooltip to the user.
    /// </summary>
    public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// The on pointer enter function.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.Self.Tooltipobjectpanel.gameObject.SetActive(true);
			PartPreview.Self.gameObject.SetActive(true);

            switch (this.gameObject.name)
            {
                case "THRUSTER1":
                    ToolTip.Self.Objectdescription = "Rust Type\n Requirements:\n Steel Cost: 200\n Fuel Cost: 50\n Quality: 20";
					PartPreview.Self.rustT.SetActive(true);
					break;
                case "THRUSTER2":
                    ToolTip.Self.Objectdescription = "Color Type\n Requirements:\n Steel Cost: 200 \n Fuel Cost: 50\n Quality: 50";
					PartPreview.Self.colorT.SetActive(true);
					break;
                case "THRUSTER3":
                    ToolTip.Self.Objectdescription = "Flame Type\n Requirements:\n Steel Cost: 200 \n Fuel Cost: 50\n Quality: 80";
					PartPreview.Self.flameT.SetActive(true);
					break;
                case "COCKPIT1":
                    ToolTip.Self.Objectdescription = "Rust Type\n Requirements:\n Steel Cost: 200 \n Fuel Cost: 0\n Quality: 20";
					PartPreview.Self.rustCP.SetActive(true);
					break;
                case "COCKPIT2":
                    ToolTip.Self.Objectdescription = "Color Type\n Requirements:\n Steel Cost: 200 \n Fuel Cost: 0\n Quality: 50";
					PartPreview.Self.colorCP.SetActive(true);
					break;
                case "COCKPIT3":
                    ToolTip.Self.Objectdescription = "Flame Type\n Requirements:\n Steel Cost: 200 \n Fuel Cost: 0\n Quality: 80";
					PartPreview.Self.flameCP.SetActive(true);
					break;
                case "WING1":
                    ToolTip.Self.Objectdescription = "Rust Type\n Requirements:\n Steel Cost: 200 \n Fuel Cost: 0\n Quality: 20";
					PartPreview.Self.rustW.SetActive(true);
					break;
                case "WING2":
                    ToolTip.Self.Objectdescription = "Color Type\n Requirements:\n Steel Cost: 200 \n Fuel Cost: 0\n Quality: 50";
					PartPreview.Self.colorW.SetActive(true);
					break;
                case "WING3":
                    ToolTip.Self.Objectdescription = "Flame Type\n Requirements:\n Steel Cost: 200 \n Fuel Cost: 0\n Quality: 80";
					PartPreview.Self.flameW.SetActive(true);
					break;
                default:
					break;
            }
        }

        /// <summary>
        /// The on pointer exit function.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        public void OnPointerExit(PointerEventData eventData)
        {
			foreach(GameObject t in PartPreview.Self.exampleParts)
			{
				t.transform.eulerAngles = new Vector3(0, 0, 35);
				t.SetActive(false);
			}
			ToolTip.Self.Objectdescription = " ";
            UIManager.Self.Tooltipobjectpanel.gameObject.SetActive(false);
			PartPreview.Self.gameObject.SetActive(false);
        }
    }
}
