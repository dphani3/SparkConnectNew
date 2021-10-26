using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using TF.REVCheckOut;
using System.Net;

/// <summary>
/// Summary description for Common
/// </summary>
public sealed class Common
{

    #region Member Variables

    public static Stream stream;

    //Consumer details for Jetpay
    public const string AID                                                     = "AID";
    public const string PWD                                                     = "PWD";
    public const string NAME                                                    = "Name";
    public const string ADDRESS                                                 = "Address";
    public const string EMAIL                                                   = "Email";
    public const string AMOUNT                                                  = "Amount";
    public const string CURRENCY                                                = "Currency";
    public const string TEST_MODE                                               = "TestMode";
    public const string CALLBACK                                                = "Callback";
    public const string TOKENIZE                                                = "Tokenize"; 
    public const string TOKEN                                                   = "Token";
    public const string TRANSACTION_TYPE                                        = "TransactionType";
    public const string REQUEST_TRANSACTIONID                                   = "TransactionID";
    public const string IS_GUEST_USER                                           = "IsGuestUser";
    public const string INVOICE_NUMBER                                          = "InvoiceNumber";
    public const string APP_KEY                                                 = "AppKey";
    public const string NOTES                                                   = "Notes";
    public const string UDField2 = "UDField2";
    public const string CustomerNo = "CustomerNo";

    //Consumer Order/create Quote Details for Smarter Commerce
    public const string ORGANIZATION_CODE                                       = "OrganizationCode";
    public const string USER_NAME                                               = "UserName";
    public const string ADDRESS_LINE1                                           = "AddressLine1";
    public const string ADDRESS_LINE2                                           = "AddreNotesssLine2";
    public const string ADDRESS_LINE3                                           = "AddressLine3";
    public const string ADDRESS_LINE4                                           = "AddressLine4";
    public const string CITY                                                    = "City";
    public const string COUNTRY                                                 = "Country";
    public const string FAX                                                     = "Fax";
    public const string OREDER_CONSUMER_NAME                                    = "Name";
    public const string PHONE                                                   = "Phone";
    public const string POSTAL_CODE                                             = "PostalCode";
    public const string STATE                                                   = "State";
    public const string ORDER_COMPANY                                           = "OrderCompany";
    public const string OREDER_AMOUNT                                           = "Amount";
    public const string OREDER_CURRENCY                                         = "Currency";
    public const string ORDER_IS_GUEST_USER                                     = "IsGuestUser";
    public const string OREDER_CALLBACK                                         = "Callback";
    public const string SAVED_CARD_CUSTOMER                                     = "SavedCardCustomer";
    public const string SAVED_CARD_LINE                                         = "Token";

    //The following paramters are requires only for Consumer order 
    public const string ORDER_NUMBER                                            = "OrderNumber";
    public const string ORDER_TYPE                                              = "OrderType";    

    //The following paramters are requires only for create quote details
    public const string CARRIER_CODE                                            = "CarrierCode";
    public const string CUSTOMER_PO                                             = "CustomerPO";
    public const string QUOTE_NUMBER                                            = "QuoteNumber";
    public const string SHIPPING_INSTRUCTIONS1                                  = "ShippingInstructions1";
    public const string SHIPPING_INSTRUCTIONS2                                  = "ShippingInstructions2";
    public const string QUOTE_COMPANY                                           = "Company";

    //Order Type value for different Organization code
    public const string ORDER_TYPE_SP                                           = "SP";
    public const string ORDER_TYPE_SO                                           = "SO";


    //Response details.
    public const string RESPONSE_CODE                                           = "ResultCode";
    public const string MESSAGE                                                 = "Message";
    public const string TRANSACTION_AMOUNT                                      = "Amount";
    public const string TRANSACTION_ID                                          = "TransactionID";
    public const string TRANSACTION_TIME                                        = "TransactionTime";
    public const string RESPONSE_TOKEN                                          = "Token";
    public const string AUTHCODE                                                = "AuthCode";
    public const string RESPONSE_ORDER_NUMBER                                   = "OrderNumber";
    public const string REMEMBER_ME_CHECKBOX                                    = "RememberMeCheckbox";
    public const string CARD_NUMBER                                             = "CardNumber";
    public const string CARD_TYPE                                               = "CardType";
    public const string PNREF                                                   = "PNRef";
    public const string RESPONSE_SAVED_CARD_CUSTOMER                            = "SavedCardCustomer";
    public const string CARD_HOLDER_NAME                                        = "CardName";

    //FocusPay request types.
    public const int LOGIN_REQUEST_TYPE                                         = 1;
    public const int CREDIT_CARD_REQUEST_TYPE                                   = 3;

    //FocusPay transaction types ID.
    public const int SALE_TRANSACTION                                           = 1;

    public const int CREDIT_TRANSACTION                                         = 2;

    public const int PRIORAUTHCAPTURE_TRANSACTION                               = 3;

    public const int AUTHORIZE_TRANSACTION                                      = 4;

    public const int VOID_TRANSACTION                                           = 5;

    public const int VOID_AUTH_TRANSACTION                                      = 6;

    public const int VOID_SALE_TRANSACTION                                      = 7;

    public const int VOID_CREDIT_TRANSACTION                                    = 8;

    public const int VOID_CAPTURE_TRANSACTION                                   = 9;

    public const int VOID_REFUND_TRANSACTION                                    = 15;

