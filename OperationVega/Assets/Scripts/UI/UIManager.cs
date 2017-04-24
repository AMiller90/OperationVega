using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Button = UnityEngine.UI.Button;
using Text = UnityEngine.UI.Text;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Assets.Scripts.Managers;
using Assets.Scripts;
using Assets.Scripts.Controllers;
using Assets.Scripts.Interfaces;
using System.Linq;

namespace UI
{


    public class UIManager : MonoBehaviour
    {
        #region -- VARIABLES --
       
        private Sprite m_defaultCraftSprite;
        [SerializeField]
        private Button m_NewGame;
        [SerializeField]
        private Button m_LoadGame;
        [SerializeField]
        private Button m_QuitGame;
        [SerializeField]
        private Button m_Instructions;
        [SerializeField]
        private Button m_Options;
        [SerializeField]
        private Button m_Workshop;
        [SerializeField]
        private Button m_Craft;
        [SerializeField]
        private Button m_Clear;
        [SerializeField]
        private Button m_CancelAction;
        [SerializeField]
        private Button m_Minerals;
        [SerializeField]
        private Button m_Food;
        [SerializeField]
        private Button m_CookedFood;
        [SerializeField]
        private Button m_Gas;
        [SerializeField]
        private Button m_Fuel;
        [SerializeField]
        private Canvas m_BackgroundUI;

        [SerializeField]
        private RectTransform m_ActionsTAB;
        public RectTransform ActionsTab
        {
            get { return m_ActionsTAB; }
        }
        [SerializeField]
        private RectTransform m_CraftingTAB;
        public RectTransform CraftingTab
        {
            get { return m_CraftingTAB; }
        }
        [SerializeField]
        private RectTransform m_UnitTAB;
        [SerializeField]
        private RectTransform m_WorkshopUI;
        [SerializeField]
        private RectTransform m_OptionsUI;
        [SerializeField]
        private RectTransform m_ThrusterChoice;
        [SerializeField]
        private RectTransform m_CockpitChoice;
        [SerializeField]
        private RectTransform m_WingChoice;
        [SerializeField]
        private RectTransform m_SettingsUI;
        [SerializeField]
        public RectTransform m_CustomizeUI;
        [SerializeField]
        private RectTransform m_ObjectiveUI;
        [SerializeField]
        private RectTransform m_MainUI;
        [SerializeField]
        private RectTransform m_AreyousureUI;
  
        public RectTransform Tooltipobjectpanel;

        [SerializeField]
        private Text m_MineralsT;
        [SerializeField]
        private Text m_FoodT;
        [SerializeField]
        private Text m_CookedFoodT;
        [SerializeField]
        private Text m_GasT;
        [SerializeField]
        private Text m_FuelT;
        [SerializeField]
        private Text m_SteelT;
        [SerializeField]
        private Text m_KillT;
        [SerializeField]
        private Text m_CraftT;
        [SerializeField]
        private Text m_MainT;
        [SerializeField]
        private Text m_minertext;
        [SerializeField]
        private Text m_extractortext;
        [SerializeField]
        private Text m_harvestertext;
        [HideInInspector]
        public GameObject foodinstance;
        [SerializeField]
        private GameObject cookedfoodPrefab;
        [SerializeField]
        public GameObject upgradepanel;
        [SerializeField]
        public GameObject tooltippanel;
        [SerializeField]
        private GameObject unitbutton;
        [HideInInspector]
        public GameObject abilityunit;

        [SerializeField]
        public Image Input1;
        [SerializeField]
        public Image Input2;
        [SerializeField]
        public Image Output;
        [SerializeField]
        public Image minerals;
        [SerializeField]
        public Image food;
        [SerializeField]
        public Image cookedFood;
        [SerializeField]
        public Image steel;
        [SerializeField]
        public Image fuel;
        [SerializeField]
        public Image gas;
        [SerializeField]
        private Image skillIcon;
        [SerializeField]
        private Image Xbutton;


        private Image defaultInput1;
        private Image defaultInput2;
        private Image defaultOutput;

        [SerializeField]
        private Toggle tooltiptoggle;

        [HideInInspector]
        public float currentcooldown;


        [SerializeField]
        private Transform contentfield;

        private readonly List<GameObject> theUnitButtonsList = new List<GameObject>();

        private static UIManager instance;

        [HideInInspector]
        public GameObject unit;


        public static UIManager Self
        {
            get
            {
                return instance;
            }
        }

        private Button[] statsbuttons;

        private bool objectiveinview;

        private int numbertobuy;

        public InputField inputtext;

        public Button buybutton;

        bool revertactionstab;
        public bool RevertActionsTab
        {
            get { return revertactionstab; }
            set { revertactionstab = value; }
        }

