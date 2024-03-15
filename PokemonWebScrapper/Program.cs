using CsvHelper;
using HtmlAgilityPack;
using PokemonWebScrapper.Model;
using System.Globalization;

namespace PokemonWebScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Scraper();
        }

        private static void Scraper() 
        {
            var web = new HtmlWeb();
            var document = web.Load("https://scrapeme.live/shop/");
            var pokemonProducts = new List<PokemonProduct>();

            // selecting all HTML product elements from the current page 
            var productHTMLElements = document.DocumentNode.QuerySelectorAll("li.product");

            pokemonProducts = ProductHtmlElements(productHTMLElements);
            ExportDataToCsv(pokemonProducts: pokemonProducts);
        }
        private static List<PokemonProduct>? ProductHtmlElements(IList<HtmlNode>? productHTMLElements)
        {
            List<PokemonProduct> pokemonProducts = new();

            foreach (var productHTMLElement in productHTMLElements)
            {
                var url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes["href"].Value);
                var image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img").Attributes["src"].Value);
                var name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("h2").InnerText);
                var price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector(".price").InnerText);

                var pokemonProduct = new PokemonProduct() { Url = url, Image = image, Name = name, Price = price };

                pokemonProducts.Add(pokemonProduct);
            }

            return pokemonProducts;
        }

        private static void ExportDataToCsv(List<PokemonProduct>? pokemonProducts)
        {
            using (var writer = new StreamWriter("pokemon-products.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(pokemonProducts);
            }
        }
    }
}
