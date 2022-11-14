using Microsoft.IdentityModel.Tokens;
using System;
    
/*стр 751*/
namespace BanOnCopyingTheExecutionContext
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Помещаем данные в контекст логического вызова потока метода Main
            CallContext.LogicalSetData("Name", "Jeffrey");
            // Заставляем поток из пула работать
            // Поток из пула имеет доступ к данным контекста логического вызова
            ThreadPool.QueueUserWorkItem(
            state => Console.WriteLine("Name={0}",
            CallContext.LogicalGetData("Name")));
            // Запрещаем копирование контекста исполнения потока метода Main
            ExecutionContext.SuppressFlow();
            // Заставляем поток из пула выполнить работу.
            // Поток из пула НЕ имеет доступа к данным контекста логического вызова
            ThreadPool.QueueUserWorkItem(state => Console.WriteLine("Name={0}",CallContext.LogicalGetData("Name")));
            // Восстанавливаем копирование контекста исполнения потока метода Main
            // на случай будущей работы с другими потоками из пула
            ExecutionContext.RestoreFlow();
            Console.ReadLine();
        }
    }
    
}
/*CallContext.LogicalSetData не работает, хз что ему надо*/
