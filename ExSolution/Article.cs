using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExSolution
{
    
    public class ArticleData
    {
        public int id { get; set; }

        public string username { get; set; }

        public string about { get; set; }

        public string updated_at { get; set; }

        public int submission_count { get; set; }

        public int comment_count { get; set; } 

        public long created_at { get; set; } 
    }

   
    public class ParentArticle
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }

        public int total_pages { get; set; }

        public List<ArticleData> data { get; set; }

        public ParentArticle()
        {
            data = new List<ArticleData>();
        }

    }


    public class Article
    {
        public static string baseUrl = "https://jsonmock.hackerrank.com";

        public static async Task<List<string>> getUserNames(int threshold)
        {
            List<string> resultStr = new List<string>();

            var queryParams = new System.Collections.Specialized.NameValueCollection();

            try
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {

                    client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                    var response = await client.GetAsync("api/article_users?" + ToQueryString(queryParams)).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {


                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;

                        ParentArticle pp = JsonConvert.DeserializeObject<ParentArticle>(result);
                        resultStr = await Article.getListOfUserNames(threshold, pp, queryParams);

                    }

                }

               
                return resultStr;
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
                string result = string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", System.Net.WebUtility.UrlEncode(key), System.Net.WebUtility.UrlEncode(nvc[key]))));
                return result;
        }

        public static async Task<List<string>> getListOfUserNames(int threshold, ParentArticle pa, NameValueCollection qp)
        {
            List<string> op = new List<string>();

            if (pa == null)
                return null;

            if (pa.total_pages > 0)
            {
                for (int i = 1; i <= pa.total_pages; i++)
                {
                    if(qp.AllKeys.Contains("page"))
                    {
                        qp.Remove("page");
                        qp.Add("page", i.ToString());
                    }
                    else
                    {
                        qp.Add("page", i.ToString());
                    }
                    
                    using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                    {
                        client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
                        HttpResponseMessage response = await client.GetAsync("api/article_users?" + ToQueryString(qp)).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                        {

                            string result = response.Content.ReadAsStringAsync().Result;

                            ParentArticle pp1 = JsonConvert.DeserializeObject<ParentArticle>(result);

                            foreach (var t in pp1.data)
                            {
                                
                                if (t.submission_count > threshold)
                                {
                                    op.Add(t.username);
                                }

                            }

                        }
                    }
                }
            }

            return op;
            
        }

        }
}
