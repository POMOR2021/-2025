using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp20.Models;
using WpfApp20.Services;

namespace WpfApp20
{
    /// <summary>
    /// Главное окно приложения для управления инвентарем.
    /// Предоставляет пользовательский интерфейс для работы с товарами:
    /// просмотр списка, добавление, редактирование, удаление и поиск товаров.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Сервис для работы с данными инвентаря
        /// </summary>
        private readonly InventoryService _inventoryService;

        /// <summary>
        /// Текущий выбранный товар для редактирования
        /// </summary>
        private Product _selectedProduct;

        /// <summary>
        /// Конструктор главного окна.
        /// Инициализирует компоненты интерфейса и загружает начальные данные.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _inventoryService = new InventoryService();
            RefreshProductsList();
        }

        /// <summary>
        /// Обновляет список товаров в таблице.
        /// Загружает актуальные данные из сервиса.
        /// </summary>
        private void RefreshProductsList()
        {
            ProductsGrid.ItemsSource = _inventoryService.GetAllProducts();
        }

        /// <summary>
        /// Очищает все поля формы редактирования товара.
        /// Сбрасывает выбранный товар.
        /// </summary>
        private void ClearForm()
        {
            NameTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            QuantityTextBox.Text = string.Empty;
            PriceTextBox.Text = string.Empty;
            CategoryTextBox.Text = string.Empty;
            _selectedProduct = null;
        }

        /// <summary>
        /// Проверяет корректность введенных данных в форме.
        /// Проверяет наличие названия, корректность количества и цены.
        /// </summary>
        /// <returns>true, если все данные корректны; иначе false</returns>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить".
        /// Создает новый товар на основе данных из формы.
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var product = new Product
                {
                    Name = NameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    Quantity = int.Parse(QuantityTextBox.Text),
                    Price = decimal.Parse(PriceTextBox.Text),
                    Category = CategoryTextBox.Text
                };

                _inventoryService.AddProduct(product);
                RefreshProductsList();
                ClearForm();
                MessageBox.Show("Товар успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Сохранить".
        /// Обновляет данные выбранного товара.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedProduct == null)
                {
                    MessageBox.Show("Выберите товар для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!ValidateInput())
                    return;

                _selectedProduct.Name = NameTextBox.Text;
                _selectedProduct.Description = DescriptionTextBox.Text;
                _selectedProduct.Quantity = int.Parse(QuantityTextBox.Text);
                _selectedProduct.Price = decimal.Parse(PriceTextBox.Text);
                _selectedProduct.Category = CategoryTextBox.Text;

                _inventoryService.UpdateProduct(_selectedProduct);
                RefreshProductsList();
                ClearForm();
                MessageBox.Show("Товар успешно обновлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Удалить".
        /// Удаляет выбранный товар после подтверждения.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct == null)
            {
                MessageBox.Show("Выберите товар для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить этот товар?", "Подтверждение", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _inventoryService.DeleteProduct(_selectedProduct.Id);
                    RefreshProductsList();
                    ClearForm();
                    MessageBox.Show("Товар успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Обработчик события выбора товара в таблице.
        /// Заполняет форму данными выбранного товара.
        /// </summary>
        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedProduct = ProductsGrid.SelectedItem as Product;
            if (_selectedProduct != null)
            {
                NameTextBox.Text = _selectedProduct.Name;
                DescriptionTextBox.Text = _selectedProduct.Description;
                QuantityTextBox.Text = _selectedProduct.Quantity.ToString();
                PriceTextBox.Text = _selectedProduct.Price.ToString();
                CategoryTextBox.Text = _selectedProduct.Category;
            }
        }

        /// <summary>
        /// Обработчик изменения текста в поле поиска.
        /// Фильтрует список товаров по введенному тексту.
        /// </summary>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                RefreshProductsList();
            }
            else
            {
                ProductsGrid.ItemsSource = _inventoryService.SearchProducts(SearchBox.Text);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Обновить".
        /// Обновляет список товаров в таблице.
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshProductsList();
        }
    }
}