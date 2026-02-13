//DESAFIO: Sistema de Pagamentos Multi-Gateway
// PROBLEMA: Uma plataforma de e-commerce precisa integrar com múltiplos gateways de pagamento
// (PagSeguro, MercadoPago, Stripe) e cada gateway tem componentes específicos (Processador, Validador, Logger)
// O código atual está muito acoplado e dificulta a adição de novos gateways

using System;
//Product
public interface IPagamentos
{
    public bool ValidateCard(string cardNumber);
    public string ProcessTransaction(decimal amount, string cardNumber);
    public void Log(string message);
    public void ProcessPayment(decimal amount, string cardNumber);


}

//ConcreteProduct
public class PagamentoPagSeguro : IPagamentos
{
    public bool ValidateCard(string cardNumber)
    {
        Console.WriteLine("PagSeguro: Validando cartão...");
        return cardNumber.Length == 16;
    }
    public string ProcessTransaction(decimal amount, string cardNumber)
    {
        Console.WriteLine($"PagSeguro: Processando R$ {amount}...");
        return $"PAGSEG-{Guid.NewGuid().ToString().Substring(0, 8)}";
    }
    public void Log(string message)
    {
        Console.WriteLine($"[PagSeguro Log] {DateTime.Now}: {message}");
    }


    public void ProcessPayment(decimal amount, string cardNumber)
    {
        if (!ValidateCard(cardNumber))
        {
            Console.WriteLine("PagSeguro: Cartão inválido");
            return;
        }
        Log($"Transação processada: {ProcessTransaction(amount, cardNumber)}");
    }

}

public class PagamentoMercadoPago : IPagamentos
{
    public bool ValidateCard(string cardNumber)
    {
        Console.WriteLine("MercadoPago: Validando cartão...");
        return cardNumber.Length == 16 && cardNumber.StartsWith("5");
    }

    public string ProcessTransaction(decimal amount, string cardNumber)
    {
        Console.WriteLine($"MercadoPago: Processando R$ {amount}...");
        return $"MP-{Guid.NewGuid().ToString().Substring(0, 8)}";
    }

    public void Log(string message)
    {
        Console.WriteLine($"[MercadoPago Log] {DateTime.Now}: {message}");
    }
    public void ProcessPayment(decimal amount, string cardNumber)
    {
        if (!ValidateCard(cardNumber))
        {
            Console.WriteLine("MercadoPago: Cartão inválido");
            return;
        }
        Log($"Transação processada: {ProcessTransaction(amount, cardNumber)}");
    }

}

public class PagamentoStripe : IPagamentos
{
    public bool ValidateCard(string cardNumber)
    {
        Console.WriteLine("Stripe: Validando cartão...");
        return cardNumber.Length == 16 && cardNumber.StartsWith("4");
    }

    public string ProcessTransaction(decimal amount, string cardNumber)
    {
        Console.WriteLine($"Stripe: Processando ${amount}...");
        return $"STRIPE-{Guid.NewGuid().ToString().Substring(0, 8)}";
    }

    public void Log(string message)
    {
        Console.WriteLine($"[Stripe Log] {DateTime.Now}: {message}");
    }

    public void ProcessPayment(decimal amount, string cardNumber)
    {
        if (!ValidateCard(cardNumber))
        {
            Console.WriteLine("Stripe: Cartão inválido");
            return;
        }
        Log($"Transação processada: {ProcessTransaction(amount, cardNumber)}");
    }
}


//Creator
public interface IPagamentoCreator
{
    public IPagamentos CreatePagamento();
}

//ConcreteCreator

public class PagSeguroCreator : IPagamentoCreator
{
    public IPagamentos CreatePagamento()
    {
        return new PagamentoPagSeguro();
    }
}

public class MercadoPagoCreator : IPagamentoCreator
{
    public IPagamentos CreatePagamento()
    {
        return new PagamentoMercadoPago();
    }
}

public class StripeCreator : IPagamentoCreator
{
    public IPagamentos CreatePagamento()
    {
        return new PagamentoStripe();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Sistema de Pagamentos ===\n");

        // Problema: Cliente precisa saber qual gateway está usando
        // e o código de processamento está todo acoplado

        PagSeguroCreator pagSeguroCreator = new PagSeguroCreator();        
        IPagamentos pagSeguroService = pagSeguroCreator.CreatePagamento();
        pagSeguroService.ProcessPayment(150.00m, "1234567890123456");

        Console.WriteLine();

        MercadoPagoCreator MercadoPagoCreator = new MercadoPagoCreator();
        IPagamentos mercadoPagoService = MercadoPagoCreator.CreatePagamento();
        mercadoPagoService.ProcessPayment(200.00m, "5234567890123456");


        Console.WriteLine();

        // Pergunta para reflexão:
        // - Como adicionar um novo gateway sem modificar PaymentService?
        // - Como garantir que todos os componentes de um gateway sejam compatíveis entre si?
        // - Como evitar criar componentes de gateways diferentes acidentalmente?
    }
}
