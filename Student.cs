using System.Diagnostics;

namespace SurveyApp;

public static class Student
{ 
    public static void StudentWork()
    {
        Console.WriteLine("write the name of the test file without file format to pass." +
                          "\ntesWrite \"done\" to get back");
        var filename = Console.ReadLine() ?? string.Empty;
        
        while (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine("Try again");
            filename = Console.ReadLine() ?? string.Empty;
        }
        while (filename != "done" && filename != "try again")
        {
            if (File.Exists(filename + ".txt"))
            {
                var lineCount = File.ReadLines(filename + "Answers.txt").Count();
                var right = 0;
                var wrong = 0;

                for (var i = 1; i <= lineCount; i++)
                {
                    var line = GetLine.Get(filename + ".txt", i);
                    var count = 0;
                    var questionName = "";
                    var answerName = "";
                    foreach (var c in line)
                    {
                        if (count != 2)
                        {
                            if (c != '{')
                                questionName += c;
                            else
                                count++;
                        }
                        else if (c != '}' && c != ',')
                            answerName += c;
                        else if (c == ',')
                        {
                            answerName += '\n';
                        }
                    }

                    Console.WriteLine(questionName + "\n" + ' ' + answerName);

                    var studentAnswer = Console.ReadLine();
                    var trueAnswer = GetLine.Get(filename + "Answers.txt", i);

                    while (string.IsNullOrWhiteSpace(studentAnswer))
                    {
                        Console.WriteLine("Try again");
                        studentAnswer = Console.ReadLine() ?? string.Empty;
                    }

                    if (trueAnswer.Contains(studentAnswer))
                    {
                        Console.WriteLine("Right! next question:");
                        Thread.Sleep(1000);
                        right++;
                    }
                    else
                    {
                        Console.WriteLine("Wrong ;( next question:");
                        Thread.Sleep(1000);
                        wrong++;
                    }
                }

                
                double percent = right * 100 / lineCount;
                
                Console.WriteLine($"Done! {right} : right answers {wrong} : wrong answers." +
                                  $"\nPercent of right : {percent} %");
                Thread.Sleep(1000);
                
                File.AppendAllText(filename + "Statistics.txt", percent + "\n");

                if (percent != 100)
                {
                    Console.WriteLine("there is room to grow. Write \"try again\" or \"done\"");
                    filename = Console.ReadLine() ?? string.Empty;
                    continue;
                }

            }
            
            Console.WriteLine("write the name of the test file without file format to pass." +
                              "Write \"done\" to get back");
            filename = Console.ReadLine() ?? string.Empty;
        }
    }
}