export interface OrderLine {
  productId: number;
  productName?: string;
  quantity: number;
  unitPrice: number;
  totalLine?: number;
}
