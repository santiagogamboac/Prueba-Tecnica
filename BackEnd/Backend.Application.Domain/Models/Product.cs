using Backend.Application.Domain.Common;
using System;
using System.Collections.Generic;

namespace Backend.Application.Domain;

public partial class Product : Entity
{
    protected Product() { }

    public Product(string productName, int supplierId, int categoryId, string quantityPerUnit,
                   decimal unitPrice, int unitsInStock, int unitsOnOrder, short reorderLevel, bool discontinued)
    {
        Productname = productName;
        Supplierid = supplierId;
        Categoryid = categoryId;
        Quantityperunit = quantityPerUnit;
        Unitprice = unitPrice;
        Unitsinstock = unitsInStock;
        Unitsonorder = unitsOnOrder;
        Reorderlevel = reorderLevel;
        Discontinued = discontinued;
    }

    public int Productid { get; set; }

    public string Productname { get; set; } = null!;

    public int Supplierid { get; set; }

    public int Categoryid { get; set; }

    public string? Quantityperunit { get; set; }

    public decimal Unitprice { get; set; }

    public int Unitsinstock { get; set; }

    public int Unitsonorder { get; set; }

    public short Reorderlevel { get; set; }

    public bool Discontinued { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Supplier? Supplier { get; set; }

  
}
