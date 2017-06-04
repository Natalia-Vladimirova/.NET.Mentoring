## Task 8 - HTTP Fundamentals

Implement a library and console application which uses this library to create a local copy of a site ([wget](https://www.nuget.org/packages/Microsoft.Net.Http) program analogue).
  
The program should work in the next way: a user specify a start point (URL) and a folder which site(s) should be saved to and the program runs through all available links and downloads site(s) recursively.

#### Program/library options:
+ restriction on the depth of the references analysis (i.e. if you download the page the user specified - this is level 0, all pages linked from it - this is level 1, etc.);
+ restriction on the transfer to other domains (without restrictions/only inside the current domain/not higher than the path in the source URL);
+ restriction on the extension of downloadable resources (it is possible to specify a list i.e. gif,jpeg,jpg,pdf);
+ tracing (verbose mode): displaying the current processed page/document on the screen.

### Implementation guide

The following libraries can be used as a basis:
+ work with HTTP:
  + System.Net.Http.HttpClient - recommended option.
    + It is included to .Net Framework from the .Net 4.5+ versions. It should be downloaded via [NuGet](https://www.nuget.org/packages/Microsoft.Net.Http) in the earlier versions or other platforms.
    + Introduction to work with it can be found [here](https://blogs.msdn.microsoft.com/henrikn/2012/02/16/httpclient-is-here/).
    + Pay attention: it works using asynchronous operations (but it is possible to work using synchronous mode).
  + System.Net.HttpWebRequest - legacy.
+ work with HTML:
  + you can use one of the libraries listed [here](http://ru.stackoverflow.com/questions/420354/%D0%9A%D0%B0%D0%BA-%D1%80%D0%B0%D1%81%D0%BF%D0%B0%D1%80%D1%81%D0%B8%D1%82%D1%8C-html-%D0%B2-net/450586). The most popular option is HtmlAgilityPack although it is old enough and has issues.
