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
        static void Main(string[] args)
        {
            for(int i = 0; i < args.Length; ++i)
                Console.WriteLine(args[i]);
            processArgs(args);
            Console.WriteLine();
            Console.WriteLine(dataSrc);
            Console.WriteLine(templateSrc);
            Console.WriteLine(outputPath);
            string inputFileName = "defaultInput.txt";
            string outputFileName = "defaultOutput.txt";
            if (args.Length == 2)
            {
                inputFileName = args[0];
                outputFileName = args[1];
            }

            using (StreamReader inputFile = new StreamReader(inputFileName))
            using(StreamWriter outputFile = new StreamWriter(outputFileName))
            {
                string line; //test
                while((line = inputFile.ReadLine()) != null)
                {
                    string outputLine = "***" + line;
                    Console.WriteLine("Write line: " + outputLine);
                    outputFile.WriteLine(outputLine);
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
