using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Systems.Combat
{
    /**
     * Initializes a deployed unit on the field with the information involving a card.
     * It stores this information and updates accordingly, eventually returning the data
     * back to the player's permanant storage
     */
    public class UnitZone : MonoBehaviour, IFieldZone
    {
        private int id;
        private float doubleClickTimer = 0.5f;
        
        private CardUnit zoneData;
        private CardUnit pendingUnitData;
        private SubPhase subPhase;

        public Image portrait;
        public Image classIcon;
        public TMP_Text troopCount;

        // Start is called before the first frame update
        void Start()
        { 
            id = GetInstanceID();
            //CombatEventBroker.instance.onUnitSelect += OnUnitSelectHandler;
            //CombatEventBroker.instance.onUnitDoubleSelect += OnUnitDoubleSelectHandler;
            GameEvents.instance.onSubPhaseStarted += SetSubPhase;
            GameEvents.instance.onDeployModeRequested += SetPendingUnitData;
        }

        /*
         * Applies the data from a card onto the zone.
         * A card in hand can be deployed onto a zone during the deployment subphase
         */
        public void SetZoneData(Card cardData)
        {
            zoneData = (CardUnit)cardData;
            troopCount.text = zoneData.baseTroopCount.ToString();
            portrait.sprite = zoneData.portrait;
            classIcon.sprite = zoneData.getUnitRoleIcon();
        }
        
        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            //If current sub-phase is in DEPLOYMENT
            if(subPhase == SubPhase.DEPLOYMENT)
            {
                //Alert that a focused card has been played, so it can be removed
                GameEvents.instance.CardInHandPlayed();
                SetZoneData(pendingUnitData);
                GameEvents.instance.UnitDeployed(zoneData);
                pendingUnitData = null;
            }
            else
            {
                Debug.Log("Test Click on Deployed Card");
            }
            //StartCoroutine(OnDoubleClick());
        }

        private void SetSubPhase(SubPhase subPhase)
        {
            this.subPhase = subPhase;
        }

        private void SetPendingUnitData(CardUnit unit)
        {
            pendingUnitData = unit;
        }

        private IEnumerator OnDoubleClick()
        {
            //pause a frame so you don't pick up the same mouse down event.
            yield return new WaitForEndOfFrame();

            float count = 0f;
            while (count < doubleClickTimer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Vector3 pos = transform.position;
                    //cameraScript.StartCoroutine(cameraScript.CameraJump(pos));
                    CombatEventBroker.instance.UnitDoubleSelect(id);
                   
                    yield break;
                }
                count += Time.deltaTime;// increment counter by change in time between frames
                yield return null; // wait for the next frame
            }
        }

        private void OnUnitSelectHandler(int id)
        {
            if (id == this.id)
            {
                /*
               
                */
               
            }
            //Else just print a test message

        }
        private void OnUnitDoubleSelectHandler(int id)
        {
           // if (id == this.id) Debug.Log("double " + id);
        }
    }

}
