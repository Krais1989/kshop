namespace KShop.Payments.Persistence
{
    public enum EPaymentStatus : int
    {
        /// <summary>
        /// Инициализация платежа (требуется инициализация во внешней системе)
        /// </summary>
        Initializing = 0,
        /// <summary>
        /// Ожидание платежа
        /// </summary>
        Pending,
        /// <summary>
        /// Оплачен
        /// </summary>
        Paid,
        /// <summary>
        /// Отмена протежа (требуется отмена во внешней системе)
        /// </summary>
        Cancelling,
        /// <summary>
        /// Отменён до обработки
        /// </summary>
        Canceled,
        /// <summary>
        /// Отклонено внешней системой
        /// </summary>
        Rejected,
        Error
    }
}
