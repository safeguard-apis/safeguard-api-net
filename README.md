# safeguard-api-net

## V1


## V1.2


## V2.0

### Métodos:
  * ValidateOTP
  * ValidateTransactionToken
  * AnalyzeRisk
  * IssueLocators
  * NotifyFailure


### Obs:
  Todos os métodos lançam uma exceção (SafeGuardExcption) contendo um código de erro e uma mensagem, caso o HTTP code retornado pelo SafeGuard seja diferente de 200

### ValidateOtp

Faz a validação do token digitado pelo usuário

```c#
  public bool ValidateOtp(string transactionToken, string otp, string device_type);
```

### ValidateTransactionToken

Verifica se a transactionToken é válida

```c#
  public bool ValidateTransactionToken(string transactionToken);
```


### AnalyzeRisk

Analisa o grau de risco de uma lista de localizadores


```c#
  public List<Risk> AnalyzeRisk(List<Locator> locators, string transactionToken);
```

#### Exemplo de uso

```c#
    //Instancia a API
    SafeGuardClient sg = new SafeGuardClient(transactionToken, "http://192.168.245.128:3010/");

    //Criação de um localizador
    Locator loc = new Locator("LOC1", false);
    
    //Criação de um ticket
    Ticket ticket = new Ticket("123321", new DateTime(), "MR PAX", 100, "BLR");
    ticket.AddFlightGroup("SAO", "RJ", new DateTime(), new DateTime());
    loc.AddTicket(ticket);//Adição do ticket no localizador

    //Criação de um Cartão
    CreditCardInfo card  = new CreditCardInfo("teste", "12312312312");
    loc.AddPayment(card);//Adição de uma forma de pagamento
    
    //Lista de localizadores
    List<Locator> locators = new List<Locator>();
    locators.Add(loc);
    
    //Análise de risco dos localizadores
    List<Risk> risks = sg.AnalyzeRisk(locators, transactionToken);
```


### IssueLocators

Informa ao Safeguard a emissãp de  uma lista de localizadores


```c#
  public List<Risk> IssueLocators(List<Locator> locators, string transactionToken);
```

#### Exemplo de uso


```c#
    //Instancia a API
    SafeGuardClient sg = new SafeGuardClient(transactionToken, "http://192.168.245.128:3010/");

    //Criação de um localizador
    Locator loc = new Locator("LOC1", false);
    
    //Criação de um ticket
    Ticket ticket = new Ticket("123321", new DateTime(), "MR PAX", 100, "BLR");
    ticket.AddFlightGroup("SAO", "RJ", new DateTime(), new DateTime());
    loc.AddTicket(ticket);//Adição do ticket no localizador

    //Criação de um Cartão
    CreditCardInfo card  = new CreditCardInfo("teste", "12312312312");
    loc.AddPayment(card);//Adição de uma forma de pagamento
    
    //Lista de localizadores
    List<Locator> locators = new List<Locator>();
    locators.Add(loc);
    
    //Análise de risco dos localizadores
    List<Risk> risks = sg.IssueLocators(locators, transactionToken);
```

### NotifyFailure

Notifica o Safeguard falhas de emissão

```c#
  public bool NotifyFailure(string transactionToken, List<LocatorFailure> failures);
```


#### Exemplo de Uso:
```c#
  SafeGuardClient sg = new SafeGuardClient(transactionToken, "http://192.168.245.128:3010/");

  //Criação de Falha
  List<LocatorFailure> failures = new List<LocatorFailure>();
  failures.Add(new LocatorFailure("LOC121", "Error"));

  Boolean resp = sg.NotifyFailure(transactionToken, failures);
```
