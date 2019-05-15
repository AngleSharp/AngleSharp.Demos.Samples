﻿namespace AngleSharp.Samples.Demos.Snippets
{
    using System;
    using System.Threading.Tasks;

    class CustomEventScripting : ISnippet
    {
        public async Task Run()
        {
            // We require a custom configuration with JavaScript
            var config = Configuration.Default
                .WithJs()
                .WithConsoleLogger(context => new StandardConsoleLogger());

            // This is our sample source, we will trigger the load event
            var source = @"<!doctype html>
<html>
<head><title>Custom Event sample</title></head>
<body>
<script>
console.log('Before setting the handler!');

document.addEventListener('load', function() {
    console.log('Document loaded!');
});

document.addEventListener('hello', function() {
    console.log('hello world from JavaScript!');
});

console.log('After setting the handler!');
</script>
</body>";
            var document = await BrowsingContext.New(config).OpenAsync(m => m.Content(source));

            // HTML should be output in the end
            Console.WriteLine(document.DocumentElement.OuterHtml);

            // Register Hello event listener from C# (we also have one in JS)
            document.AddEventListener("hello", (s, ev) =>
            {
                Console.WriteLine("hello world from C#!");
            });

            var e = document.CreateEvent("event");
            e.Init("hello", false, false);
            document.Dispatch(e);
        }
    }
}