        bool revertcraftingtab;
        public bool RevertCraftingTab
        {
            get { return revertcraftingtab; }
            set { revertcraftingtab = value; }
        }
        bool revertunittab;
        bool input1b;
        bool input2b;
        bool undo1;
        bool undo2;
        bool undo3;
        bool selected;
        private float objectivescale;
        private float Scalefactor;
        public float UIScaleFactor
        {
            get { return Scalefactor; }
            set { UIScaleFactor = Scalefactor; }
        }



        #endregion

        #region -- PROPERTIES --
        public Canvas BackgroundUI
        {
            get { return m_BackgroundUI; }
        }
        #endregion

        protected void Awake()
        {

            //Bool use to manage action tab
            revertactionstab = true;
            //Bool use to manage crafting tab
            revertcraftingtab = true;
            //Bool use to manage unit tab
            revertunittab = true;

            defaultInput1 = Input1;
            defaultInput2 = Input2;
            defaultOutput = Output;
            m_defaultCraftSprite = defaultInput1.sprite;

            input1b = true;
            input2b = true;

            instance = this;

            undo1 = true;
            undo2 = true;
            undo3 = true;

            selected = false;

            m_OptionsUI.GetComponentsInChildren<Text>()[1].text = "Audio Volume: " + AudioManager.ChangedVolume * 10;
            m_OptionsUI.GetComponentsInChildren<Slider>()[1].value = CameraController.MoveSpeed;

            this.statsbuttons = upgradepanel.GetComponentsInChildren<Button>();
            ScaleFactor();

            #region -- Ingame Subscribers --
            EventManager.Subscribe("Mine", this.OnMine);
            EventManager.Subscribe("Extract", this.OnExtract);
            EventManager.Subscribe("UnitTab", this.OnUnit);
            EventManager.Subscribe("OnMChoice", this.OnMChoice);
            EventManager.Subscribe("OnHChoice", this.OnHChoice);
            EventManager.Subscribe("OnEChoice", this.OnEChoice);
            EventManager.Subscribe("Player choose yes", this.OnYes);
            EventManager.Subscribe("Player choose no", this.OnNo);
            #endregion

            #region -- Main Menu Subscribers --
            EventManager.Subscribe("NewGame", this.NewGame);
            EventManager.Subscribe("Options Menu", this.OnOptions);
            EventManager.Subscribe("Instructions", this.OnInstructions);
            EventManager.Subscribe("QuitGame", this.OnQuitGame);
            EventManager.Subscribe("Close Options", this.CloseOptions);
            EventManager.Subscribe("Settings", this.OnSettings);
            EventManager.Subscribe("SettingsClose", this.OnSettingsClose);
            EventManager.Subscribe("Customize", this.OnCustomize);
            EventManager.Subscribe("QuitToMenu", this.OnQuitToMenu);
            EventManager.Subscribe("VolumeSlider", this.OnVolumeSlider);
            EventManager.Subscribe("CameraSpeedSlider", this.OnCameraSpeedSlider);
            EventManager.Subscribe("CustomizeClose", this.OnCustomizeClose);
            EventManager.Subscribe("CustomizeRestore", this.OnCustomRestore);
            EventManager.Subscribe("ObjectiveClick", this.OnObjective);
            #endregion

            EventManager.Publish("CameraSpeedSlider");

            #region --Upgrades--
            EventManager.Subscribe("MaxHealth", this.OnMaxHealth);
            EventManager.Subscribe("Strength", this.OnStrength);
            EventManager.Subscribe("Defense", this.OnDefense);
            EventManager.Subscribe("Speed", this.OnSpeed);
            EventManager.Subscribe("AttackSpeed", this.OnAttackSpeed);
            EventManager.Subscribe("SkillCoolDown", this.OnSkillCoolDown);
            EventManager.Subscribe("AttackRange", this.OnAttackRange);
            EventManager.Subscribe("Close Upgrades", this.OnUpgradeClose);
            
            #endregion



        }

