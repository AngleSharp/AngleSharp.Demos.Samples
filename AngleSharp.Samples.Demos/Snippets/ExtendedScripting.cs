namespace AngleSharp.Samples.Demos.Snippets
{
    using System;
    using System.Threading.Tasks;

    class ExtendedScripting : ISnippet
    {
        public async Task Run()
        {
            //We require a custom configuration with JavaScript and CSS
            var config = Configuration.Default
                                      .WithJavaScript()
                                      .WithCss();

            // This is our sample source, we will do some DOM manipulation
            var source = @"<!doctype html>
<html>
<head><title>Sample</title></head>
<style>
.bold {
    font-weight: bold;
}
.italic {
    font-style: italic;
}
span {
    font-size: 12pt;
}
div {
    background: #777;
    color: #f3f3f3;
}
</style>
<body>
<div id=content></div>
<script>
(function() {
    var doc = document;
    var content = doc.querySelector('#content');
    var span = doc.createElement('span');
    span.id = 'myspan';
    span.classList.add('bold', 'italic');
    span.textContent = 'Some sample text';
    content.appendChild(span);
    var script = doc.querySelector('script');
    script.parentNode.removeChild(script);
})();
</script>
</body>";
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(source));

            // HTML will have changed completely (e.g., no more script element)
            Console.WriteLine(document.DocumentElement.OuterHtml);
        }
    }
}
