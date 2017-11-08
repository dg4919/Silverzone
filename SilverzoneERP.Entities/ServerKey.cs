using System;
using System.Text.RegularExpressions;

namespace SilverzoneERP.Entities
{
    public class ServerKey
    {
        public const string serverapikey= "AAAAiAAg4z8:APA91bH5faB94vH8KCQSZja2nDYx0j-1cSBLJbpm83creCDSteD3jR8iLNEBOGgcg6W1nJXiNmXLqQdR6VHSGdZtZQ2ytPu0FFTB4pBaGebr7paZfunX29Ep72z7_nfkXz_pKbjkOC8q";
        //public const string serverapikey = "AIzaSyAq1lp2P9f_Kscv9H1IGIIs-Ez8qk1YzfM";
        public const string senderId = "584117707583";

        public static string Event_Previous_Year = (DateTime.Now.Month < 4 ? (((DateTime.Now.Year - 1) - 1) + "-" + (DateTime.Now.Year - 1).ToString().Substring(2, 2)) : ((DateTime.Now.Year - 1) + "-" + ((DateTime.Now.Year - 1) + 1).ToString().Substring(2, 2)));
        public static int Event_Previous_YearCode = (DateTime.Now.Month < 4 ? (((DateTime.Now.Year - 1) - 1) + (DateTime.Now.Year - 1)) : ((DateTime.Now.Year - 1) + ((DateTime.Now.Year - 1) + 1)));


        public static string Event_Current_Year = (DateTime.Now.Month < 4 ? ((DateTime.Now.Year - 1) + "-" + DateTime.Now.Year.ToString().Substring(2, 2)) : (DateTime.Now.Year + "-" + (DateTime.Now.Year + 1).ToString().Substring(2, 2)));
        public static int Event_Current_YearCode = (DateTime.Now.Month < 4 ? ((DateTime.Now.Year - 1) + DateTime.Now.Year) : (DateTime.Now.Year +  (DateTime.Now.Year + 1)));

        public static string Event_Next_Year = (DateTime.Now.Month < 4 ? ((DateTime.Now.Year) + "-" + (DateTime.Now.Year+1).ToString().Substring(2, 2)) : ((DateTime.Now.Year + 1) + "-" + (DateTime.Now.Year + 2).ToString().Substring(2, 2)));
        public static int Event_Next_YearCode = (DateTime.Now.Month < 4 ? (DateTime.Now.Year +( DateTime.Now.Year+1)) : ((DateTime.Now.Year + 1) + (DateTime.Now.Year + 2)));

        public static string  HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

    }
}
