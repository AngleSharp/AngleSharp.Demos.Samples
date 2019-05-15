﻿namespace AngleSharp.Samples.Demos.Snippets
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using System;
    using System.Threading.Tasks;

    class Construction : ISnippet
    {
        public async Task Run()
        {
            // Create empty document
            var document = await BrowsingContext.New().OpenAsync(m => m.Content(String.Empty));

            // Set title
            document.Title = "My document";

            // Get the body
            var body = document.Body;

            //Create elements using generics, set properties
            var p1 = document.CreateElement<IHtmlParagraphElement>();
            p1.TextContent = "First paragraph";
            body.AppendChild(p1);
            var p2 = document.CreateElement<IHtmlParagraphElement>();
            p2.TextContent = "Second paragraph";
            body.AppendChild(p2);
            var a = document.CreateElement<IHtmlAnchorElement>();
            a.TextContent = "Some hyperlink";
            a.Href = "http://www.myurl";
            body.AppendChild(a);
            var img = document.CreateElement<IHtmlImageElement>();
            img.Source = "http://www.example.com/link";
            body.AppendChild(img);

            // Output the resulting DOM
            Console.WriteLine(document.DocumentElement.OuterHtml);
        }
    }
}
