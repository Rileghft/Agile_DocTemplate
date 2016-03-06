using System;
using System.IO;
using System.Linq;
using System.Text;

/**
 * [FINISH]
 * @author 楊舜宇
 * @since 2016/3/6
 * 這個程式依照資料和樣板來產生對應文件
 * 
 * 參數說明:
 *      -i  資料來源路徑
 *      -t  樣板來源路徑
 *      -r  輸出檔案路徑
 *
 * 注意事項:
 *      1. 資料檔第一列必須包含資料欄位資訊
 *      2. 資料檔以TAB分隔每一欄資料
 *      3. 資料檔最後要空一列
 *      4. 樣板以${VAR}表示欄位變數
 *
 * 測試檔案:
 *      資料檔: data.txt
 *      樣板檔: template.txt
 *
 * 預設輸出路徑: ./result.txt
 *
 */

namespace MergeDataAndDoc
{
    class Program
    {
        static private String dataSrc;
        static private String templateSrc;
        static private String outputPath;
        static private String []columns;
        static private String []info;
        static private String template;

        static private StreamReader reader;

        static void Main(string[] args)
        {
            outputPath = "result.txt";
            processArgs(args);
            readFile();
            generateDoc();

            //open file externally
            System.Diagnostics.Process.Start(outputPath);
        }

        static void processArgs(String[] args)
        {
            int state = 0; //1 dataSrc, 2 templateSrc, 3 outputPath
            for (int i = 0; i < args.Length; ++i)
            {
                if (state == 0)
                    if (args[i][0] == '-')
                        //get args type
                        switch (args[i])
                        {
                            case "-i":
                                state = 1;
                                break;
                            case "-t":
                                state = 2;
                                break;
                            case "-r":
                                state = 3;
                                break;
                            default:
                                break;
                        }
                    else
                        throw new Exception("Invalid argument format.");
                else
                {
                    //get args
                    switch(state)
                    {
                        case 1:
                            dataSrc = args[i];
                            break;
                        case 2:
                            templateSrc = args[i];
                            break;
                        case 3:
                            outputPath = args[i];
                            break;
                    }
                    state = 0;
                }
            }
        }

        static void readFile()
        {
            using (reader = new StreamReader(dataSrc))
            {
                String line;
                line = reader.ReadLine();
                if (line != null)
                    columns = line.Split('\t');
                String data = reader.ReadToEnd();
                info = data.Split('\n');
            }

            using (reader = new StreamReader(templateSrc))
            {
                template = reader.ReadToEnd();
            }
        }

        static void generateDoc()
        {
            StreamWriter writer;
            StringBuilder docBuilder;

            using (writer = new StreamWriter(outputPath, false, Encoding.Unicode))
            {
                for(int i = 0; i < info.Length; ++i)
                {
                    docBuilder = new StringBuilder(template);
                    if (info[i] == "")
                        break;
                    String[] data = info[i].Remove(info[i].Length - 1, 1).Split('\t');
                    //ignore malform data
                    if (columns.Length != data.Length) {
                        Console.WriteLine("Invalid data format: " + info[i]);
                        continue;
                    }

                    //repalce variables in template
                    for (int j = 0; j < columns.Count(); ++j)
                    {
                        docBuilder.Replace("${}".Insert(2, columns[j]), data[j]);
                    }
                    writer.WriteLine(docBuilder.ToString());
                }
            }
        }
    }
}