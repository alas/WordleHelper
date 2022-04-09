using System.IO.Compression;
using WordListsGenerator;

var english = WordList.GetAllWordsEnglish();
var spanish = WordList.GetAllWordsSpanish();


//this generates the smallest files
File.WriteAllText(@"english gzip Optimal.txt", Compress.ToGzipAsync(english).Result);
File.WriteAllText(@"spanish gzip Optimal.txt", Compress.ToGzipAsync(spanish).Result);
