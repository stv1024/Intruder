using UnityEngine;

namespace Assets.Scripts.UI.Foreground.Leaderboard
{
    /// <summary>
    /// 排行榜条目
    /// </summary>
    public class LeaderboardSlot : MonoBehaviour
    {
        public UILabel LblRank;
        public UILabel LblNickname;
        public UILabel LblScore;

        public void Refresh(int rank, string nickname, int score)
        {
            LblRank.text = rank + ".";
            LblNickname.text = nickname;
            LblScore.text = "" + score;
        }
    }
}