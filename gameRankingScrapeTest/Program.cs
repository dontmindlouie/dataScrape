using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
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
            Console.WriteLine("List of game names:");
            foreach(var name in gameName)
            {
                Console.WriteLine(name);
            }
            string connstring = "Data Source=DESKTOP-5LVATAU\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";
            string insertQuery = "INSERT INTO dbo.GameRankings (gameName) VALUES (@gameName)";
            using (SqlConnection conn = new SqlConnection(connstring))
            using (SqlCommand command = new SqlCommand(insertQuery, conn))
            {
                command.Parameters.Add("@gameName", SqlDbType.NVarChar);
                conn.Open();
                foreach(var name in gameName)
                {
                    command.Parameters["@gameName"].Value = name;
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
            Console.WriteLine("\nGame rankings have been saved in the SQL database.");
            Console.ReadLine();
            //change 1 test

        }
    }
}
