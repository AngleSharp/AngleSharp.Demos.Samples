namespace AngleSharp.Samples.Demos.Snippets
{
    using System;
    using System.Threading.Tasks;

    class LegacyEventScripting : ISnippet
    {
        public async Task Run()
        {
            // We require a custom configuration with JavaScript
            var config = new Configuration().WithJavaScript();

            // This is our sample source, we will trigger the load event
            var source = @"<!doctype html>
<html>
<head><title>Legacy event sample</title></head>
<body>
<script>
console.log('Before setting the handler via onload!');

document.onload = function() {
    console.log('Document loaded (legacy way)!');
};

console.log('After setting the handler via onload!');
</script>
</body>";
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(source));

            // HTML should be output in the end
            Console.WriteLine(document.DocumentElement.OuterHtml);
        }
    }
}
