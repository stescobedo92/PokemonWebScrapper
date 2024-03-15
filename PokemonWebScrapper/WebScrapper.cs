using CsvHelper;
using HtmlAgilityPack;
using PokemonWebScrapper.Adapters;
using PokemonWebScrapper.Extra;
using PokemonWebScrapper.Model;
using System.Globalization;

namespace PokemonWebScrapper;

public static class HtmlNodeExtensions
{
    public static string GetAttributeValue(this IHtmlNode htmlNode, string attributeName, string defaultValue = "")
    {
        if (htmlNode == null)
        {
            return defaultValue;
        }

        var attribute = htmlNode.QuerySelector(attributeName);
        return attribute != null ? attribute.InnerText : defaultValue;
    }
}

public class WebScrapper
{
    /// <summary>
    /// Scrapes data from a website and exports it to a CSV file.
    /// </summary>
    /// <remarks>
    /// This method scrapes product data from the specified website and exports it to a CSV file.
    /// </remarks>
    /// <seealso cref="HtmlWeb"/>
    /// <seealso cref="HtmlDocument"/>
    /// <seealso cref="PokemonProduct"/>
    /// <seealso cref="ExportDataToCsv(List{PokemonProduct})"/>
    public static void Scraper()
    {
        var web = new HtmlWeb();
        var document = web.Load("https://scrapeme.live/shop/");
        var pokemonProducts = new List<PokemonProduct>();

        // selecting all HTML product elements from the current page 
        var productHTMLElements = document.DocumentNode.QuerySelectorAll("li.product")
                                                   .Select(node => new HtmlNodeAdapter(node))
                                                   .ToList();

        pokemonProducts.AddRange(ProductHtmlElements(productHTMLElements));
        ExportDataToCsv(pokemonProducts);
    }

    /// <summary>
    /// Extracts Pokemon product information from HTML elements.
    /// </summary>
    /// <param name="productHTMLElements">The HTML elements representing Pokemon products.</param>
    /// <returns>A list of PokemonProduct objects extracted from the HTML elements, or null if the input is null.</returns>
    /// <remarks>
    /// This method takes a collection of HTML elements representing Pokemon products and extracts relevant information such as URL, image, name, and price.
    /// It returns a list of PokemonProduct objects containing this information.
    /// </remarks>
    /// <seealso cref="IHtmlNode"/>
    /// <seealso cref="PokemonProduct"/>
    private static IEnumerable<PokemonProduct> ProductHtmlElements(List<HtmlNodeAdapter> productHTMLElements)
    {
        if (productHTMLElements == null)
            return new List<PokemonProduct>();

        List<PokemonProduct> pokemonProducts = new();

        foreach (var productHTMLElement in productHTMLElements)
        {
            var url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a")?.GetAttributeValue("href") ?? "");
            var image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img")?.GetAttributeValue("src") ?? "");
            var name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("h2")?.InnerText ?? "");
            var price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector(".price")?.InnerText ?? "");

            var pokemonProduct = new PokemonProduct() { Url = url, Image = image, Name = name, Price = price };
            pokemonProducts.Add(pokemonProduct);
        }

        return pokemonProducts;
    }

    /// <summary>
    /// Exports a list of Pokemon products to a CSV file.
    /// </summary>
    /// <param name="pokemonProducts">The list of Pokemon products to export.</param>
    /// <remarks>
    /// This method exports the provided list of Pokemon products to a CSV file named "pokemon-products.csv".
    /// Each Pokemon product is represented as a row in the CSV file, with columns for URL, image, name, and price.
    /// </remarks>
    /// <seealso cref="PokemonProduct"/>
    private static void ExportDataToCsv(List<PokemonProduct>? pokemonProducts)
    {
        using (var writer = new StreamWriter("pokemon-products.csv"))

        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(pokemonProducts);
        }
    }
}
