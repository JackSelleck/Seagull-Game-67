namespace Scripts.SaveSystem
{
    public class LeaderboardManager
    {
        public void AddScore(GameState state, int newScore, int newChipsNicked, int maxEntries = 10)
        {
            state.leaderboard.Add(new LeaderboardEntryData
            {
                score = newScore,
                chipsNicked = newChipsNicked
            });

            state.leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

            if (state.leaderboard.Count > maxEntries)
            {
                state.leaderboard.RemoveRange(maxEntries, state.leaderboard.Count - maxEntries);
            }
        }
    }
}