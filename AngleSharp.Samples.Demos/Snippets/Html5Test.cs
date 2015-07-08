namespace AngleSharp.Samples.Demos.Snippets
{
    using System;
    using System.Threading.Tasks;

    // Not used at the moment (wait for more to be integrated)
    class Html5Test// : ISnippet
    {
        public async Task Run()
        {
            // We require a custom configuration with JavaScript, CSS and the default loader
            var config = Configuration.Default
                                      .WithJavaScript()
                                      .WithCss()
                                      .WithDefaultLoader();

            // We create a new context
            var context = BrowsingContext.New(config);

            // The address we want to use
            var address = Url.Create("http://html5test.com");

            // Load the document
            var document = await context.OpenAsync(address);

            // Get the scored points
            var points = document.QuerySelector("#score > .pointsPanel > h2 > strong").TextContent;

            // Print it out
            Console.WriteLine("AngleSharp received {0} points form HTML5Test.com", points);
        }
    }
}
