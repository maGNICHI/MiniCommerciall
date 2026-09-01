import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/services/order-service.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orders: any[] = [];
  loading = true;
  errorMessage = '';

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading = true;
    this.orderService.getOrders().subscribe({
      next: (data) => {
        console.log('Données reçues :', data); // Pour débogage
        this.orders = data;
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = "Erreur lors de la récupération des commandes.";
        this.loading = false;
      }
    });
  }

  // Changer le statut rapidement sans ouvrir le formulaire complet
  changeStatus(order: any, newStatus: string) {
    const updatedOrder = { ...order, statut: newStatus };
    this.orderService.updateOrder(order.id, updatedOrder).subscribe(() => {
      this.loadOrders();
    });
  }

  deleteOrder(id: number): void {
    if (confirm('Voulez-vous vraiment supprimer cette commande ?')) {
      this.orderService.deleteOrder(id).subscribe(() => {
        this.loadOrders();
      });
    }
  }
}