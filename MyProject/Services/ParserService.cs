using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using MyProject.DAL;
using MyProject.Models;


namespace MyProject.Services
{
    public class ParserService : System.Data.Entity.DropCreateDatabaseIfModelChanges<ParseContext>
    {
        const string url = "https://habr.com/all/";
        readonly Dictionary<string, string> months = new Dictionary<string, string>() {
            { "января" , "january" },
            { "февраля" , "febrary" },
            { "марта" , "march" },
            { "апреля" , "april" },
            { "мая" , "may" },
            { "июня" , "june" },
            { "июля" , "july" },
            { "августа" , "august" },
            { "сентября" , "september" },
            { "октября" , "october" },
            { "ноября" , "november" },
            { "декабря" , "december" }
        };

        public async Task<string> Parse(int count = 30)
        {
            ParseContext context = new ParseContext();
            context.ParsedPosts.RemoveRange(context.ParsedPosts);
            DateTime currenDate = DateTime.Now;

            int postsCounter = 0;
            int pageNum = 1;
            while (postsCounter < count)
            {

                HttpClient hc = new HttpClient();
                HttpResponseMessage result = await hc.GetAsync(url + "page" + pageNum + "/");

                Stream stream = await result.Content.ReadAsStreamAsync();

                HtmlDocument doc = new HtmlDocument();

                doc.Load(stream, Encoding.UTF8);

                HtmlNodeCollection posts = doc.DocumentNode.SelectNodes("//*[contains(@class, 'post_preview')]");

                var parsedPost = new List<ParsedPost>();
                foreach (HtmlNode post in posts)
                {

                    var postTitle = post.SelectSingleNode(".//*[contains(@class, 'post__title_link')]");
                    var title = postTitle.InnerText;
                    var link = postTitle.GetAttributeValue("href", "");

                    var text = post.SelectSingleNode(".//*[contains(@class, 'post__text post__text-html')]").InnerText;

                    var postTimeString = post.SelectSingleNode(".//*[contains(@class, 'post__meta')]").SelectSingleNode(".//*[contains(@class, 'post__time')]").InnerHtml;
                    var postDate = GetDateByString(postTimeString);
                    Regex idRegex = new Regex(@"[0-9]{1,}");
                    int ID = int.Parse(idRegex.Match(link).Value);

                    parsedPost.Add(new ParsedPost
                    {
                        ID = ID,
                        PostBody = text,
                        PostDate = postDate,
                        Topic = title,
                        ParseDate = currenDate,
                        PostUrl = link
                    });
                    postsCounter++;
                    if (postsCounter >= count)
                    {
                        break;
                    }

                }
                parsedPost.ForEach(s => context.ParsedPosts.Add(s));
                context.SaveChanges();

                pageNum++;
            }
            // return View(links);
            return "ok";
        }

        private DateTime GetDateByString(string TimeString)
        {
            string dateString = "";
            DateTime postDate = DateTime.Today;

            Regex dayRegex = new Regex(@"^[а-я]{2,}");
            Match dayMatch = dayRegex.Match(TimeString);
            if (dayMatch.Success)
            {
                if (dayMatch.Value == "вчера")
                {
                    postDate = DateTime.Today.AddDays(-1);
                }
            }
            else
            {
                Regex dateRegex = new Regex(@"^[0-9]{1,2} \S[а-я]{2,} [0-9]{4}");
                Match dateMatch = dateRegex.Match(TimeString);
                Regex monthRegex = new Regex(@"\S[а-я]{2,}");
                Match monthMatch = monthRegex.Match(TimeString);
                string date;
                date = dateMatch.Success ? dateMatch.Value.Replace(monthMatch.Value,months[monthMatch.Value]) : "";
                postDate = DateTime.ParseExact(date, "d MMMM yyyy", CultureInfo.InvariantCulture);
            }


            Regex timeRegex = new Regex(@"[0-9]{2}:[0-9]{2}");
            Match timeMatch = timeRegex.Match(TimeString);
            if (timeMatch.Success)
            {
                var timeArr = timeMatch.Value.Split(':');
                var time = new TimeSpan(0, int.Parse(timeArr[0]), int.Parse(timeArr[1]), 0, 0);
                postDate = postDate.Add(time);
            }

            return postDate;
        }

    }
}