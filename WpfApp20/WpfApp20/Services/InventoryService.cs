using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WpfApp20.Models;

namespace WpfApp20.Services
{
    // Сервис для управления инвентарем.
    // Предоставляет методы для работы с товарами: добавление, удаление, 
    // обновление, поиск и получение списка всех товаров.
    // Обеспечивает сохранение данных в JSON-файл и их загрузку при запуске.
    public class InventoryService
    {
        // Список товаров в памяти
        private List<Product> _products;

        // Путь к файлу для хранения данных
        private readonly string _dataFile = "inventory.json";

        // Конструктор сервиса.
        // Инициализирует сервис и загружает данные из файла при создании.
        public InventoryService()
        {
            LoadData();
        }

        // Загружает данные из JSON-файла.
        // Если файл не существует или возникла ошибка при чтении,
        // создает пустой список товаров.
        private void LoadData()
        {
            try
            {
                if (File.Exists(_dataFile))
                {
                    string jsonString = File.ReadAllText(_dataFile);
                    _products = JsonSerializer.Deserialize<List<Product>>(jsonString) ?? new List<Product>();
                }
                else
                {
                    _products = new List<Product>();
                }
            }
            catch (Exception)
            {
                _products = new List<Product>();
            }
        }

        // Сохраняет текущий список товаров в JSON-файл.
        private void SaveData()
        {
            string jsonString = JsonSerializer.Serialize(_products);
            File.WriteAllText(_dataFile, jsonString);
        }

        // Добавляет новый товар в список и сохраняет изменения.
        // <param name="product">Товар для добавления</param>
        // <exception cref="ArgumentNullException">Возникает, если товар равен null</exception>
        // <exception cref="ArgumentException">Возникает при некорректных данных товара</exception>
        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Название товара не может быть пустым");

            if (product.Quantity < 0)
                throw new ArgumentException("Количество товара не может быть отрицательным");

            if (product.Price < 0)
                throw new ArgumentException("Цена товара не может быть отрицательной");

            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
            SaveData();
        }

        // Обновляет информацию о существующем товаре.
        // <param name="product">Товар с обновленными данными</param>
        // <exception cref="ArgumentNullException">Возникает, если товар равен null</exception>
        // <exception cref="ArgumentException">Возникает, если товар не найден</exception>
        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                throw new ArgumentException("Товар не найден");

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
            existingProduct.LastUpdated = DateTime.Now;

            SaveData();
        }

        // Удаляет товар по его идентификатору.
        // <param name="id">Идентификатор товара для удаления</param>
        public void DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
                SaveData();
            }
        }

        // Возвращает список всех товаров.
        // <returns>Копия списка всех товаров</returns>
        public List<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        // Выполняет поиск товаров по названию или описанию.
        // Поиск осуществляется без учета регистра.
        // <param name="searchTerm">Строка для поиска</param>
        // <returns>Список товаров, соответствующих критериям поиска</returns>
        public List<Product> SearchProducts(string searchTerm)
        {
            return _products
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                           p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
} 