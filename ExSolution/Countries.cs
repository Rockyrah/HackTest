using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExSolution
{
    public class Datum
    {
        public string name { get; set; }
        public string nativeName { get; set; }
        public List<string> topLevelDomain { get; set; }
        public string alpha2Code { get; set; }
        public string numericCode { get; set; }
        public string alpha3Code { get; set; }
        public List<string> currencies { get; set; }
        public List<string> callingCodes { get; set; }
        public string capital { get; set; }
        public List<string> altSpellings { get; set; }
        public string relevance { get; set; }
        public string region { get; set; }
        public string subregion { get; set; }
        public List<string> language { get; set; }
        public List<string> languages { get; set; }
        public Translations translations { get; set; }
        public int population { get; set; }
        public List<int> latlng { get; set; }
        public string demonym { get; set; }
        public List<string> borders { get; set; }
        public int area { get; set; }
        public double gini { get; set; }
        public List<string> timezones { get; set; }
    }

    public class Root
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Translations
    {
        public string de { get; set; }
        public string es { get; set; }
        public string fr { get; set; }
        public string it { get; set; }
        public string ja { get; set; }
        public string nl { get; set; }
        public string hr { get; set; }
    }

    public class Countries
    {
        public static string baseUrl = "https://jsonmock.hackerrank.com";

        public static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
            string result = string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", System.Net.WebUtility.UrlEncode(key), System.Net.WebUtility.UrlEncode(nvc[key]))));
            return result;
        }

        public static async Task<List<string>> getCapital(string country)
        {
            List<string> resultStr = new List<string>();

            var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                 {"name", country }
            };


            if (string.IsNullOrEmpty(country))
            {
                resultStr.Add("-1");
                return resultStr;
            }
                

            try
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {

                    client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                    var response = await client.GetAsync("api/countries?" + ToQueryString(queryParams)).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {


                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;

                        Root pp = JsonConvert.DeserializeObject<Root>(result);

                        foreach (var ttp in pp.data)
                        {
                            resultStr.Add(ttp.capital.ToLower());
                        }

                        if(resultStr.Count > 0)
                        {
                            //Don't do anything
                        }
                        else
                        {
                            resultStr.Add("-1");
                        }

                    }

                }


                return resultStr;
            }
            catch (Exception ex)
            {
                resultStr.Add("-1");
                return resultStr;
            }

        }

    }
}
