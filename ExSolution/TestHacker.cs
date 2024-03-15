using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;

namespace ExSolution
{

    public class FoodOutletRecord
    {
        public string city { get; set; }
        public string name { get; set; }
        public int estimated_cost { get; set; }
        public UserRating user_rating { get; set; }
        public int id { get; set; }
    }

    public class FoodOutletPage
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<FoodOutletRecord> data { get; set; }
    }

    public class UserRating
    {
        public double average_rating { get; set; }
        public int votes { get; set; }
    }




    public class TestHacker
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

        public static string bestRestaurant(string city, int cost)
        {
            string food=null;
            int previouscost = 0;
            string previousfood=null;

            var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                 {"city", city },
                 
            };

            double rating = 0.0;
            double inputrating = 0;
            

            if ( (string.IsNullOrEmpty(city)) || cost <= 0 )
            {   
                return null;
            }

            try
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {

                    client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                    var response = client.GetAsync("api/food_outlets?" + ToQueryString(queryParams)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;

                        FoodOutletPage pp = JsonConvert.DeserializeObject<FoodOutletPage>(result);

                        if (pp.total_pages > 1)
                        {
                            food = getFoodCourts(pp, queryParams,city, cost );

                        }
                        else
                        {

                            foreach (var ttp in pp.data)
                            {
                                if(ttp.estimated_cost <= cost)
                                {
                                    Console.WriteLine($"Restaurant - {ttp.name} - {ttp.estimated_cost}");
                                    if (ttp.user_rating != null)
                                    {
                                        if (inputrating <= ttp.user_rating.average_rating)
                                        {
                                            if (inputrating == ttp.user_rating.average_rating)
                                            {
                                                if(previouscost < ttp.estimated_cost)
                                                {
                                                    inputrating = ttp.user_rating.average_rating;
                                                    previouscost = previouscost;
                                                    previousfood = previousfood;
                                                    food = previousfood;
                                                }
                                                else
                                                {
                                                    inputrating = ttp.user_rating.average_rating;
                                                    previouscost = ttp.estimated_cost;
                                                    previousfood = ttp.name;
                                                    food = ttp.name;
                                                }
                                                
                                            }
                                            else
                                            {
                                                inputrating = ttp.user_rating.average_rating;
                                                previouscost = ttp.estimated_cost;
                                                previousfood = ttp.name;
                                                food = ttp.name;

                                            }
                                        }
                                    }
                                    
                                }
                            }

                            
                        }
                    }

                }


                return food;
            }
            catch (Exception ex)
            {
                
                return null;
            }

        }

        public static string getFoodCourts(FoodOutletPage pp, NameValueCollection q, string city, int cost)
        {
            List<string> ss = new List<string>();

            string food1 = null;
            int previouscost1 = 0;
            string previousfood1 = null;
            double rating1 = 0.0;
            double inputrating1 = 0;

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

                        client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                        var response = client.GetAsync("api/food_outlets?" + ToQueryString(q)).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;

                            FoodOutletPage pp12 = JsonConvert.DeserializeObject<FoodOutletPage>(result);

                            if (pp12.data.Count > 0)
                            {
                                foreach (var ttp in pp12.data)
                                {
                                    if (ttp.estimated_cost <= cost)
                                    {
                                        Console.WriteLine($"Restaurant - {ttp.name} - {ttp.estimated_cost}");
                                        if (ttp.user_rating != null)
                                        {
                                            if (inputrating1 <= ttp.user_rating.average_rating)
                                            {
                                                if (inputrating1 == ttp.user_rating.average_rating)
                                                {
                                                    if (previouscost1 <= ttp.estimated_cost)
                                                    {
                                                        inputrating1 = ttp.user_rating.average_rating;
                                                        previouscost1 = previouscost1;
                                                        previousfood1 = previousfood1;
                                                        food1 = previousfood1;
                                                    }
                                                    else
                                                    {
                                                        inputrating1 = ttp.user_rating.average_rating;
                                                        previouscost1 = ttp.estimated_cost;
                                                        previousfood1 = ttp.name;
                                                        food1 = ttp.name;
                                                    }

                                                }
                                                else
                                                {
                                                    inputrating1 = ttp.user_rating.average_rating;
                                                    previouscost1 = ttp.estimated_cost;
                                                    previousfood1 = ttp.name;
                                                    food1 = ttp.name;

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }


            return food1;
        }


    }
}
