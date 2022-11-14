using System;
using System.Threading;
/*Страница 749 4-го издания рихтера*/
namespace AsynchronouThreads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main thread: queuing an asynchronous operation");
            ThreadPool.QueueUserWorkItem(ComputeBoundOp, 5);
            Console.WriteLine("Main thread: Doing other work here...");
            Thread.Sleep(10000); // Имитация другой работы (10 секунд)
            Console.WriteLine("Hit <Enter> to end this program...");
            Console.ReadLine();
        }

        // Сигнатура метода совпадает с сигнатурой делегата WaitCallback
        private static void ComputeBoundOp(Object state)
        {
            // Метод выполняется потоком из пула
            Console.WriteLine("In ComputeBoundOp: state={0}", state);
            Thread.Sleep(1000); // Имитация другой работы (1 секунда)
                                // После возвращения управления методом поток
                                // возвращается в пул и ожидает следующего задания
        }
    }
}

/*
 Результат компиляции и запуска этого кода:
Main thread: queuing an asynchronous operation
Main thread: Doing other work here...
In ComputeBoundOp: state=5

Впрочем, возможен и такой результат:
Main thread: queuing an asynchronous operation
In ComputeBoundOp: state=5
Main thread: Doing other work here...
    Разный порядок следования строк в данном случае объясняется асинхронным
выполнением методов. Планировщик Windows решает, какой поток должен вы-
полняться первым, или же планирует их для одновременного выполнения на
многопроцессорном компьютере.
    ПримечАние
Если метод обратного вызова генерирует необработанное исключение, CLR заверша-
ет процесс (если это не противоречит политике хоста). Необработанные исключения
обсуждались в главе 20.
    ПримечАние
В приложениях Windows Store класс System.Threading.ThreadPool недоступен для
открытого использования. Впрочем, он косвенно используется при использовании
типов из пространства имен System.Threading.Tasks (см. раздел «Задания» далее
в этой главе).


 
 
 */