        protected void OnDestroy()
        {
            #region -- Ingame Unsubscribers --
            EventManager.UnSubscribe("Mine", this.OnMine);
            EventManager.UnSubscribe("Extract", this.OnExtract);
            EventManager.UnSubscribe("UnitTab", this.OnUnit);
            EventManager.UnSubscribe("OnMChoice", this.OnMChoice);
            EventManager.UnSubscribe("OnHChoice", this.OnHChoice);
            EventManager.UnSubscribe("OnEChoice", this.OnEChoice);
            EventManager.UnSubscribe("Player choose yes", this.OnYes);
            EventManager.UnSubscribe("Player choose no", this.OnNo);
            #endregion

            #region -- Main Menu Unsubscribers --
            EventManager.UnSubscribe("NewGame", this.NewGame);
            EventManager.UnSubscribe("Options Menu", this.OnOptions);
            EventManager.UnSubscribe("Instructions", this.OnInstructions);
            EventManager.UnSubscribe("QuitGame", this.OnQuitGame);
            EventManager.UnSubscribe("Close Options", this.CloseOptions);
            EventManager.UnSubscribe("Settings", this.OnSettings);
            EventManager.UnSubscribe("SettingsClose", this.OnSettingsClose);
            EventManager.UnSubscribe("Customize", this.OnCustomize);
            EventManager.UnSubscribe("QuitToMenu", this.OnQuitToMenu);
            EventManager.UnSubscribe("CustomizeClose", this.OnCustomizeClose);
            EventManager.UnSubscribe("CustomizeRestore", this.OnCustomRestore);
            EventManager.UnSubscribe("ObjectiveClick", this.OnObjective);
            #endregion

            #region -- Upgrades Unsubscribers --
            EventManager.UnSubscribe("MaxHealth", this.OnMaxHealth);
            EventManager.UnSubscribe("Strength", this.OnStrength);
            EventManager.UnSubscribe("Defense", this.OnDefense);
            EventManager.UnSubscribe("Speed", this.OnSpeed);
            EventManager.UnSubscribe("AttackSpeed", this.OnAttackSpeed);
            EventManager.UnSubscribe("SkillCoolDown", this.OnSkillCoolDown);
            EventManager.UnSubscribe("AttackRange", this.OnAttackRange);
            EventManager.UnSubscribe("Close Upgrades", this.OnUpgradeClose);
            #endregion
        }
        #region -- VOID FUNCTIONS --

        void Update()
        {
            m_KillT.text = ObjectiveManager.Instance.TheObjectives[ObjectiveType.Kill].GetObjectiveInfo();
            m_CraftT.text = ObjectiveManager.Instance.TheObjectives[ObjectiveType.Craft].GetObjectiveInfo();
            m_MainT.text = ObjectiveManager.Instance.TheObjectives[ObjectiveType.Main].GetObjectiveInfo();

            m_minertext.text = User.MinerCount + " / " + User.MaxCountOfUnits;
            m_extractortext.text = User.ExtractorCount + " / " + User.MaxCountOfUnits;
            m_harvestertext.text = User.HarvesterCount + " / " + User.MaxCountOfUnits;

            //Updates the amount of resources the player has.
            m_MineralsT.text = " " + User.MineralsCount;
            m_FoodT.text = " " + User.FoodCount;
            m_CookedFoodT.text = "" + User.CookedFoodCount;
            m_GasT.text = " " + User.GasCount;
            m_FuelT.text = "" + User.FuelCount;
            m_SteelT.text = "" + User.SteelCount;


            if (this.abilityunit != null)
            {
                if (this.currentcooldown < this.abilityunit.GetComponent<Stats>().MaxSkillCooldown)
                {
                    this.currentcooldown += Time.deltaTime;
                    this.skillIcon.fillAmount = this.currentcooldown / this.abilityunit.GetComponent<Stats>().MaxSkillCooldown;
                }
                else
                {
                    this.skillIcon.fillAmount = this.currentcooldown / this.abilityunit.GetComponent<Stats>().MaxSkillCooldown;
                }
            }

            if (this.objectiveinview && this.m_ObjectiveUI.offsetMin.x > 0.1f)
            {
                this.m_ObjectiveUI.offsetMin -= new Vector2(1, 0) * 100 * Time.deltaTime;
                this.m_ObjectiveUI.offsetMax -= new Vector2(1, 0) * 100 * Time.deltaTime;
            }
            else if (!this.objectiveinview && this.m_ObjectiveUI.offsetMin.x < this.objectivescale)
            {
                this.m_ObjectiveUI.offsetMin += new Vector2(1, 0) * 100 * Time.deltaTime;
                this.m_ObjectiveUI.offsetMax += new Vector2(1, 0) * 100 * Time.deltaTime;
            }

        }

        public void SetToolTip()
        {
            ToolTip.Istooltipactive = this.tooltiptoggle.isOn;
        }

        public void OnChangeKeyClicked(GameObject clicked)
        {
            KeyBind.Self.CurrentKey = clicked;
        }

