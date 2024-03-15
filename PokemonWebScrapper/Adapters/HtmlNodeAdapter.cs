using HtmlAgilityPack;
using PokemonWebScrapper.Extra;

namespace PokemonWebScrapper.Adapters;

public class HtmlNodeAdapter : IHtmlNode
{
    private readonly HtmlNode _htmlNode;

    public HtmlNodeAdapter(HtmlNode htmlNode)
    {
        _htmlNode = htmlNode;
    }

    public IHtmlNode? QuerySelector(string selector)
    {
        var innerHtml = _htmlNode.QuerySelector(selector);
        return innerHtml != null ? new HtmlNodeAdapter(innerHtml) : null;
    }

    public string InnerText => _htmlNode.InnerText;

    public string OuterHtml => _htmlNode.OuterHtml;
}