    public const int ADJUST_TRANSACTION                                         = 11;

    public const int CARD_UNMASKED_COUNT                                        = 4;

    public const int REFUND_TRANSACTION                                         = 14;

    public const int VOID_REFUND = 15;

    public const int SAVE_CARD = 16;

    //Card Present indicator.
    public const int CP_INDICATOR                                               = 0;

    //FocusConnect http request header details.
    public const string HTTP_METHOD                                             = "POST";
    public const string CONTENT_TYPE                                            = "text/xml";   

    //Cancel transaction response code
    public const int TRANSACTION_CANCEL_CODE                                    = 53;
    public const string TRANSACTION_CANCEL_MESSAGE                              ="Transaction canceled by user";

    //Milacron Gateway error response code
    public const int MILACRON_GATEWAY_ERROR_CODE                                = 202;
    public const string MILACRON_GATEWAY_ERROR_MESSAGE                          = "Gateway Error";

    public const int INSUFFICIENT_PARAMETERS_ERROR_CODE                         = 1;
    public const string INSUFFICIENT_PARAMETERS_ERROR_MESSAGE                   = "Insufficient Parameters";

    public const int INVALID_CURRENCY_CODE_ERROR_CODE                           = 205;
    public const string INVALID_CURRENCY_CODE_ERROR_MESSAGE                     = "Invalid Currency Type.";

    public const int SERVER_ERROR_CODE                                          = 500;
    public const string SERVER_ERROR_MESSAGE                                    = "Internal Server Error.";

    public const int INVALID_URL_CODE = 12;
    public const string INVALID_URL_message = "Invalid URL .";
    public const string EXCEPTION_MESSAGE = "Exception  Occured While Processing Transaction";
    public const string RESPONSE_NULL = "Null Response Received From Host";
    public const string REQUEST_NULL = "Request data is null.";
  
    public const string XML_SERIALIZATION_ERROR = "XML Serialization Error. ";
    public const string LOGIN_RESPONSE_NULL = "Login Response Null .";
    public const string INVALID_FOCUSCONNECT_URL = "Invalid Focusconnet URL";
    public const string LOGIN_REQUEST_NULL = "Login XML Serialixation Error .";
    public const string SESSION_DATA_NULL = "Request Session Has No Data .";
    public const string INVALID_REQUEST = "Invalid Request";

    public const string HTTP_RESPONSE_TIMEOUT = "Response Timeout ";

    public const string APPLICATION_SESSION_NAME                                = "AuthToken";

    //log4net logger interface.       
   // public static readonly ILog REVCheckoutLogger                          = null;

    //private const string cardRegex = "^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";

    private static string userName                                              = (ConfigurationManager.AppSettings["UserName"].ToString() != string.Empty) ? ConfigurationManager.AppSettings["UserName"].ToString() : string.Empty;
    private static string password                                              = (ConfigurationManager.AppSettings["Password"].ToString() != string.Empty) ? ConfigurationManager.AppSettings["Password"].ToString() : string.Empty;
    public static string tokenUrl                                               = (ConfigurationManager.AppSettings["NewTokenUrl"].ToString() != string.Empty) ? ConfigurationManager.AppSettings["NewTokenUrl"].ToString() : string.Empty;
    public static string authorizePaymentCardUrl                                = (ConfigurationManager.AppSettings["AuthorizePaymentCardUrl"].ToString() != string.Empty) ? ConfigurationManager.AppSettings["AuthorizePaymentCardUrl"].ToString() : string.Empty;
    public static string convertQuoteUrl                                        = (ConfigurationManager.AppSettings["ConvertQuoteUrl"].ToString() != string.Empty) ? ConfigurationManager.AppSettings["ConvertQuoteUrl"].ToString() : string.Empty;

    public static string TYPE_OF_SOAP_METHOD_TOKEN                              = "Token";
    public static string TYPE_OF_SOAP_METHOD_AUTHORIZE_CARD                     = "AuthorisePaymentCard";
    public static string TYPE_OF_SOAP_METHOD_CONVERT_QUOTE                      = "ConvertQuote";

    //HTTP Post data fields.
    public static SortedList<string, string> requestFields                            = null;
    #endregion

    #region Static Constructor
    static Common ()
    {


        /// <summary>
        /// Static Constructor that initializes the focus connect logger.
        /// </summary>

        //Initialize the logger.
        //REVCheckoutLogger = LogManager.GetLogger("REVCheckOut");
    }
    #endregion

    #region ConstructCheckOutResponse

