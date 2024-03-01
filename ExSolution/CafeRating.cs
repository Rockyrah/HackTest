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
    public class CafeRatingRecord
    {
        public string city { get; set; }
        public string name { get; set; }
        public int estimated_cost { get; set; }
        public UserRating user_rating { get; set; }
        public int id { get; set; }
    }

    public class CafeRatingPage
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<CafeRatingRecord> data { get; set; }
    }

    public class UserRating
    {
        public double average_rating { get; set; }
        public int votes { get; set; }
    }


    public class CafeRating
    {
        public static string baseUrl = "https://jsonmock.hackerrank.com";

        public static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
            string result = string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", System.Net.WebUtility.UrlEncode(key), System.Net.WebUtility.UrlEncode(nvc[key]))));
            return result;
        }

        public static async Task<List<string>> GetFoodOutLetInCity(string city)
        {
            List<string> food = new List<string>();

            var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                 {"city", city }
            };


            if (string.IsNullOrEmpty(city))
            {
                food.Add("-1");
                return food;
            }

            try
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {

                    client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                    var response = await client.GetAsync("api/food_outlets?" + ToQueryString(queryParams)).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {   
                        string result = response.Content.ReadAsStringAsync().Result;

                        CafeRatingPage pp = JsonConvert.DeserializeObject<CafeRatingPage>(result);

                        if (pp.total_pages > 1)
                        {
                            food = await getFoodCourts(pp, queryParams).ConfigureAwait(false);

                        }
                        else
                        {

                            foreach (var ttp in pp.data)
                            {
                                food.Add(ttp.name);
                            }

                            if (food.Count > 0)
                            {
                                //Don't do anything
                            }
                            else
                            {
                                food.Add("-1");
                            }
                        }
                    }

                }


                return food;
            }
            catch (Exception ex)
            {
                food.Add("-1");
                return food;
            }

        }

        public static async Task<List<string>> getFoodCourts(CafeRatingPage pp, NameValueCollection q)
        {
            List<string> ss = new List<string>();

            if(pp.total_pages > 1)
            {
                for(int i=1;i<=pp.total_pages;i++)
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

                        client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                        var response = await client.GetAsync("api/food_outlets?" + ToQueryString(q)).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;

                            CafeRatingPage pp12 = JsonConvert.DeserializeObject<CafeRatingPage>(result);

                            if (pp12.data.Count > 0)
                            {
                                foreach (var t in pp12.data)
                                {
                                    ss.Add(t.name+"-"+t.user_rating+"-"+t.estimated_cost);

                                    if(t.estimated_cost < 120)
                                    {
                                        ss.Add(t.name);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            

            return ss;
        }

    }
}