        /// <summary>
        /// The update panel function.
        /// Updates the passed panel with the units information.
        /// <para></para>
        /// <remarks><paramref name="thepanel"></paramref> -The panel to update with the units information.</remarks>
        /// </summary>
        public void UpdateStatsPanel(GameObject thepanel)
        {
            Text[] theUIStats = thepanel.transform.GetComponentsInChildren<Text>();

            Stats unitstats = unit.GetComponent<Stats>();

            // Skip assigning index 0 because it the "Stats" text.
            theUIStats[1].text = "Health: " + unitstats.Health;
            theUIStats[2].text = "MaxHealth: " + unitstats.Maxhealth;
            theUIStats[3].text = "Strength: " + unitstats.Strength;
            theUIStats[4].text = "Defense: " +  unitstats.Defense;
            theUIStats[5].text = "Speed: " + unitstats.Speed;
            theUIStats[6].text = "AttackSpeed: " + unitstats.Attackspeed;
            theUIStats[7].text = "SkillCooldown: " + unitstats.MaxSkillCooldown;
            theUIStats[8].text = "AttackRange: " + unitstats.Attackrange;
            theUIStats[9].text = "ResourceCount: " + unitstats.Resourcecount;

            if (thepanel.name == upgradepanel.name)
            {
                theUIStats[10].text = "Upgrade Points Available: " + User.UpgradePoints;
            }
        }

        void ScaleFactor()
        {
            this.Scalefactor = 0;
            this.objectivescale = 130;

            if (Screen.width == 1280 && Screen.height == 720)
            {
                Scalefactor = -90;
                this.objectivescale = 155;
            }
            else if (Screen.width == 1360 && Screen.height == 768)
            {
                Scalefactor = -95;
                this.objectivescale = 165;
            }
            else if (Screen.width == 1366 && Screen.height == 768)
            {
                Scalefactor = -95;
                this.objectivescale = 165;
            }
            else if (Screen.width == 1600 && Screen.height == 900)
            {
                Scalefactor = -115;
                this.objectivescale = 200;
            }
            else if (Screen.width == 1920 && Screen.height == 1080)
            {
                Scalefactor = -145;
                this.objectivescale = 240;
            }
            else
            {
                Scalefactor = -120;
            }

            if (Scalefactor < 0)
            {
                m_ActionsTAB.offsetMax = new Vector2(m_ActionsTAB.offsetMax.x, Scalefactor);
                m_ActionsTAB.offsetMin = new Vector2(m_ActionsTAB.offsetMin.x, -115);

                m_CraftingTAB.offsetMax = new Vector2(m_CraftingTAB.offsetMax.x, Scalefactor);
                m_CraftingTAB.offsetMin = new Vector2(m_CraftingTAB.offsetMin.x, -115);

                m_UnitTAB.offsetMax = new Vector2(m_UnitTAB.offsetMax.x, Scalefactor);
                m_UnitTAB.offsetMin = new Vector2(m_UnitTAB.offsetMin.x, -115);

                m_ObjectiveUI.offsetMin = new Vector2(this.objectivescale, 0);
                m_ObjectiveUI.offsetMax = new Vector2(this.objectivescale, 0);

            }
        }

        /// <summary>
        /// The create unit button function.
        /// This function populates the panel with the a button for the unit that was
        /// passed in.
        /// <para></para>
        /// <remarks><paramref name="theunit"></paramref> -The unit to pass in so the unit button will have reference to it.</remarks>
        /// </summary>
        public void CreateUnitButton(GameObject theunit)
        {
            GameObject button = Instantiate(this.unitbutton);
            button.transform.SetParent(this.contentfield);

            IUnit u = (IUnit)theunit.GetComponent(typeof(IUnit));

            if (u is Harvester)
            {
                button.GetComponentInChildren<Text>().text = "H";
            }
            else if (u is Miner)
            {
                button.GetComponentInChildren<Text>().text = "M";
            }
            else if (u is Extractor)
            {
                button.GetComponentInChildren<Text>().text = "E";
            }

            button.AddComponent<UnitButton>().Unit = theunit;

            this.theUnitButtonsList.Add(button);
        }

        /// <summary>
        /// The clear unit buttons list function.
        /// This function destroys the buttons populated for a unit and clears the list.
        /// </summary>
        public void ClearUnitButtonsList()
        {
            foreach (GameObject go in this.theUnitButtonsList)
            {
                Destroy(go);
            }
            this.theUnitButtonsList.Clear();
        }

        public void Clicked(Text thetext)
        {
            int.TryParse(thetext.text, out this.numbertobuy);

            if (this.numbertobuy > User.FoodCount / 5 || this.numbertobuy <= 0)
            {
                this.buybutton.interactable = false;
            }
            else
            {
                this.buybutton.interactable = true;
            }
        }

        public void Minus()
        {
            if (this.numbertobuy <= 1)
            {
                this.numbertobuy = User.FoodCount / 5;
                this.inputtext.text = this.numbertobuy.ToString();
            }
            else
            {
                this.numbertobuy--;
                this.inputtext.text = this.numbertobuy.ToString();
            }

            if (this.numbertobuy > User.FoodCount / 5 || this.numbertobuy <= 0)
            {
                this.buybutton.interactable = false;
            }
            else
            {
                this.buybutton.interactable = true;
            }
        }

