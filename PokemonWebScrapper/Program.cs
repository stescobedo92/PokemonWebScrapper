using CsvHelper;
using HtmlAgilityPack;
using PokemonWebScrapper.Model;
using System.Globalization;
using System.Linq;

namespace PokemonWebScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebScrapper.Scraper();
        }
    }
}
