using System.IO.Compression;
using WordListsGenerator;

var english = WordList.GetAllWordsEnglish();
var spanish = WordList.GetAllWordsSpanish();

/*
File.WriteAllText(@"english bzip2 4096.txt", Compress.ToBZip2Async(english).Result);
File.WriteAllText(@"spanish bzip2 4096.txt", Compress.ToBZip2Async(spanish).Result);

File.WriteAllText(@"english bzip2 1.txt", Compress.ToBZip2Async(english, 1).Result);
File.WriteAllText(@"spanish bzip2 1.txt", Compress.ToBZip2Async(spanish, 1).Result);

File.WriteAllText(@"english bzip2 2.txt", Compress.ToBZip2Async(english, 2).Result);
File.WriteAllText(@"spanish bzip2 2.txt", Compress.ToBZip2Async(spanish, 2).Result);

File.WriteAllText(@"english bzip2 3.txt", Compress.ToBZip2Async(english, 3).Result);
File.WriteAllText(@"spanish bzip2 3.txt", Compress.ToBZip2Async(spanish, 3).Result);

File.WriteAllText(@"english bzip2 4.txt", Compress.ToBZip2Async(english, 4).Result);
File.WriteAllText(@"spanish bzip2 4.txt", Compress.ToBZip2Async(spanish, 4).Result);

File.WriteAllText(@"english bzip2 5.txt", Compress.ToBZip2Async(english, 5).Result);
File.WriteAllText(@"spanish bzip2 5.txt", Compress.ToBZip2Async(spanish, 5).Result);

File.WriteAllText(@"english bzip2 6.txt", Compress.ToBZip2Async(english, 6).Result);
File.WriteAllText(@"spanish bzip2 6.txt", Compress.ToBZip2Async(spanish, 6).Result);

File.WriteAllText(@"english bzip2 7.txt", Compress.ToBZip2Async(english, 7).Result);
File.WriteAllText(@"spanish bzip2 7.txt", Compress.ToBZip2Async(spanish, 7).Result);

File.WriteAllText(@"english bzip2 8.txt", Compress.ToBZip2Async(english, 8).Result);
File.WriteAllText(@"spanish bzip2 8.txt", Compress.ToBZip2Async(spanish, 8).Result);

File.WriteAllText(@"english bzip2 9.txt", Compress.ToBZip2Async(english, 9).Result);
File.WriteAllText(@"spanish bzip2 9.txt", Compress.ToBZip2Async(spanish, 9).Result);

File.WriteAllText(@"english gzip small.txt", Compress.ToGzipAsync(english).Result);
File.WriteAllText(@"spanish gzip small.txt", Compress.ToGzipAsync(spanish).Result);
*/

//this generates the smallest files
File.WriteAllText(@"english gzip optimal.txt", Compress.ToGzipAsync(english, CompressionLevel.Optimal).Result);
File.WriteAllText(@"spanish gzip optimal.txt", Compress.ToGzipAsync(spanish, CompressionLevel.Optimal).Result);
