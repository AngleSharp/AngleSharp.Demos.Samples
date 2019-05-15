﻿namespace AngleSharp.Samples.Demos.Snippets
{
    using AngleSharp.Dom;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    class UsingLinq : ISnippet
    {
        public async Task Run()
        {
            // Create a new document from the given source
            var document = await BrowsingContext.New().OpenAsync(m => 
                m.Content("<ul><li>First item<li>Second item<li class='blue'>Third item!<li class='blue red'>Last item!</ul>"));

            // Do something with LINQ
            var blueListItemsLinq = document.All.Where(m => m.LocalName == "li" && m.ClassList.Contains("blue"));

            // Or directly with CSS selectors
            var blueListItemsSelector = document.QuerySelectorAll("li.blue");

            Console.WriteLine("Comparing both ways ...");

            Console.WriteLine();
            Console.WriteLine("LINQ:");

            foreach (var item in blueListItemsLinq)
                Console.WriteLine(item.Text());

            Console.WriteLine();
            Console.WriteLine("CSS:");

            foreach (var item in blueListItemsLinq)
                Console.WriteLine(item.Text());
        }
    }
}
