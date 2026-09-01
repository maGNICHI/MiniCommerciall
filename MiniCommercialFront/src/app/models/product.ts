export interface Product {
  id?: number;
  reference: string;
  name: string;
  description: string;
  unitPriceHT: number;
  stockQuantity: number;
}