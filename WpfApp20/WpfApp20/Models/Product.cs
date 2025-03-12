using System;

namespace WpfApp20.Models
{
    // Класс, представляющий товар на складе.
    // Содержит всю необходимую информацию о товаре, включая его идентификатор,
    // название, описание, количество, цену и категорию.
    public class Product
    {
        // Уникальный идентификатор товара
        public int Id { get; set; }

        // Название товара
        public string Name { get; set; }

        // Подробное описание товара
        public string Description { get; set; }

        // Количество товара на складе
        public int Quantity { get; set; }

        // Цена товара
        public decimal Price { get; set; }

        // Категория товара для группировки и фильтрации
        public string Category { get; set; }

        // Дата и время последнего обновления информации о товаре
        public DateTime LastUpdated { get; set; }

        // Конструктор класса Product.
        // Инициализирует новый экземпляр товара с текущей датой и временем.
        public Product()
        {
            LastUpdated = DateTime.Now;
        }

        // Переопределение метода ToString для удобного отображения информации о товаре.
        // Возвращает строку в формате "Название - Количество шт."
        // <returns>Строковое представление товара</returns>
        public override string ToString()
        {
            return $"{Name} - {Quantity} шт.";
        }
    }
} 