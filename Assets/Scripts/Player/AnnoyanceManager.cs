using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class AnnoyanceManager : MonoBehaviour
    {
        private int _annoyance;
        public int Annoyance { get => _annoyance; private set => _annoyance = Mathf.Max(0, value); }

        public void IncreaseAnnoyance(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError("Trying to increase annoyance negative amount...");
            }

            int prevAnnoyance = _annoyance;
            Annoyance += amount;
            Debug.Log($"Increased annoyance from {prevAnnoyance} to {_annoyance}");
        }
        public void DecreaseAnnoyance(int amount)
        {
            if (amount > 0)
            {
                Debug.LogError("Trying to decrease annoyance positive amount...");
            }

            int prevAnnoyance = _annoyance;
            Annoyance -= amount;
            Debug.Log($"Increased annoyance from {prevAnnoyance} to {_annoyance}");
        }
    }
}