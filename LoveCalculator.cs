using System;

namespace LoveApp.Lib
{
    public class LoveCalculator
    {
        public int CalculateLove(string name1, string name2)
        {
            int value = (name1 + name2).Length * 37 % 101;
            return value;
        }

        public string GetMessage(int score)
        {
            if (score > 80) return "Hai bạn là một cặp hoàn hảo ❤️";
            if (score > 50) return "Có duyên nhưng cần cố gắng hơn 💌";
            return "Cần thêm thời gian để hiểu nhau 😊";
        }
    }
}
