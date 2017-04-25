
namespace Assets.Scripts
{
    using System.Collections.Generic;
    using Managers;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;


    /// <summary>
    /// The key bind class.
    /// Handles the functionality of each bound key.
    /// </summary>
    public class KeyBind : MonoBehaviour
    {
        /// <summary>
        /// The timer reference.
        /// This will help stop the button from firing an event during the key change process.
        /// </summary>
        [HideInInspector]
        public float Timer;

        /// <summary>
        /// The current key to change.
        /// </summary>
        [HideInInspector]
        public GameObject CurrentKey;

        /// <summary>
        /// The customize ui.
        /// Reference to the customize ui to have access to the buttons.
        /// </summary>
        [SerializeField]
        private RectTransform customizeUi;

        /// <summary>
        /// The instance reference.
        /// </summary>
        private static KeyBind instance;

        /// <summary>
        /// The key bind dictionary reference.
        /// The dictionary of bound keys.
        /// </summary>
        private readonly Dictionary<string, KeyCode> thekeybinddictionary = new Dictionary<string, KeyCode>();

        /// <summary>
        /// The sprite dictionary reference.
        /// Holds reference to all the sprites used for the keys.
        /// </summary>
        private readonly Dictionary<KeyCode, Sprite> spritedictionary = new Dictionary<KeyCode, Sprite>();

        /// <summary>
        /// The possible keys reference.
        /// Reference to all the possible keys that register on press.
        /// </summary>
        private readonly List<KeyCode> possiblekeys = new List<KeyCode>();

        /// <summary>
        /// The key bind mapping reference.
        /// Reference to the events that are used with a specific key.
        /// </summary>
        private readonly Dictionary<string, UnityAction> keybindmapping = new Dictionary<string, UnityAction>();

        /// <summary>
        /// The hot keys reference.
        /// The Serialized hot keys.
        /// </summary>
        [SerializeField]
        private HotKeySerializables hotkeys = new HotKeySerializables();

        /// <summary>
        /// The button images reference.
        /// Holds reference in the hierarchy for all sprites.
        /// </summary>
        [SerializeField]
        private List<Sprite> buttonimages = new List<Sprite>();

        /// <summary>
        /// The hot keys config reference.
        /// Reference to the representation of the serialized keys as a string.
        /// </summary>
        private string hotkeysconfig;

        /// <summary>
        /// Gets the instance of the KeyBind object.
        /// </summary>
        public static KeyBind Self
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// The hotkey change function.
        /// Changes the hotkey accordingly.
        /// </summary>
        public void HotkeyChange()
        {
            if (this.CurrentKey != null)
            {
                Event e = Event.current;

                // Check if its a key and if its a mappable key
                if (e.isKey && this.possiblekeys.Contains(e.keyCode))
                {
                    // Get the key
                    string kb_key = this.CurrentKey.GetComponentInChildren<Text>().text;

                    // Map key code to the key
                    this.thekeybinddictionary[kb_key] = e.keyCode;

                    this.hotkeys = new HotKeySerializables();

                    foreach (var val in this.thekeybinddictionary)
                    {
                        this.MakeHotKey(val.Key, val.Value.ToString());
                    }

                    this.SaveHotkeys();

                    // Set sprite here
                    this.CurrentKey.GetComponent<Image>().sprite = this.spritedictionary[e.keyCode];
                    this.CurrentKey = null;

                    // Reset timer
                    this.Timer = 0.0f;
                }
            }
        }

