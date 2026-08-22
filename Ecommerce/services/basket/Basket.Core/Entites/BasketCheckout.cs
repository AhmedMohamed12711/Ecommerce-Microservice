namespace Basket.Core.Entites;

public class BasketCheckout
{
    public string UserName { get; set; } = null!;
    public decimal TotalPrice  { get; set; } 


    public string EmailAddress {  get; set; }= null!;
    public string FirstName {  get; set; }= null!;
    public string LastName {  get; set; }= null!;
    public string AddressLine {  get; set; }= null!;
    public string Country {  get; set; }= null!;
    public string City {  get; set; }= null!;
    public string ZipCode {  get; set; }= null!;
    public string CardName {  get; set; }= null!;
    public string CardNumber {  get; set; }= null!;
    public string Expiration {  get; set; }= null!;
    public string CVV {  get; set; }= null!;

    public int PaymentMethode { get; set; }
}
