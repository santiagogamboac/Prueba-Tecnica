using Backend.Application.Domain.Common;
using System;
using System.Collections.Generic;

namespace Backend.Application.Domain;

public partial class Category : Entity
{
    public int Categoryid { get; set; }

    public string Categoryname { get; set; } = null!;

    public string? Description { get; set; }

    public string? Picture { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public Category(string Categoryname, string? description, string? picture)
    {
        this.Categoryname = Categoryname;
        this.Description = description;
        this.Picture = picture;
    }
}
