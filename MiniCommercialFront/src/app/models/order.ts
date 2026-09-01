import { OrderLine } from "./OrderLine";


export interface Order {
  id?: number;
  orderNumber?: string;
  clientId: number;
  clientName?: string;
  orderDate?: Date;
  status?: string;
  totalHT?: number;
  totalTTC?: number;
  lines: OrderLine[];
}