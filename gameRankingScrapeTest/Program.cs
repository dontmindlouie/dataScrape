using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HtmlAgilityPack;

namespace gameRankingScrapeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to scrape the gamerankings website.");
            Console.ReadLine();
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument htmlDoc = hw.Load("https://www.gamerankings.com/browse.html");
            HtmlNodeCollection tables = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"main_col\"]//div//div//table//tr//td//a");
            var gameName = tables.Select(node => node.InnerText);
            var gameName2 = "super mario";
            var gameName4 = "zelda";
            IEnumerable<string> gameName3 = gameName;
            string sqlquery = "INSERT into dbo.GameRankings (gameName) VALUES ('";
            sqlquery = sqlquery + string.Join("'), ('", gameName);
            //sqlquery = string.Concat(sqlquery, "')");
            sqlquery += "')'";
            Console.WriteLine(sqlquery);

            Console.WriteLine("\nPress a key to output the game rankings to SQL database.");
            Console.ReadLine();
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-5LVATAU\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True"))
            {
                using (SqlCommand command = conn.CreateCommand())
                {


                    //command.CommandText = "INSERT into dbo.GameRankings (gameName) VALUES (@gameName)";
                    //command.Parameters.AddWithValue("@gameName", gameName2);
                    //command.Parameters.AddWithValue("@gameName", gameName4);
                    command.CommandText = sqlquery;
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            Console.WriteLine("\nGame rankings have been saved in the SQL database.");
            Console.ReadLine();
            //change 1 test

        }
    }
}
