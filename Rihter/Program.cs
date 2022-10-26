namespace Rihter
{
    /// <summary>
    /// Объявление делегата; экземпляр ссылается на метод
    /// с параметром типа Int32, возвращающий значение void
    /// </summary>
    /// <param name="value"></param>
    internal delegate void Feedback(Int32 value);
    public sealed class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            StaticDelegateDemo();
            InstanceDelegateDemo();
            ChainDelegateDemo1(new Program());
            ChainDelegateDemo2(new Program());

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        /// <summary>
        /// Вызывает метод Counter, передавая в третьем параметре fb значение null.
        /// В результате при обработке элементов не задействуется метод обратного вызова
        /// </summary>
        private static void StaticDelegateDemo()
        {
            Console.WriteLine("----- Static Delegate Demo -----");
            /*Вызывает метод Counter, передавая в третьем параметре fb значение null.
              В результате при обработке элементов не задействуется метод обратного вызова*/
            Counter(1, 3, null);
            /*
             При втором вызове метода Counter методом StaticDelegateDemo третьему параметру передается только что созданный делегат Feedback. 
             Этот делегат служит  оболочкой для другого метода, позволяя выполнить обратный вызов последнего косвенно, через оболочку. 
             В рассматриваемом примере имя статического метода Program.FeedbackToConsole передается конструктору Feedback, указывая, 
             что именно для него требуется создать оболочку.  
            Возвращенная оператором new ссылка
            передается третьему параметру метода Counter, который в процессе выполнения будет вызывать статический метод FeedbackToConsole. 
            Последний же просто выводит на консоль строку с названием обрабатываемого элемента.*/
            Counter(1, 3, new Feedback(Program.FeedbackToConsole));

            /**/
            Counter(1, 3, new Feedback(FeedbackToMsgBox)); // Префикс "Program."
                                                           // не обязателен
            Console.WriteLine();
        }

        private static void InstanceDelegateDemo()
        {
            Console.WriteLine("----- Instance Delegate Demo -----");
            Program p = new Program();
            Counter(1, 3, new Feedback(p.FeedbackToFile));
            Console.WriteLine();
        }

        private static void ChainDelegateDemo1(Program p)
        {
            Console.WriteLine("----- Chain Delegate Demo 1 -----");
            Feedback fb1 = new Feedback(FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            Feedback fb3 = new Feedback(p.FeedbackToFile);
            Feedback fbChain = null;
            fbChain = (Feedback)Delegate.Combine(fbChain, fb1);
            fbChain = (Feedback)Delegate.Combine(fbChain, fb2);
            fbChain = (Feedback)Delegate.Combine(fbChain, fb3);
            Counter(1, 2, fbChain);
            Console.WriteLine();
            fbChain = (Feedback)
            Delegate.Remove(fbChain, new Feedback(FeedbackToMsgBox));
            Counter(1, 2, fbChain);
        }

        private static void ChainDelegateDemo2(Program p)
        {
            Console.WriteLine("----- Chain Delegate Demo 2 -----");
            Feedback fb1 = new Feedback(FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            Feedback fb3 = new Feedback(p.FeedbackToFile);
            Feedback fbChain = null;
            fbChain += fb1;
            fbChain += fb2;
            fbChain += fb3;
            Counter(1, 2, fbChain);
            Console.WriteLine();
            fbChain -= new Feedback(FeedbackToMsgBox);
            Counter(1, 2, fbChain);
        }

        /// <summary>
        /// Перебор целых чисел
        /// </summary>
        /// <param name="from">Параметр отвечающий за начальное значение диапозона целых чисел</param>
        /// <param name="to">Параметр отвечающий за конечное значение диапозона целых чисел</param>
        /// <param name="fb">Ссылка на делегат Feedback</param>
        private static void Counter(Int32 from, Int32 to, Feedback fb)
        {
            for (Int32 val = from; val <= to; val++)
            {
                // Если указаны методы обратного вызова, вызываем их
                if (fb != null)
                    fb(val);
            }
        }

        /// <summary>
        /// Метод FeedbackToConsole определен в типе Program как закрытый, но при этом может быть вызван методом Counter. Так как оба метода определены в пределах одного
        ///типа, проблем с безопасностью не возникает.Но даже если бы метод Counter был
        ///определен в другом типе, это не сказалось бы на работе коде.Другими словами,
        ///если код одного типа вызывает посредством делегата закрытый член другого типа,
        ///проблем с безопасностью или уровнем доступа не возникает, если делегат создан
        ///в коде, имеющем нужный уровень доступа.
        /// </summary>
        /// <param name="value"></param>
        private static void FeedbackToConsole(Int32 value)
        {
            Console.WriteLine("Item=" + value);
        }

        private static void FeedbackToMsgBox(Int32 value)
        {
            MessageBox.Show("Item=" + value);
        }

        private void FeedbackToFile(Int32 value)
        {
            using (StreamWriter sw = new StreamWriter("Status", true))
            {
                sw.WriteLine("Item=" + value);
            }
        }
    }
}