        /// <summary>
        /// The restore to default function.
        /// Restores the keys back to default along with the appropriate sprite.
        /// <para></para>
        /// <remarks><paramref name="hotkeybuttons"></paramref> -The array of buttons to parse through and restore.</remarks>
        /// </summary>
        public void RestoreToDefault(Button[] hotkeybuttons)
        {
            this.thekeybinddictionary["Settings"] = KeyCode.Escape;
            this.thekeybinddictionary["Objectives"] = KeyCode.O;
            this.thekeybinddictionary["Crafting"] = KeyCode.C;
            this.thekeybinddictionary["Actions"] = KeyCode.Tab;
            this.thekeybinddictionary["Units"] = KeyCode.U;
            this.thekeybinddictionary["Workshop"] = KeyCode.M;
            this.thekeybinddictionary["Ability"] = KeyCode.Space;
            this.thekeybinddictionary["Home"] = KeyCode.H;
            this.thekeybinddictionary["Cancel"] = KeyCode.X;
            this.thekeybinddictionary["SAExtractors"] = KeyCode.F1;
            this.thekeybinddictionary["SAMiners"] = KeyCode.F2;
            this.thekeybinddictionary["SAHarvesters"] = KeyCode.F3;
            this.thekeybinddictionary["SAUnits"] = KeyCode.F4;
            this.thekeybinddictionary["BuyExtractor"] = KeyCode.Alpha1;
            this.thekeybinddictionary["BuyMiner"] = KeyCode.Alpha2;
            this.thekeybinddictionary["BuyHarvester"] = KeyCode.Alpha3;
            this.thekeybinddictionary["Controls"] = KeyCode.Slash;
            this.thekeybinddictionary["Instructions"] = KeyCode.F5;

            // There are 20 buttons but only the first 18 can be modified.
            // This excludes the x button and restore to default button.
            for (int i = 0; i < 18; i++)
            {
                string thetext = hotkeybuttons[i].GetComponentInChildren<Text>().text;
                hotkeybuttons[i].GetComponent<Image>().sprite = this.spritedictionary[this.thekeybinddictionary[thetext]];
            }
        }

        /// <summary>
        /// The start function.
        /// </summary>
        private void Start()
        {
            instance = this;

            if (!System.IO.Directory.Exists(@"..\KeyBindings"))
            {
                System.IO.Directory.CreateDirectory(@"..\KeyBindings");
            }

            this.SetUpPossibleKeys();
            this.SetUpSprites();
            this.SetUpDefaultKeys();

            this.Timer = 1.0f;
        }

        /// <summary>
        /// The update function.
        /// </summary>
        private void Update()
        {
            if (this.Timer > 1.0f)
            {
                this.PerformHotKeyAction();
            }

            this.Timer += 1 * Time.deltaTime;
        }
        
        /// <summary>
        /// The perform hot key action function.
        /// Performs the appropriate action for the key pressed.
        /// </summary>
        private void PerformHotKeyAction()
        {
            foreach (var val in this.keybindmapping)
            {
                if (Input.GetKeyDown(this.thekeybinddictionary[val.Key]))
                {
                    val.Value.Invoke();
                }
            }
        }

        /// <summary>
        /// The set up default keys function.
        /// Sets up the default keys.
        /// </summary>
        private void SetUpDefaultKeys()
        {
            // The default keys and values
            this.thekeybinddictionary.Add("Settings", KeyCode.Escape);
            this.thekeybinddictionary.Add("Objectives", KeyCode.O);
            this.thekeybinddictionary.Add("Crafting", KeyCode.C);
            this.thekeybinddictionary.Add("Actions", KeyCode.Tab);
            this.thekeybinddictionary.Add("Units", KeyCode.U);
            this.thekeybinddictionary.Add("Workshop", KeyCode.M);
            this.thekeybinddictionary.Add("Ability", KeyCode.Space);
            this.thekeybinddictionary.Add("Home", KeyCode.H);
            this.thekeybinddictionary.Add("Cancel", KeyCode.X);
            this.thekeybinddictionary.Add("SAExtractors", KeyCode.F1);
            this.thekeybinddictionary.Add("SAMiners", KeyCode.F2);
            this.thekeybinddictionary.Add("SAHarvesters", KeyCode.F3);
            this.thekeybinddictionary.Add("SAUnits", KeyCode.F4);
            this.thekeybinddictionary.Add("BuyExtractor", KeyCode.Alpha1);
            this.thekeybinddictionary.Add("BuyMiner", KeyCode.Alpha2);
            this.thekeybinddictionary.Add("BuyHarvester", KeyCode.Alpha3);
            this.thekeybinddictionary.Add("Controls", KeyCode.Slash);
            this.thekeybinddictionary.Add("Instructions", KeyCode.F5);

            this.keybindmapping.Add("Settings", delegate { EventManager.Publish("Settings"); });
            this.keybindmapping.Add("Objectives", delegate { EventManager.Publish("ObjectiveClick"); });
            this.keybindmapping.Add("Crafting", delegate { EventManager.Publish("Crafting"); });
            this.keybindmapping.Add("Actions", delegate { EventManager.Publish("Actions"); });
            this.keybindmapping.Add("Units", delegate { EventManager.Publish("UnitTab"); });
            this.keybindmapping.Add("Workshop", delegate { EventManager.Publish("Workshop"); });
            this.keybindmapping.Add("Ability", delegate { EventManager.Publish("ActivateAbility"); });
            this.keybindmapping.Add("Home", delegate { EventManager.Publish("Recall"); });
            this.keybindmapping.Add("Cancel", delegate { EventManager.Publish("CancelAction"); });
            this.keybindmapping.Add("SAExtractors", delegate { EventManager.Publish("SAExtract"); });
            this.keybindmapping.Add("SAMiners", delegate { EventManager.Publish("SAMiner"); });
            this.keybindmapping.Add("SAHarvesters", delegate { EventManager.Publish("SAHarvest"); });
            this.keybindmapping.Add("SAUnits", delegate { EventManager.Publish("SAUnit"); });
            this.keybindmapping.Add("BuyExtractor", delegate { EventManager.Publish("OnEChoice"); });
            this.keybindmapping.Add("BuyMiner", delegate { EventManager.Publish("OnMChoice"); });
            this.keybindmapping.Add("BuyHarvester", delegate { EventManager.Publish("OnHChoice"); });
            this.keybindmapping.Add("Controls", delegate { EventManager.Publish("Customize"); });
            this.keybindmapping.Add("Instructions", delegate { EventManager.Publish("Instructions"); });

            // Load the hotkeys - If they can't be loaded then make the keys
            if (!this.LoadHotkeys())
            {
                foreach (var val in this.thekeybinddictionary)
                {
                    this.MakeHotKey(val.Key, val.Value.ToString());
                }
            }
        }

