
namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// The instructions text class.
    /// Responsible for storing and displaying the information for the instructions panel.
    /// </summary>
    public class InstructionsText : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// The display text reference.
        /// This is reference to the text that will display the info for the clicked word.
        /// </summary>
        [SerializeField]
        private Text displayText;

        /// <summary>
        /// The on pointer click function.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (InstructionsInformation.Self.InformationDictionary.ContainsKey(this.GetComponent<Text>().text))
            {
                this.displayText.text = InstructionsInformation.Self.InformationDictionary[this.GetComponent<Text>().text];
            }
        }

        /// <summary>
        /// The on pointer enter function.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            Color OpBlue = new Color(108f/255f, 167f/255f, 191f/255f, 1);
            this.GetComponent<Text>().color = OpBlue;
        }

        /// <summary>
        /// The on pointer exit function.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        public void OnPointerExit(PointerEventData eventData)
        {
            this.GetComponent<Text>().color = Color.white;
        }
    }

    /// <summary>
    /// The instructions information class.
    /// Holds reference to the proper information for each word.
    /// </summary>
    public sealed class InstructionsInformation
    {
        /// <summary>
        /// The information dictionary reference.
        /// This will hold the names and descriptions for the text objects.
        /// </summary>
        public Dictionary<string, string> InformationDictionary;

        /// <summary>
        /// The instance reference of the Instructions Information.
        /// </summary>
        private static InstructionsInformation instance;

        /// <summary>
        /// Prevents a default instance of the <see cref="InstructionsInformation"/> class from being created. 
        /// Initializes a new instance of the <see cref="InstructionsInformation"/> class.
        /// </summary>
        private InstructionsInformation()
        {
            InformationDictionary = new Dictionary<string, string>();
            
            InformationDictionary.Add("Camera", "<color>Move</color> with W,A,S,D keys or Up, Left, Down, Right arrows.\n "
                        + "To <color>rotate</color> the camera - Hold left shift and alt then rotate mouse.\n "
                        + "<color>Press T</color> - To reset the position and rotation of the camera. \n "
                        + "<color>Press Y</color> - To toggle panning of the camera by using the mouse at the borders of the screen.");

            InformationDictionary.Add("Unit Selection", "<color>Left click</color> a single unit - This will select the unit. Also, display the units health above their head, momentarily.\n"
                         + "<color>Left click and drag</color> over multiple units to multi select.");

            InformationDictionary.Add("Unit Commands", "<color>Resources</color> - Right-click with selected unit(s) for harvesting.\n"
                         + "<color>Decontamination building</color> - Right-click if unit(s) are holding tainted resources.\n"
                         + "<color>Silo</color> - Right-click and units will stock resources if they are holding any.");

            InformationDictionary.Add("Actions Tab", "<color>Harvest Button</color> - All Units will find closest resource and harvest\n"
                         + "<color>Special Ability</color> - All unit(s) selected will activate their special ability.\n"
                         + "<color>Cancel Action</color> - All unit(s) selected will stop current action and stop moving.\n"
                         + "<color>Call Home</color> - Selects all units and sends them back to the barracks.\n"
                         + "Buttons with tools on them will select all the appropriate unit(s) that hold that tool.");

            InformationDictionary.Add("Crafting Tab", "Clicking a resource button will place it into a slot on the right hand side.\n"
                         + "If the first 2 slots are full then clicking the <color>craft</color> button will craft a new item.\n"
                         + "If an <color>X</color> pops up into the 3 box, then it is not craftable.\n"
                         + "Clicking <color>clear</color> will clear the resources from all the slots.");

            InformationDictionary.Add("Units Tab", "This tab populates with buttons when a unit or units are selected. <color>Hovering</color> over a button\n"
                         + "in this tab will allow the corresponding units selection tile to be changed another color\n"
                         + "and its stats will pop up on the top of the screen. <color>Clicking</color> the button will bring up the\n"
                         + "upgrade screen for upgrading that unit. Click on a <color>+ button</color> to increment the specified stat.\n"
                         + "The stats are only upgradeable at the cost of <color>upgrade points</color>. These are earned from finishing\n"
                         + "objectives on the objectives menu.");

            InformationDictionary.Add("Objectives Tab", "Click the objectives button to have the panel slide in/out of scene view.\n"
                         + "This displays current objectives. <color>The main objective is the ship build objective</color>. This is the win condition.\n"
                         + "The other 2 objectives are just side objectives that the user can finish to earn upgrade points!\n"
                         + "<color>Upgrade Points</color> are earned from finishing\n"
                         + "objectives on the objectives menu.");

            InformationDictionary.Add("Settings Button", "The <color>gear</color> in the top left corner when clicked on will pop up the settings menu. Allowing to open the <color>options menu</color>, <color>quit to main menu</color>, and <color>quit to desktop</color>.");

            InformationDictionary.Add("+1 Buttons", "These buttons will prompt the user with a screen of how many units to buy of the specified type.\n"
                        + "The type is specified by the image next to the button.\n\n"
                        + "<color>Note</color>: When purchasing units, they will spawn in front the barracks in the same position.\n"
                        + "So if you buy more than 1, they will be standing inside each other. They will all fan out\n"
                        + "if you select all of them and give them an order.");

            InformationDictionary.Add("HotKeys", "<color>Hotkeys</color> can now be used and changed.\n"
                        + "When opening the <color>customize window</color>, click on a button you would like to change, once clicked, then\n"
                        + "press a key on the key board. This will register the key as the new hot key. If the image\n"
                        + "on the screen for the button does not change, then that key is not valid. So try again.");

            InformationDictionary.Add("Units", "<color>Units</color> can attack enemies and harvest resources.\n"
                        + "No more than 30 of each type of unit can be on the field at one time.");

            InformationDictionary.Add("Enemies", "<color>Left clicking</color> on an enemy will show the enemy health above their head, momentarily.\n"
                        + "If you <color>attack them from behind</color> they will take more damage.\n"
                        + "They can overwhelm your units one on one.\n"
                        + "They can <color>taint</color> the mineral deposits and trees, yielding tainted resources that need to\n"
                        + "be decontamintated, before stocking.");

            InformationDictionary.Add("Resources", "<color>Harvesters</color> harvester food from the trees.\n"
                        + "<color>Miners</color> mine minerals from the mineral ore veins.\n"
                        + "<color>Extractors</color> extract gas from the geysers.");

            InformationDictionary.Add("How To Build A Ship", "Open the <color>workshop</color> panel to access the menu for ship building.\n"
                        + "There is a tool tip that shows the cost of a part. Once you purchase a part, it will be"
                        + "instantiated and the proper amount of resources will be calculated form the user resource"
                        + "count. Once their is a <color>thruster</color>, <color>wings</color>, and <color>cockpit</color> bought, then clicking <color>build</color> will activate"
                        + "the ship parts pulling together to build the ship. There will be a prompt to the user, saying that"
                        + "the game is won. After 5 seconds, the credit screen will load.");
        }

        /// <summary>
        /// Gets the self reference to the InstructionsInformation object.
        /// </summary>
        public static InstructionsInformation Self
        {
            get
            {
                if (instance == null)
                    instance = new InstructionsInformation();

                return instance;
            }
        }

    }
}