        public void Plus()
        {
            if (this.numbertobuy >= User.FoodCount / 5 && this.numbertobuy > 0)
            {
                this.numbertobuy = 1;
                this.inputtext.text = this.numbertobuy.ToString();
            }
            else if (User.FoodCount > 5)
            {
                this.numbertobuy++;
                this.inputtext.text = this.numbertobuy.ToString();
            }

            if (this.numbertobuy > User.FoodCount / 5 || this.numbertobuy <= 0)
            {
                this.buybutton.interactable = false;
            }
            else
            {
                this.buybutton.interactable = true;
            }
        }

        public void Buy()
        {
            if (this.numbertobuy <= User.FoodCount / 5 && this.numbertobuy > 0)
            {
                if (UnitController.PurchaseHarvester)
                {
                    UnitController.PurchaseHarvester = false;

                    // Just return can't buy anything
                    if (User.HarvesterCount >= User.MaxCountOfUnits) return;

                    int total = User.HarvesterCount + this.numbertobuy;

                    // If what the user wants to buy will total greater than 30
                    if (total > User.MaxCountOfUnits)
                    {
                        // Subtract the difference of how many are already on the field from the max
                        // this will equal the max number they can buy.
                        this.numbertobuy = User.MaxCountOfUnits - User.HarvesterCount;
                        this.StartCoroutine(
                            UnitController.Self.CombatText(
                                null,
                                Color.white,
                                "Max harvesters on field!"));

                    }
                    for (int i = 0; i < this.numbertobuy; i++)
                    {
                        UnitController.Self.SpawnUnit(UnitController.Self.Harvester);
                    }
                }
                else if (UnitController.PurchaseExtractor)
                {
                    UnitController.PurchaseExtractor = false;

                    // Just return can't buy anything
                    if (User.ExtractorCount >= User.MaxCountOfUnits) return;

                    int total = User.ExtractorCount + this.numbertobuy;

                    // If what the user wants to buy will total greater than 30
                    if (total > User.MaxCountOfUnits)
                    {
                        // Subtract the difference of how many are already on the field from the max
                        // this will equal the max number they can buy.
                        this.numbertobuy = User.MaxCountOfUnits - User.ExtractorCount;
                        this.StartCoroutine(
                            UnitController.Self.CombatText(
                                null,
                                Color.white,
                                "Max extractors on field!"));
                    }

                    for (int i = 0; i < this.numbertobuy; i++)
                    {
                        UnitController.Self.SpawnUnit(UnitController.Self.Extractor);
                    }

                }
                else if (UnitController.PurchaseMiner)
                {
                    UnitController.PurchaseMiner = false;

                    // Just return can't buy anything
                    if (User.MinerCount >= User.MaxCountOfUnits) return;

                    int total = User.MinerCount + this.numbertobuy;

                    // If what the user wants to buy will total greater than 30
                    if (total > User.MaxCountOfUnits)
                    {
                        // Subtract the difference of how many are already on the field from the max
                        // this will equal the max number they can buy.
                        this.numbertobuy = User.MaxCountOfUnits - User.MinerCount;
                        this.StartCoroutine(
                            UnitController.Self.CombatText(
                                null,
                                Color.white,
                                "Max miners on field!"));
                    }

                    for (int i = 0; i < this.numbertobuy; i++)
                    {
                        UnitController.Self.SpawnUnit(UnitController.Self.Miner);
                    }
                }

                User.FoodCount -= this.numbertobuy * 5;
                this.inputtext.text = "0";
                this.numbertobuy = 0;

                // Close the panel
                EventManager.Publish("Player choose no");
            }
        }

        public void OnObjectiveClick()
        {
            EventManager.Publish("ObjectiveClick");

        }
        private void OnObjective()
        {
            this.objectiveinview = !objectiveinview;
        }
        public void OnUnitClick()
        {
            EventManager.Publish("UnitTab");
        }
        private void OnUnit()
        {
            if(revertunittab)
            {
                m_UnitTAB.offsetMax = new Vector2(m_UnitTAB.offsetMax.x, 0);
                m_UnitTAB.offsetMin = new Vector2(m_UnitTAB.offsetMin.x, 0);

                revertunittab = false;
            }
            else if(!revertunittab)
            {
                revertunittab = true;

                m_UnitTAB.offsetMax = new Vector2(m_UnitTAB.offsetMax.x, Scalefactor);
                m_UnitTAB.offsetMin = new Vector2(m_UnitTAB.offsetMin.x, -115);
            }

            Debug.Log("Move Unit Tab down");
        }
             
