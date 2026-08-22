export interface IOrder {
  id: number;
  userName: string;
  firstName?: string;
  lastName?: string;
  totalPrice: number;
  emailAddress?: string;
  addressLine?: string;
  country?: string;
  state?: string;
  zipCode?: string;
  paymentMethod?: number;
}
