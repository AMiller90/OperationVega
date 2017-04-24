
namespace Assets.Scripts
{
	using Assets.Scripts.Interfaces;

	using UI;

	using UnityEngine;
	using UnityEngine.EventSystems;

    /// <summary>
	/// The minerals class.
	/// </summary>
	public class Minerals : MonoBehaviour, IResources
	{
		/// <summary>
		/// The max amount.
		/// The max amount of the resource available.
		/// </summary>
		private int maxAmount;

		/// <summary>
		/// The amount.
		/// The current amount of the resource available.
		/// Accessible through the Count property.
		/// </summary>
		private int amount;

		/// <summary>
		/// The refill.
		/// Boolean for if the resource is renewable.
		/// Accessible through the Renewable property.
		/// </summary>
		private bool refill;

		/// <summary>
		/// The state.
		/// Boolean for if the resource is tainted.
		/// Accessible through the Taint property.
		/// </summary>
		public bool state;

		/// <summary>
		/// The refill timer.
		/// Float to represent a timer.
		/// Used to calculate when the amount will be increased.
		/// </summary>
		private float refillTimer;

		/// <summary>
		/// The reset timer.
		/// Float to represent a timer.
		/// Used to calculate when the state should be set to false.
		/// </summary>
		private float resetTimer;

		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		public int Count
		{
			get
			{
				return this.amount;
			}

			set
			{
				this.amount = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether renewable.
		/// </summary>
		public bool Renewable
		{
			get
			{
				return this.refill;
			}

			set
			{
				this.refill = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether taint.
		/// </summary>
		public bool Taint
		{
			get
			{
				return this.state;
			}

			set
			{
				this.state = value;

                // Tainted
                if (this.state) // Play particle system
                    this.gameObject.GetComponentInChildren<ParticleSystem>().Play();
            }
		}

		/// <summary>
		/// The refresh.
		/// Checks if the current amount of Minerals.
		/// Checks if Minerals is renewable.
		/// Starts a timer that will increment the amount.
		/// if it is renewable and less than a specified amount.
		/// </summary>
		public void Refresh()
		{
			if (this.amount < this.maxAmount && this.refill)
			{
				this.refillTimer += Time.fixedDeltaTime;
				if (this.refillTimer >= 15.0f)
				{
                    this.amount += 0;
					this.refillTimer = 0.0f;
				}
			}
		}

		/// <summary>
		/// The reset.
		/// Checks if Minerals are tainted.
		/// And starts a timer that will reset its state.
		/// </summary>
		public void Reset()
		{
			if (this.state)
			{
				this.resetTimer += Time.fixedDeltaTime * 2;
				if (this.resetTimer >= 60.0f)
				{
                    this.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                    this.state = false;
					this.resetTimer = 0.0f;
				}
			}
		}

		/// <summary>
		/// Use this for initialization
		/// </summary>
		private void Start()
		{
			this.maxAmount = 250;
			this.amount = this.maxAmount;
			this.refill = false;
			this.state = false;
		}

		/// <summary>
		/// Update is called once per frame
		/// </summary>
		private void Update()
		{
			this.Reset();
		}

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
                ToolTip.Self.Objectdescription = "Mineral Deposit.\n This resource provides " +
                " minerals when harvested. Minerals are used in crafting.";
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