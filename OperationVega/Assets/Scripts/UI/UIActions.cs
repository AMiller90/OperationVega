using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers;
using Assets.Scripts;
using Assets.Scripts.Controllers;
using Assets.Scripts.Interfaces;

public class UIActions : MonoBehaviour {
    UI.UIManager uiManager;
    bool revertactionstab;
    RectTransform m_ActionsTAB;
    float Scalefactor;

    void Awake()
    {
       revertactionstab = true;
       uiManager= FindObjectOfType<UI.UIManager>();
       EventManager.Subscribe("Actions", OnActions);
       EventManager.Subscribe("ActivateAbility", this.OnActivateAbility);
       EventManager.Subscribe("Harvest", this.OnHarvest);
       EventManager.Subscribe("Recall", this.OnRecall);
       EventManager.Subscribe("CancelAction", this.OnCancelAction);
       EventManager.Subscribe("SAExtract", this.OnSAExtract);
       EventManager.Subscribe("SAMiner", this.OnSAMiner);
       EventManager.Subscribe("SAUnit", this.OnSAUnit);
       EventManager.Subscribe("SAHarvest", this.OnSAHarvest);
    }
	// Use this for initialization
	void Start ()
    {
        
        m_ActionsTAB = uiManager.ActionsTab;
        Scalefactor = uiManager.UIScaleFactor;
        revertactionstab = uiManager.RevertActionsTab;

       
       
    }

    void OnDestroy()
    {
        EventManager.UnSubscribe("Actions", OnActions);
        EventManager.UnSubscribe("ActivateAbility", this.OnActivateAbility);
        EventManager.UnSubscribe("Harvest", this.OnHarvest);
        EventManager.UnSubscribe("Recall", this.OnRecall);
        EventManager.UnSubscribe("CancelAction", this.OnCancelAction);
        EventManager.UnSubscribe("SAExtract", this.OnSAExtract);
        EventManager.UnSubscribe("SAMiner", this.OnSAMiner);
        EventManager.UnSubscribe("SAUnit", this.OnSAUnit);
        EventManager.UnSubscribe("SAHarvest", this.OnSAHarvest);
    }

    public void OnActivateAbilityClick()
    {
        EventManager.Publish("ActivateAbility");
    }
    private void OnActivateAbility()
    {
        //Function will be use to rally upon click.
     
    }
    public void OnHarvestClick()
    {
        EventManager.Publish("Harvest");
    }
    private void OnHarvest()
    {
        //Function will be use to harvest upon click.
        UnitController.Self.Harvest();
       
    }
    public void OnRecallClick()
    {
        EventManager.Publish("Recall");
    }

    private void OnRecall()
    {
        UnitController.Self.CallHome();
        //Function will be use to recall upon click.
      
    }
    public void OnCancelActionClick()
    {
        EventManager.Publish("CancelAction");
    }
    private void OnCancelAction()
    {
        UnitController.Self.CancelAction();
        //Function will cancel previous action upon click.
       
    }

    public void OnSAExtractClick()
    {
        EventManager.Publish("SAExtract");
    }
    private void OnSAExtract()
    {
        //Calls the SelectAll Extractors function within the UnitController.
        UnitController.Self.SelectAllExtractors();
        
    }

    public void OnSAMinerClick()
    {
        EventManager.Publish("SAMiner");
    }
    private void OnSAMiner()
    {
        //Calls the SelectAll Miners function within the UnitController.
        UnitController.Self.SelectAllMiners();
        
    }

    public void OnSAUnitClick()
    {
        EventManager.Publish("SAUnit");
    }
    private void OnSAUnit()
    {
        //Calls the Select All Units function within the UnitController.
        UnitController.Self.SelectAllUnits();
    
    }
    public void OnSAHarvestClick()
    {
        EventManager.Publish("SAHarvest");
    }
    private void OnSAHarvest()
    {
        //Calls the Select All Harvesters function within the UnitController.
        UnitController.Self.SelectAllHarvesters();
       
    }


    public void OnActionsClick()
    {
        EventManager.Publish("Actions");
    }


    private void OnActions()
    {
        //If true set values to zero
        if (revertactionstab)
        {
            m_ActionsTAB.offsetMax = new Vector2(m_ActionsTAB.offsetMax.x, 0);
            m_ActionsTAB.offsetMin = new Vector2(m_ActionsTAB.offsetMin.x, 0);

            revertactionstab = false;
        }
        //If not true set to this position
        else if (!revertactionstab)
        {
            revertactionstab = true;

            m_ActionsTAB.offsetMax = new Vector2(m_ActionsTAB.offsetMax.x, Scalefactor);
            m_ActionsTAB.offsetMin = new Vector2(m_ActionsTAB.offsetMin.x, -115);
        }
       
    } 

    

}
