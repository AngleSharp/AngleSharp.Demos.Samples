namespace AngleSharp.Samples.Demos.Snippets
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    class GatherDataFromRssFeed : ISnippet
    {
        public async Task Run()
        {
            // We create a new configuration with the default loader
            var config = new Configuration().WithDefaultLoader();

            // We create a new context
            var context = BrowsingContext.New(config);

            // The address we want to use
            var address = Url.Create("http://www.florian-rappl.de/RSS");

            // We load the feed
            var feed = await context.OpenAsync(address);

            // We query the desired items
            var items = feed.QuerySelectorAll("item").Select(m => new
            {
                Updated = DateTime.Parse(m.GetElementsByTagName("a10:updated").First().TextContent),
                Title = m.QuerySelector("title").TextContent
            });

            Console.WriteLine("Available titles:");

            foreach (var item in items)
                Console.WriteLine("- {0} ({1})", item.Title, item.Updated);
        }
    }
}
