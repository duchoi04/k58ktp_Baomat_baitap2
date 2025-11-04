using System;
using System.Web;
using LoveApp.Lib;

namespace LoveApp.Web
{
    public partial class api : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cho phép chỉ xử lý POST hoặc GET (tùy client)
            string name1 = Request.Form["name1"];
            string name2 = Request.Form["name2"];

            // Nếu không có POST data thì thử QueryString (GET)
            if (String.IsNullOrEmpty(name1)) name1 = Request.QueryString["name1"];
            if (String.IsNullOrEmpty(name2)) name2 = Request.QueryString["name2"];

            Response.ContentType = "application/json";
            if (String.IsNullOrEmpty(name1) || String.IsNullOrEmpty(name2))
            {
                Response.Write("{\"error\":\"Missing parameters: name1 and name2 are required\"}");
                Response.End();
                return;
            }

            try
            {
                LoveCalculator calc = new LoveCalculator();
                int score = calc.CalculateLove(name1, name2);
                string message = calc.GetMessage(score);

                string escMessage = JsonEscape(message);
                string json = "{\"score\":" + score.ToString() + ",\"message\":\"" + escMessage + "\"}";
                Response.Write(json);
            }
            catch (Exception ex)
            {
                // Trả lỗi đơn giản
                string err = JsonEscape(ex.Message);
                Response.Write("{\"error\":\"" + err + "\"}");
            }
            Response.End();
        }

        // Hàm đơn giản escape chuỗi sang JSON (thích hợp cho .NET 2.0)
        private string JsonEscape(string s)
        {
            if (s == null) return "";
            s = s.Replace("\\", "\\\\");
            s = s.Replace("\"", "\\\"");
            s = s.Replace("\r", "\\r");
            s = s.Replace("\n", "\\n");
            return s;
        }
    }
}
