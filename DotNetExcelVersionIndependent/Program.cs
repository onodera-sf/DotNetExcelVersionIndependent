// 実行プログラムの場所にある Excel ファイル
var excelFilePath = $@"{Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName)}\Sample.xlsx";

// Excel のオブジェクトは参照したら必ず解放する必要があります。
// 解放しないと Excel のプロセスが残り続けます。
dynamic? excel = null;
dynamic? books = null;
dynamic? book = null;
dynamic? sheets = null;
dynamic? sheet = null;
dynamic? cells = null;
dynamic? range1 = null;
dynamic? range2 = null;
try
{
  // Excel.Application の Type を取得
  var type = Type.GetTypeFromProgID("Excel.Application");
  if (type == null)
  {
    Console.WriteLine("Excel がインストールされていません。");
    return;
  }

  // Excel アプリケーションを生成(起動)します
  excel = Activator.CreateInstance(type);
  if (excel == null)
  {
    Console.WriteLine("Excel.Application のインスタンスの生成に失敗しました。");
    return;
  }
  excel.DisplayAlerts = false;

  // ワークブック一覧を参照します。参照する場合は必ず変数に保持します
  books = excel.Workbooks;

  // Excel ファイルを開きます
  book = books.Open(excelFilePath);

  // シート一覧を参照するので変数に保持します
  sheets = book.Worksheets;

  // シートを参照します。最初のシートは 1 になります
  sheet = sheets[1];

  // セル一覧を参照します
  cells = sheet.Cells;

  // 左上のセルを参照します。一番左上は [1, 1] になります
  range1 = cells[1, 1];

  // セルの値を取得します。値の取得なので後で解放する必要はありません
  var value = (int)range1.Value;

  // 下のセルを参照します
  range2 = cells[2, 1];

  // 日付分足してセットします
  range2.Value = DateTime.Now.Day + value;

  // 保存して閉じます
  book.Close(SaveChanges: true);

  Console.WriteLine("処理が完了しました。");
}
catch (Exception ex)
{
  // 閉じていなければ保存せずに閉じます
  // 開きっぱなしだと Excel 起動時に保存されていないデータとして表示される場合があります
  if (book != null) book.Close(SaveChanges: false);

  Console.WriteLine("処理が失敗しました。");
  Console.WriteLine(ex);
}
finally
{
  // 終了していなければ終了します
  if (excel != null) excel.Quit();

  // 例外が発生した場合でも必ずリソースを解放するようにします
  if (range1 != null) System.Runtime.InteropServices.Marshal.FinalReleaseComObject(range1);
  if (range2 != null) System.Runtime.InteropServices.Marshal.FinalReleaseComObject(range2);
  if (cells != null) System.Runtime.InteropServices.Marshal.FinalReleaseComObject(cells);
  if (sheet != null) System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sheet);
  if (sheets != null) System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sheets);
  if (book != null) System.Runtime.InteropServices.Marshal.FinalReleaseComObject(book);
  if (books != null) System.Runtime.InteropServices.Marshal.FinalReleaseComObject(books);
  if (excel != null) System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
}