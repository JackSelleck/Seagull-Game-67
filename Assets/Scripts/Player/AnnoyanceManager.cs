using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class AnnoyanceManager : MonoBehaviour
    {
        private int annoyance;
        public int Annoyance { get => annoyance; private set => annoyance = Mathf.Max(0, value); }

        public void IncreaseAnnoyance(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError("Trying to increase annoyance negative amount...");
            }

            int prevAnnoyance = annoyance;
            Annoyance += amount;
            Debug.Log($"Increased annoyance from {prevAnnoyance} to {annoyance}");
        }
        public void DecreaseAnnoyance(int amount)
        {
            if (amount > 0)
            {
                Debug.LogError("Trying to decrease annoyance positive amount...");
            }

            int prevAnnoyance = annoyance;
            Annoyance -= amount;
            Debug.Log($"Increased annoyance from {prevAnnoyance} to {annoyance}");
        }
    }
}