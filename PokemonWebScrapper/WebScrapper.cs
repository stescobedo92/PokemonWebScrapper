﻿using CsvHelper;
using HtmlAgilityPack;
using PokemonWebScrapper.Model;
using System.Globalization;

namespace PokemonWebScrapper;

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
        var productHTMLElements = document.DocumentNode.QuerySelectorAll("li.product");

        pokemonProducts = ProductHtmlElements(productHTMLElements);
        ExportDataToCsv(pokemonProducts: pokemonProducts);
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
    /// <seealso cref="HtmlNode"/>
    /// <seealso cref="PokemonProduct"/>
    private static List<PokemonProduct>? ProductHtmlElements(IList<HtmlNode>? productHTMLElements)
    {
        List<PokemonProduct> pokemonProducts = new();

        pokemonProducts.AddRange(from productHTMLElement in productHTMLElements
                                 let url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes["href"].Value)
                                 let image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img").Attributes["src"].Value)
                                 let name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("h2").InnerText)
                                 let price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector(".price").InnerText)
                                 let pokemonProduct = new PokemonProduct() { Url = url, Image = image, Name = name, Price = price }
                                 select pokemonProduct);

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