class Program
{
    static void Main(string[] args)
    {
        MainMenu();
    }

    static int SelectMenu(List<string> menu)
    {
        ConsoleKeyInfo key;
        (int left, int top) = Console.GetCursorPosition();

        int option = 1;
        bool show = true;
        string color = "\u001b[38;5;240m";

        while (show)
        {
            Console.SetCursorPosition(left, top);

            Console.WriteLine();
            for (int i = 0; i < menu.Count; i++)
            {
                Console.WriteLine($"{(option == i ? $" - {color}{menu[i]}\u001b[0m - " : $"   {menu[i]}   ")}");
                if (i == 0 || i == menu.Count - 2)
                {
                    Console.WriteLine();
                }
                Console.CursorVisible = false;
            }

            key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    option = (option == menu.Count - 2 ? 1 : option + 1);
                    break;

                case ConsoleKey.UpArrow:
                    option = (option == 1 ? menu.Count - 2 : option - 1);
                    break;

                case ConsoleKey.Enter:
                    show = false;
                    return option;

                case ConsoleKey.Escape:

                    break;

                default:
                    break;
            }
        }

        return option;
    }

    static bool EnterMenu()
    {
        Console.CursorVisible = false;

        bool show = true;
        while (show)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Enter:
                    show = false;
                    return true;

                default:
                    break;
            }
        }

        return true;
    }

    static bool WarningMenu(List<string> menu)
    {
        Console.Clear();
        Console.CursorVisible = false;

        bool show = true;

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == 1 ? true : false);

            return show;

        }

        return true;
    }

    static void MainMenu()
    {

        string path = @"Data";

        List<string> test = new List<string>();
        List<List<string>> testquestion = new List<List<string>>();
        List<List<List<string>>> testanswer = new List<List<List<string>>>();
        List<List<string>> testtrueanswer = new List<List<string>>();
        List<List<string>> testgroup = new List<List<string>>();
        List<List<List<string>>> testparticipant = new List<List<List<string>>>();
        List<List<List<List<string>>>> participantanswer = new List<List<List<List<string>>>>();
        List<List<List<List<double>>>> participantresult = new List<List<List<List<double>>>>();
        List<string> group = new List<string>();
        List<List<string>> participant = new List<List<string>>();
        List<List<DataLP>> grouplp = new List<List<DataLP>>();
        List<List<List<DataLP>>> participantlp = new List<List<List<DataLP>>>();

        DataLP adminlp = new DataLP();
        UploadDataLPAdmin(ref adminlp, path);

        var data = (test, testquestion, testanswer, testtrueanswer, testgroup, testparticipant, participantanswer, participantresult, group, participant, grouplp, participantlp, path);


        UploadAllF(data);

        bool show = true;


        List<string> menu = new List<string>() { "Select the menu bar...", "Administrator", "User", "Exit", "Press Enter to continue..." };

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == 1 ? Administrator(adminlp, data) :
                option == 2 ? User(data) : Exit());


            Console.Clear();
        }

    }

    //--------------------------------------------------------|ЗАГРУЗКА ВСЕХ ФАЙЛОВ|--------------------------------
    static void UploadAllF((List<string> test, List<List<string>> testquestion, List<List<List<string>>> testanswer, List<List<string>> testtrueanswer, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) data)
    {
        UploadTest(data.test, data.path);
        UploadTestQuestion(data.testquestion, data.path);
        UploadTestAnswer(data.testanswer, data.path);
        UploadTestTrueAnswer(data.testtrueanswer, data.path);
        UploadTestGroup(data.testgroup, data.path);
        UploadTestParticipant(data.testparticipant, data.path);
        UploadAnswer(data.participantanswer, data.path);
        UploadResult(data.participantresult, data.path);
        UploadGroup(data.group, data.path);
        UploadParticipant(data.participant, data.path);
        UploadDataLPGroup(data.grouplp, data.path);
        UploadDataLPParticipants(data.participantlp, data.path);
    }

    static void UploadTest(List<string> test, string path)
    {
        test.Clear();
        string[] tests = Directory.GetFiles(path + @"\Tests");

        foreach (string tsts in tests)
        {
            string nametest = tsts.Substring(tsts.LastIndexOf(@"\") + 1).Replace(".txt", "");
            test.Add(nametest);
        }
    }

    static void UploadTestQuestion(List<List<string>> testquestion, string path)
    {
        testquestion.Clear();
        string[] tests = Directory.GetFiles(path + @"\Tests");

        foreach (string tsts in tests)
        {
            List<string> questions = new List<string>();
            using (StreamReader sr = new StreamReader(tsts))
            {
                while (!sr.EndOfStream)
                {
                    string question = sr.ReadLine();
                    if (question.StartsWith('.'))
                    {
                        questions.Add(question);
                    }
                }
            }
            testquestion.Add(questions);
        }
    }

    static void UploadTestAnswer(List<List<List<string>>> testanswer, string path)
    {
        testanswer.Clear();
        string[] tests = Directory.GetFiles(path + @"\Tests");

        foreach (string tsts in tests)
        {
            List<List<string>> question = new List<List<string>>();
            List<string> answers = new List<string>();

            int nl = 0;
            using (StreamReader sr = new StreamReader(tsts))
            {
                while (!sr.EndOfStream)
                {
                    string answer = sr.ReadLine();
                    if (!answer.StartsWith('.') || !answer.StartsWith(""))
                    {
                        answer = answer.Replace(" /", "");
                        answers.Add(answer);
                    }
                    if (answer == "" || sr.EndOfStream)
                    {
                        nl++;
                    }
                }

                int temp1 = 0;
                for (int i = 0; i < nl; i++)
                {
                    List<string> temp = new List<string>();
                    for (int j = temp1; j < answers.Count; j++)
                    {
                        if (answers[j] != "")
                        {
                            temp.Add(answers[j]);
                        }
                        else
                        {
                            temp1 = j + 1;
                            j = answers.Count;
                        }
                    }
                    question.Add(temp);
                }

            }
            testanswer.Add(question);
        }
    }

    static void UploadTestTrueAnswer(List<List<string>> testtrueanswer, string path)
    {
        testtrueanswer.Clear();
        string[] tests = Directory.GetFiles(path + @"\Tests");

        foreach (string tsts in tests)
        {
            List<string> test = new List<string>();
            if (!tsts.Contains("form"))
            {

                using (StreamReader sr = new StreamReader(tsts))
                {
                    while (!sr.EndOfStream)
                    {
                        string trueanswer = sr.ReadLine();
                        if (trueanswer.Contains(@"/"))
                        {
                            trueanswer = trueanswer.Replace(" /", "");
                            test.Add(trueanswer);

                        }
                    }
                }

            }
            else
            {
                test.Add("This test does not contain any answers!");
            }

            testtrueanswer.Add(test);
        }
    }

    static void UploadTestGroup(List<List<string>> testgroup, string path)
    {
        testgroup.Clear();
        string[] groups = Directory.GetDirectories(path + @"\Groups");

        foreach (string group in groups)
        {
            List<string> tgroup = new List<string>();

            string[] tests = Directory.GetFiles(group + @"\Tests");

            foreach (string tsts in tests)
            {

                string nametest = tsts.Substring(tsts.LastIndexOf(@"\") + 1).Replace(".txt", "");
                tgroup.Add(nametest);
            }
            testgroup.Add(tgroup);
        }
    }

    static void UploadTestParticipant(List<List<List<string>>> testparticipant, string path)
    {
        testparticipant.Clear();
        string[] groups = Directory.GetDirectories(path + @"\Groups");

        foreach (string group in groups)
        {
            List<List<string>> mbrs = new List<List<string>>();

            string[] members = Directory.GetDirectories(group + @"\Participants");

            foreach (string member in members)
            {
                List<string> tmember = new List<string>();

                string[] tests = Directory.GetDirectories(member + @"\Tests");

                foreach (string tst in tests)
                {
                    string nametest = tst.Substring(tst.LastIndexOf(@"\") + 1).Replace(".txt", "");
                    tmember.Add(nametest);
                }
                mbrs.Add(tmember);
            }
            testparticipant.Add(mbrs);
        }
    }

    static void UploadAnswer(List<List<List<List<string>>>> participantanswer, string path)
    {
        participantanswer.Clear();
        string[] groups = Directory.GetDirectories(path + @"\Groups");

        foreach (string group in groups)
        {
            List<List<List<string>>> mbrs = new List<List<List<string>>>();

            string[] members = Directory.GetDirectories(group + @"\Participants");

            foreach (string member in members)
            {
                List<List<string>> tmember = new List<List<string>>();

                string[] tests = Directory.GetDirectories(member + @"\Tests");

                foreach (string tst in tests)
                {
                    string pathtest = tst + @"\Answer.txt";
                    List<string> answers = new List<string>();

                    using (StreamReader sr = new StreamReader(pathtest))
                    {
                        int i = 0;
                        while (!sr.EndOfStream)
                        {
                            i++;
                            string answer = sr.ReadLine().Replace($"{i}. ", "");
                            answers.Add(answer);
                        }
                    }
                    tmember.Add(answers);

                }
                mbrs.Add(tmember);
            }
            participantanswer.Add(mbrs);
        }
    }

    static void UploadResult(List<List<List<List<double>>>> participantresult, string path)
    {
        participantresult.Clear();
        string[] groups = Directory.GetDirectories(path + @"\Groups");

        foreach (string group in groups)
        {
            List<List<List<double>>> mbrs = new List<List<List<double>>>();

            string[] members = Directory.GetDirectories(group + @"\Participants");

            foreach (string member in members)
            {
                List<List<double>> tmember = new List<List<double>>();

                string[] tests = Directory.GetDirectories(member + @"\Tests");

                foreach (string tst in tests)
                {
                    string pathtest = tst + @"\Result.txt";
                    List<double> result = new List<double>();

                    using (StreamReader sr = new StreamReader(pathtest))
                    {
                        result.Add(double.Parse(sr.ReadLine()));
                    }
                    tmember.Add(result);
                }
                mbrs.Add(tmember);
            }
            participantresult.Add(mbrs);
        }

    }

    static void UploadGroup(List<string> group, string path)
    {
        group.Clear();
        string[] groups = Directory.GetDirectories(path + @"\Groups");

        foreach (string group1 in groups)
        {
            string namegroup = group1.Substring(group1.LastIndexOf(@"\") + 1);
            group.Add(namegroup);
        }
    }

    static void UploadParticipant(List<List<string>> participant, string path)
    {
        participant.Clear();
        string[] groups = Directory.GetDirectories(path + @"\Groups");

        foreach (string group in groups)
        {
            List<string> mbrs = new List<string>();

            string[] members = Directory.GetDirectories(group + @"\Participants");

            foreach (string member in members)
            {
                string nameparticipant = member.Substring(member.LastIndexOf(@"\") + 1);
                mbrs.Add(nameparticipant);
            }
            participant.Add(mbrs);
        }
    }

    static void UploadDataLPGroup(List<List<DataLP>> grouplp, string path)
    {
        grouplp.Clear();
        string[] groups = Directory.GetDirectories(path + @"\Groups");

        foreach (string group in groups)
        {
            List<DataLP> datagroup = new List<DataLP>();
            string pathg = group + @"\Authorization.txt";

            using (StreamReader sr = new StreamReader(pathg))
            {
                DataLP d = new DataLP();
                d.login = sr.ReadLine().Replace("login: ", "");
                d.password = sr.ReadLine().Replace("password: ", "");
                datagroup.Add(d);
            }
            grouplp.Add(datagroup);
        }
    }

    static void UploadDataLPParticipants(List<List<List<DataLP>>> participantlp, string path)
    {
        participantlp.Clear();
        string[] groups = Directory.GetDirectories(path + @"\Groups");

        foreach (string group in groups)
        {
            List<List<DataLP>> mbrs = new List<List<DataLP>>();

            string[] members = Directory.GetDirectories(group + @"\Participants");

            foreach (string member in members)
            {
                List<DataLP> data = new List<DataLP>();
                string pathmember = member + @"\Authorization.txt";

                using (StreamReader sr = new StreamReader(pathmember))
                {
                    DataLP d = new DataLP();

                    d.login = sr.ReadLine().Replace("login: ", "");
                    d.password = sr.ReadLine().Replace("password: ", "");

                    data.Add(d);
                }
                mbrs.Add(data);
            }
            participantlp.Add(mbrs);
        }
    }

    static void UploadDataLPAdmin(ref DataLP adminlp, string path)
    {
        string pathadmin = path + @"\Admin\Authorization.txt";

        using (StreamReader sr = new StreamReader(pathadmin))
        {
            adminlp.login = sr.ReadLine().Replace("login: ", "");
            adminlp.password = sr.ReadLine().Replace("password: ", "");
        }
    }

    struct DataLP
    {
        public string login;
        public string password;
    }

    static bool Authorization(DataLP datalp)
    {
        Console.Clear();

        DataLP temp = new DataLP();

        Console.WriteLine("\n   Enter the authorization data...");
        Console.WriteLine("\n   Enter the login: ");
        Console.WriteLine("\n   Enter the password: ");
        Console.WriteLine("\n   Press Enter to continue...");

        Console.SetCursorPosition(20, 3);
        temp.login = Console.ReadLine();

        Console.SetCursorPosition(23, 5);
        temp.password = Console.ReadLine();

        return CheckCorrectDataAuthorization(temp, datalp);
    }

    static bool CheckCorrectDataAuthorization(DataLP temp, DataLP datalp)
    {
        Console.Clear();
        if (temp.login == datalp.login && temp.password == datalp.password)
        {
            return true;
        }
        else
        {
            List<string> menu = new List<string>() { "Data entered incorrectly. Would you like to try again?", "Yes", "No", "Press Enter to continue..." };
            if (WarningMenu(menu))
            {
                return Authorization(datalp);
            }
            else
            {
                return false;
            }
        }


    }


    static bool Administrator(DataLP adminlp, (List<string> test, List<List<string>> testquestion, List<List<List<string>>> testanswer, List<List<string>> testtrueanswer, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) data)
    {

        if (Authorization(adminlp))
        {
            var datav = (data.testgroup, data.testparticipant, data.participantresult, data.group, data.participant);
            var datat = (data.test, data.testquestion, data.testanswer, data.testtrueanswer, data.testgroup, data.group, data.path);
            var datac = (data.testgroup, data.testparticipant, data.participantanswer, data.participantresult, data.group, data.participant, data.grouplp, data.participantlp, data.path);
            var datap = (data.group, data.participant, data.grouplp, data.participantlp, data.path);

            bool show = true;

            List<string> menu = new List<string>() { "Select the menu bar...", "Viewing", "Test", "Change a group or participant", "Personal account", "Logout", "Press Enter to continue..." };

            while (show)
            {
                int option = SelectMenu(menu);

                Console.Clear();
                show = (option == 1 ? Viewing(datav) :
                    option == 2 ? Test(datat) :
                    option == 3 ? ChangeGorP(datac) :
                    option == 4 ? PersonalAccount(datap) : Logout());


                Console.Clear();
            }
        }

        return true;
    }

    static bool Viewing((List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant) datav)
    {
        bool show = true;

        List<string> menu = new List<string>() { "Select the menu bar...", "Group", "Participant", "Result by group", "Result by participant", "Back", "Press Enter to continue..." };

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == 1 ? Group(datav.group) :
                option == 2 ? Participant(datav.group, datav.participant) :
                option == 3 ? ResultByGroup(datav.group, datav.testgroup, datav.testparticipant, datav.participantresult) :
                option == 4 ? ResultByParticipant(datav) : false);


            Console.Clear();
        }
        return true;
    }

    static bool Group(List<string> group)
    {
        ShowGroup(group);
        return EnterMenu();
    }

    static void ShowGroup(List<string> group)
    {
        Console.WriteLine("\n   All groups...\n");
        foreach (string grp in group)
        {
            Console.WriteLine("   " + grp);
        }
        Console.WriteLine("\n   Press Enter to continue...");
    }
    static void ShowParticipant(int numgroup, List<List<string>> participant)
    {
        Console.WriteLine("\n   All participants...\n");
        foreach (string partspnt in participant[numgroup])
        {
            Console.WriteLine("   " + partspnt);
        }
        Console.WriteLine("\n   Press Enter to continue...");
    }

    static bool Participant(List<string> group, List<List<string>> participant)
    {
        bool show = true;

        List<string> menu = AddMenuGroup(group);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : Participant(option - 1, participant));


            Console.Clear();
        }
        return true;
    }

    static List<string> AddMenuGroup(List<string> group)
    {
        List<string> menu = new List<string>() { "Select a group..." };
        foreach (string grp in group)
        {
            menu.Add(grp);
        }
        menu.Add("Back");
        menu.Add("Press Enter to continue...");
        return menu;
    }
    static List<string> AddMenuParticipant(int numgroup, List<List<string>> participant)
    {
        List<string> menu = new List<string>() { "Select a participant..." };
        foreach (string partspnt in participant[numgroup])
        {
            menu.Add(partspnt);
        }
        menu.Add("Back");
        menu.Add("Press Enter to continue...");
        return menu;
    }

    static List<string> AddMenuTestGroup(int numgroup, List<List<string>> testgroup)
    {
        List<string> menu = new List<string>() { "Select a test..." };
        foreach (string testg in testgroup[numgroup])
        {
            menu.Add(testg);
        }
        menu.Add("Back");
        menu.Add("Press Enter to continue...");
        return menu;
    }

    static List<string> AddMenuTestParticipant(int numgroup, int numparticipant, List<List<List<string>>> testparticipant)
    {
        List<string> menu = new List<string>() { "Select a participant..." };
        foreach (string testpartspnt in testparticipant[numgroup][numparticipant])
        {
            menu.Add(testpartspnt);
        }
        menu.Add("Back");
        menu.Add("Press Enter to continue...");
        return menu;
    }

    static bool Participant(int numgroup, List<List<string>> participant)
    {
        ShowParticipant(numgroup, participant);
        return EnterMenu();
    }



    static bool ResultByGroup(List<string> group, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<double>>>> participantresult)
    {
        bool show = true;

        List<string> menu = AddMenuGroup(group);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ResultByGroup(option - 1, testgroup, testparticipant, participantresult));


            Console.Clear();
        }


        return true;
    }

    static bool ResultByGroup(int numgroup, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<double>>>> participantresult)
    {
        bool show = true;

        List<string> menu = AddMenuTestGroup(numgroup, testgroup);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ShowResultGroup(numgroup, option - 1, testgroup, testparticipant, participantresult));


            Console.Clear();
        }

        return true;
    }

    static bool ShowResultGroup(int numgroup, int numtest, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<double>>>> participantresult)
    {

        string test = testgroup[numgroup][numtest];

        int count = 0;
        if (!test.Contains(" form"))
        {
            double sum = 0;
            int npart = 0;
            foreach (List<string> participant in testparticipant[numgroup])
            {
                int ntest = 0;
                foreach (string nametest in participant)
                {

                    if (test == nametest)
                    {
                        sum += participantresult[numgroup][npart][ntest][0];
                        count++;
                    }
                    ntest++;
                }
                npart++;
            }

            if (count == 0)
            {
                Console.WriteLine("\n   None of the participants passed this test");
            }
            else
            {
                double result = sum / count;
                Console.WriteLine("\n   Average test result: " + result + "%");
            }
        }
        else
        {
            Console.WriteLine("\n   This test has no average results");
        }

        Console.WriteLine("\n   Press Enter to continue...");
        return EnterMenu();
    }



    static bool ResultByParticipant((List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant) datav)
    {

        bool show = true;

        List<string> menu = AddMenuGroup(datav.group);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ResultByParticipant(option - 1, datav.testparticipant, datav.participantresult, datav.participant));


            Console.Clear();
        }

        return true;
    }

    static bool ResultByParticipant(int numgroup, List<List<List<string>>> testparticipant, List<List<List<List<double>>>> participantresult, List<List<string>> participant)
    {
        bool show = true;

        List<string> menu = AddMenuParticipant(numgroup, participant);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ResultByParticipant(numgroup, option - 1, testparticipant, participantresult));


            Console.Clear();
        }

        return true;
    }
    static bool ResultByParticipant(int numgroup, int numparticipant, List<List<List<string>>> testparticipant, List<List<List<List<double>>>> participantresult)
    {
        bool show = true;

        List<string> menu = AddMenuTestParticipant(numgroup, numparticipant, testparticipant);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ShowResultParticipant(numgroup, numparticipant, option - 1, testparticipant, participantresult));


            Console.Clear();
        }

        return true;
    }

    static bool ShowResultParticipant(int numgroup, int numparticipant, int numtest, List<List<List<string>>> testparticipant, List<List<List<List<double>>>> participantresult)
    {
        if (!testparticipant[numgroup][numparticipant][numtest].Contains("form"))
        {
            Console.WriteLine($"\n   Your result for the test: {participantresult[numgroup][numparticipant][numtest][0]}%");
            Console.WriteLine("\n   Press Enter to continue...");
        }
        else
        {
            Console.WriteLine("\n   This test has no average results");
            Console.WriteLine("\n   Press Enter to continue...");
        }

        return EnterMenu();
    }



    static bool Test((List<string> test, List<List<string>> testquestion, List<List<List<string>>> testanswer, List<List<string>> testtrueanswer, List<List<string>> testgroup, List<string> group, string path) datat)
    {

        bool show = true;

        List<string> menu = new List<string>() { "Select the menu bar...", "Name of the tests", "Tests questions", "Answers to the test", "Open a test for a group", "Back", "Press Enter to continue..." };

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == 1 ? NameTests(datat.test) :
                option == 2 ? TestsQuestions(datat.test, datat.testquestion, datat.testanswer) :
                option == 3 ? AnswersTest(datat.test, datat.testquestion, datat.testtrueanswer) :
                option == 4 ? OpenTestForGroup(datat.test, datat.testgroup, datat.group, datat.path) : false);


            Console.Clear();
        }

        return true;
    }

    static bool NameTests(List<string> test)
    {
        ShowNameTest(test);
        return EnterMenu();
    }

    static void ShowNameTest(List<string> test)
    {
        Console.WriteLine("\n   All tests...\n");
        foreach (string name in test)
        {
            Console.WriteLine("   " + name);
        }
        Console.WriteLine("\n   Press Enter to continue...");
    }

    static List<string> AddMenuTest(List<string> test)
    {
        List<string> menu = new List<string>() { "Select a test..." };
        foreach (string name in test)
        {
            menu.Add(name);
        }
        menu.Add("Back");
        menu.Add("Press Enter to continue...");
        return menu;
    }

    static bool TestsQuestions(List<string> test, List<List<string>> testquestion, List<List<List<string>>> testanswer)
    {
        bool show = true;

        List<string> menu = AddMenuTest(test);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ShowTestQuestion(option - 1, testquestion, testanswer));


            Console.Clear();
        }
        return true;
    }


    static bool ShowTestQuestion(int numtest, List<List<string>> testquestion, List<List<List<string>>> testanswer)
    {

        if (testanswer[numtest][0].Count != 0)
        {
            Console.WriteLine("\n   List of all questions...");
            for (int i = 0; i < testquestion[numtest].Count; i++)
            {
                Console.WriteLine("\n   " + testquestion[numtest][i]);
                foreach (string answer in testanswer[numtest][i])
                {
                    Console.WriteLine("   " + answer);
                }

            }
        }
        else
        {
            Console.WriteLine("\n   List of all questions. This test does not contain any answers...");
            for (int i = 0; i < testquestion[numtest].Count; i++)
            {
                Console.WriteLine("\n   " + testquestion[numtest][i]);
            }
        }

        Console.WriteLine("\n   Press Enter to continue...");

        return EnterMenu();
    }

    static bool AnswersTest(List<string> test, List<List<string>> testquestion, List<List<string>> testtrueanswer)
    {
        bool show = true;

        List<string> menu = AddMenuTest(test);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ShowTestTrueQuestion(option - 1, testquestion, testtrueanswer));


            Console.Clear();
        }
        return true;
    }

    static bool ShowTestTrueQuestion(int numtest, List<List<string>> testquestion, List<List<string>> testtrueanswer)
    {

        if (testtrueanswer[numtest].Count != 1)
        {
            Console.WriteLine("\n   List of all responses...");
            for (int i = 0; i < testquestion[numtest].Count; i++)
            {
                Console.WriteLine("\n   " + testquestion[numtest][i]);
                Console.WriteLine("   " + testtrueanswer[numtest][i]);

            }
        }
        else
        {
            Console.WriteLine($"\n   List of all responses. {testtrueanswer[numtest][0]}");
        }

        Console.WriteLine("\n   Press Enter to continue...");

        return EnterMenu();
    }


    static bool OpenTestForGroup(List<string> test, List<List<string>> testgroup, List<string> group, string path)
    {
        bool show = true;

        List<string> menu = AddMenuGroup(group);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : OpenTestForGroup(option - 1, test, testgroup, group, path));

            Console.Clear();
        }

        return true;
    }

    static bool OpenTestForGroup(int numgroup, List<string> test, List<List<string>> testgroup, List<string> group, string path)
    {
        bool show = true;

        List<string> menu = AddMenuTest(test);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : OpenTestForGroup(numgroup, option - 1, test, testgroup, group, path));

            Console.Clear();
        }

        return true;
    }

    static bool OpenTestForGroup(int numgroup, int numtest, List<string> test, List<List<string>> testgroup, List<string> group, string path)
    {


        if (!testgroup[numgroup].Contains(test[numtest]))
        {

            string pathout = path + $@"\Tests\{test[numtest]}.txt";
            string pathin = path + $@"\Groups\{group[numgroup]}\Tests\{test[numtest]}.txt";

            File.Copy(pathout, pathin);
            UploadTestGroup(testgroup, path);

            Console.WriteLine("\n   This test was successfully opened...");
            Console.WriteLine("\n   Press Enter to continue...");
        }
        else
        {
            Console.WriteLine("\n   This test is already open for passing...");
            Console.WriteLine("\n   Press Enter to continue...");
        }

        return EnterMenu();
    }




    static bool ChangeGorP((List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        bool show = true;

        List<string> menu = new List<string>() { "Select the menu bar...", "Add a group", "Add a participant", "Delete a group", "Delete a participant", "Back", "Press Enter to continue..." };

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == 1 ? AddGroup(datac) :
                option == 2 ? AddParticipant(datac) :
                option == 3 ? DeleteGroup(datac) :
                option == 4 ? DeleteParticipant(datac) : false);


            Console.Clear();
        }
        return true;
    }

    static void UploadChangeGorP((List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        UploadTestGroup(datac.testgroup, datac.path);
        UploadTestParticipant(datac.testparticipant, datac.path);
        UploadAnswer(datac.participantanswer, datac.path);
        UploadResult(datac.participantresult, datac.path);
        UploadGroup(datac.group, datac.path);
        UploadParticipant(datac.participant, datac.path);
        UploadDataLPGroup(datac.grouplp, datac.path);
        UploadDataLPParticipants(datac.participantlp, datac.path);
    }

    static bool AddGroup((List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        Console.Clear();

        string line = "Enter the group's nickname: ";
        string name = ChangeName(line);

        for (int i = 0; i < datac.group.Count; i++)
        {
            if (name == datac.group[i])
            {
                Console.Clear();

                List<string> menu = new List<string>() { "A group with this nickname already exists. Will you try to enter it again?", "Yes", "No", "Press Enter to continue..." };
                i = datac.group.Count;

                if (WarningMenu(menu))
                {
                    return AddGroup(datac);
                }
                else
                {
                    return true;
                }

            }
        }

        Console.Write("\n   Enter the group's login: ");
        string login = Console.ReadLine();

        Console.Write("\n   Enter the group's password: ");
        string password = Console.ReadLine();

        AddGroup(name, login, password, datac);
        return true;
    }




    static void AddGroup(string name, string login, string password, (List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {

        Console.Clear();
        List<string> menu = new List<string>() { "Are you sure you want to add this group?", "Yes", "No", "Press Enter to continue..." };

        if (WarningMenu(menu))
        {
            string pathg = datac.path + $@"\Groups\{name}";
            string pathp = datac.path + $@"\Groups\{name}\Participants";
            string patha = datac.path + $@"\Groups\{name}\Authorization.txt";
            string patht = datac.path + $@"\Groups\{name}\Tests";
            Directory.CreateDirectory(pathg);
            Directory.CreateDirectory(pathp);
            Directory.CreateDirectory(patht);
            File.Create(patha).Close();
            /* можно методом *///-----------------------------------------------
            using (StreamWriter sw = new StreamWriter(patha))
            {
                sw.WriteLine("login: " + login);
                sw.WriteLine("password: " + password);
            }
            UploadChangeGorP(datac);

            Console.Clear();
            Console.WriteLine("\n   You have successfully added a group!");
            Console.WriteLine("\n   Press Enter to continue...");
            EnterMenu();
        }

    }


    static bool AddParticipant((List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        bool show = true;

        List<string> menu = AddMenuGroup(datac.group);

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : AddParticipant(option - 1, datac));

            Console.Clear();
        }

        return true;
    }

    static bool AddParticipant(int numgroup, (List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        Console.Clear();

        string line = "Enter the user's nickname: ";
        string name = ChangeName(line);

        for (int i = 0; i < datac.participant[numgroup].Count; i++)
        {
            if (name == datac.participant[numgroup][i])
            {
                Console.Clear();

                List<string> menu = new List<string>() { "A user with this nickname already exists. Will you try to enter it again?", "Yes", "No", "Press Enter to continue..." };
                i = datac.participant[numgroup].Count;

                if (WarningMenu(menu))
                {
                    return AddParticipant(numgroup, datac);
                }
                else
                {
                    return true;
                }

            }
        }

        Console.Write("\n   Enter the user's login: ");
        string login = Console.ReadLine();

        Console.Write("\n   Enter the user's password: ");
        string password = Console.ReadLine();

        AddParticipant(name, login, password, numgroup, datac);
        return true;
    }

    static void AddParticipant(string name, string login, string password, int numgroup, (List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {

        Console.Clear();
        List<string> menu = new List<string>() { "Are you sure you want to add this user?", "Yes", "No", "Press Enter to continue..." };

        if (WarningMenu(menu))
        {


            string pathp = datac.path + $@"\Groups\{datac.group[numgroup]}\Participants\{name}";
            string patha = datac.path + $@"\Groups\{datac.group[numgroup]}\Participants\{name}\Authorization.txt";
            string patht = datac.path + $@"\Groups\{datac.group[numgroup]}\Participants\{name}\Tests";
            Directory.CreateDirectory(pathp);
            File.Create(patha).Close();
            Directory.CreateDirectory(patht);
            /* можно методом *///-----------------------------------------------
            using (StreamWriter sw = new StreamWriter(patha))
            {
                sw.WriteLine("login: " + login);
                sw.WriteLine("password: " + password);
            }
            UploadChangeGorP(datac);

            Console.Clear();
            Console.WriteLine("\n   You have successfully added a user!");
            Console.WriteLine("\n   Press Enter to continue...");
            EnterMenu();
        }

    }

    static bool DeleteGroup((List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        bool show = true;

        while (show)
        {
            List<string> menu = AddMenuGroup(datac.group);

            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : DeleteGroup(option - 1, datac));

            Console.Clear();
        }
        return true;
    }

    static bool DeleteGroup(int numgroup, (List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {

        List<string> menu = new List<string>() { "Are you sure you want to delete the group? Select the menu bar...", "Yes", "No", "Press Enter to continue..." };
        if (WarningMenu(menu))
        {
            string pathg = datac.path + $@"\Groups\{datac.group[numgroup]}";
            Directory.Delete(pathg, true);
            UploadChangeGorP(datac);
        }

        return true;
    }


    static bool DeleteParticipant((List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        bool show = true;

        while (show)
        {
            List<string> menu = AddMenuGroup(datac.group);

            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : DeleteParticipant(option - 1, datac));

            Console.Clear();
        }
        return true;
    }

    static bool DeleteParticipant(int numgroup, (List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        bool show = true;

        while (show)
        {
            List<string> menu = AddMenuParticipant(numgroup, datac.participant);

            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : DeleteParticipant(numgroup, option - 1, datac));

            Console.Clear();
        }
        return true;
    }

    static bool DeleteParticipant(int numgroup, int numparticipant, (List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datac)
    {
        List<string> menu = new List<string>() { "Are you sure you want to delete the group? Select the menu bar...", "Yes", "No", "Press Enter to continue..." };
        if (WarningMenu(menu))
        {
            string pathp = datac.path + $@"\Groups\{datac.group[numgroup]}\Participants\{datac.participant[numgroup][numparticipant]}";
            Directory.Delete(pathp, true);
            UploadChangeGorP(datac);
        }

        return true;
    }


    static void UploadAllDataLP((List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datap)
    {
        UploadGroup(datap.group, datap.path);
        UploadParticipant(datap.participant, datap.path);
        UploadDataLPGroup(datap.grouplp, datap.path);
        UploadDataLPParticipants(datap.participantlp, datap.path);
    }

    static bool PersonalAccount((List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datap)
    {
        bool show = true;

        List<string> menu = new List<string>() { "Select the menu bar...", "Action on a group", "Change login details", "Back", "Press Enter to continue..." };

        string patha = datap.path + @"\Admin\Authorization.txt";

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == 1 ? ActionOnGroup(datap) :
                option == 2 ? ChangeDataLP(patha) : false);

            UploadAllDataLP(datap);

            Console.Clear();
        }
        return true;
    }

    static bool ActionOnGroup((List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datap)
    {
        bool show = true;

        while (show)
        {

            List<string> menu = AddMenuGroup(datap.group);

            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ActionOnGroup(option - 1, datap));



            Console.Clear();
        }
        return true;
    }

    static bool ActionOnGroup(int numgroup, (List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datap)
    {
        {
            bool show = true;

            List<string> menu = new List<string>() { "Select the menu bar...", "Action on a participant", "Сhange the name of the group", "Change login details", "Back", "Press Enter to continue..." };

            string pathg = datap.path + $@"\Groups\{datap.group[numgroup]}\Authorization.txt";
            string pathgd = datap.path + $@"\Groups\{datap.group[numgroup]}";

            while (show)
            {
                int option = SelectMenu(menu);

                Console.Clear();
                show = (option == 1 ? ActionOnParticipant(numgroup, datap) :
                    option == 2 ? ChangeNameData(pathgd) :
                    option == 3 ? ChangeDataLP(pathg) : false);

                UploadAllDataLP(datap);

                pathg = datap.path + $@"\Groups\{datap.group[numgroup]}\Authorization.txt";
                pathgd = datap.path + $@"\Groups\{datap.group[numgroup]}";

                Console.Clear();
            }
        }
        return true;
    }

    static bool ActionOnParticipant(int numgroup, (List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datap)
    {
        bool show = true;

        while (show)
        {
            List<string> menu = AddMenuParticipant(numgroup, datap.participant);

            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : ActionOnParticipant(numgroup, option - 1, datap));

            Console.Clear();
        }
        return true;
    }

    static bool ActionOnParticipant(int numgroup, int numparticipant, (List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datap)
    {
        {
            bool show = true;

            List<string> menu = new List<string>() { "Select the menu bar...", "Сhange the name of the participant", "Change login details", "Back", "Press Enter to continue..." };

            string pathp = datap.path + $@"\Groups\{datap.group[numgroup]}\Participants\{datap.participant[numgroup][numparticipant]}\Authorization.txt";
            string pathpd = datap.path + $@"\Groups\{datap.group[numgroup]}\Participants\{datap.participant[numgroup][numparticipant]}";

            while (show)
            {
                int option = SelectMenu(menu);

                Console.Clear();
                show = (option == 1 ? ChangeNameData(pathpd) :
                    option == 2 ? ChangeDataLP(pathp) : false);

                UploadAllDataLP(datap);

                pathp = datap.path + $@"\Groups\{datap.group[numgroup]}\Participants\{datap.participant[numgroup][numparticipant]}\Authorization.txt";
                pathpd = datap.path + $@"\Groups\{datap.group[numgroup]}\Participants\{datap.participant[numgroup][numparticipant]}";

                Console.Clear();
            }
        }
        return true;
    }

    static bool ChangeNameData(string path)
    {
        string line = "Enter a new name: ";
        string newname = ChangeName(line);

        List<string> menu = new List<string>() { "Are you sure you want to change your nickname?", "Yes", "No", "Press Enter to continue..." };

        if (WarningMenu(menu))
        {
            string oldname = path.Substring(path.LastIndexOf(@"\") + 1);
            string pathn = path.Replace(oldname, newname);
            if (oldname != newname)
            {
                Directory.Move(path, pathn);
            }

            Console.WriteLine("\n   You have successfully changed your nickname...");
            Console.WriteLine("\n   Press Enter to continue...");

            return EnterMenu();
        }
        else
        {
            return true;
        }

    }

    static bool ChangeDataLP(string path)
    {
        int option = 1;
        string newdata = ChangeDataLP(ref option);

        if (newdata != "false")
        {


            List<string> menu = new List<string>() { "Are you sure you want to change your login details?", "Yes", "No", "Press Enter to continue..." };

            if (WarningMenu(menu))
            {

                string login, password;

                using (StreamReader sr = new StreamReader(path))
                {
                    login = sr.ReadLine().Replace("login: ", "");
                    password = sr.ReadLine().Replace("password: ", "");
                }

                using (StreamWriter sw = new StreamWriter(path))
                {
                    if (option == 1)
                    {
                        sw.WriteLine("login: " + newdata);
                        sw.WriteLine("password: " + password);
                    }
                    else
                    {
                        sw.WriteLine("login: " + login);
                        sw.WriteLine("password: " + newdata);
                    }
                }

                Console.WriteLine("\n   You have successfully changed your login details...");
                Console.WriteLine("\n   Press Enter to continue...");

                return EnterMenu();
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    static string ChangeDataLP(ref int option)
    {
        option = ChangeSelectLorP();
        if (option == 1)
        {
            return ChangeLogin();
        }
        if (option == 2)
        {
            return ChangePassword();
        }
        if (option == 3)
        {
            return "false";
        }

        return "false";
    }

    static int ChangeSelectLorP()
    {
        bool show = true;

        List<string> menu = new List<string>() { "Select the menu bar...", "Сhange login", "Change password", "Back", "Press Enter to continue..." };

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            return option;
        }

        return -1;
    }

    static string ChangeLogin()
    {
        Console.Write("\n   Enter a new login: ");
        string login = Console.ReadLine();

        return login;
    }

    static string ChangePassword()
    {
        Console.Write("\n   Enter a new password: ");
        string passworrd = Console.ReadLine();

        return passworrd;
    }

    static string ChangeName(string line)
    {
        string error = "";
        WriteMenuName(line, error);

        string? name = "";
        bool t = true;

        while (t)
        {
            name = Console.ReadLine();
            if (name == "" || name.Contains("  ") || name.StartsWith(" "))
            {
                CorrectResponseInputName(line, name);
            }
            else
            {
                t = false;
            }
        }

        return name;
    }

    static void CorrectResponseInputName(string line, string name)
    {
        string error = "The string must not be empty or contain more than one space!\n\n   ";
        WriteMenuName(line, error);
    }

    static void WriteMenuName(string line, string error)
    {
        Console.Clear();
        Console.Write($"\n   {error}{line}");
    }


    static bool User((List<string> test, List<List<string>> testquestion, List<List<List<string>>> testanswer, List<List<string>> testtrueanswer, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) data)
    {
        bool show = true;

        while (show)
        {

            List<string> menu = AddMenuGroup(data.group);

            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == menu.Count - 2 ? false : User(option - 1, data));

            Console.Clear();
        }
        return true;
    }
    static bool User(int numgroup, (List<string> test, List<List<string>> testquestion, List<List<List<string>>> testanswer, List<List<string>> testtrueanswer, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) data)
    {

        if (Authorization(data.grouplp[numgroup][0]))
        {
            bool show = true;

            while (show)
            {

                List<string> menu = AddMenuParticipant(numgroup, data.participant);

                int option = SelectMenu(menu);

                Console.Clear();
                show = (option == menu.Count - 2 ? false : User(numgroup, option - 1, data));

                Console.Clear();
            }
        }
        return true;
    }

    static bool User(int numgroup, int numparticipant, (List<string> test, List<List<string>> testquestion, List<List<List<string>>> testanswer, List<List<string>> testtrueanswer, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) data)
    {
        if (Authorization(data.participantlp[numgroup][numparticipant][0]))
        {
            bool show = true;

            List<string> menu = new List<string>() { "Select the menu bar...", "Take the test", "Viewing result", "Personal account", "Logout", "Press Enter to continue..." };

            var datap = (data.group, data.participant, data.grouplp, data.participantlp, data.path);
            var datav = (data.test, data.testquestion, data.testparticipant, data.participantanswer, data.participantresult, data.path);
            var datat = (data.test, data.testquestion, data.testanswer, data.testtrueanswer, data.testgroup, data.testparticipant, data.participantanswer, data.participantresult, data.group, data.participant, data.path);

            while (show)
            {
                int option = SelectMenu(menu);

                Console.Clear();
                show = (option == 1 ? TakeTest(numgroup, numparticipant, datat) :
                    option == 2 ? ViewingResult(numgroup, numparticipant, datav) :
                    option == 3 ? PersonalAccount(numgroup, numparticipant, datap) : Logout());

                Console.Clear();
            }
        }
        return true;
    }

    static bool TakeTest(int numgroup, int numparticipant, (List<string> test, List<List<string>> testquestion, List<List<List<string>>> testanswer, List<List<string>> testtrueanswer, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, string path) datat)
    {

        bool show = true;
        while (show)
        {
            List<string> menutest = AddMenuTestGroup(numgroup, datat.testgroup);
            int option = SelectMenu(menutest);

            if (option == menutest.Count - 2)
            {
                return true;
            }
            else
            {
                if (IsThereATest(numgroup, numparticipant, option - 1, datat.testgroup, datat.testparticipant))
                {

                    int ntest = FindNumberTest(numgroup, option - 1, datat.test, datat.testgroup);

                    return TakeTest(numgroup, numparticipant, option - 1, ntest, datat.testquestion, datat.testanswer, datat.testtrueanswer, datat.testgroup, datat.testparticipant, datat.participantanswer, datat.participantresult, datat.group, datat.participant, datat.path);
                }
                else
                {
                    show = TestPassed();
                    Console.Clear();
                }
            }
        }

        return true;
    }

    static bool IsThereATest(int numgroup, int numparticipant, int numtest, List<List<string>> testgroup, List<List<List<string>>> testparticipant)
    {
        foreach (string test in testparticipant[numgroup][numparticipant])
        {
            if (testgroup[numgroup][numtest] == test)
            {
                return false;
            }
        }
        return true;
    }

    static bool TestPassed()
    {
        Console.Clear();
        Console.Write("\n   This test has already been passed!");
        Console.Write("\n   Press Enter to continue...");
        return EnterMenu();
    }

    static int FindNumberTest(int numgroup, int numtest, List<string> test, List<List<string>> testgroup)
    {

        int ntest = -1;
        foreach (string namet in test)
        {
            ntest++;
            if (namet == testgroup[numgroup][numtest])
            {
                return ntest;
            }
        }

        return ntest;
    }

    static bool TakeTest(int numgroup, int numparticipant, int ntest, int numtest, List<List<string>> testquestion, List<List<List<string>>> testanswer, List<List<string>> testtrueanswer, List<List<string>> testgroup, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, List<string> group, List<List<string>> participant, string path)
    {

        double result = 0;
        List<List<string>> menutestquestion = new List<List<string>>();
        List<string> answer = new List<string>();

        if (!testgroup[numgroup][ntest].Contains("form"))
        {
            menutestquestion = CreateQuestionMenu(numtest, testquestion, testanswer);
            answer = AnswersTheTest(menutestquestion);
            result = CalculateTheResult(numtest, answer, testtrueanswer); //зедсь вызвало исключение при правильонм овтете
        }
        else
        {
            menutestquestion = CreateQuestionMenuForm(numtest, testquestion);
            answer = AnswersTheTestForm(menutestquestion);
        }

        RecordTheResult(answer, result, numgroup, numparticipant, ntest, testgroup, group, participant, path);

        UploadAllTest(testparticipant, participantanswer, participantresult, path);

        return true;
    }

    static void UploadAllTest(List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, string path)
    {
        UploadTestParticipant(testparticipant, path);
        UploadAnswer(participantanswer, path);
        UploadResult(participantresult, path);
    }

    static List<List<string>> CreateQuestionMenu(int numtest, List<List<string>> testquestion, List<List<List<string>>> testanswer)
    {
        List<List<string>> menutest = new List<List<string>>();

        for (int i = 0; i < testquestion[numtest].Count; i++)
        {
            List<string> quest = new List<string>();

            quest.Add(testquestion[numtest][i]);
            foreach (string answer in testanswer[numtest][i])
            {
                quest.Add(answer);
            }
            quest.Add("Press Enter to continue...");

            menutest.Add(quest);
        }

        return menutest;
    }

    static List<string> AnswersTheTest(List<List<string>> menutestquestion)
    {
        List<string> answer = new List<string>();

        for (int i = 0; i < menutestquestion.Count; i++)
        {
            Console.Clear();
            int trueanswer = SelectMenu(menutestquestion[i]);
            answer.Add(menutestquestion[i][trueanswer]);
        }

        return answer;
    }

    static double CalculateTheResult(int numtest, List<string> answer, List<List<string>> testtrueanswer)
    {

        int trueanswer = 0;
        double result = 0;

        for (int i = 0; i < answer.Count; i++)
        {
            if (answer[i] == testtrueanswer[numtest][i])
            {
                trueanswer++;
            }
        }

        result = ((double)trueanswer / (double)answer.Count) * 100;

        return result;
    }

    static List<List<string>> CreateQuestionMenuForm(int numtest, List<List<string>> testquestion)
    {
        List<List<string>> menutest = new List<List<string>>();

        for (int i = 0; i < testquestion[numtest].Count; i++)
        {
            List<string> quest = new List<string>();

            quest.Add(testquestion[numtest][i]);
            quest.Add("Your answer: ");
            quest.Add("Press Enter to continue...");

            menutest.Add(quest);
        }

        return menutest;
    }

    static List<string> AnswersTheTestForm(List<List<string>> menutestquestion)
    {
        string error = "";
        List<string> answer = new List<string>();

        for (int i = 0; i < menutestquestion.Count; i++)
        {
            WriteMenuAnswer(menutestquestion, i, error);

            string? trueanswer = "";
            bool t = true;

            while (t)
            {
                trueanswer = Console.ReadLine();
                if (trueanswer == "" || trueanswer.Contains("  ") || trueanswer.StartsWith(" "))
                {
                    CorrectResponseInput(menutestquestion, i, trueanswer);
                }
                else
                {
                    t = false;
                }
            }
            Console.WriteLine("\u001b[0m");

            answer.Add(trueanswer);
        }

        return answer;
    }

    static void CorrectResponseInput(List<List<string>> menutestquestion, int i, string trueanswer)
    {
        string error = "The string must not be empty or contain more than one space! ";
        WriteMenuAnswer(menutestquestion, i, error);
    }

    static void WriteMenuAnswer(List<List<string>> menutestquestion, int i, string error)
    {
        string colorred = "\u001b[38;5;240m";
        string colorwhite = "\u001b[0m";

        Console.Clear();

        Console.WriteLine(colorwhite + $"\n   {error}{menutestquestion[i][0]}");

        Console.WriteLine(colorred + $"\n   {menutestquestion[i][1]}" + colorwhite);

        Console.WriteLine($"\n   {menutestquestion[i][2]}");

        Console.WriteLine(colorred);
        Console.SetCursorPosition(16, 3);
    }

    static void RecordTheResult(List<string> answer, double result, int numgroup, int numparticipant, int ntest, List<List<string>> testgroup, List<string> group, List<List<string>> participant, string path)
    {

        string patht = path + $@"\Groups\{group[numgroup]}\Participants\{participant[numgroup][numparticipant]}\Tests\{testgroup[numgroup][ntest]}";
        string patha = path + $@"\Groups\{group[numgroup]}\Participants\{participant[numgroup][numparticipant]}\Tests\{testgroup[numgroup][ntest]}\Answer.txt";
        string pathr = path + $@"\Groups\{group[numgroup]}\Participants\{participant[numgroup][numparticipant]}\Tests\{testgroup[numgroup][ntest]}\Result.txt";

        Directory.CreateDirectory(patht);
        File.Create(patha).Close();
        File.Create(pathr).Close();

        WriteDownTheAnswer(answer, result, patha, pathr);

    }

    static void WriteDownTheAnswer(List<string> answer, double result, string patha, string pathr)
    {

        using (StreamWriter sw = new StreamWriter(patha))
        {
            for (int i = 1; i <= answer.Count; i++)
            {
                sw.WriteLine($"{i}. {answer[i - 1]}");
            }
        }

        using (StreamWriter sw = new StreamWriter(pathr))
        {
            sw.WriteLine($"{result:f2}");
        }

    }



    static bool ViewingResult(int numgroup, int numparticipant, (List<string> test, List<List<string>> testquestion, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer, List<List<List<List<double>>>> participantresult, string path) datav)
    {
        bool show = true;

        List<string> menu = new List<string>() { "Select the menu bar...", "Viewing answer", "Viewing result", "Back", "Press Enter to continue..." };

        while (show)
        {
            int option = SelectMenu(menu);

            Console.Clear();
            show = (option == 1 ? AnswerByParticipant(numgroup, numparticipant, datav.test, datav.testquestion, datav.testparticipant, datav.participantanswer) :
                option == 2 ? ResultByParticipant(numgroup, numparticipant, datav.testparticipant, datav.participantresult) : false);

            Console.Clear();
        }
        return true;
    }


    static bool AnswerByParticipant(int numgroup, int numparticipant, List<string> test, List<List<string>> testquestion, List<List<List<string>>> testparticipant, List<List<List<List<string>>>> participantanswer)
    {
        bool show = true;

        List<string> menu = AddMenuTestParticipant(numgroup, numparticipant, testparticipant);

        while (show)
        {
            int option = SelectMenu(menu);
            if (option != menu.Count - 2)
            {
                int numtest = FindNumberTest(numparticipant, option - 1, test, testparticipant[numgroup]);

                Console.Clear();
                show = (option == menu.Count - 2 ? false : ShowAnswerParticipant(numgroup, numparticipant, numtest, option - 1, testquestion, participantanswer));


                Console.Clear();
            }
            else
            {
                show = false;
                Console.Clear();
            }

        }

        return true;
    }

    static bool ShowAnswerParticipant(int numgroup, int numparticipant, int numtest, int ntest, List<List<string>> testquestion, List<List<List<List<string>>>> participantanswer)
    {

        Console.WriteLine("\n   List of all responses...");
        for (int i = 0; i < testquestion[numtest].Count; i++)
        {
            Console.WriteLine($"\n   {testquestion[numtest][i]}");

            string answer = participantanswer[numgroup][numparticipant][ntest][i];
            answer = answer.Replace($"{i + 1}. ", "");

            Console.WriteLine($"   {answer}");
        }
        Console.WriteLine("\n   Press Enter to continue...");

        return EnterMenu();

    }


    static bool PersonalAccount(int numgroup, int numparticipant, (List<string> group, List<List<string>> participant, List<List<DataLP>> grouplp, List<List<List<DataLP>>> participantlp, string path) datap)
    {
        ActionOnParticipant(numgroup, numparticipant, datap);

        return true;
    }

    static bool Logout()
    {
        List<string> menu = new List<string>() { "Do you want to log out of your account? Select the menu bar...", "Yes", "No", "Press Enter to continue..." };

        bool show;
        return show = (SelectMenu(menu) == 1 ? false : true);
    }

    static bool Exit()
    {

        List<string> menu = new List<string>() { "Do you want to exit the program? Select the menu bar...", "Yes", "No", "Press Enter to continue..." };

        bool show;
        return show = (SelectMenu(menu) == 1 ? false : true);
    }


}


