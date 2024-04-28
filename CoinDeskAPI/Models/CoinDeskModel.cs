using System.ComponentModel.DataAnnotations;

namespace CoinDeskAPI.Models
{
    public class CoinDeskModel
    {
        public class CoinDeskData
        {
            public TimeData time { get; set; }
            public Dictionary<string, CurrencyData> bpi { get; set; }
        }
        public class TimeData
        {
            public string updated { get; set; }
            public string updatedISO { get; set; }
            public string updateduk { get; set; }
        }

        public class CurrencyData
        {
            public string code { get; set; }
            public string symbol { get; set; }
            public string rate { get; set; }
        }

    }

    public class CurrencyItem
    {
        public int Id { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public string Rate { get; set; }

   
        public string Description { get; set; }

        [Required]
        public double Rate_float { get; set; }

 
        public string Chinese { get; set; }

   
        public string Japanese { get; set; }

     
        public string English { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }



    public class PostCurrencyItem
    {

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public string Rate { get; set; }


        public string Description { get; set; }

        [Required]
        public double Rate_float { get; set; }

 
        public string Chinese { get; set; }


        public string Japanese { get; set; }


        public string English { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }

    public interface ICurrencyDictionary
    {
        Dictionary<string, List<string>> CurrencyNames { get; }
    }

    public class CurrencyDictionary : ICurrencyDictionary
    {
        public Dictionary<string, List<string>> CurrencyNames { get; } = new Dictionary<string, List<string>>
    {
        { "USD", new List<string> { "美元", "米ドル" } },
            { "EUR", new List<string> { "歐元", "ユーロ" } },
            { "GBP", new List<string> { "英鎊", "英ポンド" } },
            { "JPY", new List<string> { "日圓", "円" } },
            { "AUD", new List<string> { "澳幣", "豪ドル" } },
            { "CAD", new List<string> { "加幣", "カナダドル" } },
            { "CHF", new List<string> { "瑞士法郎", "スイスフラン" } },
            { "CNY", new List<string> { "人民幣", "人民元" } },
            { "HKD", new List<string> { "港幣", "香港ドル" } },
            { "NZD", new List<string> { "紐西蘭幣", "ニュージーランドドル" } },
            { "SGD", new List<string> { "新加坡幣", "シンガポールドル" } },
            { "SEK", new List<string> { "瑞典幣", "スウェーデンクローナ" } },
            { "NOK", new List<string> { "挪威幣", "ノルウェークローネ" } },
            { "MXN", new List<string> { "墨西哥比索" } },
            { "KRW", new List<string> { "韓元", "ウォン" } },
            { "INR", new List<string> { "印度盧比" } },
            { "BRL", new List<string> { "巴西雷亞爾" } },
            { "RUB", new List<string> { "俄羅斯盧布" } },
            { "ZAR", new List<string> { "南非幣", "ランド" } },
            { "TRY", new List<string> { "土耳其里拉" } }
        // 其他貨幣的對應
    };
    }


}
