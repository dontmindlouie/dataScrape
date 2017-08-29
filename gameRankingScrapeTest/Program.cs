﻿using System;
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
            HtmlNodeCollection gameNameNode = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"main_col\"]//div//div//table//tr//td//a");
            HtmlNodeCollection gameRankingNode = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"main_col\"]//div//div//table//tr//td/span//b");
            var gameName = gameNameNode.Select(node => node.InnerText);
            var gameRanking = gameRankingNode.Select(node => node.InnerText);
            var gameName2 = "super mario";
            Console.WriteLine("List of game names:");

            var columns = gameName.Join(gameRanking, name => name, ranking => ranking, (name, ranking) => new { Name = name, Ranking = ranking});
            var columns2 = gameName.Concat(gameRanking);
            var columns3 = gameName.Zip(gameRanking, (n, r) => new { Name = n, Ranking = r });
            foreach (var entry in columns3)
            {
                Console.WriteLine(entry.Name + " " + entry.Ranking);
            }
            

            Console.ReadLine();
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