        public void OnMineClick()
        {
            EventManager.Publish("Mine");
        }
        private void OnMine()
        {
            //This function will prompt the mining function
            Debug.Log("Begin Mining");
        }
        public void OnExtractClick()
        {
            EventManager.Publish("Extract");
        }
        private void OnExtract()
        {
            Debug.Log("Begin Extracting");
        }
        #region -- Main Menu Functions --
        public void OnOptionsClick()
        {
            EventManager.Publish("Options Menu");
        }
        private void OnOptions()
        {
            m_OptionsUI.GetComponentsInChildren<Slider>()[0].value = AudioManager.Self.Volume * 10;
            m_OptionsUI.gameObject.SetActive(true);
            m_SettingsUI.gameObject.SetActive(false);
            Debug.Log("Options Menu");
        }
        public void CloseOptionsClick()
        {
            EventManager.Publish("Close Options");
        }

        private void CloseOptions()
        {
            //Sets the options panel to false when the back button is clicked.
            m_OptionsUI.gameObject.SetActive(false);
            m_SettingsUI.gameObject.SetActive(true);
            Debug.Log("Close Options");
        }

        public void NewGameClick()
        {
            EventManager.Publish("NewGame");
        }
        private void NewGame()
        {
            SceneManager.LoadScene(1);
            GameManager.Instance.SetUpNewGame();
            //Function will begin game from main menu
            Debug.Log("New Game");
        }

        public void OnInstructionsClick()
        {
            EventManager.Publish("Instructions");
        }

        private void OnInstructions()
        {
            //Function will bring up the instructions.
            Debug.Log("Instructions");
        }
        public void OnSettingsClick()
        {
            EventManager.Publish("Settings");
        }
        private void OnSettings()
        {
            m_SettingsUI.gameObject.SetActive(true);
            Debug.Log("Settings Menu");
        }
        public void OnSettingsCloseClick()
        {
            EventManager.Publish("SettingsClose");
        }
        private void OnSettingsClose()
        {
            m_SettingsUI.gameObject.SetActive(false);
            Debug.Log("Settings Close");
        }
        public void OnQuitToMenuClick()
        {
            EventManager.Publish("QuitToMenu");
        }
        private void OnQuitToMenu()
        {
            SceneManager.LoadScene(0);
            Debug.Log("Quit to Menu");
        }
        
        public void OnVolumeSliderClick()
        {
            EventManager.Publish("VolumeSlider");
        }
        private void OnVolumeSlider()
        {
            AudioManager.Self.Volume = m_OptionsUI.GetComponentsInChildren<Slider>()[0].value / 10;
            AudioManager.ChangedVolume = AudioManager.Self.Volume;
            m_OptionsUI.GetComponentsInChildren<Text>()[1].text = "Audio Volume: " + AudioManager.ChangedVolume * 10;

            m_OptionsUI.GetComponentsInChildren<Text>()[2].text = "Audio Volume";
            Debug.Log("Volume Slider");
        }

        public void OnCameraSpeedSliderClick()
        {
            EventManager.Publish("CameraSpeedSlider");
        }            
        private void OnCameraSpeedSlider()
        {
            CameraController.MoveSpeed = (uint)m_OptionsUI.GetComponentsInChildren<Slider>()[1].value;
            m_OptionsUI.GetComponentsInChildren<Text>()[2].text = "Camera Speed: " + CameraController.MoveSpeed;
            Debug.Log("CameraSpeed Slider");
        }
        public void OnCustomizeClick()
        {
            EventManager.Publish("Customize");
        }
        private void OnCustomize()
        {
            //Used to set all the UI to inactive when the customize menu is open.
            m_CustomizeUI.gameObject.SetActive(true);
            m_OptionsUI.gameObject.SetActive(false);
            m_MainUI.gameObject.SetActive(false);
            m_ActionsTAB.gameObject.SetActive(false);
            m_CraftingTAB.gameObject.SetActive(false);
            m_Workshop.gameObject.SetActive(false);
            m_ObjectiveUI.gameObject.SetActive(false);
            
            Debug.Log("Customize Menu");
        }
        public void OnCustomRestoreClick()
        {
            EventManager.Publish("CustomizeRestore");

        }
        private void OnCustomRestore()
        {
            KeyBind.Self.RestoreToDefault(m_CustomizeUI.GetComponentsInChildren<Button>());
        }
        public void OnCustomizeCloseClick()
        {
            EventManager.Publish("CustomizeClose");
        }
     
