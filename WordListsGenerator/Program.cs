using WordListsGenerator;

var english = WordList.GetAllWordsEnglish();
var spanish = WordList.GetAllWordsSpanish();

//this generates the smallest files not counting brotli that is not supported on platform: browser even when js blazor is loaded using brotli...
File.WriteAllText(@"english gzip Optimal.txt", english.ToGzipAsync().Result);
File.WriteAllText(@"spanish gzip Optimal.txt", spanish.ToGzipAsync().Result);
