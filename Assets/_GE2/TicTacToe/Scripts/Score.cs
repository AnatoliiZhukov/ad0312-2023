using TMPro;
using UnityEngine;

namespace Tictactoe
{
    public class Score : MonoBehaviour
    {
        public static Score Instance;

        [SerializeField] private TextMeshProUGUI _p1Score;
        [SerializeField] private TextMeshProUGUI _p2Score;

        public void SetPlayerScore(bool p, int score)
        {
            if (score < 100)
            {
                if (!p) _p1Score.text = score.ToString("0");
                else _p2Score.text = score.ToString("0");
            }
            else
            {
                score = 0;
                SetPlayerScore(p, score);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) SetPlayerScore(true, 80);
        }
    }
}