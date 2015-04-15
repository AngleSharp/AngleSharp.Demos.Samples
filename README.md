# AngleSharp.Samples

A set of sample codes for using AngleSharp.

## App

This is the Visual Studio project for a simple DOM driven browser. It does not have a renderer, but it provides the user with a lot of information that is exposed by AngleSharp. It also integrates `AngleSharp.Scripting` to provide a little bit of a pseudo JavaScript console.

The application depends on the mahapps.metro package. The ViewModels that also implement the `ITabViewModel` will be updated once the `Document` property changes. Each `ViewModel` is therefore a live example on its own.

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