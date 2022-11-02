using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    //Make combat event broker a child of a generalized event class? no need to be monobehavior? Needs a separate way to init if so
    public class CombatEventBroker : MonoBehaviour
    {
        //Establish Singleton
        public static CombatEventBroker instance = null;
        public CombatEventBroker()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Error: Singleton not initialized"); //Error
            }
        }

        //Events are established based on the Action delegate, invocations can be called to raise the event/notify subscribers
        public event Action<int> onUnitSelect;
        public event Action<int> onUnitDoubleSelect;
        public event Action<int> onUnitMouseOver;
        public event Action<int> onUnitMouseOut;
        public event Action onResetWindows;

        public event Action<Deck> onPublishDeck;
        public event Action onDrawOneCard;

        public event Action<CardUnit> onUnitDataAvailable;
        public event Action<CardUnit, Vector3> onCardInHandSelected;

        public void UnitSelect(int id)
        {
            onUnitSelect?.Invoke(id);
        }
        public void UnitDoubleSelect(int id)
        {
            onUnitDoubleSelect?.Invoke(id);
        }
        public void UnitMouseOver(int id)
        {
            onUnitMouseOver?.Invoke(id);
        }
        public void UnitMouseOut(int id)
        {
            onUnitMouseOut?.Invoke(id);
        }

        public void ResetWindows()
        {
            onResetWindows?.Invoke();
        }

        public void PublishDeck(Deck deck)
        {
            onPublishDeck?.Invoke(deck);
        }
        public void DrawOneCard()
        {
            onDrawOneCard?.Invoke();
        }

        public void UnitDataAvailable(CardUnit unit)
        {
            onUnitDataAvailable?.Invoke(unit);
        }

        public void CardInHandSelected(CardUnit unit, Vector3 pos)
        {
            onCardInHandSelected?.Invoke(unit, pos);
        }

    }
}
