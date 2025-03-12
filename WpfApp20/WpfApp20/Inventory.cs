using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp20;

namespace WpfApp20
{
    public class Inventory
    {
        public List<Product> Products { get; set; }

        public Inventory()
        {
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public void RemoveProduct(string productName)
        {
            var product = Products.FirstOrDefault(p => p.Name == productName);
            if (product != null)
            {
                Products.Remove(product);
            }
        }

        public Product FindProduct(string productName)
        {
            return Products.FirstOrDefault(p => p.Name == productName);
        }

        public void SaveToFile(string fileName)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Products);
            System.IO.File.WriteAllText(fileName, json);
        }

        public void LoadFromFile(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                var json = System.IO.File.ReadAllText(fileName);
                Products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
            }
        }
    }
}