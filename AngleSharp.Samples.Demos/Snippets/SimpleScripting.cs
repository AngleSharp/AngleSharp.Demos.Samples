namespace AngleSharp.Samples.Demos.Snippets
{
    using System;
    using System.Threading.Tasks;

    class SimpleScripting : ISnippet
    {
        public async Task Run()
        {
            // We require a custom configuration with JavaScript
            var config = new Configuration().WithJavaScript();

            // This is our sample source, we will set the title and write on the document
            var source = @"<!doctype html>
<html>
<head><title>Sample</title></head>
<body>
<script>
document.title = 'Simple manipulation...';
document.write('<span class=greeting>Hello World!</span>');
</script>
</body>";
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(source));

            // Modified HTML will be output
            Console.WriteLine(document.DocumentElement.OuterHtml);
        }
    }
}
