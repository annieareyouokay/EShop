﻿using Core;
using Core.Payments;
using DAL;
using EShop.Pages;

namespace EShop.Commands.PaymentCommands
{
    public class CreatePaymentCommand : ICommandExecutable, IDisplayable
    {

        private List<Payment> _paymentList;
        private IRepository<Order> _orders;

        /// <summary>
        /// Наименование
        /// </summary>
        public const string Name = "CreatePayment";

        /// <summary>
        /// Результат оплаты
        /// </summary>
        public string? Result { get; private set; }

        public CreatePaymentCommand(RepositoryFactory repositoryFactory, List<Payment> payments)
        {
            _paymentList = payments;
            _orders = repositoryFactory.CreateOrderFactory();
        }

        /// <summary>
        /// Получить описание команды
        /// </summary>
        /// <returns></returns>
        public static string GetInfo()
        {
            return "Создать оплату";
        }

        /// <summary>
        /// Вывести на экран
        /// </summary>
        public void Display()
        {
            Console.WriteLine(GetInfo());
        }


        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="args"></param>
        public void Execute(string[]? args)
        {
            if (args is null || args.Length == 0 || args.Length < 3)
            {
                Result = "Передано не корректное количество параметров";
                return;
            }

            if (!Guid.TryParse(args[0], out Guid orderId))
            {
                Result = "Некорректно указан id заказа";
                return;
            }

            if (!int.TryParse(args[1], out int typeNumber) || typeNumber > 2)
            {
                Result = "Некорректно указан тип оплаты";
                return;
            }

            if (!decimal.TryParse(args[2], out decimal amount))
            {
                Result = "Некорректно введено количество средств для оплаты";
                return;
            }

            --typeNumber;
            var order = _orders.FirstOrDefault(o => o.Id.Equals(orderId));
            if (order is null)
            {
                Result = "Заказ не найден";
                return;
            }
            _paymentList.Add(order.createPaymentFromOrder(amount,(PaymentType)typeNumber));
            Result = $"Оплата на заказ {orderId} успешно создана";
        }
    }
}
