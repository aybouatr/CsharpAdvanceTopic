//namespace New_Order_Event
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Hello, World!");
//        }
//    }
//}

using System;
using System.Collections.Generic;


public class OrderEventArgs : EventArgs
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
   
    public string Emai { get; set; }


    public OrderEventArgs(int id, decimal totalAmount, string emai)
    {
        Id = id;
        TotalAmount = totalAmount;
        Emai = emai;
    }
}


public class Order
{

    public event EventHandler<OrderEventArgs>? OnOrderChanged;

    public void  Create(int orderId,string email, decimal totalAmount)
    {
        Console.WriteLine($"Order Create I will notify with Event \n\n");
        if (OnOrderChanged != null)
        {
            OnOrderChanged(this, new OrderEventArgs(orderId, totalAmount, email));
        }
    }
}

public class EmailService
{
  
    public void Subscribe(Order order)
    {
        order.OnOrderChanged += SendEmail;
    }

    public void Unsubscribe(Order order)
    {
        order.OnOrderChanged -= SendEmail;
    }

    void SendEmail(object sender, OrderEventArgs e)
    {
        Console.WriteLine($"Sending email to {e.Emai} for Order ID: {e.Id} with Total Amount: {e.TotalAmount}");
    }

}

public class ShipingService
{
    public void Subscribe(Order order)
    {
        order.OnOrderChanged += ArrangeShipping;
    }
    public void Unsubscribe(Order order)
    {
        order.OnOrderChanged -= ArrangeShipping;
    }
    void ArrangeShipping(object sender, OrderEventArgs e)
    {
        Console.WriteLine($"Arranging shipping for Order ID: {e.Id} with Total Amount: {e.TotalAmount}");
    }
}


internal class Program
{
    static void Main(string [] args)
    {

        Console.WriteLine("Creating Order and Subscribing Services...\n");

        Order order = new Order();
        EmailService emailService = new EmailService();
        ShipingService shipingService = new ShipingService();

        shipingService.Subscribe(order);
        emailService.Subscribe(order);

       
        order.Create(1, "casque", 99.99m);

        Console.WriteLine("\nUnsubscribing Shipping Service...\n");


        shipingService.Unsubscribe(order);

        order.Create(2, "souris", 49.99m);

    }
}
