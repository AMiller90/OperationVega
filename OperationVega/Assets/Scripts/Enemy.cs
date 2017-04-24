
namespace Assets.Scripts
{
    using System.Collections;

    using Controllers;

    using Interfaces;

    using Managers;

    using UI;

    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.UI;

    /// <summary>
    /// The enemy class.
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(EnemyAI))]
    [RequireComponent(typeof(Stats))]
    public class Enemy : MonoBehaviour, ICombat
    {
        /// <summary>
        /// The current target reference.
        /// </summary>
        [HideInInspector]
        public GameObject Currenttarget;

        /// <summary>
        /// The enemy finite state machine.
        /// Used to keep track of the enemy states.
        /// </summary>
        private readonly FiniteStateMachine<string> theEnemyFSM = new FiniteStateMachine<string>();

        /// <summary>
        /// The target to attack.
        /// </summary>
        private ICombat target;

        /// <summary>
        /// The resource to taint.
        /// </summary>
        private IResources targetResource;

        /// <summary>
        /// The my stats reference.
        /// This reference will contain all of this enemy stats data.
        /// </summary>
        private Stats mystats;

        /// <summary>
        /// The time to taint reference.
        /// </summary>
        private float timetotaint;

        /// <summary>
        /// The time between attacks reference.
        /// Stores the reference to the timer between attacks.
        /// </summary>
        private float timebetweenattacks;

        /// <summary>
        /// The navigation agent reference.
        /// </summary>
        private NavMeshAgent navagent;

        /// <summary>
        /// The enemy controller reference.\
        /// Reference to Animator Controller.
        /// </summary>
        private Animator enemycontroller;

        /// <summary>
        /// The death prefab reference.
        /// Reference to a rag doll prefab.
        /// </summary>
        [SerializeField]
        private GameObject deathPrefab;

        /// <summary>
        /// The particle system reference.
        /// The particle system that simulates the taint attack.
        /// </summary>
        [SerializeField]
        private ParticleSystem particlesystem;

        /// <summary>
        /// The attack function gives the enemy functionality to attack.
        /// </summary>
        public void Attack()
        {
            if (this.timebetweenattacks >= this.mystats.Attackspeed && Vector3.Distance(this.Currenttarget.transform.position, this.transform.position) <= this.mystats.Attackrange)
            {
                if (this.navagent.velocity == Vector3.zero && this.mystats.Health > 0)
                {
                    Debug.Log("Enemy attacked!");
                    this.timebetweenattacks = 0;
                    this.enemycontroller.SetTrigger("AttackTrigger");
                }
            }
        }

        /// <summary>
        /// The take damage function allows an enemy to take damage.
        /// <para></para>
        /// <remarks><paramref name="damage"></paramref> -The amount to be calculated when the object takes damage.</remarks>
        /// </summary>
        public void TakeDamage(int damage)
        {
            this.mystats.Health -= damage;

            // Check if unit dies
            if (this.mystats.Health <= 0)
            {
                // Switch to death animation
                this.enemycontroller.SetTrigger("Death");
            }
        }

        /// <summary>
        /// The change states function.
        /// This function changes the state to the passed in state.
        /// <para></para>
        /// <remarks><paramref name="destinationState"></paramref> -The state to transition to.</remarks>
        /// </summary>
        public void ChangeStates(string destinationState)
        {
            string thecurrentstate = this.theEnemyFSM.CurrentState.Statename;

            switch (destinationState)
            {
                case "Battle":
                    this.target = (ICombat)this.Currenttarget.GetComponent(typeof(ICombat));
                    this.theEnemyFSM.Feed(thecurrentstate + "To" + destinationState);
                    break;
                case "Idle":
                    this.theEnemyFSM.Feed(thecurrentstate + "To" + destinationState);
                    break;
                case "TaintResource":
                    this.targetResource = (IResources)this.Currenttarget.GetComponent(typeof(IResources));
                    this.theEnemyFSM.Feed(thecurrentstate + "To" + destinationState);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The on death function.
        /// Provides the functionality on when the enemy dies.
        /// This function is called in the animator, under events for the death animation.
        /// </summary>
        public void OnDeath()
        {
            ObjectiveManager.Instance.TheObjectives[ObjectiveType.Kill].Currentvalue++;
            Vector3 pos = this.transform.position;
            Quaternion rot = Quaternion.AngleAxis(-70, this.transform.forward);

            UnitController.Self.RagDoll = Instantiate(this.deathPrefab, pos, rot);
            Destroy(this.gameObject);
        }

        /// <summary>
        /// The on taint function.
        /// Provides the functionality on when the enemy taints a resource.
        /// This function is called in the animator, under events for the taint animation.
        /// </summary>
        public void OnTaint()
        {
            // Incase the unit was walking before tainting, set it to false
            this.enemycontroller.SetBool("Walk", false);
            this.enemycontroller.SetBool("Taint", false);
            this.enemycontroller.SetTrigger("Idle");
            this.particlesystem.Play();
            this.targetResource.Taint = true;
            this.targetResource = null;
        }

        /// <summary>
        /// The on unit hit function.
        /// This function is called as an animation event function in the attack animation.
        /// </summary>
        public void OnUnitHit()
        {
            if (Vector3.Distance(this.Currenttarget.transform.position, this.transform.position) > this.mystats.Attackrange)
            {
                this.enemycontroller.SetBool("Walk", false);
                this.navagent.SetDestination(this.transform.position);
                this.enemycontroller.SetTrigger("Idle");
            }
            else
            {
                this.enemycontroller.SetTrigger("Idle");
                this.enemycontroller.SetBool("Walk", false);
                IUnit u = this.target as IUnit;
                u.AutoTarget(this.gameObject);
                this.target.TakeDamage(this.mystats.Strength);

                // If unit is not null
                if (UnitController.Self.Unithit != null && UnitController.Self.Unithit.GetComponent<Stats>().Health > 0)
                {
                    // Start a coroutine to print the text to the screen -
                    // It is a coroutine to assist in helping prevent text objects from
                    // spawning on top one another.
                    this.StartCoroutine(UnitController.Self.CombatText(UnitController.Self.Unithit, Color.white, null));
                }
            }
        }

        /// <summary>
        /// The taint function allows the enemy to taint a resource.
        /// </summary>
        public void Taint()
        {
            if (Vector3.Distance(this.Currenttarget.transform.position, this.transform.position) <= this.mystats.Attackrange && this.timetotaint >= 3)
            {
                if (this.navagent.velocity == Vector3.zero)
                {
                    this.enemycontroller.SetBool("Taint", true);
                    this.ChangeStates("Idle");
                    this.timetotaint = 0;
                }
            }
        }

        /// <summary>
        /// The awake function.
        /// </summary>
        private void Awake()
        {
            this.theEnemyFSM.CreateState("Init", null);
            this.theEnemyFSM.CreateState("Idle", null);
            this.theEnemyFSM.CreateState("Battle", null);
            this.theEnemyFSM.CreateState("TaintResource", null);

            this.theEnemyFSM.AddTransition("Init", "Idle", "auto");
            this.theEnemyFSM.AddTransition("Idle", "Battle", "IdleToBattle");
            this.theEnemyFSM.AddTransition("Battle", "Idle", "BattleToIdle");
            this.theEnemyFSM.AddTransition("Idle", "TaintResource", "IdleToTaintResource");
            this.theEnemyFSM.AddTransition("TaintResource", "Idle", "TaintResourceToIdle");
            this.theEnemyFSM.AddTransition("Battle", "TaintResource", "BattleToTaintResource");
            this.theEnemyFSM.AddTransition("TaintResource", "Battle", "TaintResourceToBattle");
        }

        /// <summary>
        /// The start function
        /// </summary>
        private void Start()
        {
            this.mystats = this.GetComponent<Stats>();
            this.mystats.Health = 100;
            this.mystats.Maxhealth = 100;
            this.mystats.Strength = 3;
            this.mystats.Defense = 2;
            this.mystats.Speed = 2;
            this.mystats.Attackspeed = 2;
            this.mystats.MaxSkillCooldown = 20;
            this.mystats.CurrentSkillCooldown = this.mystats.MaxSkillCooldown;
            this.mystats.Attackrange = 2.0f;

            this.timetotaint = 3;
            this.timebetweenattacks = this.mystats.Attackspeed;
            this.navagent = this.GetComponent<NavMeshAgent>();
            this.navagent.speed = this.mystats.Speed;
            this.enemycontroller = this.GetComponent<Animator>();
            this.theEnemyFSM.Feed("auto");
        }

        /// <summary>
        /// The battle state function.
        /// The function called while in the battle state.
        /// </summary>
        private void BattleState()
        {
            if (this.target != null)
            {
                // If the unit has died
                if (this.Currenttarget == null)
                {
                    this.target = null;
                    this.enemycontroller.SetBool("Walk", false);
                    this.navagent.SetDestination(this.transform.position);
                    this.enemycontroller.SetTrigger("Idle");
                    this.ChangeStates("Idle");
                }
                // If unit is alive but out of range
                else if (Vector3.Distance(this.Currenttarget.transform.position, this.transform.position) > this.GetComponent<EnemyAI>().Radius)
                {
                    if (!this.enemycontroller.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        this.enemycontroller.SetBool("Walk", false);
                        this.navagent.SetDestination(this.transform.position);
                        this.enemycontroller.SetTrigger("Idle");
                    }

                    this.Currenttarget = null;
                    this.target = null;
                    this.ChangeStates("Idle");
                    this.GetComponent<EnemyAI>().taunted = false;
                }
                else
                {
                    this.Attack();
                }
            }
        }

        /// <summary>
        /// The Taint Resource state function.
        /// The function called while in the taint resource state.
        /// </summary>
        private void TaintResourceState()
        {
            if (this.targetResource != null && !this.targetResource.Taint)
            {
                this.Taint();
            }
        }

        /// <summary>
        /// The update enemy function.
        /// This updates the enemy behavior.
        /// </summary>
        private void UpdateEnemy()
        {
            this.timebetweenattacks += 1 * Time.deltaTime;
            this.timetotaint += 1 * Time.deltaTime;

            switch (this.theEnemyFSM.CurrentState.Statename)
            {
                case "Idle":
                    break;
                case "Battle":
                    this.BattleState();
                    break;
                case "TaintResource":
                    this.TaintResourceState();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The update rotation function.
        /// This function updates the enemy rotation accordingly.
        /// </summary>
        private void UpdateRotation()
        {
            if (this.Currenttarget != null)
            {
                Vector3 dir = this.Currenttarget.transform.position - this.transform.position;
                Quaternion lookrotation = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(this.transform.rotation, lookrotation, Time.deltaTime * 5).eulerAngles;
                this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
        }

        /// <summary>
        /// The update function.
        /// </summary>
        private void Update()
        {
            this.UpdateEnemy();
            this.UpdateRotation();
        }
    }
}