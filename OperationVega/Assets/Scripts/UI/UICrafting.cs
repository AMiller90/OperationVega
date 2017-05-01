using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts;
using Assets.Scripts.Controllers;
using Assets.Scripts.Interfaces;

using UI;

public class UICrafting : MonoBehaviour
{
    private Sprite m_defaultCraftSprite;
    UI.UIManager uiManager;
    bool revertcraftingtab;
    RectTransform m_CraftingTAB;
    float Scalefactor;
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

    [HideInInspector]
    public GameObject foodinstance;

    [SerializeField]
    private GameObject cookedfoodPrefab;

    bool input1b;
    bool input2b;

    private Image defaultInput1;
    private Image defaultInput2;
    private Image defaultOutput;


    void Awake()
    {
        revertcraftingtab = true;



        defaultInput1 = Input1;
        defaultInput2 = Input2;
        defaultOutput = Output;
        m_defaultCraftSprite = defaultInput1.sprite;

        input1b = true;
        input2b = true;

        uiManager = FindObjectOfType<UI.UIManager>();
        EventManager.Subscribe("Crafting", OnCrafting);
        EventManager.Subscribe("Craft", OnCraft);
        EventManager.Subscribe("Minerals", this.OnMinerals);
        EventManager.Subscribe("Food", this.OnFood);
        EventManager.Subscribe("CookedFood", this.OnCookedFood);
        EventManager.Subscribe("Gas", this.OnGas);
        EventManager.Subscribe("Fuel", this.OnFuel);
        EventManager.Subscribe("Steel", this.OnSteel);
        EventManager.Subscribe("Clear", this.OnClear);



    }
    // Use this for initialization
    void Start()
    {
        m_CraftingTAB = uiManager.CraftingTab;
        Scalefactor = uiManager.UIScaleFactor;
        revertcraftingtab = uiManager.RevertCraftingTab;
    }

    void OnDestroy()
    {
        EventManager.UnSubscribe("Crafting", OnCrafting);
        EventManager.UnSubscribe("Craft", OnCraft);
        EventManager.UnSubscribe("Minerals", this.OnMinerals);
        EventManager.UnSubscribe("Food", this.OnFood);
        EventManager.UnSubscribe("CookedFood", this.OnCookedFood);
        EventManager.UnSubscribe("Gas", this.OnGas);
        EventManager.UnSubscribe("Fuel", this.OnFuel);
        EventManager.UnSubscribe("Steel", this.OnSteel);
        EventManager.UnSubscribe("Clear", this.OnClear);
    }

    public void Minerals()
    {
        EventManager.Publish("Minerals");
    }
    private void OnMinerals()
    {
        //Will Change the source image to the first craft slot
        //Second Slot if first one is selected.
        if (input1b)
        {
            Input1.sprite = minerals.sprite;

            input1b = false;
        }
        else if (!input1b)
        {
            Input2.sprite = minerals.sprite;

            input1b = true;
        }
        
    }

    public void Food()
    {

        EventManager.Publish("Food");
    }
    private void OnFood()
    {
        //Will Change the source image to the first craft slot
        //Second Slot if first one is selected.
        if (input1b)
        {
            Input1.sprite = food.sprite;

            input1b = false;
        }
        else if (!input1b)
        {
            Input2.sprite = food.sprite;

            input2b = false;
        }
        
    }

    public void CookedFood()
    {
        EventManager.Publish("CookedFood");
    }

    private void OnCookedFood()
    {
        //Will Change the source image to the first craft slot
        //Second Slot if first one is selected.
        if (User.CookedFoodCount > 0)
        {
            Vector3 mousePosition;
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            UIManager.Self.foodinstance = Instantiate(cookedfoodPrefab, mousePosition, Quaternion.identity);
           

            User.CookedFoodCount--;
        }
    }

    public void Gas()
    {
        EventManager.Publish("Gas");
    }

    private void OnGas()
    {
        //Will Change the source image to the first craft slot
        //Second Slot if first one is selected.
        if (input1b)
        {
            Input1.sprite = gas.sprite;

            input1b = false;
        }
        else if (!input1b)
        {
            Input2.sprite = gas.sprite;

            input2b = false;
        }
      
    }

    public void Fuel()
    {
        EventManager.Publish("Fuel");
    }

    private void OnFuel()
    {
        //Will Change the source image to the first craft slot

        if (input1b)
        {
            Input1.sprite = fuel.sprite;

            input1b = false;
        }
        else if (!input1b)
        {
            Input2.sprite = fuel.sprite;

            input2b = false;
        }
        //Second Slot if first one is selected.
    
    }
    public void Steel()
    {
        EventManager.Publish("Steel");
    }

    private void OnSteel()
    {
      
    }


    public void OnCraftingClick()
    {

        EventManager.Publish("Crafting");
    }



    private void OnCrafting()
    {
        //Moves the crafting tab up
        if (revertcraftingtab)
        {
            m_CraftingTAB.offsetMax = new Vector2(m_CraftingTAB.offsetMax.x, 0);
            m_CraftingTAB.offsetMin = new Vector2(m_CraftingTAB.offsetMin.x, 0);

            revertcraftingtab = false;

        }

        //Sets the crafting tab to its original state
        else if (!revertcraftingtab)
        {
            revertcraftingtab = true;

            m_CraftingTAB.offsetMax = new Vector2(m_CraftingTAB.offsetMax.x, Scalefactor);
            m_CraftingTAB.offsetMin = new Vector2(m_CraftingTAB.offsetMin.x, -115);
        }


    
    }

