using System.Collections.Generic;

namespace Scripts.SaveSystem
{
    public class GameState
    {
        public List<LeaderboardEntryData> leaderboard = new();

        // Settings
        public bool invertY = false;
    }
}