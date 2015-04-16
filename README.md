# AngleSharp.Samples

A set of sample codes for using AngleSharp.

## App

This is the Visual Studio project for a simple DOM driven browser. It does not have a renderer, but it provides the user with a lot of information that is exposed by AngleSharp. It also integrates `AngleSharp.Scripting` to provide a little bit of a pseudo JavaScript console.

The application depends on the mahapps.metro package. The ViewModels that also implement the `ITabViewModel` will be updated once the `Document` property changes. Each `ViewModel` is therefore a live example on its own. Some other ViewModels only present information that is aggregated from subscribing to events published via the `IEventAggregator`. These ViewModels implement the `IEventViewModel` interface, which lets them being resetted prior to a new request.

### DOM

The DOM view represents the basic DOM methods and properties in a view similar to the one provided by the debugging tools in modern browsers. Therefore it also reflects all properties, methods and indexers with their respective JavaScript name. This implies reading the `INode`, finding the `DomNameAttribute` values on the class and its methods.

![AngleSharp DOM](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Samples/master/images/dom.png)

The sample is great for learning how to use the published DOM API. It can be easily adjusted to output the original .NET method names instead of the adjusted ones, which match the official naming as specified by the W3C.

### Console

This is a very simple example of using the `AngleSharp.Scripting.JavaScript` library. It uses Jint to create JavaScript bindings to the published API. Please note that the sample app does not use JavaScript actively. JavaScript is only active in this console view. Hence no JavaScript is loaded or executed on the page.

![AngleSharp Console](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Samples/master/images/console.png)

The code uses a 3rd party WPF control called Terminal. This control is a little bit buggy and sometimes may refuse to alter input. If you have a better suggestion or know the problem, then please contribute.

### Queries

One of the best things about AngleSharp is the integrated support for queries via `QuerySelector` and `QuerySelectorAll`. This sample provides the user with a textbox that allows input of arbitrary queries. The query is executed and the result is then displayed. Due to WPFs overhead the displaying takes usually a lot longer than the query itself. The time for the query is also portrayed in the upper right corner.

![AngleSharp Queries](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Samples/master/images/queries.png)

Invalid queries result in a `DomException`, as we would see it in a browser. The sample catches these exceptions and sets the background of the input box to a red to indicate the failure.

### Sheets

AngleSharp does not only offer utilities for parsing HTML, but also for inspecting CSS. This sample exposes the stylesheets that are found in currently active document.

![AngleSharp Sheets](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Samples/master/images/sheets.png)

Once a stylesheet is selected the contents are displayed in a tree like view. The sample is great for seeing AngleSharp's CSSOM in action.

### Tree

Of course we can also represent the document as a tree of nodes, including elements, text, comments and the doctype.

![AngleSharp Tree](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Samples/master/images/tree.png)

This example makes also use of the expoxed `HtmlMarkupFormatter`, which is the default `IMarkupFormatter` for HTML code.

### Statistics

This sample uses some techniques to extract a few interesting numbers from the loaded document. We could be interested in the number and the frequency of used CSS class names. Also id names, the distribution of used elements and much more could be of interest.

![AngleSharp Statistics](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Samples/master/images/statistics.png)

We use the library Oxyplot to plot the pie charts. If you know other interesting statistics then let us know. We'll always look for contributions here.

### Profiler

Every browser comes with a profiler to detect potential network bottlenecks. The profiler in the samples gives us an easy way to see the performance of a webpage and AngleSharp.

![AngleSharp Profiler](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Samples/master/images/profiler.png)

The profiler is implemented by subscribing to the `IEventAggregator` with events that carry, e.g., the `HtmlParseStartEvent` to detect the beginning of parsing HTML. These start events do not have a corresponding end event. This is by design and should simplify making connections between start and end events. Since the end event is dependent on the start event, we have to listen for it specifically by adding an event listener to the `Ended` event on the arguments given by the start event.

### Errors

Similarly to the profiler we can use the `IEventAggregator` to read out the errors. We combine all CSS errors into one log. The HTML errors are in a separated log. 

![AngleSharp Errors](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Samples/master/images/errors.png)

Errors noted in the parsing of HTML and CSS documents may indicate invalid markup. They can, however, also indicate a more modern version or a version tailered to a specific user-agent. For instance in stylesheets we will regularly see errors that declaration names are unknown. Such names may be just a regular (known) declaration with a prefix, to support a specific browser. Hence these errors are usually no problem for displaying the page.

## Demos

A collection of samples that is distributed via the website. Most samples are actually shown in the wiki. Samples may go in any direction (DOM manipulation, scripting, querying elements, events, ...).

These demos can be run via the command line. The executable supports some command line parameters, such as `--pause` (or `-p`) to pause between the different samples or `--clear` (shortform `-c`) to reset the console buffer after each sample.

Since AngleSharp is built around async and console applications are usually very synchronous by nature, the demos are actually run all run in a fashion to block the execution thread. This way the samples can also be invoked incrementally without any problems.

## License

The MIT License (MIT)

Copyright (c) 2015 AngleSharp

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.