    public void OnCraftClick()
    {

        //This function will run the craft function
        EventManager.Publish("Craft");
    }

    private void OnCraft()
    {
        //If Input is minerals and gas, produce fuel.
        if (Input1.sprite == minerals.sprite && Input2.sprite == gas.sprite)
        {
            Output.sprite = fuel.sprite;
            if (User.MineralsCount > 0 && User.GasCount > 0)
            {
                User.MineralsCount--;
                User.GasCount--;
                User.FuelCount++;
            }
            else if (User.MineralsCount <= 0 && User.GasCount <= 0)
            {
                User.MineralsCount = 0;
                User.GasCount = 0;
                Output.sprite = Xbutton.sprite;

            }
            else
            {
                Output.sprite = Xbutton.sprite;
            }
            ObjectiveManager.Instance.TheObjectives[ObjectiveType.Craft].Currentvalue++;
        }
        if (Input2.sprite == minerals.sprite && Input1.sprite == gas.sprite)
        {
            Output.sprite = fuel.sprite;
            if (User.MineralsCount > 0 && User.GasCount > 0)
            {
                User.MineralsCount--;
                User.GasCount--;
                User.FuelCount++;
            }
            else if (User.MineralsCount <= 0 && User.GasCount <= 0)
            {
                User.MineralsCount = 0;
                User.GasCount = 0;
                Output.sprite = Xbutton.sprite;

            }
            else
            {
                Output.sprite = Xbutton.sprite;
            }
            ObjectiveManager.Instance.TheObjectives[ObjectiveType.Craft].Currentvalue++;
        }

        //If Input is minerals and food, produce steel.
        if (Input1.sprite == minerals.sprite && Input2.sprite == food.sprite)
        {
            Output.sprite = steel.sprite;
            if (User.MineralsCount > 0 && User.FoodCount > 0)
            {
                User.MineralsCount--;
                User.FoodCount--;
                User.SteelCount++;
            }
            else if (User.MineralsCount <= 0 && User.FoodCount <= 0)
            {
                User.MineralsCount = 0;
                User.FoodCount = 0;
                Output.sprite = Xbutton.sprite;

            }
            else
            {
                Output.sprite = Xbutton.sprite;
            }
            ObjectiveManager.Instance.TheObjectives[ObjectiveType.Craft].Currentvalue++;
        }
        if (Input2.sprite == minerals.sprite && Input1.sprite == food.sprite)
        {
            Output.sprite = steel.sprite;
            if (User.MineralsCount > 0 && User.FoodCount > 0)
            {
                User.MineralsCount--;
                User.FoodCount--;
                User.SteelCount++;
            }
            else if (User.MineralsCount <= 0 && User.FoodCount <= 0)
            {
                User.MineralsCount = 0;
                User.FoodCount = 0;
                Output.sprite = Xbutton.sprite;

            }
            else
            {
                Output.sprite = Xbutton.sprite;
            }
            ObjectiveManager.Instance.TheObjectives[ObjectiveType.Craft].Currentvalue++;
        }

        //If Input is food and gas, produce Cooked Food.
        if (Input1.sprite == food.sprite && Input2.sprite == gas.sprite)
        {
            Output.sprite = cookedFood.sprite;
            if (User.FoodCount > 0 && User.GasCount > 0)
            {
                User.FoodCount--;
                User.GasCount--;
                User.CookedFoodCount++;
            }
            else if (User.FoodCount <= 0 && User.GasCount <= 0)
            {
                User.FoodCount = 0;
                User.GasCount = 0;
                Output.sprite = Xbutton.sprite;

            }
            else
            {
                Output.sprite = Xbutton.sprite;
            }
            ObjectiveManager.Instance.TheObjectives[ObjectiveType.Craft].Currentvalue++;
        }

        if (Input2.sprite == food.sprite && Input1.sprite == gas.sprite)
        {
            Output.sprite = cookedFood.sprite;
            if (User.FoodCount > 0 && User.GasCount > 0)
            {
                User.FoodCount--;
                User.GasCount--;
                User.CookedFoodCount++;
            }
            else if (User.FoodCount <= 0 && User.GasCount <= 0)
            {
                User.FoodCount = 0;
                User.GasCount = 0;
                Output.sprite = Xbutton.sprite;

            }
            else
            {
                Output.sprite = Xbutton.sprite;
            }
            ObjectiveManager.Instance.TheObjectives[ObjectiveType.Craft].Currentvalue++;
        }
        if (Input1.sprite == minerals.sprite && Input2.sprite == minerals.sprite)
        {
            Output.sprite = Xbutton.sprite;
        }
        if(Input1.sprite == steel.sprite && Input2.sprite == steel.sprite)
        {
            Output.sprite = Xbutton.sprite;
        }

    
        
        

    }

    public void OnClearClick()
    {
        EventManager.Publish("Clear");
    }
    private void OnClear()
    {

        if (Input1.sprite == minerals.sprite || Input2.sprite == minerals.sprite)
        {
            //User.MineralsCount++;
        }
        if (Input1.sprite == steel.sprite || Input2.sprite == steel.sprite)
        {
            //User.SteelCount++;
        }
        if (Input1.sprite == gas.sprite || Input2.sprite == gas.sprite)
        {
            //User.GasCount++;
        }
        if (Input1.sprite == fuel.sprite || Input2.sprite == fuel.sprite)
        {
            //User.FuelCount++;
        }

        List<Image> images = new List<Image>() { Input1, Input2, Output };
        foreach (Image image in images)
            image.sprite = m_defaultCraftSprite;

        input1b = true;
        input2b = true;






        //This function will clear items in the craft.
       
    }
}