        /// <summary>
        /// The set up possible keys function.
        /// Adds the keys to the list that can be mapped.
        /// </summary>
        private void SetUpPossibleKeys()
        {
            // Possible keys for mapping - center keyboard
            this.possiblekeys.Add(KeyCode.Alpha0);
            this.possiblekeys.Add(KeyCode.Alpha1);
            this.possiblekeys.Add(KeyCode.Alpha2);
            this.possiblekeys.Add(KeyCode.Alpha3);
            this.possiblekeys.Add(KeyCode.Alpha4);
            this.possiblekeys.Add(KeyCode.Alpha5);
            this.possiblekeys.Add(KeyCode.Alpha6);
            this.possiblekeys.Add(KeyCode.Alpha7);
            this.possiblekeys.Add(KeyCode.Alpha8);
            this.possiblekeys.Add(KeyCode.Alpha9);
            this.possiblekeys.Add(KeyCode.F1);
            this.possiblekeys.Add(KeyCode.F2);
            this.possiblekeys.Add(KeyCode.F3);
            this.possiblekeys.Add(KeyCode.F4);
            this.possiblekeys.Add(KeyCode.F5);
            this.possiblekeys.Add(KeyCode.F6);
            this.possiblekeys.Add(KeyCode.F7);
            this.possiblekeys.Add(KeyCode.F8);
            this.possiblekeys.Add(KeyCode.B);
            this.possiblekeys.Add(KeyCode.C);
            this.possiblekeys.Add(KeyCode.E);
            this.possiblekeys.Add(KeyCode.F);
            this.possiblekeys.Add(KeyCode.G);
            this.possiblekeys.Add(KeyCode.H);
            this.possiblekeys.Add(KeyCode.I);
            this.possiblekeys.Add(KeyCode.J);
            this.possiblekeys.Add(KeyCode.K);
            this.possiblekeys.Add(KeyCode.L);
            this.possiblekeys.Add(KeyCode.M);
            this.possiblekeys.Add(KeyCode.N);
            this.possiblekeys.Add(KeyCode.O);
            this.possiblekeys.Add(KeyCode.P);
            this.possiblekeys.Add(KeyCode.Q);
            this.possiblekeys.Add(KeyCode.R);
            this.possiblekeys.Add(KeyCode.U);
            this.possiblekeys.Add(KeyCode.V);
            this.possiblekeys.Add(KeyCode.X);
            this.possiblekeys.Add(KeyCode.Z);
            this.possiblekeys.Add(KeyCode.Return);
            this.possiblekeys.Add(KeyCode.Escape);
            this.possiblekeys.Add(KeyCode.Slash);
            this.possiblekeys.Add(KeyCode.Space);
            this.possiblekeys.Add(KeyCode.Tab);

            // Possible keys for mapping - right keypad
            this.possiblekeys.Add(KeyCode.Keypad0);
            this.possiblekeys.Add(KeyCode.Keypad1);
            this.possiblekeys.Add(KeyCode.Keypad2);
            this.possiblekeys.Add(KeyCode.Keypad3);
            this.possiblekeys.Add(KeyCode.Keypad4);
            this.possiblekeys.Add(KeyCode.Keypad5);
            this.possiblekeys.Add(KeyCode.Keypad6);
            this.possiblekeys.Add(KeyCode.Keypad7);
            this.possiblekeys.Add(KeyCode.Keypad8);
            this.possiblekeys.Add(KeyCode.Keypad9);
            this.possiblekeys.Add(KeyCode.KeypadDivide);
        }

