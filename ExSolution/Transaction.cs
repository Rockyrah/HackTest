using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExSolution
{
    public class TransactionRecord
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public object timestamp { get; set; }
        public string txnType { get; set; }
        public string amount { get; set; }
        public Location location { get; set; }
        public string ip { get; set; }
    }

    public class Location
    {
        public int id { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public int zipCode { get; set; }
    }

    public class TransactionPage
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<TransactionRecord> data { get; set; }
    }
    public class Transaction
    {
        public static string BASE_URL = "https://jsonmock.hackerrank.com";

        //public static Dictionary<int, decimal> transData = new Dictionary<int, decimal>();

        //SortedDictionary
        public static SortedDictionary<int, decimal> transData = new SortedDictionary<int, decimal>();
        public static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
            string result = string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", System.Net.WebUtility.UrlEncode(key), System.Net.WebUtility.UrlEncode(nvc[key]))));
            return result;
        }

        public static List<List<object>> GetTransactions(int inputid, string txnType)
        {
            List<List<object>> result = new List<List<object>>();

            List<string> optionList = new List<string>
            { "debit", 
              "credit",};

            decimal oo;
            List<object> resultList = new List<object>();

            if (inputid <=0 || !(optionList.Any(s => txnType.Contains(s))))
            {
                for (int i = 0; i < 1; i++)
                {
                    List<object> row = new List<object>();

                    for (int j = 0; j < 2; j++)
                    {
                        row.Add(-1);
                           
                    }
                    result.Add(row);
                }
                return result;
            }

            var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                 {"txnType", txnType }
            };


            try
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {

                    client.BaseAddress = new Uri(BASE_URL);
                    var response = client.GetAsync("api/transactions/search?" + ToQueryString(queryParams)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string result1 = response.Content.ReadAsStringAsync().Result;

                        TransactionPage pp = JsonConvert.DeserializeObject<TransactionPage>(result1);

                        if (pp.total_pages > 1)
                        {
                            result = getTransactionDetails(pp, queryParams, inputid, txnType);

                        }
                        else
                        {
                            
                            foreach (var ttp in pp.data)
                            {
                                
                                if(ttp.location != null)
                                {
                                    if(ttp.location.id == inputid && ttp.txnType.Equals(txnType, StringComparison.OrdinalIgnoreCase))
                                    {
                                        oo = decimal.Parse(ttp.amount);
                                        resultList.Add(ttp.location.id);
                                        resultList.Add(oo.ToString());
                                    }
                                }
                            }

                          
                        }
                    }

                }


                return result;
            }
            catch (Exception ex)
            {
                for (int i = 0; i < 1; i++)
                {
                    List<object> row = new List<object>();

                    for (int j = 0; j < 2; j++)
                    {
                        row.Add(-1);

                    }
                    result.Add(row);
                }
                return result;
            }

        }

        public static List<List<object>> getTransactionDetails(TransactionPage pp, NameValueCollection q, int inputlID, string txType)
        {
            List<List<object>> finalss = new List<List<object>>();
            List<object> ss = new List<object>();
            decimal oo = 0.0m;

            if (pp.total_pages > 1)
            {
                for (int i = 1; i <= pp.total_pages; i++)
                {
                    if (q.AllKeys.Contains("page"))
                    {
                        q.Remove("page");
                        q.Add("page", i.ToString());
                    }
                    else
                    {
                        q.Add("page", i.ToString());
                    }

                    using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                    {

                        client.BaseAddress = new Uri(BASE_URL);
                        var response = client.GetAsync("api/transactions/search?" + ToQueryString(q)).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;

                            TransactionPage pp12 = JsonConvert.DeserializeObject<TransactionPage>(result);

                            if (pp12.data.Count > 0)
                            {
                                foreach (var t in pp12.data)
                                {
                                    if (t.location != null)
                                    {
                                        if (t.location.id == inputlID && t.txnType.Equals(txType, StringComparison.OrdinalIgnoreCase))
                                        {
                                            oo = decimal.Parse(t.amount.Replace("$", "").Replace(",", ""));
                                                
                                            AddDictTransaction(t.userId, oo );
                                        }
                                    }

                                }
                            }
                        }
                    }

                }

                var arr = transData.Select(kvp => new[] { kvp.Key, kvp.Value }).ToArray();


                foreach (var pppp in transData)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        List<object> row = new List<object>();

                        for (int j = 0; j < 2; j++)
                        {

                            if (j == 0)
                            {
                                row.Add(pppp.Key);
                                
                            }

                            if (j == 1)
                            {
                                row.Add(pppp.Value);
                                
                            }
                        }
                        finalss.Add(row);

                    }

                }

            }


            return finalss;
        }

        public static void AddDictTransaction(int userId, decimal amount)
        {
            if (transData.ContainsKey(userId))
            {
                transData[userId] = transData[userId] + amount;
            }
            else
            {
                transData.Add(userId, amount);
            }
        }
    }
}
