using System;

namespace MiniCommercial.Models.Entities
{
    public class Product
    {
        // Identifiant
        public int Id { get; set; }

        // Référence
        public string Reference { get; set; } = string.Empty;

        // Nom du produit
        public string Name { get; set; } = string.Empty;

        // Description
        public string Description { get; set; } = string.Empty;

        // Prix unitaire HT (Utilisation de decimal pour la précision monétaire)
        public decimal UnitPriceHT { get; set; }

        // Quantité en stock
        public int StockQuantity { get; set; }

        // Date de création
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}