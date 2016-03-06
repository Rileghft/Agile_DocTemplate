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

        static private StreamReader reader;

        static void Main(string[] args)
        {
            processArgs(args);
            Console.WriteLine();
            Console.WriteLine("資料來源: " + dataSrc);
            Console.WriteLine("樣板來源: " + templateSrc);
            Console.WriteLine("輸出路徑: " + outputPath);
            Console.WriteLine();

            using (reader = new StreamReader(dataSrc))
            {
                String line; 
                while((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine();

            using (reader = new StreamReader(templateSrc))
            {
                String line; 
                while((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        static void processArgs(String[] args)
        {
            int state = 0; //1 dataSrc, 2 templateSrc, 3 outputPath
            for (int i = 0; i < args.Length; ++i)
            {
                if (state == 0)
                    if (args[i][0] == '-')
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
    }
}