using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main()
    {
        using (var context = new MyDbContext())
        {
            var unitOfWork = new UnitOfWork(context);

            var newProduct = new Product { Name = "لپ تاپ", Price = 1500 };
            unitOfWork.ProductRepository.Insert(newProduct);

            unitOfWork.Save();

            var products = unitOfWork.ProductRepository.GetAll();
            foreach (var product in products)
            {
                Console.WriteLine($"نام محصول: {product.Name}, قیمت: {product.Price}");
            }
        }
    }
}

public class MyDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString");
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class ProductRepository
{
    private readonly MyDbContext _context;

    public ProductRepository(MyDbContext context)
    {
        _context = context;
    }

    public void Insert(Product product)
    {
        _context.Products.Add(product);
    }

    public List<Product> GetAll()
    {
        return _context.Products.ToList();
    }
}

public class UnitOfWork
{
    private readonly MyDbContext _context;
    public ProductRepository ProductRepository { get; }

    public UnitOfWork(MyDbContext context)
    {
        _context = context;
        ProductRepository = new ProductRepository(_context);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}