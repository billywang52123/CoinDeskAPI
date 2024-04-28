using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using static CoinDeskAPI.Models.CoinDeskModel;
using System.Globalization;
using CoinDeskAPI.Models;
using Microsoft.EntityFrameworkCore;
using static CoinDeskAPI.Core.EncryptionAndDecrypt;

namespace CoinDeskAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CoinDeskController : ControllerBase
    {
        string aesKey = "QW07ScdU0WvV0WVm"; // 16 bit AES 金鑰
        static readonly HttpClient client = new HttpClient();
        private readonly CurrencyContext _context;
        private readonly ICurrencyDictionary _currencyDictionary;

        public CoinDeskController(CurrencyContext context, ICurrencyDictionary currencyDictionary)
        {
            _context = context;
            _currencyDictionary = currencyDictionary;
        }

        [HttpGet("GetTransformedDataAndSet")]
        public async Task<IActionResult> GetTransformedDataAndSet()
        {
            try
            {
                var response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var coinDeskData = JsonSerializer.Deserialize<CoinDeskData>(content);
                var newData = new
                {
                    cointime = new
                    {
                        Updated = DateTime.ParseExact(coinDeskData.time.updated, "MMM dd, yyyy HH:mm:ss UTC", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd HH:mm:ss"),
                        UpdatedISO = coinDeskData.time.updatedISO,
                        Updateduk = coinDeskData.time.updateduk
                    },
                    currencies = new List<object>()
                };
                var currencyItems = await _context.CurrencyItems.FromSqlRaw("EXEC GetAllCurrency").ToListAsync();
                DateTime currentTime = DateTime.Now;
                string now = currentTime.ToString("yyyy/MM/dd HH:mm:ss");
                var orderlist = coinDeskData.bpi.OrderBy(X => X.Key);
                var currencyNames = _currencyDictionary.CurrencyNames;
                foreach (var currency in orderlist)
                {
                    newData.currencies.Add(new
                    {
                        Code = currency.Key,
                        Symbol = currency.Value.symbol,
                        Rate = currency.Value.rate
                    });
                    string encrypted_Key = AESEncrypt(currency.Key, aesKey);

                    if (currencyItems.Any(X => X.Currency == encrypted_Key))
                    {

                        await _context.Database.ExecuteSqlRawAsync("EXEC UpdateCurrency_ByCurrency {0}, {1}, {2}, {3}, {4}",
                         encrypted_Key, currency.Value.symbol, currency.Value.rate, double.Parse(currency.Value.rate), newData.cointime.Updated);
                    }
                    else
                    {
                        if (currencyNames.Any(X => X.Key == currency.Key.ToUpper()))
                        {
                            KeyValuePair<string, List<string>> CurremcyList = currencyNames.Where(X => X.Key == currency.Key).FirstOrDefault();
                            await _context.Database.ExecuteSqlRawAsync("EXEC AddCurrency {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                                encrypted_Key, currency.Value.symbol, currency.Value.rate, "",
                                double.Parse(currency.Value.rate), CurremcyList.Value[0], CurremcyList.Value[1], currency.Key, now, newData.cointime.Updated);
                        }
                        else
                        {
                            await _context.Database.ExecuteSqlRawAsync("EXEC AddCurrency {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                                    encrypted_Key, currency.Value.symbol, currency.Value.rate, "",
                                    double.Parse(currency.Value.rate), "", "", currency.Key, now, newData.cointime.Updated);
                        }
                    }

                }
                return Ok(newData);

            }
            catch (HttpRequestException ex)
            {
                // API 請求失敗，可能是網絡問題等原因
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            catch (JsonException ex)
            {
                // JSON 解析錯誤
                return StatusCode(500, "Error parsing JSON response: " + ex.Message);
            }
            catch (Exception ex)
            {
                // 其他未預期的錯誤
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }

        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyContext _context;
        string aesKey = "QW07ScdU0WvV0WVm"; // 16 bit AES 金鑰
        public CurrencyController(CurrencyContext context)
        {
            _context = context;
        }

        // GET: api/Currency
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyItem>>> GetAllCurrency()
        {
            try
            {
                // 執行預存程序
                var currencyItems = await _context.CurrencyItems.FromSqlRaw("EXEC GetAllCurrency").ToListAsync();

                foreach (var currency in currencyItems)
                {
                    currency.Currency = DecryptWithAES(currency.Currency, aesKey);
                }

                currencyItems.OrderBy(X => X.Currency);
                return currencyItems;
            }
            catch (Exception ex)
            {
                // 如果發生異常，返回 Internal Server Error
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // GET: api/Currency/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyItem>> GetCurrency_ById(int id)
        {
            try
            {
                // 執行預存程序
                var currencyItem = (await _context.CurrencyItems
                                            .FromSqlRaw("EXEC GetCurrency_ById {0}", id).ToListAsync()).FirstOrDefault();
                if (currencyItem == null)
                {
                    return NotFound();
                }

                currencyItem.Currency = DecryptWithAES(currencyItem.Currency, aesKey);

                return currencyItem;
            }
            catch (Exception ex)
            {
                // 如果發生異常，返回 Internal Server Error
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // POST: api/Currency
        [HttpPost]
        public async Task<ActionResult<PostCurrencyItem>> PostCurrencyItem(PostCurrencyItem currencyItem)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                string now = currentTime.ToString("yyyy/MM/dd HH:mm:ss");
                currencyItem.Currency = AESEncrypt(currencyItem.Currency, aesKey);
                // 執行預存程序
                await _context.Database.ExecuteSqlRawAsync("EXEC AddCurrency {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                    currencyItem.Currency, currencyItem.Symbol, currencyItem.Rate, currencyItem.Description,
                    currencyItem.Rate_float, currencyItem.Chinese, currencyItem.Japanese, currencyItem.English, now, now);

                // 獲取資料庫分配的 Id
                int newId = (int)await _context.CurrencyTables
                                                .OrderByDescending(c => c.Id)
                                                .Select(c => c.Id)
                                                .FirstOrDefaultAsync();


                return CreatedAtAction(nameof(GetCurrency_ById), new { id = newId }, currencyItem);
            }
            catch (Exception ex)
            {
                // 如果發生異常，返回 Internal Server Error
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT: api/Currency/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrencyItem(int id, CurrencyItem currencyItem)
        {
            try
            {
                if (id != currencyItem.Id)
                {
                    return BadRequest();
                }
                DateTime currentTime = DateTime.Now;
                string now = currentTime.ToString("yyyy/MM/dd HH:mm:ss");
                currencyItem.Currency = AESEncrypt(currencyItem.Currency, aesKey);
                // 執行預存程序
                await _context.Database.ExecuteSqlRawAsync("EXEC UpdateCurrency {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                    id, currencyItem.Currency, currencyItem.Symbol, currencyItem.Rate, currencyItem.Description,
                    currencyItem.Rate_float, currencyItem.Chinese, currencyItem.Japanese, currencyItem.English, now);

                return NoContent();
            }
            catch (Exception ex)
            {
                // 如果發生異常，返回 Internal Server Error
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // DELETE: api/Currency/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            try
            {
                // 執行預存程序
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteCurrency {0}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                // 如果發生異常，返回 Internal Server Error
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }



}