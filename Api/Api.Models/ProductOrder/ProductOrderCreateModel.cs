﻿namespace Api.Models.ProductOrder
{
    public class ProductOrderCreateModel
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
