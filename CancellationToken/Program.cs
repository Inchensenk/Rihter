using System;

/*стр 753*/
namespace CancellationToken
{
    internal static class CancellationDemo
    {
        public static void Main()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            // Передаем операции CancellationToken и число
            ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));
            Console.WriteLine("Press <Enter> to cancel the operation.");
            Console.ReadLine();
            cts.Cancel(); // Если метод Count уже вернул управления,
                          // Cancel не оказывает никакого эффекта
                          // Cancel немедленно возвращает управление, метод продолжает работу...
            Console.ReadLine();
        }

        private static void Count(System.Threading.CancellationToken token, Int32 countTo)
        {
            for (Int32 count = 0; count < countTo; count++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Count is cancelled");
                    break; // Выход их цикла для остановки операции
                }
                Console.WriteLine(count);
                Thread.Sleep(200); // Для демонстрационных целей просто ждем
            }
            Console.WriteLine("Count is done");
        }
    }
}

/*
 ПримечАние
Чтобы предотвратить отмену операции, ей можно передать экземпляр
CancellationToken, возвращенный статическим свойством None структуры
CancellationToken. Это очень удобное свойство возвращает специальный экземпляр
CancellationToken, не связанный с каким-либо объектом CancellationTokenSource (его
закрытое поле имеет значение null). При отсутствии объекта CancellationTokenSource
отсутствует и код, который может вызвать метод Cancel. А значит, запрос к свойству
IsCancellationRequested упомянутого экземпляра CancellationToken всегда будет полу-
чать в ответ значение false. Аналогичная ситуация с запросом к свойству CanBeCanceled. 
Значение true возвращается только для экземпляров CancellationToken, полученных
через свойство Token перечисления CancellationTokenSource.
 */