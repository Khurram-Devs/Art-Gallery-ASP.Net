﻿namespace Art_Gallery.Models.DTOs;
public class OrderDetailModalDTO
{
    public string DivId { get; set; }
    public IEnumerable<OrderDetail> OrderDetails { get; set; }
}
