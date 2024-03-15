using HtmlAgilityPack;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using PokemonWebScrapper;
using System.Xml;

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
        var mockNode1 = new Mock<HtmlNode>();
        var mockNode2 = new Mock<HtmlNode>();
        var mockProductHTMLElements = new List<HtmlNode> { mockNode1.Object, mockNode2.Object };

        mockWeb.Setup(web => web.Load(It.IsAny<string>())).Returns(mockDocument.Object);
        mockDocument.Setup(doc => doc.DocumentNode.QuerySelectorAll("li.product")).Returns(mockProductHTMLElements);

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