        private void OnCustomizeClose()
        {
            //Used to set all UI to active when the customize menu is open.
            m_CustomizeUI.gameObject.SetActive(false);
            m_OptionsUI.gameObject.SetActive(true);
            m_MainUI.gameObject.SetActive(true);
            m_ActionsTAB.gameObject.SetActive(true);
            m_CraftingTAB.gameObject.SetActive(true);
            m_Workshop.gameObject.SetActive(true);
            m_ObjectiveUI.gameObject.SetActive(true);
            Debug.Log("Customize closed");
        }
        public void OnMChoiceClick()
        {
            EventManager.Publish("OnMChoice");
        }
        private void OnMChoice()
        {
            List<Miner> miners = GameObject.FindObjectsOfType<Miner>().ToList();

            if (miners.Count >= 30) return;
            //Spawns Miner Upon clicking Yes/No
            m_AreyousureUI.gameObject.SetActive(true);
            UnitController.PurchaseMiner = true;
            Debug.Log("Miners Choice");
        }
        public void OnHChoiceClick()
        {
            EventManager.Publish("OnHChoice");
        }
        private void OnHChoice()
        {
            List<Harvester> harvesters = GameObject.FindObjectsOfType<Harvester>().ToList();

            if (harvesters.Count >= 30) return;
            //Spawns Harvester Upon clicking Yes/No
            m_AreyousureUI.gameObject.SetActive(true);
            UnitController.PurchaseHarvester = true;
            Debug.Log("Harvester Choice");
        }
        public void OnEChoiceClick()
        {
            EventManager.Publish("OnEChoice");
        }
        private void OnEChoice()
        {
            List<Extractor> extractors = GameObject.FindObjectsOfType<Extractor>().ToList();

            if (extractors.Count >= 30) return;
            //Spawns Extracter Upon clicking Yes/No
            m_AreyousureUI.gameObject.SetActive(true);
            UnitController.PurchaseExtractor = true;
            Debug.Log("Extractor Choice");
        }
        public void OnYesClick()
        {
            EventManager.Publish("Player choose yes");
        }
        private void OnYes()
        {
            Debug.Log("User choose Yes");
            m_AreyousureUI.gameObject.SetActive(false);
        }
        public void OnNoClick()
        {
            EventManager.Publish("Player choose no");
        }
        private void OnNo()
        {
            Debug.Log("User choose No");
            m_AreyousureUI.gameObject.SetActive(false);
        }

        public void OnQuitGameClick()
        {
            EventManager.Publish("QuitGame");
        }
        private void OnQuitGame()
        {
            Application.Quit();
            //Function will quit game upon click.
            Debug.Log("Quit Game");
        }
        #endregion
        #endregion


        #region -- Upgrades --
        public void OnMaxHealthClick()
        {
            EventManager.Publish("MaxHealth");
        }
        private void OnMaxHealth()
        {
            //Updates the MaxHealth when the button is clicked.
            Stats unitstats = unit.GetComponent<Stats>();

            if (User.UpgradePoints < 1) return;

            unitstats.Maxhealth += 20;
            User.UpgradePoints--;
            if (unitstats.Maxhealth >= 500)
            {
                this.statsbuttons[1].gameObject.SetActive(false);
            }
            //Updates the health within the upgrade panel
            Text[] theUIStats = this.upgradepanel.transform.GetComponentsInChildren<Text>();
            theUIStats[1].text = "Health: " + unitstats.Health;
            theUIStats[9].text = "ResourceCount: " + unitstats.Resourcecount;
            theUIStats[2].text = "MaxHealth: " + unitstats.Maxhealth;
            theUIStats[10].text = "Upgrade Points Available: " + User.UpgradePoints;

            Debug.Log("Increases Max Health");
        }
        public void OnStrengthClick()
        {
            EventManager.Publish("Strength");
        }
        private void OnStrength()
        {
            //Updates the Strength when the button is clicked.
            Stats unitstats = unit.GetComponent<Stats>();

            if (User.UpgradePoints < 1) return;

            unitstats.Strength += 2;
            User.UpgradePoints--;
            if (unitstats.Strength >= 100)
            {
                this.statsbuttons[2].gameObject.SetActive(false);
            }
            //Updates the Strength within the upgrade panel
            Text[] theUIStats = this.upgradepanel.transform.GetComponentsInChildren<Text>();
            theUIStats[1].text = "Health: " + unitstats.Health;
            theUIStats[9].text = "ResourceCount: " + unitstats.Resourcecount;
            theUIStats[3].text = "Strength: " + unitstats.Strength;
            theUIStats[10].text = "Upgrade Points Available: " + User.UpgradePoints;
            Debug.Log("Increases Strength");
  
        }
        public void OnDefenseClick()
        {
            EventManager.Publish("Defense");
        }
               
