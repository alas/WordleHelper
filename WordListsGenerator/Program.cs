using WordListsGenerator;

var english = WordList.GetAllWordsEnglish();
var spanish = WordList.GetAllWordsSpanish();
var spanishFull = WordList.GetAllWordsSpanishFull();
var lunfardo = WordList.GetAllWordsLunfardo();

File.WriteAllText(@"english.txt", english);
File.WriteAllText(@"spanish.txt", spanish);
File.WriteAllText(@"spanishFull.txt", spanishFull);
File.WriteAllText(@"lunfardo.txt", lunfardo);

File.WriteAllText(@"english.gzip.txt", english.ToGzipAsync().Result);
File.WriteAllText(@"spanish.gzip.txt", spanish.ToGzipAsync().Result);
File.WriteAllText(@"spanishFull.gzip.txt", spanishFull.ToGzipAsync().Result);
File.WriteAllText(@"lunfardo.gzip.txt", lunfardo.ToGzipAsync().Result);
