
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {

        BankTransferConfig config = BankTransferConfig.LoadFromFile("bank_transfer_config.json");

        if (config.Language == "en")
        {
            Console.WriteLine("Please insert the amount of money to transfer:");
        }
        else if (config.Language == "id")
        {
            Console.WriteLine("Masukkan jumlah uang yang akan di-transfer:");
        }

        decimal amount = Convert.ToDecimal(Console.ReadLine());

        decimal fee;
        if (amount < config.TransferThreshold)
        {
            fee = config.TransferLowFee;
        }
        else
        {
            fee = config.TransferHighFee;
        }
        Console.WriteLine("Available transfer methods:");
        foreach (string method in config.TransferMethods)
        {
            Console.WriteLine(method);
        }
        Console.WriteLine("Confirmation:");
        if (config.Language == "en")
        {
            Console.WriteLine("Are you sure you want to transfer {0:C}?", amount);
        }
        else if (config.Language == "id")
        {
            Console.WriteLine("Apakah Anda yakin ingin mentransfer {0:C}?", amount);
        }
        Console.WriteLine("Transfer fee: {0:C}", fee);
    }
}

class BankTransferConfig
{
    public string Language { get; set; }
    public decimal TransferThreshold { get; set; }
    public decimal TransferLowFee { get; set; }
    public decimal TransferHighFee { get; set; }
    public List<string> TransferMethods { get; set; }
    public string ConfirmationEn { get; set; }
    public string ConfirmationId { get; set; }


    public static BankTransferConfig LoadFromFile(string fileName)
    {
        string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string json = System.IO.File.ReadAllText(path + "/bank_transfer_config.json");
        BankTransferConfig config = JsonSerializer.Deserialize<BankTransferConfig>(json);


        if (string.IsNullOrEmpty(config.Language))
        {
            config.Language = "en";
        }
        if (config.TransferThreshold <= 0)
        {
            config.TransferThreshold = 25000000;
        }
        if (config.TransferLowFee <= 0)
        {
            config.TransferLowFee = 6500;
        }
        if (config.TransferHighFee <= 0)
        {
            config.TransferHighFee = 15000;
        }
        if (config.TransferMethods == null || config.TransferMethods.Count == 0)
        {
            config.TransferMethods = new List<string> { "RTO (real-time)", "SKN", "RTGS", "BI FAST" };
        }
        if (string.IsNullOrEmpty(config.ConfirmationEn))
        {
            config.ConfirmationEn = "yes";
        }
        if (string.IsNullOrEmpty(config.ConfirmationId))
        {
            config.ConfirmationId = "ya";
        }

        return config;
    }
}