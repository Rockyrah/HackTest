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
    public class MovieRecord
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string imdbID { get; set; }
    }

    public class MoviePage
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<MovieRecord> data { get; set; }
    }
    public class Movies
    {
        public static string baseUrl = "https://jsonmock.hackerrank.com";

        public static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
            string result = string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", System.Net.WebUtility.UrlEncode(key), System.Net.WebUtility.UrlEncode(nvc[key]))));
            return result;
        }

        public static async Task<List<string>> GetMovieTitles(string title)
        {
            List<string> resultStr = new List<string>();

            var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                 {"Title", title }
            };


            if (string.IsNullOrEmpty(title))
            {
                resultStr.Add("-1");
                return resultStr;
            }


            try
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {

                    client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                    var response = await client.GetAsync("api/movies/search?" + ToQueryString(queryParams)).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {


                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;

                        MoviePage pp = JsonConvert.DeserializeObject<MoviePage>(result);

                        if (pp.total_pages > 1)
                        {
                            resultStr = await getMovieSubtitles(pp,queryParams).ConfigureAwait(false);

                        }
                        else
                        {

                            foreach (var ttp in pp.data)
                            {
                                resultStr.Add(ttp.Title);
                            }

                            if (resultStr.Count > 0)
                            {
                                //Don't do anything
                            }
                            else
                            {
                                resultStr.Add("-1");
                            }
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

        public static async Task<List<string>> getMovieSubtitles(MoviePage pp1, NameValueCollection q)
        {
            List<string> pp3 = new List<string>();

            if(pp1.total_pages > 1)
            {
                for(int i=0;i<pp1.total_pages; i++)
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
                        var response = await client.GetAsync("api/movies/search?" + ToQueryString(q)).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                        {


                            response.EnsureSuccessStatusCode();
                            string result = response.Content.ReadAsStringAsync().Result;

                            MoviePage pp12 = JsonConvert.DeserializeObject<MoviePage>(result);

                            if (pp1.data.Count > 0)
                            {
                                foreach (var t in pp12.data)
                                {
                                    pp3.Add(t.Title);
                                }
                            }
                        }
                }   }
            }

            return pp3;
        }

    }
}
