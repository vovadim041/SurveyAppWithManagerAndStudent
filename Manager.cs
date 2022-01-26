using System.Text;

namespace SurveyApp;

public static class Manager
{
    public static void ManagerWork()
    {
        
        Console.WriteLine("Write the name of the test to edit or create." +
                          "\nWrite \"done\" to get back");
        var filename = Console.ReadLine() ?? string.Empty;
        TryString(ref filename);
        
        
        while (filename != "done")
        {
            if (File.Exists(filename + ".txt"))
            {
                Console.WriteLine($"You opened {filename}");
                Thread.Sleep(3000);
                Console.WriteLine("Choose command:");
                Thread.Sleep(500);
                Console.WriteLine("1 add question\n2 delete question\n3 watch statistic");

                var flag = true;

                while (flag)
                {
                    var managerChoose = (Console.ReadLine() ?? string.Empty).ToLower();
                    TryString(ref managerChoose);

                    switch (managerChoose)
                    {
                        case "1" or "add question":
                            AddQuestion(filename);
                            Console.WriteLine("You added questions to " + filename);
                            flag = false;
                            break;
                        case "2" or "delete question":
                            DeleteQuestion(filename);
                            Console.WriteLine("You deleted questions from " + filename);
                            flag = false;
                            break;
                        case "3" or "watch statistic":
                            if (!File.Exists(filename + "Statistics.txt"))
                                Console.WriteLine("Your test is not passed");
                            else
                                WatchStatistic(filename);
                            
                            flag = false;
                            break;
                        default:
                            Console.WriteLine("Choose one of commands above");
                            break;
                    }
                    Thread.Sleep(1000);
                }
            }
            else
            {
                AddQuestion(filename);
                Console.WriteLine($"You created {filename + ".txt"}");
                Thread.Sleep(1000);
                Console.WriteLine($"You created {filename + "Answers.txt"}");
                Thread.Sleep(1000);
                Console.WriteLine($"After someone passes the test will be created {filename + "Statistics.txt"}");
                Thread.Sleep(1000);
            }
            
            Console.WriteLine("Write the name of the test file to edit or create." +
                              "\nWrite \"done\" to get back");
            filename = Console.ReadLine() ?? string.Empty;
            TryString(ref filename);
        }
        
    }

    private static void AddQuestion(string filename)
    {
        Console.WriteLine("Write the name of question to be added." +
                          "\nWrite \"done\" to get back");
        var questionName = Console.ReadLine() ?? string.Empty;
        TryString(ref questionName);

        while (questionName != "done")
        {
            var answersCount = 1;
            var answers = new Dictionary<int, string>();

            Console.WriteLine($"Write {answersCount} answer (write all numbers in words)" +
                              "\nAdd \".correct\" if answer is correct (only one correct answer)");  
            var answerName = Console.ReadLine() ?? string.Empty;
            TryString(ref answerName);
            
            while (answerName != "done")
            {
                if (answerName.Contains(".correct")) 
                    File.AppendAllText(filename + "Answers.txt", 
                        answersCount.ToString() + answerName.Replace(".correct", "") + "\n");
                
                answerName = answerName.Replace(".correct", "");
                answers.Add(answersCount, answerName);
                answersCount++;

                Console.WriteLine($"Write {answersCount} answer (write all numbers in words)" +
                                  "\nWrite \"done\" to get back");
                answerName = Console.ReadLine() ?? string.Empty;
                TryString(ref answerName);
            }
            

            var answersString = ToPrettyString(answers);
            var question = new Dictionary<string, string> {{questionName, answersString}};


            File.AppendAllText(filename + ".txt", ToPrettyString(question) + "\n");
            Console.WriteLine("you created " + filename + ".txt");
            Thread.Sleep(1000);
            Console.WriteLine("Done");
            Thread.Sleep(1000);
            Console.WriteLine("Write the name of question to be added." + 
                              "\nWrite \"done\" to get back");
            questionName = Console.ReadLine() ?? string.Empty;
            TryString(ref questionName);
        }
    }

    private static void DeleteQuestion(string filename)
    {
        var lineCount = File.ReadLines(filename + ".txt").Count();

        for (var i = 1; i <= lineCount; i++)
        {
            var str = GetLine.Get(filename + ".txt", i);
            var count = 0;
            var questionName = "";
            foreach (var c in str)
            {

                if (count != 2)
                {
                    if (c != '{')
                        questionName += c;
                    else
                        count++;
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine(questionName);
        }
        
        Console.WriteLine("Write name of question to be deleted." +
                          "\nWrite \"done\" to get back");
        var question = Console.ReadLine() ?? string.Empty;
        TryString(ref question);

        while (question != "done")
        {
            for (var i = 1; i <= lineCount; i++)
            {
                if (!GetLine.Get(filename + ".txt", i).Contains(question)) continue;
                DeleteLine(i, filename + ".txt");
                DeleteLine(i, filename + "Answers.txt");
            }
            Console.WriteLine("Done");
            Thread.Sleep(1000);
            Console.WriteLine("Write name of question to be deleted." +
                              "\nWrite \"done\" to get back");
            question = Console.ReadLine() ?? string.Empty;
            TryString(ref question);
        }
    }

    private static void WatchStatistic(string filename)
    {
        double statistics = 0;
        var lineCount = File.ReadLines(filename + "Statistics.txt").Count();
        for (var i = 1; i <= lineCount; i++)
        {
            statistics += double.Parse(GetLine.Get(filename + "Statistics.txt", i));
        }

        statistics /= lineCount;
        
        Console.WriteLine($"Average percent of correct answers for this test : {statistics} %");
    }


    private static string ToPrettyString<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var dictionaryString = "{";
        foreach (var (key, value) in dictionary)
        {
            dictionaryString += key + " : " + value + ", ";
        }
        return dictionaryString.TrimEnd(',', ' ') + "}";  
    }
    
    private static void DeleteLine(int line, string filename)
    {
        var builderb = new StringBuilder();
        using (StreamReader streamReader = new StreamReader(filename))
        {
            var count = 0;
            while (!streamReader.EndOfStream)
            {
                count++;
                if (count != line)
                {
                    using var sw = new StringWriter(builderb);
                    sw.WriteLine(streamReader.ReadLine());
                }
                else
                {
                    streamReader.ReadLine();
                }
            }
        }
        using (var sw = new StreamWriter(filename))
        {
            sw.Write(builderb.ToString());
        }
    }

    private static void TryString(ref string str)
    {
        while (string.IsNullOrWhiteSpace(str))
        {
            Console.WriteLine("Try again");
            str = Console.ReadLine() ?? string.Empty;
        }
    }
}