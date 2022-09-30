#Moldova Agroindbank Payment .NET SDK
#### maib-api
[![N|Solid](https://www.maib.md/images/logo.svg)](https://www.maib.md)


CONTENTS OF THIS FILE
=====================

 * Introduction
 * Requirements
 * Recommended modules
 * Installation
 * Before usage
 * Usage
 * Troubleshoting
 * Maintainers


INTRODUCTION
============

The Moldova Agroindbank Payment .NET SDK is used to easily integrate the MAIB Payment into your project.
Based on the .NET Core Libraries to connect and process the requests with the Bank server and the .net core logger library to log the requests responses.

The Moldova Agroindbank Payment .NET SDK has 2 ways of payment.
 * One way is SMS Transaction (`RegisterSmsTransaction`). When the client's money transfers on the merchant account instantly when the user do the payment. This way is recommended use.
 * Another way is DMS Transaction (`RegisterDmsAuthorization`). When the client's money has been blocked on their account before you confirm that transaction. This way is mostly used in the case of the long shipping time.

That Payment .NET SDK includes 6 methods to process the payments:
 * Registering transactions.
 * Registering DMS authorization.
 * Executing a DMS transaction.
 * Get transaction result.
 * Transaction reversal.
 * Close the work day.


REQUIREMENTS
============

 * .NET Core: >=3.0 [Combatibility](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-1)


INSTALLATION
============

 * Via terminal
 ```bash
dotnet add package Maib.Sdk --version 1.0.1-beta
 ```

* Paste into *.csproj

```xml
<PackageReference Include="Maib.Sdk" Version="1.0.1-beta" />
```


BEFORE USAGE
============

To initiate an payment transaction you need to obtain the SSL certificate, to get the access by IP and to set the callback URL.
 * Need to write an email to maib commerce support: ecom@maib.md with the request including The Merchant IP and callback URL, to receive access and certificate for testing.

USAGE
=====
 * Need to include all libraries
  ```csharp
    using System.Threading.Tasks;
    using Maib.Sdk.Abstractions;
    using Maib.Sdk.Enums;
    using Maib.Sdk.Extensions;
    using Maib.Sdk.Models;
  ````
 * Register the client
  ```csharp
    services.AddMaibClient(new MaibClientConfiguration
    {
        BaseUrl = MaibConstants.MAIB_TEST_BASE_URI,
        RedirectBaseUrl = MaibConstants.MAIB_TEST_REDIRECT_URL,
        CertificatePath = certificatePath,
        CertificatePassword = MaibConstants.MAIB_TEST_CERT_PASS
    });
  ````
 * Inject the MaibCLient
  ```csharp
    public class YourService{
        private readonly IMaibClient _maibClient;
        public YourService(IMaibClient maibClient){
            _maibClient = maibClient;
        }
    }
  ````
 * Prepare the payment parameters
  ```csharp
    // The Parameters required to use MaibClient methods
    var amount = 1M; // The amount of the transaction
    var currency = "MDL"; // The currency of the transaction
    var clientIpAddr = IPAddress.Parse("127.0.0.1"); // The client IP address
    var description = "testing"; // The description of the transaction
    var lang = 'en'; // The language for the payment gateway

    // Other parameters
    var sms_transaction_id = null;
    var dms_transaction_id = null;
    var sms_redirect_url = "";
    var dms_redirect_url = "";
  ````
 * Registering SMS transactions
   The SMS transaction has 2 steps.
      - The first step is to register the transaction on the Maib Server and obtain the TRANSACTION_ID using the RegisterSmsTransaction Method.
      - The second step is the redirected user to the Maib Payment Gateway URL using the TRANSACTION_ID.
      * When the transaction has been finalised, the Maib Payment Gateway redirects the user to your callback URL where you get the transaction status.
      ! The TRANSACTION_ID has a timeout of 10 minutes.

      - Required parameters:
        * amount = 1M; // The amount of the transaction
        * currency = "MDL"; // The currency of the transaction
        * clientIpAddr = IPAddress.Parse("127.0.0.1"); // The client IP address
        * description = "testing"; // The description of the transaction
        * lang = "en"; // The language for the payment gateway
      - Response:
        return array  TRANSACTION_ID
        * TRANSACTION_ID - transaction identifier (28 characters in base64 encoding)
        * error          - in case of an error
  ```csharp
    // The register sms transaction method
    var registerSmsTransaction = await _maibClient.RegisterSmsTransactionAsync(amount, currency, clientIpAddr, description, lang);
    sms_transaction_id = registerSmsTransaction.TransactionId;
    sms_redirect_url = registerSmsTransaction.RedirectUrl;
  ```

 * Registering DMS authorization
   The DMS transaction has 3 steps.
      - The first step is to register the transaction on the Maib Server and obtain the TRANSACTION_ID using the registerDmsAuthorization Method.
      - The second step is the redirected user to the Maib Payment Gateway URL using the TRANSACTION_ID.
      * When the transaction has been applied, the Maib Payment Gateway redirects the user to your callback URL where you get the transaction status.
      - The third step is to confirm transactions using the makeDMSTrans method.

      - Required parameters:
        * amount = 1M; // The amount of the transaction.
        * currency = "MDL"; // The currency of the transaction
        * clientIpAddr = IPAddress.Parse("127.0.0.1"); // The client IP address.
        * description = "testing"; // The description of the transaction.
        * lang = "en"; // The language for the payment gateway.
      - Response:
        return array  TRANSACTION_ID
        * TRANSACTION_ID - transaction identifier (28 characters in base64 encoding)
        * error          - in case of an error
  ```csharp
    // The register dms authorization method
    var registerDmsAuthorization = await _maibClient.RegisterDmsAuthorizationAsync(amount, currency, clientIpAddr, description, lang);
    dms_transaction_id = registerDmsAuthorization.TransactionId;
    dms_redirect_url = registerDmsAuthorization.RedirectUrl;
  ````
 * Executing a DMS transaction
      - Required parameters:
        * dms_transaction_id;// The transaction ID from registerDmsAuthorization.
        * amount = 1M; // The amount of the transaction.
        * clientIpAddr = IPAddress.Parse("127.0.0.1"); // The client IP address.
        * description = "testing"; // The description of the transaction.
        * lang = "en"; // The language for the payment gateway.
      - Response:
        return array  RESULT, RESULT_CODE, BRN, APPROVAL_CODE, CARD_NUMBER, error
        * RESULT         - transaction results: OK - successful transaction, FAILED - failed transaction
        * RESULT_CODE    - transaction result code returned from Card Suite Processing RTPS (3 digits)
        * BRN            - retrieval reference number returned from Card Suite Processing RTPS (12 characters)
        * APPROVAL_CODE  - approval code returned from Card Suite Processing RTPS (max 6 characters)
        * CARD_NUMBER    - masked card number
        * error          - in case of an error
  ```csharp
    // The execute dms transaction method
    var makeDMSTrans = await _maibClient.MakeDmsTransactionAsync(dms_transaction_id, amount, currency, clientIpAddr, description, language);
  ````
 * Get transaction result
   You can get the transaction status yourself using the getTransactionResult method. But do not forget, the transaction ID has a timeout of 10 minutes.
      - Required parameters:
        * transactionId;// The transaction ID from registerSmsTransaction or registerDmsAuthorization.
        * clientIpAddr = IPAddress.Parse("127.0.0.1"); // The client IP address.
      - Response:
        return array  RESULT, RESULT_PS, RESULT_CODE, 3DSECURE, RRN, APPROVAL_CODE, CARD_NUMBER,
                      AAV, RECC_PMNT_ID, RECC_PMNT_EXPIRY, MRCH_TRANSACTION_ID
        * RESULT
            - OK              - successfully completed transaction,
            - FAILED          - transaction has failed,
            - CREATED         - transaction just registered in the system,
            - PENDING         - transaction is not accomplished yet,
            - DECLINED        - transaction declined by ECOMM,
            - REVERSED        - transaction is reversed,
            - AUTOREVERSED    - transaction is reversed by autoreversal,
            - TIMEOUT         - transaction was timed out
        * RESULT_PS          - transaction result, Payment Server interpretation (shown only
                               if configured to return ECOMM2 specific details
            - FINISHED        - successfully completed payment,
            - CANCELLED       - cancelled payment,
            - RETURNED        - returned payment,
            - ACTIVE          - registered and not yet completed payment.
        * RESULT_CODE        - transaction result code returned from Card Suite Processing RTPS (3 digits)
        * 3DSECURE
            - AUTHENTICATED   - successful 3D Secure authorization
            - DECLINED        - failed 3D Secure authorization
            - NOTPARTICIPATED - cardholder is not a member of 3D Secure scheme
            - NO_RANGE        - card is not in 3D secure card range defined by issuer
            - ATTEMPTED       - cardholder 3D secure authorization using attempts ACS server
            - UNAVAILABLE     - cardholder 3D secure authorization is unavailable
            - ERROR           - error message received from ACS server
            - SYSERROR        - 3D secure authorization ended with system error
            - UNKNOWNSCHEME   - 3D secure authorization was attempted by wrong card scheme
                                                 (Dinners club, American Express)
        * RRN                - retrieval reference number returned from Card Suite Processing RTPS
        * APPROVAL_CODE      - approval code returned from Card Suite Processing RTPS (max 6 characters)
        * CARD_NUMBER        - Masked card number
        * AAV                - FAILED the results of the verification of hash value in AAV merchant name (only if failed)
        * RECC_PMNT_ID            - Reoccurring payment (if available) identification in Payment Server.
        * RECC_PMNT_EXPIRY        - Reoccurring payment (if available) expiry date in Payment Server in form of YYMM
        * MRCH_TRANSACTION_ID     - Merchant Transaction Identifier (if available) for Payment -
                                    shown if it was sent as additional parameter on Payment registration.
        * The RESULT_CODE and 3DSECURE fields are informative only and can be not shown.
        * The fields RRN and APPROVAL_CODE appear for successful transactions only, for informative purposes,
        * and they facilitate tracking the transactions in Card Suite Processing RTPS system.
        * error                   - In case of an error
        * warning                 - In case of warning (reserved for future use).
  ```csharp
    // The get transaction result method
    var getTransactionResult = await _maibClient.GetTransactionResultAsync(transaction_id, clientIpAddr);
  ````
 * Transaction reversal
   The ability to perform a return operation, partially or completely.
      - Required parameters:
        * transaction_id;// The transaction ID from registerSmsTransaction or registerDmsAuthorization.
        * amount = 1M; // The amount of the transaction.
      - Response:
        return array  RESULT, RESULT_CODE
        * RESULT
            - OK              - successful reversal transaction
            - REVERSED        - transaction has already been reversed
            - FAILED          - failed to reverse transaction (transaction status remains as it was)
        * RESULT_CODE    - reversal result code returned from Card Suite Processing RTPS (3 digits)
        * error          - In case of an error
        * warning        - In case of warning (reserved for future use).
  ```csharp
    // The revert transaction method
    var revertTransaction = await _maibClient.RevertTransactionAsync(transaction_id, amount);
  ````
 * Close the work day
   Execute automatic closing of the day, recommended use time: 23:59:00.
      - Required parameters:
        * No parameters Required.
      - Response:
        return array RESULT, RESULT_CODE, FLD_075, FLD_076, FLD_087, FLD_088
        * RESULT        - OK     - successful end of business day
                          FAILED - failed end of business day
        * RESULT_CODE   - end-of-business-day code returned from Card Suite Processing RTPS (3 digits)
        * FLD_075       - the number of credit reversals (up to 10 digits), shown only if result_code begins with 5
        * FLD_076       - the number of debit transactions (up to 10 digits), shown only if result_code begins with 5
        * FLD_087       - total amount of credit reversals (up to 16 digits), shown only if result_code begins with 5
        * FLD_088       - total amount of debit transactions (up to 16 digits), shown only if result_code begins with 5
  ```csharp
    //close business day
    var closeDay = await _maibClient.CloseDayAsync();
  ````
  * Test medium
    - Merchant: https://maib.ecommerce.md:21440/ecomm/MerchantHandler
    - Client: https://maib.ecommerce.md:21443/ecomm/ClientHandler
    - Test Certificates are in the "src/MaibApi/cert/" folder
    - The test certificate Password is Za86DuC$
  * Test Card
    - Card number: 5102180060101124
    - Exp. date: 06/28
    - CVV: 760


TROUBLESHOTING
==============

All transactions are considered successful it's only if you receive a predictable response from the maib server in
the format you know. If you receive any other result (NO RESPONSE, Connection Refused, something else) there
is a problem. In this case it is necessary to collect all logs and sending them to maib by email: ecom@maib.md, in
order to provide operational support. The following information should be indicated in the letter:
- Merchant name,
- Web site name,
- Date and time of the transaction made with errors
- Responses received from the server

MAINTAINERS
===========

Current maintainers:
 * [Indrivo](https://github.com/indrivo)