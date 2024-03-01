using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Collections;

namespace ExSolution
{
    public class competitionData
    {
        public string name { get; set; }
        public string country { get; set; }

        public int year { get; set; }

        public string winner { get; set; }

        public string runnerup { get; set; }

        
    }
    public class classData
    {
        public string competition { get; set; }

        public int year { get; set; }

        public string round { get; set; }

        public string team1 { get; set; }

        public string team2 { get; set; }

        public string team1goals { get; set; }

        public string team2goals { get; set; }
    }

    public class CompetitionPage
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }

        public int total_pages { get; set; }

        public List<competitionData> data { get; set; }

        public CompetitionPage()
        {
            data = new List<competitionData>();
        }
    }
    public class Page
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }

        public int total_pages { get; set; }

        public List<classData> data { get; set; }

        public Page()
        {
            data = new List<classData>();
        }

    }
    public class Football
    { 
        public static string baseUrl = "https://jsonmock.hackerrank.com";

        public static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
            string result = string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", System.Net.WebUtility.UrlEncode(key), System.Net.WebUtility.UrlEncode(nvc[key]))));

            Console.WriteLine("QueryParameter - " +result);
            return result;
        }


        public static int GetTeamCount(Page s, NameValueCollection q, string teamName, int y, int pp)
        {
            int c = 0;
            int tp = 0;
            if (s == null)
                return c;


            if (s.total_pages > 0)
            {
                for (int i = 1; i <= s.total_pages; i++)
                {
                  if(q.AllKeys.Contains("page"))
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
                        HttpResponseMessage response = client.GetAsync("api/football_matches?" + ToQueryString(q)).Result;
                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;

                        Page pp1 = JsonConvert.DeserializeObject<Page>(result);

                        foreach (var t in pp1.data)
                        {
                            if (pp == 1)
                            {
                                if (t.team1.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (t.year == y)
                                    {
                                        c = c + Int32.Parse(t.team1goals);
                                    }
                                }
                            }

                            if (pp == 2)
                            {
                                if (t.team2.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (t.year == y)
                                    {
                                        c = c+ Int32.Parse(t.team2goals);
                                    }
                                }
                            }

                        }

                        
                    }
                }
            }


            return c;
        }

        public static int GetCompetitionTeamCount(CompetitionPage s, NameValueCollection q, string teamName, int y, int pp, string CName)
        {
            int c = 0;
            int tp = 0;
            if (s == null)
                return c;


            if (s.total_pages > 0)
            {
                for (int i = 1; i <= s.total_pages; i++)
                {
                    q.Remove("name");
                    q.Add("competition", CName);
                    q.Add("page", i.ToString());

                    using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                    {
                        client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                        HttpResponseMessage response = client.GetAsync("api/football_competitions?" + ToQueryString(q)).Result;
                        if (response.IsSuccessStatusCode)
                        {

                            string result = response.Content.ReadAsStringAsync().Result;

                            CompetitionPage pp1 = JsonConvert.DeserializeObject<CompetitionPage>(result);

                            foreach (var t in pp1.data)
                            {
                                if (pp == 1)
                                {
                                    if (t.name.Equals(CName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (t.year == y)
                                        {
                                            if(t.winner.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                                            {
                                                c++;
                                            }
                                            
                                        }
                                    }
                                }

                                if (pp == 2)
                                {
                                    if (t.name.Equals(CName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (t.year == y)
                                        {
                                            c++;
                                        }
                                    }
                                }

                            }

                        }
                    }
                }
            }


            return c;
        }
        public static int getTotalGoals(string team, int year)
        {
          

            int count = 0;

           var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                {"year", year.ToString() },
                {"team1", team }
                
            };

            var queryParams1 = new System.Collections.Specialized.NameValueCollection
            {
                {"year", year.ToString() },
                {"team2", team }

            };


            try
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                    HttpResponseMessage response = client.GetAsync("api/football_matches?"+ ToQueryString(queryParams)).Result;
                    if (response.IsSuccessStatusCode)
                    {


                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;

                        Page pp = JsonConvert.DeserializeObject<Page>(result);

                        int p = Football.GetTeamCount(pp, queryParams, team, year, 1);
                        //JObject ks = JObject.Parse(result);
                        count = count + p;
                    }
                    //foreach(var e in ks)
                    //{

                    //}

                    //var resultjson = JsonConvert.DeserializeObject<Page>(result);

                }

                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                    HttpResponseMessage response = client.GetAsync("api/football_matches?" + ToQueryString(queryParams1)).Result;
                    if (response.IsSuccessStatusCode)
                    {

                        string result = response.Content.ReadAsStringAsync().Result;

                        Page pp = JsonConvert.DeserializeObject<Page>(result);

                        int pp1 = Football.GetTeamCount(pp, queryParams1, team, year, 2);
                        //JObject ks = JObject.Parse(result);
                        count = count + pp1;
                    }
                    //foreach(var e in ks)
                    //{

                    //}

                    //var resultjson = JsonConvert.DeserializeObject<Page>(result);

                }
                //Console.ReadLine()


                //using (HttpClient client = new HttpClient())
                //{
                //    string urlWithParams = baseUrl + "/api/football_matches?" + ToQueryString(queryParams);

                //    HttpResponseMessage response = await client.GetAsync(urlWithParams);

                //    if (response.IsSuccessStatusCode)
                //    {
                //        string responseData = await response.Content.ReadAsStringAsync();
                //        //Console.WriteLine(responseData);
                //        count = 1;
                //        return count;
                //    }
                //    else
                //    {
                //        //Console.WriteLine($"Failed to call the API.Status code {response.StatusCode}");
                //        return -1;
                //    }
                //}
                return count;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);

                return -1;
            }

        }


        public static int getWinnerTotalGoals(string competition, int year)
        {


            int count = 0;
            string team = "Barcelona";

            var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                {"year", year.ToString() },
                {"name", competition },
                {"team1", team }

            };

            var queryParams1 = new System.Collections.Specialized.NameValueCollection
            {
                {"year", year.ToString() },
                {"name", competition },
                {"team2", team }

            };


            try
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    queryParams.Remove("team1");
                    client.BaseAddress = new Uri(baseUrl);
                    HttpResponseMessage response = client.GetAsync("api/football_competitions?" + ToQueryString(queryParams)).Result;
                    if (response.IsSuccessStatusCode)
                    {


                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;

                        CompetitionPage pp = JsonConvert.DeserializeObject<CompetitionPage>(result);
                        queryParams.Add("team1", team);
                        int p = Football.GetCompetitionTeamCount(pp, queryParams, team, year, 1, competition);
                        //JObject ks = JObject.Parse(result);
                        count = count + p;
                    }
                    //foreach(var e in ks)
                    //{

                    //}

                    //var resultjson = JsonConvert.DeserializeObject<Page>(result);

                }

                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    queryParams1.Remove("team2");
                    client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                    HttpResponseMessage response = client.GetAsync("api/football_competitions?" + ToQueryString(queryParams1)).Result;
                    if (response.IsSuccessStatusCode)
                    {

                        string result = response.Content.ReadAsStringAsync().Result;

                        CompetitionPage pp = JsonConvert.DeserializeObject<CompetitionPage>(result);

                        queryParams1.Add("team2", team);
                        int pp1 = Football.GetCompetitionTeamCount(pp, queryParams1, team, year, 2, competition);
                        //JObject ks = JObject.Parse(result);
                        count = count + pp1;
                    }
                    //foreach(var e in ks)
                    //{

                    //}

                    //var resultjson = JsonConvert.DeserializeObject<Page>(result);

                }
                //Console.ReadLine()


                //using (HttpClient client = new HttpClient())
                //{
                //    string urlWithParams = baseUrl + "/api/football_matches?" + ToQueryString(queryParams);

                //    HttpResponseMessage response = await client.GetAsync(urlWithParams);

                //    if (response.IsSuccessStatusCode)
                //    {
                //        string responseData = await response.Content.ReadAsStringAsync();
                //        //Console.WriteLine(responseData);
                //        count = 1;
                //        return count;
                //    }
                //    else
                //    {
                //        //Console.WriteLine($"Failed to call the API.Status code {response.StatusCode}");
                //        return -1;
                //    }
                //}
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return -1;
            }

        }
        
        
        
        // Football - Number of DrawnMatches
        public static int getNumDraws(int year)
        {
            int count = 0;
            int iteamGoal = 1;
            string team = "Barcelona";

            var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                {"year", year.ToString() }
                
            };
       

            var queryParams1 = new System.Collections.Specialized.NameValueCollection
            {
                {"year", year.ToString() }
                

            };


            try
            {
                for (int i = 0; i<=10; i++)
                {

                    if (queryParams.AllKeys.Contains("page"))
                    {
                        queryParams.Remove("page");
                        
                    }

                    if (queryParams.AllKeys.Contains("team1goals"))
                        {
                           queryParams.Remove("team1goals");
                           queryParams.Add("team1goals", i.ToString());
                        }
                    else
                    {
                        queryParams.Add("team1goals", i.ToString());
                    }

                    if (queryParams.AllKeys.Contains("team2goals"))
                    {
                        queryParams.Remove("team2goals");
                        queryParams.Add("team2goals", i.ToString());
                    }
                    else
                    {
                        queryParams.Add("team2goals", i.ToString());
                    }


                    using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                    {
                        client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                        HttpResponseMessage response = client.GetAsync("api/football_matches?" + ToQueryString(queryParams)).Result;
                        if (response.IsSuccessStatusCode)
                        {

                            string result = response.Content.ReadAsStringAsync().Result;

                            Page pp = JsonConvert.DeserializeObject<Page>(result);
                            Console.WriteLine($"Page - {i}");
                            Console.WriteLine($"Page- {i} - TotalPages" + pp.total_pages);
                            int p = Football.GetTeamCount1(pp, queryParams, team, year, 1, i);
                            count = count + p;
                            Console.WriteLine($"Page- {i} - TotalCount Returned" + count);
                        }

                    }
                }

                //for (int j = 1; j <=10; j++)
                //{

                    

                //    if (queryParams1.AllKeys.Contains("team1goals"))
                //    {
                //        queryParams1.Remove("team1goals");
                //    }

                //    if (queryParams1.AllKeys.Contains("team2goals"))
                //    {
                //        queryParams1.Remove("team2goals");
                //        queryParams1.Add("team2goals", j.ToString());
                //    }
                //    else
                //    {
                //        queryParams1.Add("team2goals", j.ToString());
                //    }
                //    if (queryParams1.AllKeys.Contains("page"))
                //    {
                //        queryParams1.Remove("page");
                //        queryParams1.Add("page", j.ToString());
                //    }
                //    else
                //    {
                //        queryParams1.Add("page", j.ToString());
                //    }

                //    using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                //    {
                //        client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                //        HttpResponseMessage response = client.GetAsync("api/football_matches?" + ToQueryString(queryParams1)).Result;
                //        if (response.IsSuccessStatusCode)
                //        {

                //            string result = response.Content.ReadAsStringAsync().Result;

                //            Page pp = JsonConvert.DeserializeObject<Page>(result);
                //            Console.WriteLine($"Page - {j}");
                //            Console.WriteLine($"Page- {j} - TotalPages" + pp.total_pages);
                //            int pp1 = Football.GetTeamCount1(pp, queryParams1, team, year, 2, j);
                //            //JObject ks = JObject.Parse(result);
                //            count = count + pp1;
                //            Console.WriteLine($"Page- {j} - TotalCount Returned" + count);
                //        }

                //    }
                //}
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return -1;
            }

        }
        public static int GetTeamCount1(Page s, NameValueCollection q, string teamName, int y, int pp, int goalCount)
        {
            int c = 0;
            int totalFineCount = 0; 
            int tp = 0;
            if (s == null)
                return c;

            Console.WriteLine($"InGetTEAMCount1 - TotalPages - {s.total_pages}");
            

            if (s.total_pages > 0)
            {
                for (int i = 1; i <=s.total_pages; i++)
                {
                    if (q.AllKeys.Contains("team1goals"))
                    {
                        q.Remove("team1goals");
                        q.Add("team1goals", goalCount.ToString());
                    }
                    else
                    {
                        q.Add("team1goals", goalCount.ToString());
                    }

                    if (q.AllKeys.Contains("team2goals"))
                    {
                        q.Remove("team2goals");
                        q.Add("team2goals", goalCount.ToString());
                    }
                    else
                    {
                        q.Add("team2goals", goalCount.ToString());
                    }


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
                        HttpResponseMessage response = client.GetAsync("api/football_matches?" + ToQueryString(q)).Result;
                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;

                        Page pp1 = JsonConvert.DeserializeObject<Page>(result);
                        totalFineCount = pp1.total_pages;

                        foreach (var t in pp1.data)
                        {
                            
                                if (t.year == y)
                                {
                                    if (t.team1goals.Equals(t.team2goals, StringComparison.OrdinalIgnoreCase))
                                    {
                                        c++;
                                    }

                                }

                        }


                    }
                }
            }


            return c;
        }

        

        
    }
}
