namespace AngleSharp.Samples.Demos.Snippets
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    class BigBang : ISnippet
    {
        public async Task Run()
        {
            // Setup the configuration to support document loading
            var config = new Configuration().WithDefaultLoader();
            // Load the names of all The Big Bang Theory episodes from Wikipedia
            var address = "http://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes";
            // Asynchronously get the document
            var document = await BrowsingContext.New(config).OpenAsync(Url.Create(address));
            // This CSS selector gets the desired content
            var cellSelector = "tr.vevent td:nth-child(3)";
            // Perform the query to get all cells with the content
            var cells = document.QuerySelectorAll(cellSelector);
            // We are only interested in the text - select it with LINQ
            var titles = cells.Select(m => m.TextContent);

            Console.WriteLine("Overall {0} titles found...", titles.Count());

            foreach (var title in titles)
                Console.WriteLine("* {0}", title.Trim(new[] { '"' }));
        }
    }
}