        private void OnDefense()
        {
            //Updates the Defense when the button is clicked.
            Stats unitstats = unit.GetComponent<Stats>();

            if (User.UpgradePoints < 1) return;

            unitstats.Defense += 2;
            User.UpgradePoints--;
            if (unitstats.Defense >= 100)
            {
                this.statsbuttons[3].gameObject.SetActive(false);
            }
            //Updates the Defense within the upgrade panel
            Text[] theUIStats = this.upgradepanel.transform.GetComponentsInChildren<Text>();
            theUIStats[1].text = "Health: " + unitstats.Health;
            theUIStats[9].text = "ResourceCount: " + unitstats.Resourcecount;
            theUIStats[4].text = "Defense: " + unitstats.Defense;
            theUIStats[10].text = "Upgrade Points Available: " + User.UpgradePoints;
            Debug.Log("Upgrade Defense");
        }
        public void OnSpeedClick()
        {
            EventManager.Publish("Speed");
        }
        private void OnSpeed()
        {
            //Updates the Speed when the button is clicked.
            Stats unitstats = unit.GetComponent<Stats>();

            if (User.UpgradePoints < 2) return;
            unitstats.Speed++;
            unit.GetComponent<NavMeshAgent>().speed = unitstats.Speed;
            User.UpgradePoints -= 2;
            if (unitstats.Speed >= 7)
            {
                this.statsbuttons[4].gameObject.SetActive(false);
            }
            //Updates the Speed within the upgrade panel
            Text[] theUIStats = this.upgradepanel.transform.GetComponentsInChildren<Text>();
            theUIStats[1].text = "Health: " + unitstats.Health;
            theUIStats[9].text = "ResourceCount: " + unitstats.Resourcecount;
            theUIStats[5].text = "Speed: " + unitstats.Speed;
            theUIStats[10].text = "Upgrade Points Available: " + User.UpgradePoints;
            Debug.Log("Upgrade Speed");
        }
        public void OnAttackSpeedClick()
        {
            EventManager.Publish("AttackSpeed");
        }
        private void OnAttackSpeed()
        {
            //Updates the AttackSpeed when the button is clicked.
            Stats unitstats = unit.GetComponent<Stats>();

            if (User.UpgradePoints < 4) return;
            unitstats.Attackspeed--;
            User.UpgradePoints -= 4;
            if (unitstats.Attackspeed <= 1)
            {
                this.statsbuttons[5].gameObject.SetActive(false);
            }
            //Updates the AttackSpeed in the upgrade panel
            Text[] theUIStats = this.upgradepanel.transform.GetComponentsInChildren<Text>();
            theUIStats[1].text = "Health: " + unitstats.Health;
            theUIStats[9].text = "ResourceCount: " + unitstats.Resourcecount;
            theUIStats[6].text = "AttackSpeed: " + unitstats.Attackspeed;
            theUIStats[10].text = "Upgrade Points Available: " + User.UpgradePoints;
            Debug.Log("Upgrade Attack Speed");
        }
        
        public void OnSkillCoolDownClick()
        {
            EventManager.Publish("SkillCoolDown");
        }
        private void OnSkillCoolDown()
        {
            //Updates the SkkillCoolDowns when the button is clicked.
            Stats unitstats = unit.GetComponent<Stats>();

            unitstats.MaxSkillCooldown--;
            User.UpgradePoints--;
            if (unitstats.MaxSkillCooldown <= 10)
            {
                this.statsbuttons[6].gameObject.SetActive(false);
            }
            //Updates the SkillCoolDown in the upgrade panel
            Text[] theUIStats = this.upgradepanel.transform.GetComponentsInChildren<Text>();
            theUIStats[1].text = "Health: " + unitstats.Health;
            theUIStats[9].text = "ResourceCount: " + unitstats.Resourcecount;
            theUIStats[7].text = "SkillCooldown: " + unitstats.MaxSkillCooldown;
            theUIStats[10].text = "Upgrade Points Available: " + User.UpgradePoints;
            Debug.Log("Upgrade SkillCooldown");
        }

        public void OnAttackRangeClick()
        {
            EventManager.Publish("AttackRange");
        }
        private void OnAttackRange()
        {
            //Updates the AttackRange when clicked.
            Stats unitstats = unit.GetComponent<Stats>();

            if (User.UpgradePoints < 4) return;
            unitstats.Attackrange++;
            User.UpgradePoints -= 4;
            if (unitstats.Attackrange >= 10.0f)
            {
                statsbuttons[7].gameObject.SetActive(false);
            }
            Text[] theUIStats = this.upgradepanel.transform.GetComponentsInChildren<Text>();
            theUIStats[1].text = "Health: " + unitstats.Health;
            theUIStats[9].text = "ResourceCount: " + unitstats.Resourcecount;
            theUIStats[8].text = "AttackRange: " + unitstats.Attackrange;
            theUIStats[10].text = "Upgrade Points Available: " + User.UpgradePoints;
            Debug.Log("Upgrade Defense");
        }

        public void OnUpgradeCloseClick()
        {
            EventManager.Publish("Close Upgrades");

        }
        private void OnUpgradeClose()
        {
            upgradepanel.gameObject.SetActive(false);
        }
       
        #endregion

    }
}