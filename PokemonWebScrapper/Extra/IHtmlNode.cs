namespace PokemonWebScrapper.Extra;

public interface IHtmlNode
{
    string InnerText { get; }
    string OuterHtml { get; }
    IHtmlNode? QuerySelector(string selector);
}
