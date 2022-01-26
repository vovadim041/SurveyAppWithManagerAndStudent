using SurveyApp;

Console.WriteLine("Who are you?");
Console.WriteLine("1 Manager\n2 Student\n\"done\" to finish");

var userChoose = (Console.ReadLine() ?? string.Empty).ToLower();

while (userChoose != "done")
{
    switch (userChoose)
    {
        case "1" or "manager":
            Manager.ManagerWork();
            break;
        case "2" or "student":
            Student.StudentWork();
            break;
        case not "done":
            Console.WriteLine("invalid role");
            break;
    }
    
    Console.WriteLine("1 Manager\n2 Student\n\"done\" to finish");
    userChoose = (Console.ReadLine() ?? string.Empty).ToLower();
}