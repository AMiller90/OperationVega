using System.Collections;
using System.Collections.Generic;

using Assets.Scripts;

using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers;

public class UIWorkshop : MonoBehaviour {

    [SerializeField]
    private RectTransform m_WorkshopUI;

    [SerializeField]
    private RectTransform m_ThrusterChoice;
    [SerializeField]
    private RectTransform m_CockpitChoice;
    [SerializeField]
    private RectTransform m_WingChoice;
    [SerializeField]
    private Image Xbutton;

    private Rocket rocketFactory;

    bool undo1;
    bool undo2;
    bool undo3;

    void Awake()
    {
        this.rocketFactory = FindObjectOfType<Rocket>();

        undo1 = true;
        undo2 = true;
        undo3 = true;

        EventManager.Subscribe("Workshop", this.OnWorkShop);
        EventManager.Subscribe("Close WorkShop", this.CloseWorkShop);
        EventManager.Subscribe("Build Rocket", this.rocketFactory.BuildRocket);
        EventManager.Subscribe("Thrusters", this.OnThrusters);
        EventManager.Subscribe("Player chose TC1", this.rocketFactory.CreateThrusters1);
        EventManager.Subscribe("Player chose TC2", this.rocketFactory.CreateThrusters2);
        EventManager.Subscribe("Player chose TC3", this.rocketFactory.CreateThrusters3);
        EventManager.Subscribe("Cockpit", this.OnCockpit);
        EventManager.Subscribe("Player chose CP1", this.rocketFactory.CreateCockpit1);
        EventManager.Subscribe("Player chose CP2", this.rocketFactory.CreateCockpit2);
        EventManager.Subscribe("Player chose CP3", this.rocketFactory.CreateCockpit3);
        EventManager.Subscribe("Apply Wings", this.OnWings);
        EventManager.Subscribe("Player chose WC1", this.rocketFactory.CreateWings1);
        EventManager.Subscribe("Player chose WC2", this.rocketFactory.CreateWings2);
        EventManager.Subscribe("Player chose WC3", this.rocketFactory.CreateWings3);
    }

    void OnDestroy()
    {
        EventManager.UnSubscribe("Close WorkShop", this.CloseWorkShop);
        EventManager.UnSubscribe("Build Rocket", this.rocketFactory.BuildRocket);
        EventManager.UnSubscribe("Thrusters", this.OnThrusters);
        EventManager.UnSubscribe("Player chose TC1", this.rocketFactory.CreateThrusters1);
        EventManager.UnSubscribe("Player chose TC2", this.rocketFactory.CreateThrusters2);
        EventManager.UnSubscribe("Player chose TC3", this.rocketFactory.CreateThrusters3);
        EventManager.UnSubscribe("Cockpit", this.OnCockpit);
        EventManager.UnSubscribe("Player chose CP1", this.rocketFactory.CreateCockpit1);
        EventManager.UnSubscribe("Player chose CP2", this.rocketFactory.CreateCockpit2);
        EventManager.UnSubscribe("Player chose CP3", this.rocketFactory.CreateCockpit3);
        EventManager.UnSubscribe("Apply Wings", this.OnWings);
        EventManager.UnSubscribe("WingChoice1", this.rocketFactory.CreateWings1);
        EventManager.UnSubscribe("WingChoice2", this.rocketFactory.CreateWings2);
        EventManager.UnSubscribe("WingChoice3", this.rocketFactory.CreateWings3);

    }

    public void OnBuildClick()
    {
        EventManager.Publish("Build Rocket");
    }

    public void OnThrustersClick()
    {
        EventManager.Publish("Thrusters");
    }

    private void OnThrusters()
    {
        if (undo1)
        {
            m_ThrusterChoice.gameObject.SetActive(true);

            undo1 = false;
        }
        else if (!undo1)
        {
            m_ThrusterChoice.gameObject.SetActive(false);
            undo1 = true;
        }
    }

    public void OnCockpitClick()
    {
        EventManager.Publish("Cockpit");
    }

    private void OnCockpit()
    {
        if (undo2)
        {
            m_CockpitChoice.gameObject.SetActive(true);

            undo2 = false;
        }
        else if (!undo2)
        {
            m_CockpitChoice.gameObject.SetActive(false);

            undo2 = true;
        }
    }

    public void OnWingsClick()
    {

        EventManager.Publish("Apply Wings");
    }

    private void OnWings()
    {
        if (undo3)
        {
            m_WingChoice.gameObject.SetActive(true);

            undo3 = false;
        }
        else if (!undo3)
        {
            m_WingChoice.gameObject.SetActive(false);

            undo3 = true;
        }
    }

    public void OnWC1Click()
    {
        EventManager.Publish("Player chose WC1");
    }

    public void OnWC2Click()
    {
        EventManager.Publish("Player chose WC2");

    }

    public void OnWC3Click()
    {
        EventManager.Publish("Player chose WC3");
    }

    public void OnCP1Click()
    {
        EventManager.Publish("Player chose CP1");
    }

    public void OnCP2Click()
    {
        EventManager.Publish("Player chose CP2");
    }

    public void OnCP3Click()
    {
        EventManager.Publish("Player chose CP3");
    }

    public void OnTC1Click()
    {
        EventManager.Publish("Player chose TC1");
    }

    public void OnTC2Click()
    {
        EventManager.Publish("Player chose TC2");
    }

    public void OnTC3Click()
    {
        EventManager.Publish("Player chose TC3");
    }

    public void OnWorkShopClick()
    {
        EventManager.Publish("Workshop");
    }

    private void OnWorkShop()
    {
        m_WorkshopUI.gameObject.SetActive(true);
        //This function will bring up the workshop within the game.
     
    }

    public void CloseWorkShopClick()
    {
        EventManager.Publish("Close WorkShop");
    }

    private void CloseWorkShop()
    {
        //This function will close work shop menu
        m_WorkshopUI.gameObject.SetActive(false);
       

    }
}
