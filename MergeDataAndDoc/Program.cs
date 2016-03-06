using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            processArgs(args);
            Console.WriteLine();
            Console.WriteLine("資料來源: " + dataSrc);
            Console.WriteLine("樣板來源: " + templateSrc);
            Console.WriteLine("輸出路徑: " + outputPath);
            Console.WriteLine();

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

            using (writer = new StreamWriter(outputPath))
            {
                for(int i = 0; i < info.Length; ++i)
                {
                    docBuilder = new StringBuilder(template);
                    String[] data = info[i].Split('\t');
                    //ignore malform data
                    if (columns.Length != data.Length) {
                        Console.WriteLine("Invalid data format: " + info[i]);
                        continue;
                    }

                    //repalce variables in template
                    for (int j = 0; j < columns.Count(); ++j)
                    {
                        docBuilder.Replace("${}".Insert(2, columns[j]), data[i]);
                    }
                    writer.WriteLine(docBuilder.ToString());
                }
            }
        }
    }
}