    /// <summary>
    /// Constructs the REV CheckOut response message.
    /// </summary>
    /// <param name="responseCode">CheckOut response code.</param>
    /// <param name="responseMessage">CheckOut response message.</param>
    /// <param name="amount">CheckOut gateway transaction amount.</param>
    /// <param name="transactionId">CheckOut gateway transaction id.</param>
    /// <param name="transactionTime">CheckOut gateway transaction time.</param>
    /// <param name="token">Token value</param>
    /// <param param name="authCode"></param>
    /// <param name="cardNumber">Card number</param>
    /// <param name="rememberMe">Saved card checkbox value</param>
    /// <param name="orderNumber">Order Number</param>
    /// <param name="cardType">Card Type</param>
    /// <param name="pNRef">PN Reference</param>
    /// <param name="savedCardCustomer">Saved card Customer</param>
    /// <returns>Response message with key/value pair collection of data.</returns>
    public static NameValueCollection ConstructCheckOutResponse (int responseCode, string responseMessage, string amount, string transactionId,
                                                                 string transactionTime, string cardname, string token, string authCode, string orderNumber,
                                                                 string rememberMe, string cardNumber, string cardType, string pNRef,
                                                                 string savedCardCustomer, string AppKey)
    {
        NameValueCollection checkOutPostData = new NameValueCollection();
        checkOutPostData.Add(RESPONSE_CODE, responseCode.ToString());
        checkOutPostData.Add(MESSAGE, responseMessage);
        checkOutPostData.Add(TRANSACTION_AMOUNT, amount);
        checkOutPostData.Add(TRANSACTION_ID, transactionId);
        checkOutPostData.Add(TRANSACTION_TIME, transactionTime);
        checkOutPostData.Add(CARD_HOLDER_NAME, cardname);
        checkOutPostData.Add(RESPONSE_TOKEN, token);
        checkOutPostData.Add(AUTHCODE, authCode);
        checkOutPostData.Add(RESPONSE_ORDER_NUMBER, orderNumber);
        checkOutPostData.Add(REMEMBER_ME_CHECKBOX, rememberMe);
        checkOutPostData.Add(CARD_NUMBER, cardNumber);
        checkOutPostData.Add(CARD_TYPE, cardType);
        checkOutPostData.Add(PNREF, pNRef);
        checkOutPostData.Add(RESPONSE_SAVED_CARD_CUSTOMER, savedCardCustomer);
        checkOutPostData.Add(APP_KEY, AppKey);

        return checkOutPostData;
    }

    #endregion

    #region ConstructCheckOutResponse

    /// <summary>
    /// Constructs the REV CheckOut response message.
    /// </summary>
    /// <param name="responseCode">CheckOut response code.</param>
    /// <param name="responseMessage">CheckOut response message.</param>
    /// <param name="amount">CheckOut gateway transaction amount.</param>
    /// <param name="transactionId">CheckOut gateway transaction id.</param>
    /// <param name="transactionTime">CheckOut gateway transaction time.</param>
    /// <returns>Response message with key/value pair collection of data.</returns>
    public static string ConstructNameValueCheckOutResponse (string responseCode, string responseMessage, string amount, string transactionId,
                                                                 string transactionTime, string token, string authCode, string orderNumber,
                                                                 string rememberMe, string cardNumber,string cardName, string cardType, string pNRef,
                                                                 string savedCardCustomer, string AppKey)
    {

        KeyValuePair<string, string>[] kvpArr = new KeyValuePair<string, string>[]
                                                {
                                                            new KeyValuePair<string, string>(Common.RESPONSE_CODE, responseCode),
                                                            new KeyValuePair<string, string>(Common.MESSAGE, responseMessage),
                                                            new KeyValuePair<string,string>(Common.TRANSACTION_AMOUNT, amount),
                                                             new KeyValuePair<string,string>(Common.TRANSACTION_ID, transactionId),
                                                             new KeyValuePair<string,string>(Common.TRANSACTION_TIME, transactionTime),
                                                             new KeyValuePair<string,string>(Common.RESPONSE_TOKEN, token),
                                                             new KeyValuePair<string,string>(Common.AUTHCODE, authCode),
                                                             new KeyValuePair<string,string>(Common.RESPONSE_ORDER_NUMBER,  orderNumber),
                                                             new KeyValuePair<string,string>(Common.REMEMBER_ME_CHECKBOX, rememberMe),
                                                             new KeyValuePair<string,string>(Common.CARD_NUMBER, cardNumber),
                                                             new KeyValuePair<string,string>(Common.CARD_HOLDER_NAME,cardName),
                                                             new KeyValuePair<string,string>(Common.CARD_TYPE, cardType),
                                                             new KeyValuePair<string,string>(Common.PNREF, pNRef),
                                                             new KeyValuePair<string,string>(Common.RESPONSE_SAVED_CARD_CUSTOMER, savedCardCustomer),
                                                             new KeyValuePair<string,string>(Common.APP_KEY, AppKey)
                                                };
        var parameters = new StringBuilder();
        foreach(var item in kvpArr)
        {
            parameters.AppendFormat("{0}={1}&", item.Key, HttpUtility.UrlEncode(item.Value.ToString()));
        }
        parameters.Remove(parameters.Length - 1, 1); // remove the last '&'

        return parameters.ToString();
    }

    #endregion

    #region ApplyLuhnsFormula

