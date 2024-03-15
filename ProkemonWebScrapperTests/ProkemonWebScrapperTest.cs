using Moq;
using HtmlAgilityPack;
using PokemonWebScrapper;
using PokemonWebScrapper.Extra;
using PokemonWebScrapper.Adapters;
using System.Linq;

namespace ProkemonWebScrapperTests;

[TestFixture]
public class ProkemonWebScrapperTests
{
    [Test]
    public void Scraper_ValidHtmlElements_CreatesCsvFile()
    {
        // Arrange
        var mockWeb = new Mock<HtmlWeb>();
        var mockDocument = new Mock<HtmlDocument>();
        var mockProductHTMLElements = new List<IHtmlNode>();

        // Simular instancias de IHtmlNode utilizando HtmlNodeAdapter
        var mockNode1 = new Mock<IHtmlNode>();
        mockProductHTMLElements.Add(mockNode1.Object);
        var mockNode2 = new Mock<IHtmlNode>();
        mockProductHTMLElements.Add(mockNode2.Object);

        // Configurar mockDocument para devolver mockProductHTMLElements cuando se llame a DocumentNode.QuerySelectorAll
        mockDocument.Setup(doc => doc.DocumentNode.QuerySelectorAll("li.product")).Returns((IList<HtmlNode>)mockProductHTMLElements);

        // Configurar mockWeb para devolver mockDocument cuando se llame a Load
        mockWeb.Setup(web => web.Load(It.IsAny<string>())).Returns(mockDocument.Object);

        // Act
        WebScrapper.Scraper();

        // Assert
        Assert.That(File.Exists("pokemon-products.csv"), Is.True);
    }

    [Test]
    public void Scraper_EmptyHtmlElements_NoCsvFileCreated()
    {
        // Arrange
        var mockWeb = new Mock<HtmlWeb>();
        var mockDocument = new Mock<HtmlDocument>();
        List<HtmlNode> emptyList = new List<HtmlNode>();

        mockWeb.Setup(web => web.Load(It.IsAny<string>())).Returns(mockDocument.Object);
        mockDocument.Setup(doc => doc.DocumentNode.QuerySelectorAll("li.product")).Returns(emptyList);

        // Act
        WebScrapper.Scraper();

        // Assert
        Assert.That(File.Exists("pokemon-products.csv"), Is.False);
    }

    [Test]
    public void Scraper_NullHtmlElements_NoCsvFileCreated()
    {
        // Arrange
        var mockWeb = new Mock<HtmlWeb>();
        var mockDocument = new Mock<HtmlDocument>();

        mockWeb.Setup(web => web.Load(It.IsAny<string>())).Returns(mockDocument.Object);
        mockDocument.Setup(doc => doc.DocumentNode.QuerySelectorAll("li.product")).Returns((List<HtmlNode>)null);

        // Act
        WebScrapper.Scraper();

        // Assert
        Assert.That(File.Exists("pokemon-products.csv"), Is.False);
    }    
}