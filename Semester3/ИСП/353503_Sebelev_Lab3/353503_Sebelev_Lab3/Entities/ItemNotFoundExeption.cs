using System;

namespace _353503_Sebelev_Lab3.Entities;

public class ItemNotFoundExeption : Exception
{
    public ItemNotFoundExeption(string message) : base(message)
    {
        
    }
}