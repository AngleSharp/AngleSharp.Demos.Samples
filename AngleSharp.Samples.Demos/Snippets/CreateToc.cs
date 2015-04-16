namespace AngleSharp.Samples.Demos.Snippets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Html;

    class CreateToc : ISnippet
    {
        public async Task Run()
        {
            // Preamble for interactivity
            Console.WriteLine("Enter (full) url to use (or enter to skip):");
            var address = Console.ReadLine();
            var url = Url.Create(address);

            if (url.IsInvalid || url.IsRelative)
            {
                Console.WriteLine("Invalid url provided. Aborting.");
                return;
            }

            // Create a new configuration and include the default loader
            var config = new Configuration().WithDefaultLoader();

            // Create a new context
            var context = BrowsingContext.New(config);

            // Load the event
            var document = await context.OpenAsync(url);

            // Get the responsible headers
            var headers = document.QuerySelectorAll("h2, h3").ToArray();

            // Store the ToC here
            var toc = CreateEntries(document, headers);

            // Output the resulting DOM for the ToC
            Console.WriteLine(toc.ToHtml());
        }

        static IHtmlOrderedListElement CreateEntries(IDocument document, IElement[] headers)
        {
            var toc = document.CreateElement<IHtmlOrderedListElement>();

            // iterate over all elements satisfying the current level in the context
            for (var i = 0; i < headers.Length; ++i)
            {
                // Get the current header
                var header = headers[i];

                // Create the li entry for the element
                var item = CreateEntry(document, header);
                
                // Aggregate possible sub-headers
                var subHeaders = new List<IElement>();

                // As long as we don't see the original header level we'll collect the sub headers
                while (i + subHeaders.Count + 1 < headers.Length && 
                       headers[i].LocalName != headers[i + subHeaders.Count + 1].LocalName)
                {
                    subHeaders.Add(headers[i + 1 + subHeaders.Count]);
                }

                // If we collected any sub headers
                if (subHeaders.Count > 0)
                {
                    // Get another level of ToC (recursively)
                    var subToc = CreateEntries(document, subHeaders.ToArray());

                    // Append the ToC as a child to the current item
                    item.AppendChild(subToc);

                    // Also don't forget to skip these entries
                    i += subHeaders.Count;
                }

                // Add the current item to the ToC
                toc.AppendChild(item);
            }

            return toc;
        }

        static IHtmlListItemElement CreateEntry(IDocument document, IElement header)
        {
            // Create a new li element
            var item = document.CreateElement<IHtmlListItemElement>();

            // Initially the text of the li is the same as the h* text
            item.TextContent = header.TextContent;

            // Get a potential (named) anchor here
            var anchor = header.QuerySelector<IHtmlAnchorElement>("a");

            // If there really is an anchor
            if (anchor != null)
            {
                // Get the value of the name attribute
                var name = anchor.GetAttribute("name");

                // If the attribute exists (!= null) and makes sense (not empty)
                if (!String.IsNullOrEmpty(name))
                {
                    // Create a new anchor element
                    anchor = document.CreateElement<IHtmlAnchorElement>();

                    // Set its url to the anchor
                    anchor.Href = "#" + name;

                    // Set the anchor's text content to the header (= item) text
                    anchor.TextContent = item.TextContent;

                    // Replace the text node (= item content) with the anchor
                    item.ReplaceChild(anchor, item.FirstChild);
                }
            }

            return item;
        }
    }
}