    /// <summary>
    /// Identifies whether the given credit card number is valid or not as per Mod10 operation.
    /// </summary>
    /// <param name="value">Credit card number.</param>
    /// <returns>Flag that deatails the validity of the credit card number.</returns>
    public static bool ApplyLuhnsFormula (string value)
    {
        try
        {
            if(String.IsNullOrEmpty(value) || value.Length < 13)
                return false;

            //Array to contain individual numbers.
            ArrayList checkNumbers = new ArrayList();

            //Get the length of the card.
            int cardLength = value.Length;

            //Double the value of alternate digits, starting with the second digit from the right, i.e. back to front.
            //Loop through starting at the end.
            for(int i = cardLength - 2; i >= 0; i = i - 2)
            {
                // Now read the contents at each index, this can then be stored as an array of integers. But, before to that,double the number returned.
                checkNumbers.Add(Int32.Parse(value[i].ToString()) * 2);
            }

            //This will hold the total sum of all checksum digits.
            int checkSum = 0;

            //Add the separate digits of all products.
            for(int j = 0; j <= checkNumbers.Count - 1; j++)
            {
                int count = 0;	//This will hold the sum of the digits.

                //Check whether the current number has more than one digit.
                if((int)checkNumbers[j] > 9)
                {
                    int numberLength = ((int)checkNumbers[j]).ToString().Length;

                    //Add count to each digit.
                    for(int k = 0; k < numberLength; k++)
                    {
                        count += Int32.Parse(((int)checkNumbers[j]).ToString()[k].ToString());
                    }
                }
                else
                {
                    count = (int)checkNumbers[j];	//Single digit, just add it by itself.
                }

                checkSum += count;	//Add sum to the total sum.
            }

            //Now, add the unaffected digits. i.e. add all the digits that we didn't double starting from the right.
            //This time we need to start from the rightmost number with alternating digits.
            int originalSum = 0;

            for(int l = cardLength - 1; l >= 0; l = l - 2)
            {
                originalSum += Int32.Parse(value[l].ToString());
            }

            //Perform the final calculation, if the sum Mod 10 results in 0 then its valid, otherwise its invalid card number.
            return (((originalSum + checkSum) % 10) == 0);
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region GetDecimalMoneyValue

    /// <summary>
    /// Gets the Decimal representation value from the given money in string representation.
    /// </summary>
    /// <param name="moneyString">String representation of money value.</param>
    /// <returns>Decimal representation of given value.</returns>
    public static decimal GetDecimalMoneyValue (string moneyString)
    {
        decimal decimalMoneyValue;
        decimal.TryParse(moneyString, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimalMoneyValue);

        return decimalMoneyValue;
    }

    #endregion

    #region Card Type
    /// <summary>
    /// CreditCardTypeType copied for PayPal WebPayment Pro API
    /// (If you use the PayPal API, you do not need this definition)
    /// </summary>
    public enum CreditCardTypeType
    {
        Visa,
        MasterCard,
        Discover,
        Amex,
        DinersClub,
        //enRoute,
        JCB,
        Maestro,
        ChinaUnionPay,
        Unknown
    }
    #endregion

    #region Get Cart Type and Validate card number
    public static string GetCardType (string cardNumber)
    //public static bool IsValidNumber (string cardNumber)
    {
        string cardType = string.Empty;

        //for testing card transactions are:
        //Credit Card Type              Credit Card Number
        //American Express              378282246310005
        //American Express              371449635398431
        //American Express Corporate    378734493671000
        //Diners Club                   30569309025904
        //Diners Club                   38520000023237
        //Discover                      6011111111111117
        //Discover                      6011000990139424
        //MasterCard                    5555555555554444
        //MasterCard                    5105105105105100
        //Visa                          4111111111111111
        //Visa                          4012888888881881


        // AMEX -- 34 or 37 -- 15 length
        //if((Regex.IsMatch(cardNumber, "^(34|37)")))
        if((Regex.IsMatch(cardNumber, "^3[47][0-9]{13}$")))
        {
            cardType = CreditCardTypeType.Amex.ToString();
        }
        // MasterCard -- 51 through 55 -- 16 length
        //else if((Regex.IsMatch(cardNumber, "^(51|52|53|54|55|2221-2720)")))
        else if((Regex.IsMatch(cardNumber, "^5[1-5][0-9]{14}$")))
        {
            cardType = CreditCardTypeType.MasterCard.ToString();
        }
        // VISA -- 4 -- 13 and 16 length
        //else if((Regex.IsMatch(cardNumber, "^(4)")))
        else if((Regex.IsMatch(cardNumber, "^4[0-9]{12}(?:[0-9]{3})?$")))
        {
            cardType = CreditCardTypeType.Visa.ToString();
        }
        //Maestro -- 50 -- 16 length
        //else if((Regex.IsMatch(cardNumber, "^(50|(5[6-9]|6[0-9])")))
        else if((Regex.IsMatch(cardNumber, "^(?:5[0678]\\d\\d|6304|6390|67\\d\\d)\\d{8,15}$")))
        {
            cardType = CreditCardTypeType.Maestro.ToString();
        }
        // Diners Club -- 300-305, 36 or 38 -- 14 length
        //else if((Regex.IsMatch(cardNumber, "^(300|301|302|303|304|305|309|54|55|36|38)")))
        else if((Regex.IsMatch(cardNumber, "^3(?:0[0-5]|[68][0-9])[0-9]{11}$")))
        {
            cardType = CreditCardTypeType.DinersClub.ToString();
        }
        //// enRoute -- 2014,2149 -- 15 length
        //else if(Regex.IsMatch(cardNumber, "^(2014|2149)"))
        //{
        //    cardType = CreditCardTypeType.enRoute.ToString();
        //}
        // Discover -- 6011 -- 16 length
        //else if(Regex.IsMatch(cardNumber, "^(6011|622[126-925]{|65|644-649)"))
        else if(Regex.IsMatch(cardNumber, "^6(?:011|5[0-9]{2})[0-9]{12}$"))
        {
            cardType = CreditCardTypeType.Discover.ToString();
        }
        // CUP -- 62 -- 16 length
        //else if(Regex.IsMatch(cardNumber, "^(62)"))
        else if(Regex.IsMatch(cardNumber, "^62[0-5]\\d{13,16}$"))
        {
            cardType = CreditCardTypeType.ChinaUnionPay.ToString();
        }
        // JCB -- 3 -- 16 length
        //else if(Regex.IsMatch(cardNumber, "^(3528-3589)"))
        else if(Regex.IsMatch(cardNumber, "^(?:2131|1800|35\\d{3})\\d{11}$"))
        {
            cardType = CreditCardTypeType.JCB.ToString();
        }
        //// JCB -- 2131, 1800 -- 15 length
        //else if(Regex.IsMatch(cardNumber, "^(2131|1800)"))
        //{
        //    cardType = CreditCardTypeType.JCB.ToString();
        //}
        else
        {
            // Card type wasn't recognised, provided Unknown is in the 
            // CardTypes property, 
            cardType = CreditCardTypeType.Unknown.ToString();
        }


        //Regex cardTest = new Regex(cardRegex);

        //Determine the card type based on the number
        //CreditCardTypeType? ccType = GetCardTypeFromNumber(cardNum);
        //cardType = ccType.ToString();
        //Call the base version of IsValidNumber and pass the 
        //number and card type
        //if(ApplyLuhnsFormula(cardNumber))
            return cardType;
        //else
        //    //The card fails Luhn's test
        //    return false;
    }
    #endregion

    #region Get complete Card Epiray Date
    public static string GetCardExpiryDate (string year, int iMonth)
    {
        string cardExpirtDate = string.Empty;
        string strapend = "-";
        int y = Convert.ToInt32(year);

        string month = string.Empty;

        month = (iMonth < 10) ? "0" + iMonth.ToString() : iMonth.ToString();

        switch(month)
        {
            case "01":
            case "03":
            case "05":
            case "07":
            case "08":
            case "10":
            case "12":
                cardExpirtDate = year + strapend + month + strapend + "31";
                break;
            case "04":
            case "06":
            case "09":
            case "11":
                cardExpirtDate = year + strapend + month + strapend + "30";
                break;

            case "02":
                if((y % 4 == 0 && y % 100 != 0) || (y % 400 == 0))
                {
                    cardExpirtDate = year + strapend + month + strapend + "29";
                }
                else
                {
                    cardExpirtDate = year + strapend + month + strapend + "28";
                }

                break;

        }

        return cardExpirtDate;
    }
    #endregion

    #region Get complete Card Epiray Date
    public static string GetCardExpiryDate(int year, int iMonth)
    {       

        string month = string.Empty;

        month = (iMonth < 10) ? "0" + iMonth.ToString() : iMonth.ToString();

        Common.LogWrite("Preceded 0 for single digit month.");
        return month + year.ToString();//MMYY
    }
    #endregion

    #region MaskCreditCardNumber

    /// <summary>
    /// Masks the given credit card number until the specified unmasked count.
    /// </summary>
    /// <param name="creditCardNumber">Actual credit card number.</param>
    /// <param name="unmaskedCount">Unmasked count that should be left alone without masking.</param>
    /// <returns>Masked credit card number.</returns>
    public static string MaskCreditCardNumber (string creditCardNumber, int unmaskedCount)
    {
        //If the credit card number is empty/null.
        if(String.IsNullOrEmpty(creditCardNumber))
            return null;

        String maskedCreditCardNumber = string.Empty;

        //If the credit card number length is less than the unmask count, return the credit card number without any masking.
        if(creditCardNumber.Length < unmaskedCount)
            maskedCreditCardNumber = creditCardNumber;

        //Else if the credit card number length is equal to the unmask count, return the credit card number with mask of 10 '*'.
        else if(creditCardNumber.Length == unmaskedCount)
            maskedCreditCardNumber = "**********" + creditCardNumber;

        else
        {
            //Extract the unmasked data to append at the end for the masked data.
            String unmaskedData = creditCardNumber.Substring(creditCardNumber.Length - unmaskedCount, unmaskedCount);

            //Start masking the data.
            for(int i = 0; i < creditCardNumber.Length - unmaskedCount; i++)
            {
                maskedCreditCardNumber += "*";
            }

            //Append the unmasked data to masked data.
            maskedCreditCardNumber += unmaskedData;
        }

        //Return the masked credit card number.
        return maskedCreditCardNumber;
    }

    #endregion

    #region Remove Xml namespaces
    public static XmlDocument RemoveXmlns (String xml)
    {
        XDocument d = XDocument.Parse(xml);
        d.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

        foreach(var elem in d.Descendants())
            elem.Name = elem.Name.LocalName;

        var xmlDocument = new XmlDocument();
        xmlDocument.Load(d.CreateReader());

        return xmlDocument;
    }
    #endregion

    #region Parsing xml
    public static Object ParsingProcess (string XMLData, Type Value)
    {
        object obj = new object();
        //filter Out unwanted data(Hexadecimal Value)
        XMLData = String.Join("", XMLData.Where(c => !char.IsControl(c)));
        // var serializer = new XmlSerializer(typeof(QuiskTerminalRequest));
        var serializer = new XmlSerializer(Value);
        stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(XMLData));
        using(var reader = XmlReader.Create(stream))
        {
            var ivrRequestInfo = serializer.Deserialize(reader);
            obj = ivrRequestInfo;
        }

        return obj;

    }
    #endregion

    #region Get Token Request XML
    public static string GetTokenRequestXML ()
    {
        string tokenRequestXml = string.Empty;

        if(userName != string.Empty && password != string.Empty)
        {
            LogWrite("Got the user credentials for Token generation.");

            tokenRequestXml = "<soapenv:Envelope xmlns:soapenv= 'http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/' " +
                              "xmlns:mgs= 'http://schemas.datacontract.org/2004/07/MGSLObjects'>" + "\n" +
                              "<soapenv:Header/>" + "\n" +
                              "<soapenv:Body>" + "<tem:GetNewToken>" + "\n" +
                              "<tem:req>" + "\n" +
                              "<mgs:Password>" + password.Trim() + "</mgs:Password>" + "\n" +
                              "<mgs:User>" + userName.Trim() + "</mgs:User>" + "\n" +
                              "</tem:req>" + "\n" +
                              "</tem:GetNewToken>" + "\n" +
                              "</soapenv:Body>" + "\n" +
                              "</soapenv:Envelope>";
        }
        else
        {
            Common.LogWrite("No user credentials found for Token generation.");
        }

        return tokenRequestXml;
    }
    #endregion

    #region Get Authorise Payment Card Xml
    public static string GetAuthorizePaymentCardXml (ConsumerOrderDetails consumerOrderInformation, string authorizeToken, string cardExpiryDate,
                                                    string cardNumber, string cvvNumber, string cardType, int bsavedCard, bool isCardTransaction)
    {
        string authorizePaymentCardXml = string.Empty;
        string orderType = string.Empty;

        if(consumerOrderInformation.OrganizationCode == "1" || consumerOrderInformation.OrganizationCode == "5")
        {
            orderType = ORDER_TYPE_SP;
        }
        else if(consumerOrderInformation.OrganizationCode == "2")
        {
            orderType = ORDER_TYPE_SO;
        }
        else
        {
            orderType = ORDER_TYPE_SO;
        }

        string commonStartingxmltags = "<soapenv:Envelope xmlns:soapenv= 'http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/' " +
                                      "xmlns:mgs= 'http://schemas.datacontract.org/2004/07/MGSLObjects'>" + "\n" +
                                      "<soapenv:Header/>" + "\n" +
                                      "<soapenv:Body>" + "<tem:AuthorizePaymentCard>" + "\n" +
                                      "<tem:req>" + "\n" +
                                      "<mgs:AuthToken>" + authorizeToken + "</mgs:AuthToken>" +
                                      "\n" + "<mgs:OrganizationCode>" + WebUtility.HtmlEncode(consumerOrderInformation.OrganizationCode) + "</mgs:OrganizationCode>" + "\n" +
                                      "<mgs:UserName>" + WebUtility.HtmlEncode(consumerOrderInformation.UserName) + "</mgs:UserName>" +
                                      "<mgs:BillingAddress>" + "\n" +
                                      "<mgs:AddressLine1>" + WebUtility.HtmlEncode(consumerOrderInformation.AddressLine1) + "</mgs:AddressLine1>" + "\n" +
                                      "<mgs:AddressLine2>" + WebUtility.HtmlEncode(consumerOrderInformation.AddressLine2) + "</mgs:AddressLine2>" + "\n" +
                                      "<mgs:AddressLine3>" + WebUtility.HtmlEncode(consumerOrderInformation.AddressLine3) + "</mgs:AddressLine3>" + "\n" +
                                      "<mgs:AddressLine4>" + WebUtility.HtmlEncode(consumerOrderInformation.AddressLine4) + "</mgs:AddressLine4>" + "\n" +
                                      "<mgs:City>" + WebUtility.HtmlEncode(consumerOrderInformation.City) + "</mgs:City>" + "\n" +
                                      "<mgs:Country>" + WebUtility.HtmlEncode(consumerOrderInformation.Country) + "</mgs:Country>" + "\n" +
                                      "<mgs:Fax>" + WebUtility.HtmlEncode(consumerOrderInformation.Fax) + "</mgs:Fax>" + "\n" +
                                      "<mgs:Name>" + WebUtility.HtmlEncode(consumerOrderInformation.Name) + "</mgs:Name>" + "\n" +
                                      "<mgs:Phone>" + WebUtility.HtmlEncode(consumerOrderInformation.Phone) + "</mgs:Phone>" + "\n" +
                                      "<mgs:PostalCode>" + WebUtility.HtmlEncode(consumerOrderInformation.PostalCode) + "</mgs:PostalCode>" + "\n" +
                                      "<mgs:State>" + WebUtility.HtmlEncode(consumerOrderInformation.State) + "</mgs:State>" + "\n" +
                                      "</mgs:BillingAddress>";

        string commonEndingxmltags = "<mgs:OrderCompany>" + WebUtility.HtmlEncode(consumerOrderInformation.OrderCompany) + "</mgs:OrderCompany>" + "\n" +
                                     "<mgs:OrderNumber>" + WebUtility.HtmlEncode(consumerOrderInformation.OrderNumber) + "</mgs:OrderNumber>" + "\n" +
                                     "<mgs:OrderType>" + WebUtility.HtmlEncode(orderType) + "</mgs:OrderType>" + "\n" +
                                     "</tem:req>" + "\n" +
                                     "</tem:AuthorizePaymentCard>" + "\n" +
                                     "</soapenv:Body>" + "\n" +
                                     "</soapenv:Envelope>";

        if(isCardTransaction)
        {
            authorizePaymentCardXml = commonStartingxmltags + "\n" +
                                      "<mgs:CardInfo>" + "\n" +
                                      "<mgs:CardExpirationDate>" + cardExpiryDate + "</mgs:CardExpirationDate>" + "\n" +
                                      "<mgs:CardNumber>" + cardNumber + "</mgs:CardNumber>" + "\n" +
                                      "<mgs:CardSecurityCode>" + cvvNumber + "</mgs:CardSecurityCode>" + "\n" +
                                      "<mgs:CardTypeCode>" + cardType + "</mgs:CardTypeCode>" + "\n" +
                                      "<mgs:bSaveCard>" + bsavedCard + "</mgs:bSaveCard>" + "\n" +
                                      "</mgs:CardInfo>" + "\n" + commonEndingxmltags;

        }
        else
        {
            authorizePaymentCardXml = commonStartingxmltags + "\n" +
                                      "<mgs:CardInfo>" + "\n" +
                                      "<mgs:SavedCardCustomer>" + consumerOrderInformation.SavedCardCustomer + "</mgs:SavedCardCustomer>" + "\n" +
                                      "<mgs:SavedCardLine>" + consumerOrderInformation.SavedCardLine + "</mgs:SavedCardLine>" + "\n" +
                                      "</mgs:CardInfo>" + "\n" + commonEndingxmltags;

        }

        return authorizePaymentCardXml;

    }
    #endregion

    #region Get Authorise Payment Card Xml
    public static string CreateQuoteXml (ConvertQuoteDetails convertQuoteInfo, string authorizeToken, string cardExpiryDate,
                                                    string cardNumber, string cvvNumber, string cardType, int bsavedCard, bool isCardTransaction)
    {
        string createQuoteXml = string.Empty;
        string orderType = string.Empty;

        if(isCardTransaction)
        {
            string commonStartingxmltags = "<soapenv:Envelope xmlns:soapenv= 'http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/' " +
                                           "xmlns:mgs= 'http://schemas.datacontract.org/2004/07/MGSLObjects'>" + "\n" +
                                           "<soapenv:Header/>" + "\n" +
                                           "<soapenv:Body>" + "<tem:ConvertQuote>" + "\n" +
                                           "<tem:req>" + "\n" +
                                           "<mgs:AuthToken>" + authorizeToken + "</mgs:AuthToken>" +
                                           "\n" + "<mgs:OrganizationCode>" + WebUtility.HtmlEncode(convertQuoteInfo.OrganizationCode) + "</mgs:OrganizationCode>" + "\n" +
                                           "<mgs:UserName>" + WebUtility.HtmlEncode(convertQuoteInfo.UserName) + "</mgs:UserName>" +
                                           "<mgs:BillingAddress>" + "\n" +
                                           "<mgs:AddressLine1>" + WebUtility.HtmlEncode(convertQuoteInfo.AddressLine1) + "</mgs:AddressLine1>" + "\n" +
                                           "<mgs:AddressLine2>" + WebUtility.HtmlEncode(convertQuoteInfo.AddressLine2) + "</mgs:AddressLine2>" + "\n" +
                                           "<mgs:AddressLine3>" + WebUtility.HtmlEncode(convertQuoteInfo.AddressLine3) + "</mgs:AddressLine3>" + "\n" +
                                           "<mgs:AddressLine4>" + WebUtility.HtmlEncode(convertQuoteInfo.AddressLine4) + "</mgs:AddressLine4>" + "\n" +
                                           "<mgs:City>" + WebUtility.HtmlEncode(convertQuoteInfo.City) + "</mgs:City>" + "\n" +
                                           "<mgs:Country>" + WebUtility.HtmlEncode(convertQuoteInfo.Country) + "</mgs:Country>" + "\n" +
                                           "<mgs:Fax>" + WebUtility.HtmlEncode(convertQuoteInfo.Fax) + "</mgs:Fax>" + "\n" +
                                           "<mgs:Name>" + WebUtility.HtmlEncode(convertQuoteInfo.Name) + "</mgs:Name>" + "\n" +
                                           "<mgs:Phone>" + WebUtility.HtmlEncode(convertQuoteInfo.Phone) + "</mgs:Phone>" + "\n" +
                                           "<mgs:PostalCode>" + WebUtility.HtmlEncode(convertQuoteInfo.PostalCode) + "</mgs:PostalCode>" + "\n" +
                                           "<mgs:State>" + WebUtility.HtmlEncode(convertQuoteInfo.State) + "</mgs:State>" + "\n" +
                                           "</mgs:BillingAddress>";

            string commonEndingxmltags = "<mgs:CarrierCode>" + WebUtility.HtmlEncode(convertQuoteInfo.CarrierCode) + "</mgs:CarrierCode>" + "\n" +
                                         "<mgs:Company>" + WebUtility.HtmlEncode(convertQuoteInfo.OrderCompany) + "</mgs:Company>" + "\n" +
                                         "<mgs:CustomerPO>" + WebUtility.HtmlEncode(convertQuoteInfo.CustomerPO) + "</mgs:CustomerPO>" + "\n" +
                                         "<mgs:QuoteNumber>" + WebUtility.HtmlEncode(convertQuoteInfo.QuoteNumber) + "</mgs:QuoteNumber>" + "\n" +
                                         "</tem:req>" + "\n" +
                                         "</tem:ConvertQuote>" + "\n" +
                                         "</soapenv:Body>" + "\n" +
                                         "</soapenv:Envelope>";

        
            createQuoteXml           = commonStartingxmltags + "\n" +
                                      "<mgs:CardInfo>" + "\n" +
                                      "<mgs:CardExpirationDate>" + cardExpiryDate + "</mgs:CardExpirationDate>" + "\n" +
                                      "<mgs:CardNumber>" + cardNumber + "</mgs:CardNumber>" + "\n" +
                                      "<mgs:CardSecurityCode>" + cvvNumber + "</mgs:CardSecurityCode>" + "\n" +
                                      "<mgs:CardTypeCode>" + cardType + "</mgs:CardTypeCode>" + "\n" +
                                      "<mgs:bSaveCard>" + bsavedCard + "</mgs:bSaveCard>" + "\n" +
                                      "</mgs:CardInfo>" + "\n" + commonEndingxmltags;

        }
        else
        {
            createQuoteXml = "<soapenv:Envelope xmlns:soapenv= 'http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/' " +
                                      "xmlns:mgs= 'http://schemas.datacontract.org/2004/07/MGSLObjects'>" + "\n" +
                                      "<soapenv:Header/>" + "\n" +
                                      "<soapenv:Body>" + "<tem:ConvertQuote>" + "\n" +
                                      "<tem:req>" + "\n" +
                                      "<mgs:AuthToken>" + authorizeToken + "</mgs:AuthToken>" + "\n" +
                                      "<mgs:OrganizationCode>" + WebUtility.HtmlEncode(convertQuoteInfo.OrganizationCode) + "</mgs:OrganizationCode>" + "\n" +
                                      "<mgs:UserName>" + WebUtility.HtmlEncode(convertQuoteInfo.UserName) + "</mgs:UserName>" + "\n" +
                                      "<mgs:CardInfo>" + "\n" +
                                      "<mgs:SavedCardCustomer>" + WebUtility.HtmlEncode(convertQuoteInfo.SavedCardCustomer) + "</mgs:SavedCardCustomer>" + "\n" +
                                      "<mgs:SavedCardLine>" + WebUtility.HtmlEncode(convertQuoteInfo.SavedCardLine) + "</mgs:SavedCardLine>" + "\n" +
                                      "</mgs:CardInfo>" + "\n" +
                                      "<mgs:CarrierCode>" + WebUtility.HtmlEncode(convertQuoteInfo.CarrierCode) + "</mgs:CarrierCode>" + "\n" +
                                      "<mgs:Company>" + WebUtility.HtmlEncode(convertQuoteInfo.OrderCompany) + "</mgs:Company>" + "\n" +
                                      "<mgs:CustomerPO>" + WebUtility.HtmlEncode(convertQuoteInfo.CustomerPO) + "</mgs:CustomerPO>" + "\n" +
                                      "<mgs:QuoteNumber>" + WebUtility.HtmlEncode(convertQuoteInfo.QuoteNumber) + "</mgs:QuoteNumber>" + "\n" +
                                      "<mgs:ShippingInstructions1>" + WebUtility.HtmlEncode(convertQuoteInfo.ShippingInstructions1) + "</mgs:ShippingInstructions1>" + "\n" +
                                      "<mgs:ShippingInstructions2>" + WebUtility.HtmlEncode(convertQuoteInfo.ShippingInstructions2) + "</mgs:ShippingInstructions2>" + "\n" +
                                      "</tem:req>" + "\n" +
                                      "</tem:ConvertQuote>" + "\n" +
                                      "</soapenv:Body>" + "\n" +
                                      "</soapenv:Envelope>"; 


        }

        return createQuoteXml;

    }
    #endregion

    #region Validate Currency
    public static bool Validate_Currency (string currency, ref string currencySymbol)
    {
        bool isValidCurrency = false;

        switch(currency)
        {
            case "USD":

                currencySymbol = "$";
                isValidCurrency = true;

                break;

            case "CAD":

                currencySymbol = "$";
                isValidCurrency = true;

                break;

            case "EUR":

                currencySymbol = "€";
                isValidCurrency = true;

                break;

            case "GBP":

                currencySymbol = "£";
                isValidCurrency = true;

                break;

            case "RMB":

                currencySymbol = "¥";
                isValidCurrency = true;

                break;

            case "JPY":

                currencySymbol = "¥";
                isValidCurrency = true;

                break;
        }

        return isValidCurrency;
    }
    #endregion


   private static  string m_exePath = string.Empty;
   
    public static  void LogWrite(string logMessage)
    {
        m_exePath = ConfigurationManager.AppSettings["xmllogger"];


        try
        {
            using (StreamWriter w = File.AppendText(m_exePath + "\\" + "RevCheckOut-" + DateTime.Now.ToString("dd-MMM-yy") + ".txt"))
            {
                Log(logMessage, w);
            }
        }
        catch (Exception ex)
        {
        }
    }

    public static void Log(string logMessage, TextWriter txtWriter)
    {
        try
        {
            txtWriter.Write("\r\nLog Entry : ");
            txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            txtWriter.WriteLine("  :");
            txtWriter.WriteLine("  :{0}", logMessage);
            txtWriter.WriteLine("-------------------------------");
        }
        catch (Exception ex)
        {
        }
    }


    //public static void CheckSession()
    //{
    //    if (HttpContext.Current.Session == null)
    //        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
    //}
}