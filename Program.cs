using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        BankTransferConfig config = new BankTransferConfig();
        BankTransfer bankTransfer = config.bankTransfer;

        if(bankTransfer.lang == "en")
        {
            Console.WriteLine("please insert the amount of money to transfer:");
        }
        else if(bankTransfer.lang == "id")
        {
            Console.WriteLine("Masukkan jumlah uang yang akan di-Transfer");
        }
        
        int money = Convert.ToInt32(Console.ReadLine());

        int fee;
        if (money <= bankTransfer.transfer.threshold) 
        { 
            fee = bankTransfer.transfer.low_fee; 
        }
        else
        {
            fee = bankTransfer.transfer.high_fee;
        }

        int total = money + fee;

        if(bankTransfer.lang == "en")
        {
            Console.WriteLine("Transfer fee = {0}\ntotal amount = {1}",
                fee,
                money + fee
                );
        }
        else
        {
            Console.WriteLine("Biaya transfer = {0}\nTotal biaya = {1}",
                fee,
                money + fee
                );
        }

        Console.WriteLine((bankTransfer.lang == "en") ? "Select transfer method" : "Pilih metode transfer");
        int index = 1;
        foreach(string metode in bankTransfer.methods){
            Console.WriteLine("{0}. {1}",index++, metode);
        }
        String metodeTf = Console.ReadLine();


        if (bankTransfer.lang == "en")
        {
            Console.WriteLine("Please type \"{0}\" to confirm the transaction:",
                bankTransfer.confirmation.en
                );
        }
        else
        {
            Console.WriteLine("Ketik \"{0}\" untuk mengkonfirmasu transaksi:",
                bankTransfer.confirmation.id
                );
        }
        string confirmasi = Console.ReadLine();

        if(bankTransfer.lang == "en" && confirmasi == bankTransfer.confirmation.en)
        {
            Console.WriteLine("The transfer is complated");
        }
        else if (bankTransfer.lang == "id" && confirmasi == bankTransfer.confirmation.id)
        {
            Console.WriteLine("Proses transfer berhasil");
        }
        else
        {
            Console.WriteLine((bankTransfer.lang == "en") ? "Transfer is cancelled" : "Transfer dibatalkan");
        }

    }
}

public class BankTransferConfig
{

    public BankTransfer bankTransfer;
    public const String filePath = @".json";
    public BankTransferConfig()
    {
        try
        {
            ReadConfig();
        }
        catch
        {
            SetDefault();
            WriteNewConfig();
        }
    }
    public BankTransfer ReadConfig()
    {
        String readedConfig = File.ReadAllText(filePath);
        bankTransfer = JsonSerializer.Deserialize<BankTransfer>(readedConfig);
        return bankTransfer;
    }

    public void SetDefault()
    {
        bankTransfer = new BankTransfer(
            "en", 
            new Transfer(25000000,6500,15000),
            new List<String>() { "RTO(real-time)", "SKN","RTGS","BI FAST"}, 
            new Confirmation("yes", "ya"));
    }

    public void WriteNewConfig()
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        String jsonString = JsonSerializer.Serialize(bankTransfer, options);
        File.WriteAllText(filePath, jsonString);
    }
}


public class BankTransfer
{
    public string lang { set; get; }
    public Transfer transfer { set; get; }
    public List<string> methods { set; get; }
    public Confirmation confirmation { set; get; }
    public BankTransfer() { }
    public BankTransfer(string lang, Transfer transfer, List<string> methods, Confirmation confirmation)
    {
        this.lang = lang;
        this.transfer = transfer;
        this.methods = methods;
        this.confirmation = confirmation;
    }
}

public class Transfer
{
    public int threshold { set; get; }
    public int low_fee { set; get; }
    public int high_fee { set; get; }
    public Transfer() { }
    public Transfer(int threshold, int low_fee, int high_fee)
    {
        this.threshold = threshold;
        this.low_fee = low_fee;
        this.high_fee = high_fee;
    }
}

public class Confirmation
{
    public string en { set; get; }
    public string id { set; get; }

    public Confirmation() { }
    public Confirmation(string en, string id)
    {
        this.en = en;
        this.id = id;
    }
}