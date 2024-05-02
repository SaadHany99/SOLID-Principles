// Ex:1
/*
The Problem: the ISP violation occurs in the IMediaPlayer interface.
The interface includes methods for playing audio, playing video, displaying subtitles,
and loading media. However, the implementation classes AudioPlayer and VideoPlayer 
do not need to implement all these methods.

The Solution: Is to create separate interfaces for each type of media.
For example, IAudioPlayer, IVideoPlayer, and ISubtitlePlayer.
The implementation classes can implement only the methods that are relevant to them.
For example, the AudioPlayer class can implement only the IAudioPlayer interface.
*/

//Here,we have an interface for only Audios that represents just 2 methods(PlayAudio,LoadMedia).
public interface IAudioPlayer
{
    void PlayAudio();
    void LoadMedia(string filePath);
}
//Here,we have an interface for only Videos that represents just 3 methods(PlayVideo,DisplaySubtitles,LoadMedia).
public interface IVideoPlayer
{
    void PlayVideo();
    void DisplaySubtitles();
    void LoadMedia(string filePath);
}
//and we can use with each class the interface that it needs.
public class AudioPlayer : IAudioPlayer
{
    public void PlayAudio()
    {
    // Code to play audio
    }
    public void LoadMedia(string filePath)
    {
    // Code to load audio file
    }
}
***************************************
public class VideoPlayer : IVideoPlayer
{
    public void PlayVideo()
    {
    // Code to play video
    }
    public void DisplaySubtitles()
    {
    // Code to display subtitles
    }
    public void LoadMedia(string filePath)
    {
    // Code to load video file
    }
}
*************************************************************************************
// Ex:2
/*
The Problem: The FileProcessor class directly depends on concrete implementations of FileReader and FileWriter,
creating tight coupling between the classes.This tight coupling makes it challenging to extend or modify
the system without altering (make changes in) the FileProcessor class.

The Solution: the FileProcessor class should depend on abstractions rather than concrete implementations.
By introducing interfaces for (FileReader & FileWriter), we can decouple/separate the FileProcessor class from
specific implementations, allowing for easier changes without modifying the FileProcessor itself.
*/

//Here, we have an interface (IReader) for FileReader that represents just ReadFile Method.
public interface IReader
{
    string ReadFile(string filePath);
}
//Here, we have an interface (IWriter) for FileWriter that represents just WriteFile Method.
public interface IWriter
{
    void WriteFile(string filePath, string content);
}
//Here, we have the 2 classes (FileReader) that depends on IReader and (FileWriter) that depends on IWriter.
public class FileReader : IReader
{
    public string ReadFile(string filePath)
    {
        // Code to read file content
        return "File content";
    }
}

public class FileWriter : IWriter
{
    public void WriteFile(string filePath, string content)
    {
        // Code to write file content
    }
}

//Here, we have the FileProcessor class that depends on IReader and IWriter.so,any changes that we need to 
//make to the IReader or IWriter will be reflected in the FileProcessor class.
public class FileProcessor
{
    private IReader _fileReader;
    private IWriter _fileWriter;

    public FileProcessor(IReader reader, IWriter writer)
    {
        _fileReader = reader;
        _fileWriter = writer;
    }

    public void ProcessFile(string inputFilePath, string outputFilePath)
    {
        string fileContent = _fileReader.ReadFile(inputFilePath);
        // Process the file content
        _fileWriter.WriteFile(outputFilePath, fileContent);
    }
}
*************************************************************************************************
// Ex:3
// we need to consider in this Exercise all Solid Principles we have learned and can be used in this case.
//this Examples violates the following SOLID Principles:

/*
1) Single Responsibility Principle (SRP):
The ECommerceSystem class is responsible for more than one responsibility such as: managing products,
orders, processing different payment methods, and sending order confirmation emails.

2) Open/Closed Principle (OCP):
The ECommerceSystem class is not easily extensible for example for adding new payment methods.
To add a new payment method, the class needs to be modified directly.
*/

// 1) Single Responsibility Principle:
/*
The Solution:
*Create separate classes for managing products and orders.

*Create separate payment processing functionality classes
that implements a specific Interface with the Payment nedded Methods.

*Create a class for sending order confirmation emails.
*/
//Here,The ProductManager Class with its method.
public class ProductManager
{
    private List<Product> products = new List<Product>();

    public void AddProduct(string name, decimal price, int quantity)
    {
        products.Add(new Product { Name = name, Price = price, Quantity = quantity });
    }
}
//Here,The OrderManager Class to manage Orders.
public class OrderManager
{
    private List<Order> orders = new List<Order>();

    public void PlaceOrder(string customerName, List<Product> orderedProducts, decimal totalCost)
    {
        Order order = new Order
        {
            CustomerName = customerName,
            Products = orderedProducts,
            TotalCost = totalCost
        };
        orders.Add(order);
    }
}
//Here,The IPaymentProcessor Interface to be implemented by different Payment methods.
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
}

public class CreditCardPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of ${amount}");
    }
}

public class PayPalPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of ${amount}");
    }
}
// Here, the EmailService Class to represent order confirmation with email address.
public class EmailService
{
    public void SendOrderConfirmationEmail(Order order)
    {
        string message = $"Order confirmation for {order.CustomerName}:\n";
        message += $"Total Cost: ${order.TotalCost}\n";
        message += "Products:\n";
        foreach (Product product in order.Products)
        {
            message += $"- {product.Name} (${product.Price})\n";
        }
        // Send email
        Console.WriteLine(message);
    }
}

public class ECommerceSystem
{
    private readonly ProductManager productManager;
    private readonly OrderManager orderManager;
    private readonly IPaymentProcessor paymentProcessor;
    private readonly EmailService emailService;

    public ECommerceSystem(ProductManager productManager, OrderManager orderManager, 
                           IPaymentProcessor paymentProcessor, EmailService emailService)
    {
        this.productManager = productManager;
        this.orderManager = orderManager;
        this.paymentProcessor = paymentProcessor;
        this.emailService = emailService;
    }
}
*************************************************************************
//2) Open/Closed Principle (OCP):
/*
The Open/Closed Principle states that a class should be open for extension but closed for modification.
we can also create an interface for the EmailService class.
*/
public interface IEmailService
{
    void SendOrderConfirmationEmail(Order order);
}

//so that,we can now easily introduce email services without modifying the ECommerceSystem class.
public class ECommerceSystem
{
    private readonly ProductManager productManager;
    private readonly OrderManager orderManager;
    private readonly IPaymentProcessor paymentProcessor;
    private readonly IEmailService emailService;

    public ECommerceSystem(ProductManager productManager, OrderManager orderManager, 
                           IPaymentProcessor paymentProcessor, IEmailService emailService)
    {
        this.productManager = productManager;
        this.orderManager = orderManager;
        this.paymentProcessor = paymentProcessor;
        this.emailService = emailService;
    }
}
***********************************************************************************