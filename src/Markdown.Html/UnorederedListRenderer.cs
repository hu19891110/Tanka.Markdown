﻿namespace Tanka.Markdown.Html
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using Blocks;
    using HtmlTags;

    public class UnorederedListRenderer : BlockRendererBase<List>
    {
        private readonly ParagraphRenderer _itemRenderer;

        public UnorederedListRenderer(bool noInlineHtml = false)
        {
             _itemRenderer = new ParagraphRenderer(noInlineHtml);

            _itemRenderer.SpanRenderers.OfType<TextSpanRenderer>().Single().CleanInput = input =>
            {
                var clean = Regex.Replace(input, @"\s+", " ");
                return clean.TrimStart();
            };
        }

        protected override bool CanRender(List block)
        {
            return !block.IsOrdered;
        }

        protected override HtmlTag Render(Document document, List block)
        {
            var ol = new HtmlTag("ul");

            foreach (var item in block.Items)
            {
                var li = new HtmlTag("li", ol);
                var itemHtml = _itemRenderer.Render(document, item);
                li.AppendHtml(itemHtml.ToHtmlString());
            }

            return ol;
        }
    }
}