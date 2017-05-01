
namespace Assets.Scripts.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    using Managers;

    using UI;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// The unit controller class.
    /// This class handles functionality of the units click movement and actions.
    /// </summary>
    public class UnitController : MonoBehaviour
    {
        public LayerMask allbutui;
        /// <summary>
        /// The purchase harvester reference.
        /// Determines if the purchase harvester button was clicked.
        /// </summary>
        public static bool PurchaseHarvester;

        /// <summary>
        /// The purchase miner reference.
        /// Determines if the purchase miner button was clicked.
        /// </summary>
        public static bool PurchaseMiner;

        /// <summary>
        /// The purchase extractor reference.
        /// Determines if the purchase extractor button was clicked.
        /// </summary>
        public static bool PurchaseExtractor;

        /// <summary>
        /// The Harvester reference.
        /// Reference to a Harvester prefab.
        /// </summary>
        public GameObject Harvester;

        /// <summary>
        /// The Miner reference.
        /// Reference to a Miner prefab.
        /// </summary>
        public GameObject Miner;

        /// <summary>
        /// The Extractor reference.
        /// Reference to a Extractor prefab.
        /// </summary>
        public GameObject Extractor;

        [HideInInspector]
        public GameObject Unithit;

        [HideInInspector]
        public GameObject RagDoll
        {
            set
            {
                this.StartCoroutine(this.DestroyRagDoll(value));
            }
        }

        /// <summary>
        /// The instance of the class.
        /// </summary>
        private static UnitController instance;

        /// <summary>
        /// The drag screen.
        /// </summary>
        private static Rect dragscreen = new Rect(0, 0, 0, 0);

        /// <summary>
        /// The list of units selected by the drag screen.
        /// </summary>
        private readonly List<GameObject> units = new List<GameObject>();

        /// <summary>
        /// The text objects reference.
        /// Reference to store text objects when damage is to be displayed.
        /// </summary>
        private readonly Queue<GameObject> textobjs = new Queue<GameObject>();

        /// <summary>
        /// The trees list.
        /// Reference to every tree in the scene.
        /// </summary>
        private List<GameObject> trees = new List<GameObject>();

        /// <summary>
        /// The mineral deposits list.
        /// Reference to every mineral deposit in the scene.
        /// </summary>
        private List<GameObject> mineraldeposits = new List<GameObject>();

        /// <summary>
        /// The geysers list.
        /// Reference to every geyser in the scene.
        /// </summary>
        private List<GameObject> geysers = new List<GameObject>();

        /// <summary>
        /// The combat text reference.
        /// Reference to the combat text prefab.
        /// </summary>
        [SerializeField]
        private GameObject combattext;

        /// <summary>
        /// The click destination of where to send the unit.
        /// </summary>
        private Vector3 clickdestination;

        /// <summary>
        /// The selection highlight is the texture.
        /// </summary>
        [SerializeField]
        private Texture2D selectionHighlight;

        /// <summary>
        /// The start click reference.
        /// </summary>
        private Vector3 startclick = -Vector3.one;

        /// <summary>
        /// The selected object reference.
        /// This is the object that is left clicked on.
        /// </summary>
        private GameObject theselectedobject;

        /// <summary>
        /// The barracks reference.
        /// </summary>
        private GameObject theBarracks;

        /// <summary>
        /// The unit that has been selected.
        /// </summary>
        private IUnit theUnit;

        /// <summary>
        /// Gets the instance of the UnitController.
        /// </summary>
        public static UnitController Self
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the drag screen.
        /// </summary>
        public static Rect DragScreen
        {
            get
            {
                return dragscreen;
            }

            set
            {
                dragscreen = value;
            }
        }

        /// <summary>
        /// The invert y function.
        /// Inverts the y so the drag screen will drag accordingly.
        /// <para>
        /// </para>
        /// <remarks>
        /// <paramref name="y"></paramref> -The number to subtract from the screen height.
        /// </remarks>
        /// </summary>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public static float InvertY(float y)
        {
            return Screen.height - y;
        }

        /// <summary>
        /// The check if selected function.
        /// Checks if the current game object is under the drag screen.
        /// <para></para>
        /// <remarks><paramref name="theunit"></paramref> -The object to check if it is under the drag screen.</remarks>
        /// </summary>
        public void CheckIfSelected(GameObject theunit)
        {
            if (theunit.GetComponent<Renderer>().isVisible && Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 camPos = Camera.main.WorldToScreenPoint(theunit.transform.position);
                camPos.y = InvertY(camPos.y);

                if (DragScreen.Contains(camPos) & !this.units.Contains(theunit))
                {
                    GameObject selectionsquare = theunit.transform.FindChild("SelectionHighlight").gameObject;
                    selectionsquare.GetComponent<MeshRenderer>().enabled = true;
                    selectionsquare.GetComponent<MeshRenderer>().material.color = Color.black;
                    this.units.Add(theunit);
                    UIManager.Self.currentcooldown = theunit.GetComponent<Stats>().CurrentSkillCooldown;
                    UIManager.Self.abilityunit = theunit;
                    UIManager.Self.CreateUnitButton(theunit);
                }

            }
        }

        /// <summary>
        /// The select all harvesters function.
        /// This function selects all harvesters on the map.
        /// </summary>
        public void SelectAllHarvesters()
        {
            this.ClearSelectedUnits();
            List<Harvester> units = FindObjectsOfType<Harvester>().ToList();
            foreach (Harvester h in units)
            {
                GameObject selectionsquare = h.gameObject.transform.FindChild("SelectionHighlight").gameObject;
                selectionsquare.GetComponent<MeshRenderer>().enabled = true;
                selectionsquare.GetComponent<MeshRenderer>().material.color = Color.black;
                this.units.Add(h.gameObject);
                UIManager.Self.CreateUnitButton(h.gameObject);
            }
        }

        /// <summary>
        /// The select all extractors function.
        /// This function selects all extractors on the map.
        /// </summary>
        public void SelectAllExtractors()
        {
            this.ClearSelectedUnits();
            List<Extractor> units = FindObjectsOfType<Extractor>().ToList();
            foreach (Extractor e in units)
            {
                GameObject selectionsquare = e.gameObject.transform.FindChild("SelectionHighlight").gameObject;
                selectionsquare.GetComponent<MeshRenderer>().enabled = true;
                selectionsquare.GetComponent<MeshRenderer>().material.color = Color.black;
                this.units.Add(e.gameObject);
                UIManager.Self.CreateUnitButton(e.gameObject);
            }
        }

        /// <summary>
        /// The select all miners function.
        /// This function selects all miners on the map.
        /// </summary>
        public void SelectAllMiners()
        {
            this.ClearSelectedUnits();
            List<Miner> units = FindObjectsOfType<Miner>().ToList();
            foreach (Miner m in units)
            {
                GameObject selectionsquare = m.gameObject.transform.FindChild("SelectionHighlight").gameObject;
                selectionsquare.GetComponent<MeshRenderer>().enabled = true;
                selectionsquare.GetComponent<MeshRenderer>().material.color = Color.black;
                this.units.Add(m.gameObject);
                UIManager.Self.CreateUnitButton(m.gameObject);
            }
        }

        /// <summary>
        /// The select all units function.
        /// This function selects all units on the map.
        /// </summary>
        public void SelectAllUnits()
        {
            this.ClearSelectedUnits();
            List<GameObject> units = GameObject.FindGameObjectsWithTag("Player").ToList();

            foreach (GameObject go in units)
            {
                if (go.GetComponent(typeof(IUnit)))
                {
                    GameObject selectionsquare = go.transform.FindChild("SelectionHighlight").gameObject;
                    selectionsquare.GetComponent<MeshRenderer>().enabled = true;
                    selectionsquare.GetComponent<MeshRenderer>().material.color = Color.black;
                    this.units.Add(go);
                    UIManager.Self.CreateUnitButton(go);
                }
            }
        }

        /// <summary>
        /// The cancel action function.
        /// Cancels the current action of the selected unit(s).
        /// </summary>
        public void CancelAction()
        {
            if (this.theselectedobject != null)
            {
                this.theUnit.SetTheMovePosition(this.theselectedobject.transform.position);
                this.theUnit.ChangeStates("Idle");
            }

            if (this.units.Count > 0)
            {
                foreach (GameObject go in this.units)
                {
                    if (go.GetComponent(typeof(IUnit)))
                    {
                        IUnit u = (IUnit)go.GetComponent(typeof(IUnit));
                        u.SetTheMovePosition(go.transform.position);
                        u.ChangeStates("Idle");
                    }
                }
            }
        }

        /// <summary>
        /// The call home function.
        /// This function sends the units back to the barracks.
        /// </summary>
        public void CallHome()
        {
            this.SelectAllUnits();

            Vector3 doorposition = this.theBarracks.transform.FindChild("Door").position;

            if (this.units.Count > 0)
            {
                int counter = -1;
                int x = -1;

                double sqrt = Math.Sqrt(this.units.Count);

                float startx = doorposition.x;

                for (int i = 0; i < this.units.Count; i++)
                {
                    counter++;
                    x++;

                    if (x > 1)
                    {
                        x = 1;
                    }

                    doorposition = new Vector3(doorposition.x + (x * 2f), 0.5f, doorposition.z);

                    if (counter == Math.Floor(sqrt))
                    {
                        counter = 0;
                        x = 0;
                        doorposition.x = startx;
                        doorposition.z--;
                    }

                    if (!this.units[i].GetComponent(typeof(IUnit)))
                    {
                        Debug.LogWarning(string.Format("hey, no component on {0}", this.units[i].name));
                    }
                    else
                    {
                        IUnit unit = (IUnit)this.units[i].GetComponent(typeof(IUnit));

                        unit.SetTheMovePosition(doorposition);
                        unit.ChangeStates("Idle");
                    }
                }
            }
        }

        /// <summary>
        /// The harvest function.
        /// This function will send all units to harvest the closest resource respectively.
        /// </summary>
        public void Harvest()
        {
            // Find all the resources in the scene
            List<GameObject> theresources = GameObject.FindGameObjectsWithTag("Resource").ToList();
            
            // Find the appropriate types in the resources list
            this.trees = theresources.FindAll(x => x.GetComponent<Food>());
            this.mineraldeposits = theresources.FindAll(x => x.GetComponent<Minerals>());
            this.geysers = theresources.FindAll(x => x.GetComponent<Gas>());

            // Select all the units
            this.SelectAllUnits();

            foreach (GameObject go in this.units)
            {
                IUnit u = (IUnit)go.GetComponent(typeof(IUnit));

                // If its a harvester
                if (go.GetComponent<Harvester>())
                {
                    // Sort the trees list
                    this.trees.Sort(
                        delegate (GameObject a, GameObject b)
                        {
                            float distanceA = Vector3.Distance(a.transform.position, go.transform.position);
                            float distanceB = Vector3.Distance(b.transform.position, go.transform.position);

                            if (distanceA > distanceB) return 1;
                            if (distanceA < distanceB) return -1;

                            return 0;
                        });

                    // Set the target to the closest tree to the unit
                    u.SetTargetResource(this.trees[0]);
                } 
                // If it's a miner
                else if (go.GetComponent<Miner>())
                {
                    this.mineraldeposits.Sort(
                        delegate (GameObject a, GameObject b)
                        {
                            float distanceA = Vector3.Distance(a.transform.position, go.transform.position);
                            float distanceB = Vector3.Distance(b.transform.position, go.transform.position);

                            if (distanceA > distanceB) return 1;
                            if (distanceA < distanceB) return -1;

                            return 0;
                        });

                    u.SetTargetResource(this.mineraldeposits[0]);
                }
                else if (go.GetComponent<Extractor>())
                {
                    this.geysers.Sort(
                        delegate (GameObject a, GameObject b)
                        {
                            float distanceA = Vector3.Distance(a.transform.position, go.transform.position);
                            float distanceB = Vector3.Distance(b.transform.position, go.transform.position);

                            if (distanceA > distanceB) return 1;
                            if (distanceA < distanceB) return -1;

                            return 0;
                        });

                    u.SetTargetResource(this.geysers[0]);
                }
            }
        }

        /// <summary>
        /// The spawn unit function.
        /// <para></para>
        /// <remarks><paramref name="theunit"></paramref> -The object to spawn.</remarks>
        /// </summary>
        public void SpawnUnit(GameObject theunit)
        {
            Vector3 spawnposition = this.theBarracks.transform.FindChild("Door").position;
            theunit.transform.FindChild("SelectionHighlight").gameObject.GetComponent<MeshRenderer>().enabled = false;
            Instantiate(theunit, spawnposition, Quaternion.AngleAxis(-180, Vector3.up));
        }

        /// <summary>
        /// The combat text function.
        /// <para></para>
        /// <remarks><paramref name="unit"></paramref> -The unit to display is taking damage.</remarks>
        /// <para></para>
        /// <remarks><paramref name="textoutline"></paramref> -The color to display as the outline for the text.</remarks>
        /// <para></para>
        /// <remarks><paramref name="message"></paramref> -The message to display.</remarks>
        /// </summary>
        public IEnumerator CombatText(GameObject unit, Color textoutline, string message)
        {
            // Queue up a text object
            this.textobjs.Enqueue(this.combattext);

            yield return new WaitForSeconds(0.25f);

            Stats thestats;
            Vector3 spawnposition;

            // If the unit is null then just display the text at the mouse position
            if (unit == null)
            {
                spawnposition = Input.mousePosition;
                spawnposition.y += 0.5f;
            }
            else
            {
                spawnposition = unit.transform.position;
                spawnposition.y += 0.5f;
                spawnposition = Camera.main.WorldToScreenPoint(spawnposition);
            }

            GameObject textobj = this.textobjs.Dequeue();

            // Create text object to display unit health
            GameObject theTextGo = Instantiate(textobj, spawnposition, Quaternion.identity);
            Text thetext = theTextGo.GetComponent<Text>();
            theTextGo.GetComponent<Outline>().effectColor = textoutline;
            theTextGo.transform.SetParent(UIManager.Self.BackgroundUI.GetComponent<RectTransform>());

            // If no message then print the stats
            if (message == null && unit != null)
            {
                thestats = unit.GetComponent<Stats>();
                thetext.text = thestats.Health + "/" + thestats.Maxhealth;
            }
            else
            { // Else print the message
                thetext.text = message;
            }
        }

        /// <summary>
        /// The start function.
        /// </summary>
        private void Start()
        {
            instance = this;
            this.theBarracks = GameObject.Find("Barracks");
            EventManager.Subscribe("ActivateAbility", this.ActivateAbility);
        }

        /// <summary>
        /// The update function.
        /// </summary>
        private void Update()
        {

            this.ActivateDragScreen();
            this.SelectUnits();
            this.CommandUnits();
        }

        /// <summary>
        /// The on destroy function.
        /// </summary>
        private void OnDestroy()
        {
            EventManager.UnSubscribe("ActivateAbility", this.ActivateAbility);
        }

        /// <summary>
        /// The destroy rag doll function.
        /// Destroys the rag doll after a few seconds.
        /// <para></para>
        /// <remarks><paramref name="theragdoll"></paramref> -The rag doll object to destroy.</remarks>
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        private IEnumerator DestroyRagDoll(GameObject theragdoll)
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(theragdoll);
        }

        /// <summary>
        /// The select units function.
        /// This function is used for unit selection.
        /// </summary>
        private void SelectUnits()
        {
            // If the left mouse button is pressed and its not clicking on a UI element
            if (Input.GetKeyDown(KeyCode.Mouse0) && this.gameObject.layer != 5)
            {
                this.ClearSelectedUnits();
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    if (hit.transform.GetComponent(typeof(IUnit)))
                    {
                        this.theUnit = (IUnit)hit.transform.GetComponent(typeof(IUnit));
                        this.theselectedobject = hit.transform.gameObject;
                        GameObject selectionsquare = this.theselectedobject.transform.FindChild("SelectionHighlight").gameObject;
                        selectionsquare.GetComponent<MeshRenderer>().enabled = true;
                        selectionsquare.GetComponent<MeshRenderer>().material.color = Color.black;
                        UIManager.Self.currentcooldown = this.theselectedobject.GetComponent<Stats>().CurrentSkillCooldown;
                        UIManager.Self.abilityunit = this.theselectedobject;
                        UIManager.Self.CreateUnitButton(this.theselectedobject);

                        // Start a coroutine to print the text to the screen -
                        // It is a coroutine to assist in helping prevent text objects from
                        // spawning on top one another.
                        this.StartCoroutine(this.CombatText(hit.transform.gameObject, Color.white, null));

                        // Increment the count now that user has left clicked
                        CameraController.Numofclicks++;
                        CameraController.Doubleclickedobj = hit.transform.gameObject;
                    }
                    else if (hit.transform.GetComponent<Enemy>())
                    {
                        // Start a coroutine to print the text to the screen -
                        // It is a coroutine to assist in helping prevent text objects from
                        // spawning on top one another.
                        this.StartCoroutine(this.CombatText(hit.transform.gameObject, new Color(255f, 0, 180, 0.75f), null));
                        
                        // Increment the count now that user has left clicked
                        CameraController.Numofclicks++;
                        CameraController.Doubleclickedobj = hit.transform.gameObject;
                    }
                }
            }
        }

        /// <summary>
        /// The activate drag screen function.
        /// This controls the drag screen.
        /// </summary>
        private void ActivateDragScreen()
        {
            // If the left mouse button is held down and its not on a UI element
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                this.startclick = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (dragscreen.width < 0)
                {
                    dragscreen.x += dragscreen.width;
                    dragscreen.width = -dragscreen.width;
                }
                if (dragscreen.height < 0)
                {
                    dragscreen.y += dragscreen.height;
                    dragscreen.height = -dragscreen.height;
                }

                this.startclick = -Vector3.one;
            }

            if (Input.GetMouseButton(0))
            {
                dragscreen = new Rect(
                    this.startclick.x,
                    InvertY(this.startclick.y),
                    Input.mousePosition.x - this.startclick.x,
                    InvertY(Input.mousePosition.y) - InvertY(this.startclick.y));
            }
        }

        /// <summary>
        /// The clear selected units function.
        /// This clears the list of selected units and the current selected unit.
        /// </summary>
        private void ClearSelectedUnits()
        {
            if (this.units.Count > 0)
            {
                foreach (GameObject go in this.units)
                {
                    GameObject selectionsquare = go.transform.FindChild("SelectionHighlight").gameObject;
                    selectionsquare.GetComponent<MeshRenderer>().enabled = false;
                }

                UIManager.Self.ClearUnitButtonsList();
            }


            this.units.Clear();
            this.theUnit = null;

            if (this.theselectedobject != null)
            {
                GameObject selectionsquare = this.theselectedobject.transform.FindChild("SelectionHighlight").gameObject;
                selectionsquare.GetComponent<MeshRenderer>().enabled = false;
                this.theselectedobject = null;
                UIManager.Self.ClearUnitButtonsList();
            }
        }

        /// <summary>
        /// The command units function.
        /// Gives the units proper commands.
        /// </summary>
        private void CommandUnits()
        {
            // If the right mouse button is pressed
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f, allbutui))
                {
                    this.clickdestination = new Vector3(hit.point.x, 0.5f, hit.point.z);

                    this.HealUnit(hit);

                    if (hit.transform.name == "Workshop") return;

                    if (hit.transform.GetComponent<Enemy>())
                    {
                        this.CommandToAttack(hit);
                    }
                    else if (hit.transform.GetComponent(typeof(IResources)))
                    {
                        this.CommandToHarvest(hit);
                    }
                    else if (hit.transform.tag == "PickUp")
                    {
                        this.CommandToPickUp(hit);
                    }
                    else if (hit.transform.gameObject.name == "Silo")
                    {
                        this.CommandToStock(hit);
                    }
                    else if (hit.transform.gameObject.name == "Decontamination")
                    {
                        this.CommandToDecontaminate(hit);
                    }
                    else
                    {
                        this.CommandToIdle(hit);
                    }
                }
            }
        }

        /// <summary>
        /// The command to attack function.
        /// Sends unit(s) to attack.
        /// <para></para>
        /// <remarks><paramref name="hit"></paramref> -The object that was hit by the ray cast.</remarks>
        /// </summary>
        private void CommandToAttack(RaycastHit hit)
        {
            // single check only one clicked
            if (this.theUnit != null)
            {
                this.theUnit.SetTheMovePosition(hit.transform.position);
                this.theUnit.SetTarget(hit.transform.gameObject);
                this.theUnit.ChangeStates("Battle");
            } 
            // multiple selected
            else if (this.units.Count > 0)
            {
                foreach (GameObject go in this.units)
                {
                    if (!go.GetComponent(typeof(IUnit)))
                    {
                        Debug.LogWarning(string.Format("hey, no component on {0}", go.name));
                    }
                    else
                    {
                        IUnit unit = (IUnit)go.GetComponent(typeof(IUnit));

                        unit.SetTheMovePosition(hit.transform.position);
                        unit.SetTarget(hit.transform.gameObject);
                        unit.ChangeStates("Battle");
                    }
                }
            }
        }

        /// <summary>
        /// The command to harvest function.
        /// Sends unit(s) to harvest a resource.
        /// <para></para>
        /// <remarks><paramref name="hit"></paramref> -The object that was hit by the ray cast.</remarks>
        /// </summary>
        private void CommandToHarvest(RaycastHit hit)
        {
            if (this.theUnit != null)
            {
               this.theUnit.SetTargetResource(hit.transform.gameObject);
            }
            else if (this.units.Count > 0)
            {
                foreach (GameObject go in this.units)
                {
                    if (!go.GetComponent(typeof(IUnit)))
                    {
                        Debug.LogWarning(string.Format("hey, no component on {0}", go.name));
                    }
                    else
                    {
                        IUnit unit = (IUnit)go.GetComponent(typeof(IUnit));
                        unit.SetTargetResource(hit.transform.gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// The command to idle function.
        /// Send unit(s) to idle.
        /// <para></para>
        /// <remarks><paramref name="hit"></paramref> -The object that was hit by the ray cast.</remarks>
        /// </summary>
        private void CommandToIdle(RaycastHit hit)
        {
            // If the destination clicked is another unit, or the destination clicked is the barracks, just return and dont move.
            if (hit.transform.gameObject.GetComponent(typeof(IUnit)) || hit.transform.gameObject.name == "Barracks")
            {
                return;
            }

            // Send unit back to idle
            if (this.theUnit != null)
            {
                this.theUnit.SetTheMovePosition(this.clickdestination);
                this.theUnit.ChangeStates("Idle");
            }
            else if (this.units.Count > 0 && this.units.Count <= 20)
            {
                this.CircleFormation();
            }
            else if (this.units.Count > 20)
            {
                this.SquareFormation();
            }
        }

        /// <summary>
        /// The command to pick up function.
        /// This function sends unit to the pickup item.
        /// <para></para>
        /// <remarks><paramref name="hit"></paramref> -The object that was hit by the ray cast.</remarks>
        /// </summary>
        private void CommandToPickUp(RaycastHit hit)
        {
            if (this.theUnit != null)
            {
                this.theUnit.GoToPickup(hit.transform.gameObject);
            }
        }

        /// <summary>
        /// The command to stock function.
        /// Sends unit(s) to stock.
        /// <para></para>
        /// <remarks><paramref name="hit"></paramref> -The object that was hit by the ray cast.</remarks>
        /// </summary>
        private void CommandToStock(RaycastHit hit)
        {
            if (this.theUnit != null)
            {
                this.theUnit.SetTheMovePosition(this.clickdestination);
                this.theUnit.ChangeStates("Stock");
            }
            else if (this.units.Count > 0)
            {
                foreach (GameObject go in this.units)
                {
                    if (!go.GetComponent(typeof(IUnit)))
                    {
                        Debug.LogWarning(string.Format("hey, no component on {0}", go.name));
                    }
                    else
                    {
                        IUnit unit = (IUnit)go.GetComponent(typeof(IUnit));

                        unit.SetTheMovePosition(this.clickdestination);
                        unit.ChangeStates("Stock");
                    }
                }
            }
        }

        /// <summary>
        /// The command to decontaminate function.
        /// Send unit(s) to decontaminate a tainted resource.
        /// <para></para>
        /// <remarks><paramref name="hit"></paramref> -The object that was hit by the ray cast.</remarks>
        /// </summary>
        private void CommandToDecontaminate(RaycastHit hit)
        {
            if (this.theUnit != null)
            {
                if (this.theselectedobject.transform.Find("MineralsTainted") || this.theselectedobject.transform.Find("FoodTainted"))
                {
                    this.theUnit.ChangeStates("Decontaminate");
                    Transform thedoor = hit.transform.Find("FrontDoor");
                    Debug.Log(thedoor.name);
                    Vector3 destination = new Vector3(thedoor.position.x, 0.5f, thedoor.position.z);
                    this.theUnit.SetTheMovePosition(destination);
                }
            }
            else if (this.units.Count > 0)
            {
                foreach (GameObject go in this.units)
                {
                    if (!go.GetComponent(typeof(IUnit)))
                    {
                        Debug.LogWarning(string.Format("hey, no component on {0}", go.name));
                    }
                    else
                    {
                        IUnit unit = (IUnit)go.GetComponent(typeof(IUnit));

                        if (go.transform.Find("MineralsTainted") || go.transform.Find("FoodTainted"))
                        {
                            unit.ChangeStates("Decontaminate");
                            Transform thedoor = hit.transform.Find("FrontDoor");
                            Vector3 destination = new Vector3(thedoor.position.x, 0.5f, thedoor.position.z);
                            unit.SetTheMovePosition(destination);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The heal unit function.
        /// Heals the clicked unit with food.
        /// <para></para>
        /// <remarks><paramref name="hit"></paramref> -The object that was hit by the ray cast.</remarks>
        /// </summary>
        private void HealUnit(RaycastHit hit)
        {
            // If the object hit is a unit and we have an instance of the cooked food to use
            if (hit.transform.gameObject.GetComponent(typeof(IUnit)) && UIManager.Self.foodinstance != null)
            {
                GameObject theorb = hit.transform.GetChild(2).GetChild(2).GetChild(0).gameObject;
                Stats stats = hit.transform.gameObject.GetComponent<Stats>();

                // If the unit can be healed
                if (stats.Health < stats.Maxhealth)
                {
                    // Heal unit
                    stats.Health += 20;

                    if (stats.Health >= stats.Maxhealth)
                    {
                        stats.Health = stats.Maxhealth;
                    }

                    if (theorb != null)
                    {
                        int halfhealth = stats.Maxhealth / 2;
                        int quarterhealth = stats.Maxhealth / 4;

                        if (stats.Health <= quarterhealth)
                        {
                            theorb.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
                        }
                        else if (stats.Health > quarterhealth && stats.Health <= halfhealth)
                        {
                            theorb.GetComponent<SkinnedMeshRenderer>().material.color = Color.yellow;
                        }
                        else if (stats.Health > halfhealth)
                        {
                            theorb.GetComponent<SkinnedMeshRenderer>().material.color = Color.green;
                        }
                    }

                    Vector3 spawnposition = hit.transform.position;
                    spawnposition.y += 0.5f;

                    // Create text object to display unit health
                    GameObject theTextGo = Instantiate(this.combattext, Camera.main.WorldToScreenPoint(spawnposition), Quaternion.identity);
                    Text thetext = theTextGo.GetComponent<Text>();
                    theTextGo.transform.SetParent(UIManager.Self.BackgroundUI.GetComponent<RectTransform>());
                    thetext.text = stats.Health + "/" + stats.Maxhealth;
                    Destroy(UIManager.Self.foodinstance);

                } // If the current clicked units health is equal to or greater than its max health..
                else if (stats.Health >= stats.Maxhealth)
                {
                    // Destroy the cooked food object and refund the cooked food point.
                    Destroy(UIManager.Self.foodinstance);
                    User.CookedFoodCount++;

                    Vector3 spawnposition = hit.transform.position;
                    spawnposition.y += 0.5f;

                    // Create text object to display unit health
                    GameObject theTextGo = Instantiate(this.combattext, Camera.main.WorldToScreenPoint(spawnposition), Quaternion.identity);
                    Text thetext = theTextGo.GetComponent<Text>();
                    theTextGo.transform.SetParent(UIManager.Self.BackgroundUI.GetComponent<RectTransform>());
                    thetext.text = stats.Health + "/" + stats.Maxhealth;
                }
            }
        }

        /// <summary>
        /// The Activate Ability function.
        /// Activates the ability of the selected unit(s).
        /// </summary>
        private void ActivateAbility()
        {
            if (this.theUnit != null)
            {
                this.theUnit.SpecialAbility();
            }
            else if (this.units.Count > 0)
            {
                foreach (GameObject go in this.units)
                {
                    IUnit unit = (IUnit)go.GetComponent(typeof(IUnit));
                    unit.SpecialAbility();
                }
            }
        }

        /// <summary>
        /// The Square Formation function.
        /// Puts units in the Square Formation.
        /// </summary>
        private void SquareFormation()
        {
            int counter = -1;
            int x = -1;

            double sqrt = Math.Sqrt(this.units.Count);

            float startx = this.clickdestination.x;

            for (int i = 0; i < this.units.Count; i++)
            {
                counter++;
                x++;

                if (x > 1)
                {
                    x = 1;
                }

                this.clickdestination = new Vector3(this.clickdestination.x + (x * 2f), 0.5f, this.clickdestination.z);

                if (counter == Math.Floor(sqrt))
                {
                    counter = 0;
                    x = 0;
                    this.clickdestination.x = startx;
                    this.clickdestination.z++;
                }

                if (!this.units[i].GetComponent(typeof(IUnit)))
                {
                    Debug.LogWarning(string.Format("hey, no component on {0}", this.units[i].name));
                }
                else
                {
                    IUnit unit = (IUnit)this.units[i].GetComponent(typeof(IUnit));

                    unit.SetTheMovePosition(this.clickdestination);
                    unit.ChangeStates("Idle");
                }
            }
        }

        /// <summary>
        /// The Circle Formation function.
        /// Puts units in the Circle Formation.
        /// </summary>
        private void CircleFormation()
        {
            for (int i = 0; i < this.units.Count; i++)
            {
                float angle = i * (2 * 3.14159f / this.units.Count);
                float x = Mathf.Cos(angle) * 1.5f;
                float z = Mathf.Sin(angle) * 1.5f;

                this.clickdestination = new Vector3(this.clickdestination.x + x, 0.5f, this.clickdestination.z + z);

                if (!this.units[i].GetComponent(typeof(IUnit)))
                {
                    Debug.LogWarning(string.Format("hey, no component on {0}", this.units[i].name));
                }
                else
                {
                    IUnit unit = (IUnit)this.units[i].GetComponent(typeof(IUnit));

                    unit.SetTheMovePosition(this.clickdestination);
                    unit.ChangeStates("Idle");
                }
            }
        }

        /// <summary>
        /// The On GUI function.
        /// This draws the drag screen to the screen.
        /// Also, uses the KeyBind class to change hot keys.
        /// </summary>
        private void OnGUI()
        {
            if (this.startclick != -Vector3.one)
            {
                GUI.color = new Color(1, 1, 1, 0.5f);
                GUI.DrawTexture(dragscreen, this.selectionHighlight);
            }

            KeyBind.Self.HotkeyChange();
        }
    }
}