        /// <summary>
        /// The set up sprites function.
        /// Sets up sprites in the dictionary to match the correct key.
        /// </summary>
        private void SetUpSprites()
        {
            int startingindex = 0;

            // I have the sprites set in the inspector at the appropriate index
            // to match the appropriate key
            for (int i = 0; i < 53; i++)
            {
                // Set Center Keyboard Sprites
                if (i < 43)
                {
                    startingindex = i;
                }
                else if (i == 43)
                {  // Set the Right Keypad Sprites
                    startingindex = 0;
                }

                this.spritedictionary.Add(this.possiblekeys[i], this.buttonimages[startingindex]);
                startingindex++;
            }

            // The slash key sprite is the same for the right keypad slash
            this.spritedictionary.Add(KeyCode.KeypadDivide, this.buttonimages[40]);
        }

        /// <summary>
        /// The make hot key function.
        /// Makes a hot key to be serialized.
        /// <para></para>
        /// <remarks><paramref name="hk"></paramref> -The name of the key to serialize.</remarks>
        /// <para></para>
        /// <remarks><paramref name="m"></paramref> -The message to serialize.</remarks>
        /// </summary>
        private void MakeHotKey(string hk, string m)
        {
            if (this.hotkeys.Hotkeys == null)
            {
                this.hotkeys.Hotkeys = new List<HotKeySerializable>();
            }

            var hotkey = new HotKeySerializable();
            hotkey.Hotkey = hk;
            hotkey.Message = m;

            this.hotkeys.Hotkeys.Add(hotkey);
        }

        /// <summary>
        /// The save hotkeys function.
        /// Saves hotkeys to json file.
        /// </summary>
        private void SaveHotkeys()
        {
            if (this.hotkeys != null)
            {
                this.hotkeysconfig = JsonUtility.ToJson(this.hotkeys, true);
                System.IO.File.WriteAllText(@"..\KeyBindings\HotKeys.json", this.hotkeysconfig);
            }
        }

        /// <summary>
        /// The load hotkeys function.
        /// Loads hot keys from the saved file.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool LoadHotkeys()
        {
            // The folder exists and the file exists then load the keys
            if (System.IO.Directory.Exists(@"..\KeyBindings") && System.IO.File.Exists(@"..\KeyBindings\HotKeys.json"))
            {
                this.hotkeysconfig = System.IO.File.ReadAllText(@"..\KeyBindings\HotKeys.json");

                this.hotkeys = JsonUtility.FromJson(this.hotkeysconfig, typeof(HotKeySerializables)) as HotKeySerializables;

                if (this.hotkeys != null)
                {
                    Button[] keybuttons = this.customizeUi.GetComponentsInChildren<Button>();

                    for (int i = 0; i < this.hotkeys.Hotkeys.Count; i++)
                    {
                        // Convert the message string into its keycode form.
                        KeyCode keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), this.hotkeys.Hotkeys[i].Message);

                        this.thekeybinddictionary[this.hotkeys.Hotkeys[i].Hotkey] = keycode;
                        keybuttons[i].GetComponent<Image>().sprite = this.spritedictionary[keycode];
                    }

                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// The hot key serializable class.
    /// Reference to seralize hotkeys.
    /// </summary>
    [System.Serializable]
    public class HotKeySerializables
    {
        /// <summary>
        /// The hotkeys reference.
        /// </summary>
        [SerializeField]
        public List<HotKeySerializable> Hotkeys;
    }

    /// <summary>
    /// The hot key serializable class.
    /// Reference to a serlizable class for serilaizing hot keys.
    /// </summary>
    [System.Serializable]
    public class HotKeySerializable
    {
        /// <summary>
        /// The hot key reference.
        /// Name of the hot key.
        /// </summary>
        [SerializeField]
        public string Hotkey;

        /// <summary>
        /// The message reference.
        /// Message to broadcast for event to be fired.
        /// </summary>
        [SerializeField]
        public string Message;
    }

}

